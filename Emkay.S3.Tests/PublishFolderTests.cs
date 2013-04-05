using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class PublishFolderTests : PublishTestsBase
    {
        private PublishFolder _publish;

        [SetUp]
        public void SetUp()
        {
            _publish = new PublishFolder(ClientFactory, RequestTimoutMilliseconds, true, LoggerMock)
                        {
                            SourceFolder = SourceFolder,
                            Bucket = Bucket,
                            DestinationFolder = DestinationFolder
                        };
        }

        [TearDown]
        public void TearDown()
        {
            var removeBucket = new DeleteBucket(ClientFactory, RequestTimoutMilliseconds, LoggerMock)
                                {
                                    Bucket = Bucket
                                };

            Assert.IsTrue(removeBucket.Execute(), "Could not remove test bucket!");

            if (_publish != null)
                _publish.Dispose();
            _publish = null;
        }

        [Test]
        public void Execute_should_succeed()
        {
            Assert.IsTrue(_publish.Execute());
        }
    }
}
