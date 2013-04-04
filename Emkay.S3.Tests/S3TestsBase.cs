using Moq;

namespace Emkay.S3.Tests
{
    public abstract class S3TestsBase
    {
        private IS3Client _clientS3;
        private IS3Client _clientMock;
        private ITaskLogger _loggerMock;

        protected const string Key = ""; // TODO edit your AWS S3 key here
        protected const string Secret = ""; // TODO edit your AWS S3 secret here
        protected const int RequestTimoutMilliseconds = 300000;

        protected IS3Client Client
        {
            get { return ClientMock; }  // TODO Change this for using a real respectively mocked S3 client instance!
        }

        protected IS3Client ClientS3
        {
            get { return _clientS3 ?? (_clientS3 = new S3Client(Key, Secret)); }
        }

        protected IS3Client ClientMock
        {
            get { return _clientMock ?? (_clientMock = new Mock<IS3Client>().Object); }
        }

        protected ITaskLogger LoggerMock
        {
            get { return _loggerMock ?? (_loggerMock = new Mock<ITaskLogger>().Object); }
        }
    }
}