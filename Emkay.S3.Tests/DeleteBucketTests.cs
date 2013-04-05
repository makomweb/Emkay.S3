using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class DeleteBucketTests : S3TestsBase
    {
        private DeleteBucket _delete;
        private const string BucketName = "TestDummyBucket";

        [SetUp]
        public void SetUp()
        {
            Client.EnsureBucketExists(BucketName);

            _delete = new DeleteBucket(ClientFactory, RequestTimoutMilliseconds, LoggerMock)
                        {
                            Bucket = BucketName
                        };
        }

        [TearDown]
        public void TearDown()
        {
            if (_delete != null)
                _delete.Dispose();
            _delete = null;
        }

        [Test]
        public void Execute_should_succeed()
        {
            Assert.IsTrue(_delete.Execute());
        }
    }
}