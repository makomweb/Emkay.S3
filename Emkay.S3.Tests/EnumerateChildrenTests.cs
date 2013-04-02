using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class EnumerateChildrenTests : S3TestsBase
    {
        private EnumerateChildren _enumerate;
        protected const string Bucket = ""; // TODO edit your bucket name here

        [SetUp]
        public void SetUp()
        {
            _enumerate = new EnumerateChildren(RequestTimoutMilliseconds, LoggerMock)
                            {
                                Key = Key,
                                Secret = Secret,
                                Client = ClientMock, // TODO comment this here for lazy instanciation
                                Bucket = Bucket
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
