using System;

namespace Emkay.S3
{
    public abstract class PublishBase : S3Base
    {
        protected PublishBase(int timeoutMilliseconds = 1000 * 60 * 5, // 5 min default timeout,
                             bool publicRead = true,
                             ITaskLogger logger = null) : base(timeoutMilliseconds, logger)
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