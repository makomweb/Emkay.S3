using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Emkay.S3
{
    public abstract class PublishBase : Task, IDisposable
    {
        private ITaskLogger _logger;
        private IS3Client _client;

        protected PublishBase(int timeoutMilliseconds = 1000 * 60 * 5, // 5 min default timeout,
                             bool publicRead = true,
                             ITaskLogger logger = null)
        {
            _logger = logger;
            TimeoutMilliseconds = timeoutMilliseconds;
            PublicRead = publicRead;
        }

        public ITaskLogger Logger
        {
            get { return _logger ?? (_logger = new MsBuildTaskLogger(Log)); }
            set { _logger = value; }
        }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Secret { get; set; }

        [Required]
        public string Bucket { get; set; }

        public int TimeoutMilliseconds { get; set; }

        public bool PublicRead { get; set; }

        public IS3Client Client
        {
            get { return _client ?? (_client = new S3Client(Key, Secret)); }
            set { _client = value; }
        }

        protected static string CreateRelativePath(string folder, string name)
        {
            var destinationFolder = folder ?? String.Empty;

            // Append a folder seperator if a folder has been specified without one.
            if (!string.IsNullOrEmpty(destinationFolder) && !destinationFolder.EndsWith("/"))
            {
                destinationFolder += "/";
            }

            return destinationFolder + name;
        }

        public void Dispose()
        {
            if (null != Client)
                Client.Dispose();
            Client = null;
        }
    }
}