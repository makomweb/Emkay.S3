using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
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
        private AmazonS3 _amazonS3;
        private readonly bool _hasOwnership;

        /// <summary>
        /// Initialize an instance of this class.
        /// </summary>
        /// <param name="key">S3 key</param>
        /// <param name="secret">S3 secret</param>
        public S3Client(string key, string secret) :
            this(AWSClientFactory.CreateAmazonS3Client(key, secret), true)
        { }

        /// <summary>
        /// Initialize an instance of this class.
        /// </summary>
        /// <param name="amazonS3">The Amazon S3 instance.</param>
        /// <param name="takeOwnership">Flag which indicates if this instance takes care of disposing the Amazon S3 instance.</param>
        public S3Client(AmazonS3 amazonS3, bool takeOwnership = false)
        {
            _amazonS3 = amazonS3;
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

            _amazonS3.PutBucket(request);
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

            _amazonS3.DeleteBucket(request);
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
            var response = _amazonS3.ListBuckets();
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
                var response = _amazonS3.ListObjects(request);

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
            var request = new PutObjectRequest
                            {
                                CannedACL = publicRead ? S3CannedACL.PublicRead : S3CannedACL.Private,
                                FilePath = file,
                                BucketName = bucketName,
                                Key = key,
                                Timeout = timeoutMilliseconds
                            };

            _amazonS3.PutObject(request);
        }

		/// <summary>
		/// Download a file and save it to a specified location
		/// </summary>
		/// <param name="bucketName">The name of the bucket.</param>
		/// <param name="key">The key of the file to download.</param>
		/// <param name="file">The path for the file to be saved to.</param>
		/// <param name="timeoutMilliseconds">The timeout in milliseconds.</param>
		public void DownloadFile(string bucketName, string key, string file, int timeoutMilliseconds) 
		{
			var request = new GetObjectRequest
                            {
                                BucketName = bucketName,
                                Key = key,
                                Timeout = timeoutMilliseconds
                            };
			var response = _amazonS3.GetObject(request); //TODO: check response status
			response.WriteResponseStreamToFile(file);
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

            _amazonS3.DeleteObject(request);
        }

        /// <summary>
        /// Set access rights of an object.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="key">The key of the object.</param>
        /// <param name="acl">The desired access rights.</param>
        public void SetAcl(string bucketName, string key, S3CannedACL acl)
        {
            var request = new SetACLRequest
                              {
                                  BucketName = bucketName,
                                  CannedACL = acl,
                                  Key = key
                              };

            _amazonS3.SetACL(request);
        }

        /// <summary>
        /// Ensure the specified bucket exists.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        public void EnsureBucketExists(string bucketName)
        {
            if (!AmazonS3Util.DoesS3BucketExist(bucketName, _amazonS3))
            {
                CreateBucket(bucketName);
            }
        }

        /// <summary>
        /// Dispose the underlying Amazon S3 instance in case this instance has ownership.
        /// </summary>
        public void Dispose()
        {
            if (_hasOwnership && _amazonS3 != null)
                _amazonS3.Dispose();
            _amazonS3 = null;
        }
    }
}