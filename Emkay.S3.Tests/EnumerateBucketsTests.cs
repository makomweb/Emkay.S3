using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class EnumerateBucketsTests : S3TestsBase
    {
        private EnumerateBuckets _enumerate;

        [SetUp]
        public void SetUp()
        {
            _enumerate = new EnumerateBuckets(ClientFactory, RequestTimoutMilliseconds, LoggerMock);
        }

        [TearDown]
        public void TearDown()
        {
            if (_enumerate != null)
                _enumerate.Dispose();
            _enumerate = null;
        }

        [Test]
        public void Execute_should_succeed()
        {
            Assert.IsTrue(_enumerate.Execute());
            Assert.IsNotNull(_enumerate.Buckets);
        }
    }
}