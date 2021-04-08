using System;

namespace Tests.Common
{
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
