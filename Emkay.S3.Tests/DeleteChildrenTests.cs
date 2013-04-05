using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class DeleteChildrenTests : PublishTestsBase
    {
        private DeleteChildren _delete;
        private const string TestFilePath = ""; // TODO insert path to a local file here!
        private const string TestFileName = ""; // TODO insert the name of the local file here!

        [SetUp]
        public void SetUp()
        {
            var publish = new PublishFiles(RequestTimoutMilliseconds, true, LoggerMock)
                            {
                                Client = Client,
                                SourceFiles = new[] {TestFilePath},
                                Bucket = Bucket,
                                DestinationFolder = DestinationFolder
                            };

            Assert.IsTrue(publish.Execute());

            _delete = new DeleteChildren(RequestTimoutMilliseconds, LoggerMock)
                        {
                            Client = Client,
                            Bucket = Bucket,
                            Children = new[] { string.Format("{0}/{1}", DestinationFolder, TestFileName) }
                        };
        }

        [TearDown]
        public void TearDown()
        {
            var removeBucket = new DeleteBucket(RequestTimoutMilliseconds, LoggerMock)
                {
                    Client = Client,
                    Bucket = Bucket
                };

            Assert.IsTrue(removeBucket.Execute(), "Could not remove test bucket!");

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
