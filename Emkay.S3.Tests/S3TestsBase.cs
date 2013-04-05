using Moq;

namespace Emkay.S3.Tests
{
    public abstract class S3TestsBase
    {
        private IS3Client _client;
        private ITaskLogger _loggerMock;

        protected const string Key = ""; // TODO edit your AWS S3 key here
        protected const string Secret = ""; // TODO edit your AWS S3 secret here
        protected const int RequestTimoutMilliseconds = 300000;

        protected IS3Client Client
        {
            get { return _client ?? (_client = ClientFactory.Create(Key, Secret)); }
        }

        class S3ClientFactoryMock : IS3ClientFactory
        {
            public IS3Client Create(string key, string secret)
            {
#if false
                return new S3Client(key, secret);
#else
                return new Mock<IS3Client>().Object;
#endif
            }
        }

        protected IS3ClientFactory ClientFactory
        {
            get { return new S3ClientFactoryMock(); }
        }

        protected ITaskLogger LoggerMock
        {
            get { return _loggerMock ?? (_loggerMock = new Mock<ITaskLogger>().Object); }
        }
    }
}