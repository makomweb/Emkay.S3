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

            _delete = new DeleteBucket(RequestTimoutMilliseconds, LoggerMock)
                        {
                            Key = Key,
                            Secret = Secret,
                            Client = Client,
                            Bucket = BucketName
                        };
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