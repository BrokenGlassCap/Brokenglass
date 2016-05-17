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
    /// <summary>
    /// Сводное описание для ServicesApiTest
    /// </summary>
    [TestClass]
    public class ServicesApiTest
    {
        public ServicesApiTest()
        {
            
        }
        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
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
            var allServices = new List<Service>()
                    {
                        new Service() {Id = 1 },
                        new Service() {Id = 2 }
                    };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockGenricRepository = new Mock<IRepository<Service>>();
            mockGenricRepository.Setup(p => p.GetAll()).Returns(() => allServices);
            mockUnitOfWork.Setup(p => p.ServiceRepository).Returns(() => mockGenricRepository.Object);
            var controller = new ServicesController();
            controller.SetDbContext(mockUnitOfWork.Object);

            var result = controller.Get();

            mockUnitOfWork.Verify(v => v.ServiceRepository,Times.Once);
            mockGenricRepository.Verify(v => v.GetAll(), Times.Once);
            Assert.AreSame(result, allServices);
        }
    }
}
