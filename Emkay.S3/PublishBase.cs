using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Emkay.S3
{
    public abstract class PublishBase : Task, IDisposable
    {
        private ITaskLogger _logger;

        protected IS3Client Client;

        protected PublishBase(string key,
                              string secret,
                              int timeoutMilliseconds = 1000 * 60 * 5, // 5 min default timeout
                              bool publicRead = true,
                              ITaskLogger logger = null) :
                                  this(new S3Client(key, secret),
                                       timeoutMilliseconds,
                                       publicRead,
                                       logger)
        {}

        internal PublishBase(IS3Client client,
                             int timeoutMilliseconds = 1000 * 60 * 5, // 5 min default timeout,
                             bool publicRead = true,
                             ITaskLogger logger = null)
        {
            _logger = logger;
            Client = client;
            TimeoutMilliseconds = timeoutMilliseconds;
            PublicRead = publicRead;
        }

        public ITaskLogger Logger
        {
            get { return _logger ?? (_logger = new MsBuildTaskLogger(Log)); }
            set { _logger = value; }
        }

        [Required]
        public string Bucket { get; set; }

        public int TimeoutMilliseconds { get; set; }

        public bool PublicRead { get; set; }

        protected static string CreateRelativePath(string folder, string name)
        {
            var destinationFolder = folder ?? String.Empty;

            // Append a folder seperator if a folder has been specified without one.
            if (!String.IsNullOrEmpty(destinationFolder) && !destinationFolder.EndsWith("/"))
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