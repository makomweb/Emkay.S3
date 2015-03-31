namespace Emkay.S3
{
    public class S3ClientFactory : IS3ClientFactory
    {
        public IS3Client Create(string key, string secret, string region)
        {
            return new S3Client(key, secret, region);
        }
    }
}