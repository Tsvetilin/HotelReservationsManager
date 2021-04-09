using System;

namespace Tests.Common
{
    /// <summary>
    /// Class fixture to dispose all created test databases after all tests are finished
    /// </summary>
    public class DisposableClassFixture : IDisposable
    {
        public static int InitializationCounter { get; private set; }

        public DisposableClassFixture()
        {
            InitializationCounter++;
        }

        public void Dispose()
        {
            TestDatabaseConnectionProvider.RemoveTestDatabases();
        }
    }
}
