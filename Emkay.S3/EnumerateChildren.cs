using System;
using Microsoft.Build.Framework;

namespace Emkay.S3
{
    public class EnumerateChildren : S3Base
    {
        public EnumerateChildren() :
            this(DefaultRequestTimeout, null)
        {}

        public EnumerateChildren(int timeoutMilliseconds, ITaskLogger logger)
            : base(timeoutMilliseconds, logger)
        {}

        public string[] Children { get; private set; }

        public override bool Execute()
        {
            Logger.LogMessage(MessageImportance.Normal,
                              string.Format("Enumerating bucket {0}", Bucket));

            try
            {
                Enumerate();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMessage(MessageImportance.High,
                                  string.Format("Enumerating bucket has failed because of {0}", ex.Message));
                return false;
            }
        }

        private void Enumerate()
        {
            Children = Client.EnumerateChildren(Bucket);
        }
    }
}