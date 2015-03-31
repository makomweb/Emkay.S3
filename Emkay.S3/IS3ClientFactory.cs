namespace Emkay.S3
{
    public interface IS3ClientFactory
    {
        IS3Client Create(string key, string secret, string region);
    }
}