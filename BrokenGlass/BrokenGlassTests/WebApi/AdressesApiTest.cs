using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain;
using Moq;
using System.Web.Http;
using BrokenGlassWebApp.Controllers.api;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;

namespace BrokenGlassTests.WebApi
{
    /// <summary>
    /// Сводное описание для AdressesApiTest
    /// </summary>
    [TestClass]
    public class AdressesApiTest
    {
        [TestClass]
        public class ServicesApiTest
        {
            private Mock<IUnitOfWork> mockUnitOfWork;
            private Mock<IRepository<Adress>> mockGenricRepository;
            private List<Adress> allAdresses;
            private TestContext testContextInstance;
            public ServicesApiTest()
            {
                allAdresses = new List<Adress>()
                                {
                                    new Adress() {Id = 1, AdressName = "BANNANA", UpdateAt = DateTime.Now.AddDays(1)},
                                    new Adress() {Id = 2, AdressName = "ORANGE", UpdateAt = DateTime.Now.AddDays(-2) },
                                    new Adress() {Id = 2, AdressName = "APPLE", UpdateAt = DateTime.Now.AddDays(-3) },
                                    new Adress() {Id = 2, AdressName = "PEACHE", UpdateAt = DateTime.Now.AddDays(-4) },
                                    new Adress() {Id = 2, AdressName = "GREIPFRUIT", UpdateAt = DateTime.Now.AddDays(2)}
                                };

                mockUnitOfWork = new Mock<IUnitOfWork>();
                mockGenricRepository = new Mock<IRepository<Adress>>();

                mockGenricRepository.Setup(p => p.GetAllAsync()).Returns(() => Task.Factory.StartNew(() => (IEnumerable<Adress>)allAdresses));
                mockGenricRepository.Setup(p => p.GetAll()).Returns(() => allAdresses);
                mockGenricRepository.Setup(p => p.FindAsync(It.IsAny<Expression<Func<Adress, bool>>>()))
                                    .Returns<Expression<Func<Adress, bool>>>(r => {
                                        return Task.Factory.StartNew(() => allAdresses.AsQueryable<Adress>().SingleOrDefault(r));
                                    });
                mockGenricRepository.Setup(p => p.FindAllAsync(It.IsAny<Expression<Func<Adress, bool>>>()))
                                    .Returns<Expression<Func<Adress, bool>>>(r => {
                                        return Task.Factory.StartNew(() => allAdresses.AsQueryable<Adress>().Where(r).AsEnumerable());
                                    });

                mockUnitOfWork.Setup(p => p.AdressRepository).Returns(() => mockGenricRepository.Object);
            }

            public TestContext TestContext
            {
                get
                {
                    return testContextInstance;
                }
                set
                {
                    testContextInstance = value;
                }
            }

            #region Дополнительные атрибуты тестирования
            //
            // При написании тестов можно использовать следующие дополнительные атрибуты:
            //
            // ClassInitialize используется для выполнения кода до запуска первого теста в классе
            // [ClassInitialize()]
            // public static void MyClassInitialize(TestContext testContext) { }
            //
            // ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
            // [ClassCleanup()]
            // public static void MyClassCleanup() { }
            //
            // TestInitialize используется для выполнения кода перед запуском каждого теста 
            // [TestInitialize()]
            // public void MyTestInitialize() { }
            //
            // TestCleanup используется для выполнения кода после завершения каждого теста
            // [TestCleanup()]
            // public void MyTestCleanup() { }
            //
            #endregion

            [TestMethod]
            public async Task GetAllAdresses()
            {

                var controller = new AdressesController(mockUnitOfWork.Object);

                var result = await controller.Get();

                mockUnitOfWork.Verify(v => v.AdressRepository, Times.Once);
                mockGenricRepository.Verify(v => v.GetAllAsync(), Times.Once);
                Assert.AreSame(result, allAdresses);
            }


            [TestMethod]
            public async Task GetAdressesById()
            {
                int id = 1;
                var controller = new AdressesController(mockUnitOfWork.Object);

                var result = await controller.Get(id);

                mockUnitOfWork.Verify(v => v.AdressRepository, Times.Once);
                mockGenricRepository.Verify(v => v.FindAsync(It.IsAny<Expression<Func<Adress, bool>>>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual(result, allAdresses.Find(m => m.Id == id));
            }

            [TestMethod]
            [ExpectedException(typeof(HttpResponseException))]
            public async Task GetAdressesByWrongIdAndExpectException()
            {
                int id = -9999;
                var controller = new AdressesController(mockUnitOfWork.Object);

                var result = await controller.Get(id);

                mockUnitOfWork.Verify(v => v.AdressRepository, Times.Once);
                mockGenricRepository.Verify(v => v.FindAsync(It.IsAny<Expression<Func<Adress, bool>>>()), Times.Once);

            }

            [TestMethod]
            public async Task GetUpdatingAdressesByLastUpdateDate()
            {
                DateTime lastUpdateDate = DateTime.Now;
                var actualObject = allAdresses.FindAll(f => f.UpdateAt >= lastUpdateDate);
                var controllerAdresses = new AdressesController(mockUnitOfWork.Object);

                var expectedObject = await controllerAdresses.Get(lastUpdateDate);

                Assert.IsNotNull(expectedObject);

                AssertUtils.IEnumerableAreEqual(expectedObject, actualObject);

            }

            [TestMethod]
            [ExpectedException(typeof(HttpResponseException))]
            public async Task GetUpdatingAdressesByLastUpdateDateExpectException()
            {
                DateTime lastUpdateDate = DateTime.Now.AddDays(22);
                var actualObject = allAdresses.FindAll(f => f.UpdateAt >= lastUpdateDate);
                var controllerServices = new AdressesController(mockUnitOfWork.Object);

                var expectedObject = await controllerServices.Get(lastUpdateDate);

            }

        }
    }
}
