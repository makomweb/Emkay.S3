using Moq;

namespace Emkay.S3.Tests
{
    public abstract class S3TestsBase
    {
        private IS3Client _client;
        private ITaskLogger _loggerMock;

        protected const string Key = ""; // TODO edit your AWS S3 key here
        protected const string Secret = ""; // TODO edit your AWS S3 secret here
        protected const string Region = "us-east-1"; // TODO edit your AWS S3 region here
        protected const int RequestTimoutMilliseconds = 300000;

        protected IS3Client Client
        {
            get { return _client ?? (_client = ClientFactory.Create(Key, Secret, Region)); }
        }

        class S3ClientFactoryMock : IS3ClientFactory
        {
            private readonly string _key;
            private readonly string _secret;

            public S3ClientFactoryMock(string key, string secret)
            {
                _key = key;
                _secret = secret;
            }

            public IS3Client Create(string key, string secret, string region)
            {
                if (!string.IsNullOrEmpty(_key) && !string.IsNullOrEmpty(_secret))
                    return new S3Client(_key, _secret, region);
                return new Mock<IS3Client>().Object;
            }
        }

        protected IS3ClientFactory ClientFactory
        {
            get { return new S3ClientFactoryMock(Key, Secret); }
        }

        protected ITaskLogger LoggerMock
        {
            get { return _loggerMock ?? (_loggerMock = new Mock<ITaskLogger>().Object); }
        }

        protected void TearDownClient()
        {
            if (_client != null)
                _client.Dispose();
            _client = null;
        }
    }
}