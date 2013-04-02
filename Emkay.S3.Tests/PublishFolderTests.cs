using Moq;
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
            _publish = new PublishFolder(RequestTimoutMilliseconds, true, new Mock<ITaskLogger>().Object)
                                                 {
                                                     Key = Key,
                                                     Secret = Secret,
                                                     Client = new Mock<IS3Client>().Object, // TODO comment this here for lazy instanciation
                                                     SourceFolder = SourceFolder,
                                                     Bucket = Bucket,
                                                     DestinationFolder = DestinationFolder
                                                 };
        }

        [TearDown]
        public void TearDown()
        {
            _publish.Dispose();
        }

        [Test]
        public void Execute_should_succeed()
        {
            Assert.IsTrue(_publish.Execute());
        }
    }
}
