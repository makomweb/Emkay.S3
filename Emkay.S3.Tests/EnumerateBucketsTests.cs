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
            _enumerate = new EnumerateBuckets(RequestTimoutMilliseconds, LoggerMock)
                                {
                                    Key = Key,
                                    Secret = Secret,
                                    Client = ClientMock, // TODO comment this here for lazy instanciation
                                };
        }

        [TearDown]
        public void TearDown()
        {
            _enumerate.Dispose();
        }

        [Test]
        public void Execute_should_succeed()
        {
            Assert.IsTrue(_enumerate.Execute());
        }
    }
}