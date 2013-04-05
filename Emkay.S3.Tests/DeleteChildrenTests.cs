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
            var publish = new PublishFiles(ClientFactory, RequestTimoutMilliseconds, true, LoggerMock)
                            {
                                SourceFiles = new[] {TestFilePath},
                                Bucket = Bucket,
                                DestinationFolder = DestinationFolder
                            };

            Assert.IsTrue(publish.Execute());

            _delete = new DeleteChildren(ClientFactory, RequestTimoutMilliseconds, LoggerMock)
                        {
                            Bucket = Bucket,
                            Children = new[] { string.Format("{0}/{1}", DestinationFolder, TestFileName) }
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
