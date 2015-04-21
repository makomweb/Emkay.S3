using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;

namespace Emkay.S3
{
    public class PublishFiles : PublishBase
    {
        public PublishFiles() :
            this(new S3ClientFactory())
        { }

        [Obsolete("Only for test purpose!")]
        internal PublishFiles(IS3ClientFactory s3ClientFactory,
            int timeoutMilliseconds = DefaultRequestTimeout,
            bool publicRead = true,
            ITaskLogger logger = null)
            : base(s3ClientFactory, timeoutMilliseconds, publicRead, logger)
        { }

        [Required]
        public ITaskItem[] SourceFiles { get; set; }

        [Required]
        public string DestinationFolder { get; set; }

        public override bool Execute()
        {
            Logger.LogMessage(MessageImportance.Normal,
                              string.Format("Publishing {0} files", SourceFiles.Length));

            Logger.LogMessage(MessageImportance.Normal,
                              string.Format("to S3 bucket {0}", Bucket));

            if (!string.IsNullOrEmpty(DestinationFolder))
                Logger.LogMessage(MessageImportance.Normal,
                                  string.Format("destination folder {0}", DestinationFolder));

            try
            {
                Client.EnsureBucketExists(Bucket);

                Publish(Client, SourceFiles, Bucket, DestinationFolder, PublicRead, TimeoutMilliseconds);
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
            IEnumerable<ITaskItem> sourceFiles,
            string bucket,
            string destinationFolder,
            bool publicRead,
            int timeoutMilliseconds)
        {
            foreach (var fileItem in sourceFiles.Where(taskItem => taskItem != null
                && !string.IsNullOrEmpty(taskItem.GetMetadata("Identity"))))
            {
                var info = new FileInfo(fileItem.GetMetadata("Identity"));
                var headers = MsBuildHelpers.GetCustomItemMetadata(fileItem);

                Logger.LogMessage(MessageImportance.Normal, string.Format("Copying file {0}", info.FullName));
                client.PutFile(bucket, CreateRelativePath(destinationFolder, info.Name), info.FullName, headers, publicRead, timeoutMilliseconds);
            }
        }
    }
}