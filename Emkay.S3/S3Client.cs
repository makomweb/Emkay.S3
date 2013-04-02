using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace Emkay.S3
{
    public class S3Client : IS3Client
    {
        private AmazonS3 _client;

        public S3Client(string key, string secret) :
            this(AWSClientFactory.CreateAmazonS3Client(key, secret))
        {}

        public S3Client(AmazonS3 amazonS3Client)
        {
            _client = amazonS3Client;
        }

        public void CreateBucket(string bucketName)
        {
            var request = new PutBucketRequest
                              {
                                  BucketName = bucketName
                              };

            _client.PutBucket(request);
        }

        public void DeleteBucket(string bucketName)
        {
            var request = new DeleteBucketRequest
                              {
                                  BucketName = bucketName
                              };

            _client.DeleteBucket(request);
        }

        public string[] EnumerateBuckets()
        {
            var response = _client.ListBuckets();
            return response.Buckets.Select(b => b.BucketName).ToArray();
        }

        public string[] EnumerateChildren(string bucket)
        {
            var request = new ListObjectsRequest
                {
                    BucketName = bucket
                };

            var response = _client.ListObjects(request);

            return response.S3Objects.Select(o => o.Key).ToArray();
        }

        public void PutFile(string bucketName, string key, string file, bool publicRead, int timeoutMilliseconds)
        {
            var request = new PutObjectRequest
                            {
                                CannedACL = publicRead ? S3CannedACL.PublicRead : S3CannedACL.Private,
                                FilePath = file,
                                BucketName = bucketName,
                                Key = key,
                                Timeout = timeoutMilliseconds
                            };

            _client.PutObject(request);
        }

        public void DeleteObject(string bucketName, string key)
        {
            var request = new DeleteObjectRequest
                              {
                                  BucketName = bucketName, Key = key
                              };

            _client.DeleteObject(request);
        }

        public void SetAcl(string bucketName, string key, S3CannedACL acl)
        {
            var request = new SetACLRequest
                              {
                                  BucketName = bucketName,
                                  CannedACL = acl,
                                  Key = key
                              };

            _client.SetACL(request);
        }

        public void EnsureBucketExists(string bucketName)
        {
            if (!AmazonS3Util.DoesS3BucketExist(bucketName, _client))
            {
                CreateBucket(bucketName);
            }
        }

        public void Dispose()
        {
            if (_client != null)
                _client.Dispose();
            _client = null;
        }
    }
}