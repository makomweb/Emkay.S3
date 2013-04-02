using Moq;

namespace Emkay.S3.Tests
{
    public abstract class S3TestsBase
    {
        private IS3Client _clientMock;
        private ITaskLogger _loggerMock;

        protected const string Key = ""; // TODO edit your AWS S3 key here
        protected const string Secret = ""; // TODO edit your AWS S3 secret here
        protected const int RequestTimoutMilliseconds = 300000;

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