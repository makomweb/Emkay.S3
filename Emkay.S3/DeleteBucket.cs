using System;
using Microsoft.Build.Framework;

namespace Emkay.S3
{
    public class DeleteBucket : S3Base
    {
        public DeleteBucket() :
            this(DefaultRequestTimeout, null)
        { }

        public DeleteBucket(int timeoutMilliseconds, ITaskLogger logger)
            : base(timeoutMilliseconds, logger)
        { }

        public override bool Execute()
        {
            try
            {
                Client.DeleteBucket(Bucket);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMessage(MessageImportance.High,
                                  string.Format("Delete bucket has failed because of {0}", ex.Message));
                return false;
            }
        }
    }
}