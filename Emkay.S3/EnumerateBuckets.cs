using System;
using Microsoft.Build.Framework;

namespace Emkay.S3
{
    public class EnumerateBuckets : S3Base
    {
        public EnumerateBuckets() :
            this(DefaultRequestTimeout, null)
        {}

        public EnumerateBuckets(int timeoutMilliseconds, ITaskLogger logger)
            : base(timeoutMilliseconds, logger)
        {}

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
