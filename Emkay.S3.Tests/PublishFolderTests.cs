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
            _publish = new PublishFolder(RequestTimoutMilliseconds, true, LoggerMock)
                                                 {
                                                     Key = Key,
                                                     Secret = Secret,
                                                     Client = ClientMock, // TODO comment this here for lazy instanciation
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
