using System;
using System.IO;
using Microsoft.Build.Framework;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Emkay.S3
{
    public class PublishFolder : BuildTask, IDisposable
    {
        private ITaskLogger _logger;

        private IS3Client _client;

        public PublishFolder(string key,
            string secret,
            int timeoutMilliseconds = 1000 * 60 * 5, // 5 min default timeout
            bool publicRead = true,
            ITaskLogger logger = null) :
                this(new S3Client(key, secret),
                timeoutMilliseconds,
                publicRead,
                logger)
        {}

        internal PublishFolder(IS3Client client,
            int timeoutMilliseconds = 1000 * 60 * 5, // 5 min default timeout,
            bool publicRead = true,
            ITaskLogger logger = null)
        {
            _logger = logger;
            _client = client;
            TimeoutMilliseconds = timeoutMilliseconds;
            PublicRead = publicRead;
        }

        [Required]
        public string SourceFolder { get; set; }
        
        [Required]
        public string Bucket { get; set; }

        [Required]
        public string DestinationFolder { get; set; }

        public int TimeoutMilliseconds { get; set; }

        public bool PublicRead { get; set; }

        public ITaskLogger Logger
        {
            get { return _logger ?? (_logger = new MsBuildTaskLogger(Log)); }
            set { _logger = value; }
        }

        public override bool Execute()
        {
            Logger.LogMessage(MessageImportance.Normal,
                           string.Format("Publishing folder {0}", SourceFolder));

            Logger.LogMessage(MessageImportance.Normal,
                           string.Format("to S3 bucket {0}", SourceFolder));

            if (!string.IsNullOrEmpty(DestinationFolder))
                Logger.LogMessage(MessageImportance.Normal,
                               string.Format("destination folder {0}", DestinationFolder));

            try
            {
                _client.EnsureBucketExists(Bucket);
                Publish(_client, SourceFolder, Bucket, DestinationFolder, PublicRead, TimeoutMilliseconds);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMessage(MessageImportance.High,
                               string.Format("Publishing folder has failed because of {0}", ex.Message));
                return false;
            }
        }

        private static void Publish(IS3Client client,
            string sourceFolder,
            string bucket,
            string destinationFolder,
            bool publicRead,
            int timeoutMilliseconds)
        {
            var dirInfo = new DirectoryInfo(sourceFolder);
            var files = dirInfo.GetFiles();
            foreach (var f in files)
            {
                client.PutFile(bucket, CreateRelativePath(destinationFolder, f.Name), f.FullName, publicRead, timeoutMilliseconds);
            }

            var dirs = dirInfo.GetDirectories();
            foreach (var d in dirs)
            {
                Publish(client, d.FullName, bucket, CreateRelativePath(destinationFolder, d.Name), publicRead, timeoutMilliseconds);
            }
        }

        private static string CreateRelativePath(string folder, string name)
        {
            var destinationFolder = folder ?? string.Empty;

            // Append a folder seperator if a folder has been specified without one.
            if (!string.IsNullOrEmpty(destinationFolder) && !destinationFolder.EndsWith("/"))
            {
                destinationFolder += "/";
            }

            return destinationFolder + name;
        }

        public void Dispose()
        {
            if (null != _client)
                _client.Dispose();
            _client = null;
        }
    }
}
