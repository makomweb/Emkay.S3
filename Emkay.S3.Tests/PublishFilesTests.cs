using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
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


        private static ITaskItem[] EnumerateFiles(string folder)
        {
            var files = new List<ITaskItem>();

            foreach (var file in new DirectoryInfo(folder).GetFiles().Select(i => i.FullName).ToList())
            {
                var fileItem = new TaskItem(file);
                fileItem.SetMetadata("Test-name", "Test-Content");
                files.Add(fileItem);
            }

            return files.ToArray();
        }
    }
}
