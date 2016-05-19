using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrokenGlassWebApp.Controllers.api;
using Moq;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain;
using System.Web.Http;

namespace BrokenGlassTests.WebApi
{

    [TestClass]
    public class ServicesApiTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IRepository<Service>> mockGenricRepository;
        private List<Service> allServices;
        private TestContext testContextInstance;
        public ServicesApiTest()
        {
            allServices = new List<Service>()
                                {
                                    new Service() {Id = 1, Code = "BANNANA", UpdateAt = DateTime.Now.AddDays(1)},
                                    new Service() {Id = 2, Code = "ORANGE", UpdateAt = DateTime.Now.AddDays(-2) },
                                    new Service() {Id = 2, Code = "APPLE", UpdateAt = DateTime.Now.AddDays(-3) },
                                    new Service() {Id = 2, Code = "PEACHE", UpdateAt = DateTime.Now.AddDays(-4) },
                                    new Service() {Id = 2, Code = "GREIPFRUIT", UpdateAt = DateTime.Now.AddDays(2)}
                                };

            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockGenricRepository = new Mock<IRepository<Service>>();

            mockGenricRepository.Setup(p => p.GetAll()).Returns(() => allServices);
            mockUnitOfWork.Setup(p => p.ServiceRepository).Returns(() => mockGenricRepository.Object);
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
        public void GetAllServices()
        {
            
            var controller = new ServicesController(mockUnitOfWork.Object);

            var result = controller.Get();

            mockUnitOfWork.Verify(v => v.ServiceRepository,Times.Once);
            mockGenricRepository.Verify(v => v.GetAll(), Times.Once);
            Assert.AreSame(result, allServices);
        }

        [TestMethod]
        public void GetServiceByCode()
        {
            string queryCode = "ORANGE";
            var controller = new ServicesController(mockUnitOfWork.Object);

            var result = controller.Get(queryCode);

            mockUnitOfWork.Verify(v => v.ServiceRepository, Times.Once);
            mockGenricRepository.Verify(v => v.GetAll(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, allServices.Find(m => m.Code == queryCode));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetServiceByWrongCodeAndExpectException()
        {
            string queryCode = "WrongCode";
            var controller = new ServicesController(mockUnitOfWork.Object);

            var result = controller.Get(queryCode);

            mockUnitOfWork.Verify(v => v.ServiceRepository, Times.Once);
            mockGenricRepository.Verify(v => v.GetAll(), Times.Once);

        }

        [TestMethod]
        public void GetServiceById()
        {
            int id = 1;
            var controller = new ServicesController(mockUnitOfWork.Object);

            var result = controller.Get(id);

            mockUnitOfWork.Verify(v => v.ServiceRepository, Times.Once);
            mockGenricRepository.Verify(v => v.GetAll(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, allServices.Find(m => m.Id == id));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetServiceByWrongIdAndExpectException()
        {
            int id = -9999;
            var controller = new ServicesController(mockUnitOfWork.Object);

            var result = controller.Get(id);

            mockUnitOfWork.Verify(v => v.ServiceRepository, Times.Once);
            mockGenricRepository.Verify(v => v.GetAll(), Times.Once);

        }

        [TestMethod]
        public void GetUpdatingServicesByLastUpdateDate()
        {
            DateTime lastUpdateDate = DateTime.Now;
            var actualObject = allServices.FindAll(f => f.UpdateAt >= lastUpdateDate);
            var controllerServices = new ServicesController(mockUnitOfWork.Object);

            var expectedObject = controllerServices.Get(lastUpdateDate);

            Assert.IsNotNull(expectedObject);

            AssertUtils.IEnumerableAreEqual(expectedObject, actualObject);

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetUpdatingServicesByLastUpdateDateExpectException()
        {
            DateTime lastUpdateDate = DateTime.Now.AddDays(22);
            var actualObject = allServices.FindAll(f => f.UpdateAt >= lastUpdateDate);
            var controllerServices = new ServicesController(mockUnitOfWork.Object);

            var expectedObject = controllerServices.Get(lastUpdateDate);

        }

    }
}
