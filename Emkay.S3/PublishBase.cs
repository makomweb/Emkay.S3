using System;

namespace Emkay.S3
{
    public abstract class PublishBase : S3Base
    {
        protected PublishBase()
            : this(new S3ClientFactory(), DefaultRequestTimeout)
        { }

        [Obsolete("Only for test purpose!")]
        internal PublishBase(IS3ClientFactory s3ClientFactory,
            int timeoutMilliseconds,
            bool publicRead = true,
            ITaskLogger logger = null)
            : base(s3ClientFactory, timeoutMilliseconds, logger)
        {
            PublicRead = publicRead;
        }

        public bool PublicRead { get; set; }

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
    }
}