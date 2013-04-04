using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class EnumerateChildrenTests : S3TestsBase
    {
        private EnumerateChildren _enumerate;
        protected const string Bucket = ""; // TODO edit your bucket name here
        protected const string Prefix = ""; // TODO edit your prefix here

        [SetUp]
        public void SetUp()
        {
            _enumerate = new EnumerateChildren(RequestTimoutMilliseconds, LoggerMock)
                            {
                                Key = Key,
                                Secret = Secret,
                                Client = ClientMock, // TODO comment this here for lazy instanciation
                                Bucket = Bucket
                                Prefix = Prefix
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
