using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Emkay.S3
{
    public class PublishFolderWithHeaders : PublishBase
    {
        public PublishFolderWithHeaders() :
            this(new S3ClientFactory())
        { }

        [Obsolete("Only for test purpose!")]
        internal PublishFolderWithHeaders(IS3ClientFactory s3ClientFactory,
            int timeoutMilliseconds = DefaultRequestTimeout,
            bool publicRead = true,
            ITaskLogger logger = null)
            : base(s3ClientFactory, timeoutMilliseconds, publicRead, logger)
        { }

        [Required]
        public ITaskItem[] SourceFolders { get; set; }

        [Required]
        public string DestinationFolder { get; set; }

        public override bool Execute()
        {
            try
            {
                Client.EnsureBucketExists(Bucket);

                foreach (var sourceFolder in SourceFolders)
                {
                    Logger.LogMessage(MessageImportance.Normal,
                        string.Format("Publishing folder {0}", sourceFolder.GetMetadata("Identity")));

                    Logger.LogMessage(MessageImportance.Normal,
                        string.Format("to S3 bucket {0}", Bucket));

                    if (!string.IsNullOrEmpty(DestinationFolder))
                        Logger.LogMessage(MessageImportance.Normal,
                            string.Format("destination folder {0}", DestinationFolder));

                    Publish(Client, sourceFolder, Bucket, DestinationFolder, PublicRead, TimeoutMilliseconds);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMessage(MessageImportance.High,
                    string.Format("Publishing folder has failed because of {0}", ex.Message));
                return false;
            }

        }

        private void Publish(IS3Client client,
            ITaskItem sourceFolder,
            string bucket,
            string destinationFolder,
            bool publicRead,
            int timeoutMilliseconds)
        {
            var dirInfo = new DirectoryInfo(sourceFolder.GetMetadata("Identity"));
            var headers = MsBuildHelpers.GetCustomItemMetadata(sourceFolder);
            var files = dirInfo.GetFiles();

            foreach (var f in files)
            {
                Logger.LogMessage(MessageImportance.Normal, string.Format("Copying file {0}", f.FullName));
                client.PutFileWithHeaders(bucket, CreateRelativePath(destinationFolder, f.Name), f.FullName, headers, publicRead, timeoutMilliseconds);
            }

            var dirs = dirInfo.GetDirectories();
            foreach (var d in dirs)
            {
                Publish(client, new TaskItem(d.FullName, sourceFolder.CloneCustomMetadata()), bucket, CreateRelativePath(destinationFolder, d.Name), publicRead, timeoutMilliseconds);
            }
        }
    }
}
