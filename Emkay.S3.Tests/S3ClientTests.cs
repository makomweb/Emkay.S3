using System;
using System.Diagnostics;
using System.Net;
using Amazon;
using Amazon.S3;
using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class S3ClientTests : S3TestsBase
    {
        protected const string Bucket = ""; // TODO edit your bucket name here

        [Test]
        //Note: passing this test requires AWSSDK with the following patch added
        //Pull request: https://github.com/aws/aws-sdk-net/pull/79
        //diff --git a/AWSSDK/Amazon.S3/Util/AmazonS3Util.cs b/AWSSDK/Amazon.S3/Util/AmazonS3Util.cs
        //index bfe9a16..7b4438b 100644
        //--- a/AWSSDK/Amazon.S3/Util/AmazonS3Util.cs
        //+++ b/AWSSDK/Amazon.S3/Util/AmazonS3Util.cs
        //@@ -468,27 +468,29 @@ namespace Amazon.S3.Util
 
        //             HttpWebRequest httpRequest = WebRequest.Create(url) as HttpWebRequest;
        //             httpRequest.Method = "HEAD";
        //+            httpRequest.KeepAlive = false;
        //             AmazonS3Client concreteClient = s3Client as AmazonS3Client;
        //             if (concreteClient != null)
        //             {
        //                 concreteClient.ConfigureProxy(httpRequest);
        //             }
 
        //+            HttpWebResponse httpResponse = null;
        //             try
        //             {
        //-                HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse;
        //+                httpResponse = httpRequest.GetResponse() as HttpWebResponse;
        //                 // If all went well, the bucket was found!
        //                 return true;
        //             }
        //             catch (WebException we)
        //-            {
        //+            {                
        //                 using (HttpWebResponse errorResponse = we.Response as HttpWebResponse)
        //                 {
        //                     if (errorResponse != null)
        //                     {
        //                         HttpStatusCode code = errorResponse.StatusCode;
        //                         return code != HttpStatusCode.NotFound &&
        //-                            code != HttpStatusCode.BadRequest;
        //+                               code != HttpStatusCode.BadRequest;
        //                     }
 
        //                     // The Error Response is null which is indicative of either
        //@@ -496,6 +498,11 @@ namespace Amazon.S3.Util
        //                     return false;
        //                 }
        //             }
        //+            finally
        //+            {
        //+                if (httpResponse != null)
        //+                    httpResponse.Close();
        //+            }
        //         }
 
        //          /// <summary>
        public void EnsureBucketExists_DoesNot_TimeOut()
        {
            var timer = Stopwatch.StartNew();
            try
            {
                for (var i = 0; i < 5; i++)
                {
                    ((S3Client) Client).EnsureBucketExists(Bucket);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                timer.Stop();
                var elapsedTime = timer.Elapsed.TotalMilliseconds;

                Assert.IsTrue(elapsedTime < (WebRequest.Create("http://test.com") as HttpWebRequest).Timeout);                
            }
        }
    }
}