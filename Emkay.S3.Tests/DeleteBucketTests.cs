using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class DeleteBucketTests : S3TestsBase
    {
        private DeleteBucket _delete;

        [SetUp]
        public void SetUp()
        {
            using (var items = new EnumerateBuckets(RequestTimoutMilliseconds, LoggerMock)
                                {
                                    Key = Key,
                                    Secret = Secret,
                                    Client = ClientMock, // TODO comment this here for lazy instanciation
                                })
            {
                Assert.IsTrue(items.Execute());

                _delete = new DeleteBucket(RequestTimoutMilliseconds, LoggerMock)
                            {
                                Key = Key,
                                Secret = Secret,
                                Client = ClientMock, // TODO comment this here for lazy instanciation
                            };

                if (items.Buckets != null && items.Buckets.Length > 0)
                {
                    _delete.Bucket = items.Buckets[0];
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            _delete.Dispose();
        }

        [Test]
        public void Execute_should_succeed()
        {
            Assert.IsTrue(_delete.Execute());
        }
    }
}