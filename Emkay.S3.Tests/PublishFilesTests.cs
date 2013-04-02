using System.IO;
using System.Linq;
using Moq;
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
            _publish = new PublishFiles(RequestTimoutMilliseconds, true, LoggerMock)
                                        {
                                            Key = Key,
                                            Secret = Secret,
                                            Client = ClientMock, // TODO comment this here for lazy instanciation
                                            SourceFiles = EnumerateFiles(SourceFolder),
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

        private static string[] EnumerateFiles(string folder)
        {
            return new DirectoryInfo(folder).GetFiles().Select(i => i.FullName).ToArray();
        }
    }
}
