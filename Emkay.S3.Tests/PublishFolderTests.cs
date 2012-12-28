using Moq;
using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class PublishFolderTests
    {
        private PublishFolder _publishFolder;
        private const string Key = ""; // TODO edit your AWS S3 key here
        private const string Secret = ""; // TODO edit your AWS S3 secret here
        private const string SourceFolder = "."; // TODO edit your local folder here
        private const string Bucket = ""; // TODO edit your bucket name here
        private const string DestinationFolder = ""; // TODO edit your destination folder here

        [SetUp]
        public void SetUp()
        {
            _publishFolder = new PublishFolder(new Mock<IS3Client>().Object)
                                               //Key, Secret)
                                                 {
                                                     SourceFolder = SourceFolder,
                                                     Bucket = Bucket,
                                                     DestinationFolder = DestinationFolder
                                                 };
        }

        [Test]
        public void Execute_should_succeed()
        {
            Assert.IsTrue(_publishFolder.Execute());
        }
    }
}
