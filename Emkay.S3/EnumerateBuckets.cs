using System;
using Microsoft.Build.Framework;

namespace Emkay.S3
{
    public class EnumerateBuckets : S3Base
    {
        public EnumerateBuckets()
            : this(new S3ClientFactory())
        { }

        [Obsolete("Only for test purpose!")]
        internal EnumerateBuckets(IS3ClientFactory s3ClientFactory,
            int timeoutMilliseconds = DefaultRequestTimeout,
            ITaskLogger logger = null)
            : base(s3ClientFactory, timeoutMilliseconds, logger)
        { }

        public string[] Buckets { get; private set; }

        public override bool Execute()
        {
            try
            {
                Enumerate();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMessage(MessageImportance.High,
                                  string.Format("Enumerating has failed because of {0}", ex.Message));
                return false;
            }
        }

        private void Enumerate()
        {
            Buckets = Client.EnumerateBuckets();
        }
    }
}
