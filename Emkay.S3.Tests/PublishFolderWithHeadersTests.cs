using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class PublishFolderWithHeadersTests : PublishTestsBase
    {
        private PublishFolderWithHeaders _publish;

        [SetUp]
        public void SetUp()
        {
            _publish = new PublishFolderWithHeaders(ClientFactory, RequestTimoutMilliseconds, true, LoggerMock)
                        {
                            SourceFolders = GetSourceFolder(SourceFolder),
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

        private static ITaskItem[] GetSourceFolder(string folder)
        {
            var folderItem = new TaskItem(folder);
            folderItem.SetMetadata("Test-name", "Test-Content");

            return new ITaskItem[] { folderItem };
        }
    }
}
