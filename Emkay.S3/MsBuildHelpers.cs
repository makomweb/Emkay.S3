using System.Collections.Specialized;
using Microsoft.Build.Framework;

namespace Emkay.S3
{
    public class MsBuildHelpers
    {
        public static NameValueCollection GetCustomItemMetadata(ITaskItem taskItem) 
        {                     
            var nameValueCollection = new NameValueCollection();

            foreach (string key in taskItem.CloneCustomMetadata().Keys)
            {
                nameValueCollection.Add(key, taskItem.GetMetadata(key));
            }

            return nameValueCollection;
        }
    }
}
