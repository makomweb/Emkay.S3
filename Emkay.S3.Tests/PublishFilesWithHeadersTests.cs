using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class PublishFilesWithHeadersTests : PublishTestsBase
    {
        private PublishFilesWithHeaders _publishWithHeaders;

        [SetUp]
        public void SetUp()
        {            
            _publishWithHeaders = new PublishFilesWithHeaders(ClientFactory, RequestTimoutMilliseconds, true, LoggerMock)
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

            if (_publishWithHeaders != null)
                _publishWithHeaders.Dispose();
            _publishWithHeaders = null;
        }

        [Test]
        public void Execute_should_succeed()
        {
            Assert.IsTrue(_publishWithHeaders.Execute());
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
