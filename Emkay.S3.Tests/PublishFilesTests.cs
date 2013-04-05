using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class PublishFilesTests : PublishTestsBase
    {
        private PublishFiles _publish;

        [SetUp]
        public void SetUp()
        {
            _publish = new PublishFiles(ClientFactory, RequestTimoutMilliseconds, true, LoggerMock)
                        {
                            SourceFiles = EnumerateFiles(SourceFolder),
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

        private static string[] EnumerateFiles(string folder)
        {
            return new DirectoryInfo(folder).GetFiles().Select(i => i.FullName).ToArray();
        }
    }
}
