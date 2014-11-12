using System;
using System.Collections.Specialized;
using Amazon.S3;

namespace Emkay.S3
{
    /// <summary>
    /// Interface for accessing  basic S3 functionality.
    /// </summary>
    public interface IS3Client : IDisposable
    {
        /// <summary>
        /// Create a bucket with the specified name.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        void CreateBucket(string bucketName);
        
        /// <summary>
        /// Enumerate all the buckets.
        /// Note: AWS provides the list of buckets in a paged manner.
        /// A call to this function iterates over all the pages and sums up
        /// all the items.
        /// </summary>
        /// <returns>Array of bucket names</returns>
        string[] EnumerateBuckets();

        /// <summary>
        /// Enumerate all the children from a specified bucket.
        /// The children are identified by their key string.
        /// </summary>
        /// <param name="bucket">The name of the bucket.</param>
        /// <returns>Array of child keys.</returns>
        string[] EnumerateChildren(string bucket);

        /// <summary>
        /// Enumerate all the children from a specified bucket which match the specified prefix.
        /// This is commonly used for enumerating "subfolders".
        /// </summary>
        /// <param name="bucket">The name of the bucket.</param>
        /// <param name="prefix">The desired prefix</param>
        /// <returns>Array of child keys.</returns>
        string[] EnumerateChildren(string bucket, string prefix);

        /// <summary>
        /// Store the content from a local file into a bucket.
        /// The key is the path under which the file will be stored.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="key">The key under which the file will be available afterwards.</param>
        /// <param name="file">The path to the local file.</param>
        /// <param name="publicRead">Flag which indicates if the file is publicly available or not.</param>
        /// <param name="timeoutMilliseconds">The timeout in milliseconds within the upload must have happend.</param>
        void PutFile(string bucketName, string key, string file, bool publicRead, int timeoutMilliseconds);

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
        void PutFileWithHeaders(string bucketName, string key, string file, NameValueCollection headers, bool publicRead, int timeoutMilliseconds);

        /// <summary>
        /// Delete a bucket and its content.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        void DeleteBucket(string bucketName);

        /// <summary>
        /// Delete an object within a bucket.
        /// The object is identified by its key.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="key">The key of the object.</param>
        void DeleteObject(string bucketName, string key);

        /// <summary>
        /// Set access rights of an object.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="key">The key of the object.</param>
        /// <param name="acl">The desired access rights.</param>
        void SetAcl(string bucketName, string key, S3CannedACL acl);

        /// <summary>
        /// Ensure the specified bucket exists.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        void EnsureBucketExists(string bucketName);

	    /// <summary>
	    /// Download a file and save it to a specified location
	    /// </summary>
	    /// <param name="bucketName">The name of the bucket.</param>
	    /// <param name="key">The key of the file to download.</param>
	    /// <param name="file">The path for the file to be saved to.</param>
	    void DownloadFile(string bucketName, string key, string file);
    }
}