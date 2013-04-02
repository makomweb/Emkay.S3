using NUnit.Framework;

namespace Emkay.S3.Tests
{
    [TestFixture]
    public class DeleteChildrenTests : S3TestsBase
    {
        private DeleteChildren _delete;
        private const string TestBucket = "";

        [SetUp]
        public void SetUp()
        {
            using (var items = new EnumerateChildren(RequestTimoutMilliseconds, LoggerMock)
                                {
                                    Key = Key,
                                    Secret = Secret,
                                    Client = ClientMock, // TODO comment this here for lazy instanciation
                                    Bucket = TestBucket
                                })
            {
                Assert.IsTrue(items.Execute());

                _delete = new DeleteChildren(RequestTimoutMilliseconds, LoggerMock)
                            {
                                Key = Key,
                                Secret = Secret,
                                Client = ClientMock, // TODO comment this here for lazy instanciation
                                Children = items.Children
                            };
            }
        }

        [TearDown]
        public void TearDown()
        {
            _delete.Dispose();
        }

        [Test]
        public void Execute_should_succeed()
        {
            Assert.IsTrue(_delete.Execute());
        }
    }
}
