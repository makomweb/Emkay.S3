using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Emkay.S3
{
    public abstract class S3Base : Task, IDisposable
    {
        private ITaskLogger _logger;
        private IS3Client _client;

        public const int DefaultRequestTimeout = 300000; // 5 min default timeout

        protected S3Base(int timeoutMilliseconds = DefaultRequestTimeout,
                         ITaskLogger logger = null)
        {
            _logger = logger;
            TimeoutMilliseconds = timeoutMilliseconds;
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

        public IS3Client Client
        {
            get { return _client ?? (_client = new S3Client(Key, Secret)); }
            set { _client = value; }
        }

        public void Dispose()
        {
            if (null != _client)
                _client.Dispose();
            _client = null;
        }
    }
}