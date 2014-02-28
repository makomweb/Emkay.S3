using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;

namespace Emkay.S3
{
    public class MsBuildHelpers
    {
        public static IEnumerable<string> GetWellDefinedItemMetadata()
        {
            return new List<string>
            {
                "FullPath",
                "RootDir",
                "Filename",
                "Extension",
                "RelativeDir",
                "Directory",
                "RecursiveDir",
                "Identity",
                "ModifiedTime",
                "CreatedTime",
                "AccessedTime"
            };
        }

        public static NameValueCollection GetCustomItemMetadata(ITaskItem taskItem)
        {                     
            var itemMetadata = new NameValueCollection();

            foreach (var metadataName in taskItem.MetadataNames
                    .Cast<string>()
                    .ToList()
                    .Except(GetWellDefinedItemMetadata()))
            {
                itemMetadata.Add(metadataName, taskItem.GetMetadata(metadataName));
            }

            return itemMetadata;
        }
    }
}
