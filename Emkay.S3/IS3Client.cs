using System;
using Amazon.S3.Model;

namespace Emkay.S3
{
    public interface IS3Client : IDisposable
    {
        void CreateBucket(string bucketName);
        
        string[] EnumerateBuckets();

        string[] EnumerateChildren(string bucket);

        void PutFile(string bucketName, string key, string file, bool publicRead, int timeoutMilliseconds);

        void DeleteBucket(string bucketName);

        void DeleteObject(string bucketName, string key);

        void SetAcl(string bucketName, string key, S3CannedACL acl);

        void EnsureBucketExists(string bucketName);

    }
}