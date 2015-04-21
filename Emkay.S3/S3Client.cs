using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace Emkay.S3
{
    /// <summary>
    /// Implementation of the S3 client interface.
    /// </summary>
    public class S3Client : IS3Client
    {
        private AmazonS3Client _amazonS3Client;
        private readonly bool _hasOwnership;
        
        /// <summary>
        /// Initialize an instance of this class.
        /// </summary>
        /// <param name="key">S3 key</param>
        /// <param name="secret">S3 secret</param>
        /// <param name="region">S3 region server name (eg, "us-east-1")</param>
        public S3Client(string key, string secret, string region) :
            this(new AmazonS3Client(key, secret, RegionEndpoint.GetBySystemName(region) ), true)
        { }

        /// <summary>
        /// Initialize an instance of this class.
        /// </summary>
        /// <param name="amazonS3Client">The Amazon S3 instance.</param>
        /// <param name="takeOwnership">Flag which indicates if this instance takes care of disposing the Amazon S3 instance.</param>
        public S3Client(AmazonS3Client amazonS3Client, bool takeOwnership = false)
        {
            _amazonS3Client = amazonS3Client;
            _hasOwnership = takeOwnership;
        }

        /// <summary>
        /// Create a bucket with the specified name.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        public void CreateBucket(string bucketName)
        {
            var request = new PutBucketRequest
                              {
                                  BucketName = bucketName
                              };

            _amazonS3Client.PutBucket(request);
        }

        /// <summary>
        /// Delete a bucket and its content.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        public void DeleteBucket(string bucketName)
        {
            var request = new DeleteBucketRequest
                              {
                                  BucketName = bucketName
                              };

            _amazonS3Client.DeleteBucket(request);
        }

        /// <summary>
        /// Enumerate all the buckets.
        /// Note: AWS provides the list of buckets in a paged manner.
        /// A call to this function iterates over all the pages and sums up
        /// all the items.
        /// </summary>
        /// <returns>Array of bucket names</returns>
        public string[] EnumerateBuckets()
        {
            var response = _amazonS3Client.ListBuckets();
            return response.Buckets.Select(b => b.BucketName).ToArray();
        }

        /// <summary>
        /// Enumerate all the children from a specified bucket.
        /// The children are identified by their key string.
        /// </summary>
        /// <param name="bucket">The name of the bucket.</param>
        /// <returns>Array of child keys.</returns>
        public string[] EnumerateChildren(string bucket)
        {
            return EnumerateChildren(bucket, string.Empty);
        }

        /// <summary>
        /// Enumerate all the children from a specified bucket which match the specified prefix.
        /// This is commonly used for enumerating "subfolders".
        /// </summary>
        /// <param name="bucket">The name of the bucket.</param>
        /// <param name="prefix">The desired prefix</param>
        /// <returns>Array of child keys.</returns>
        public string[] EnumerateChildren(string bucket, string prefix)
        {
            var request = new ListObjectsRequest
            {
                BucketName = bucket
            };

            if (!string.IsNullOrEmpty(prefix))
                request.Prefix = prefix;

            var result = new List<string>();

            do
            {
                var response = _amazonS3Client.ListObjects(request);

                result.AddRange(response.S3Objects.Select(o => o.Key));

                // Fetch the next page in case the response was truncated.
                if (response.IsTruncated)
                {
                    request.Marker = response.NextMarker;
                }
                else
                {
                    request = null;
                }

            } while (request != null);

            return result.ToArray();
        }

        /// <summary>
        /// Store the content from a local file into a bucket.
        /// The key is the path under which the file will be stored.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="key">The key under which the file will be available afterwards.</param>
        /// <param name="file">The path to the local file.</param>
        /// <param name="publicRead">Flag which indicates if the file is publicly available or not.</param>
        /// <param name="timeoutMilliseconds">The timeout in milliseconds within the upload must have happend.</param>
        public void PutFile(string bucketName, string key, string file, bool publicRead, int timeoutMilliseconds)
        {
            PutFile(bucketName, key, file, null, publicRead, timeoutMilliseconds);
        }

        /// <summary>
        /// Store the content from a local file into a bucket.
        /// The key is the path under which the file will be stored.
        /// Use this overload to add custom headers 
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="key">The key under which the file will be available afterwards.</param>
        /// <param name="file">The path to the local file.</param>
        /// <param name="headers">The custom headers to be added to the file</param>
        /// <param name="publicRead">Flag which indicates if the file is publicly available or not.</param>
        /// <param name="timeoutMilliseconds">The timeout in milliseconds within the upload must have happend.</param>
        public void PutFile(string bucketName, string key, string file, NameValueCollection headers, bool publicRead, int timeoutMilliseconds)
        {
            var request = new PutObjectRequest
            {
                CannedACL = publicRead ? S3CannedACL.PublicRead : S3CannedACL.Private,
                FilePath = file,
                BucketName = bucketName,
                Key = key,
                Timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds)
            };

            if (headers != null)
            {
                foreach (var headerKey in headers.AllKeys)
                {
                    request.Headers[headerKey] = headers[headerKey];
                }
            }

            _amazonS3Client.PutObject(request);
        }

        public class Test : GetObjectRequest
        {   
        }

        /// <summary>
        /// Download a file and save it to a specified location
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="key">The key of the file to download.</param>
        /// <param name="file">The path for the file to be saved to.</param>
        public void DownloadFile(string bucketName, string key, string file) 
        {
	        var request = new GetObjectRequest
                            {
                                BucketName = bucketName,
                                Key = key
                            };

	        var response = _amazonS3Client.GetObject(request);
            EnsureSuccess(response);
	        response.WriteResponseStreamToFile(file);
        }

        private static void EnsureSuccess(AmazonWebServiceResponse response)
        {
            if (!IsSuccessStatusCode((int)response.HttpStatusCode))
                throw new AmazonS3Exception("Request failed");
        }

        private static bool IsSuccessStatusCode(long status)
        {
            if (status >= 200)
                return status <= 299;
            return false;
        }

        /// <summary>
        /// Delete an object within a bucket.
        /// The object is identified by its key.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="key">The key of the object.</param>
        public void DeleteObject(string bucketName, string key)
        {
            var request = new DeleteObjectRequest
                              {
                                  BucketName = bucketName, Key = key
                              };

            _amazonS3Client.DeleteObject(request);
        }

        /// <summary>
        /// Set access rights of an object.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="key">The key of the object.</param>
        /// <param name="acl">The desired access rights.</param>
        public void SetAcl(string bucketName, string key, S3CannedACL acl)
        {
            var request = new PutACLRequest
                              {
                                  BucketName = bucketName,
                                  CannedACL = acl,
                                  Key = key
                              };

            _amazonS3Client.PutACL(request);
        }

        /// <summary>
        /// Ensure the specified bucket exists.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        public void EnsureBucketExists(string bucketName)
        {
            if (!AmazonS3Util.DoesS3BucketExist(_amazonS3Client, bucketName))
            {
                CreateBucket(bucketName);
            }
        }

        /// <summary>
        /// Dispose the underlying Amazon S3 instance in case this instance has ownership.
        /// </summary>
        public void Dispose()
        {
            if (_hasOwnership && _amazonS3Client != null)
                _amazonS3Client.Dispose();
            _amazonS3Client = null;
        }
    }
}