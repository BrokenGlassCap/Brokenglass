using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain;
using BrokenGlassWebApp.Controllers.api;
using System.Web.Http;

namespace BrokenGlassTests.WebApi
{

    [TestClass]
    public class StateClaimTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IRepository<ClaimState>> mockGenricRepository;
        private List<ClaimState> stateCollection;

        public StateClaimTest()
        {
            stateCollection = new List<ClaimState>()
                                {
                                    new ClaimState() {Id = 1, Code = "STATE_NEW", UpdateAt = DateTime.Now.AddDays(1) },
                                    new ClaimState() {Id = 2, Code = "STATE_SENT", UpdateAt = DateTime.Now.AddDays(-11) },
                                    new ClaimState() {Id = 3, Code = "STATE_WORKING", UpdateAt = DateTime.Now.AddDays(2) },
                                    new ClaimState() {Id = 2, Code = "STATE_COMPLETED", UpdateAt = DateTime.Now.AddDays(0) }
                                };

            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockGenricRepository = new Mock<IRepository<ClaimState>>();

            mockGenricRepository.Setup(p => p.GetAll()).Returns(() => stateCollection);
            mockUnitOfWork.Setup(p => p.ClaimStateRepository).Returns(() => mockGenricRepository.Object);
        }

        private TestContext testContextInstance;

    
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
        public void GetAllClaimStates()
        {
            
            var controllerClaimStates = new ClaimStatesController(mockUnitOfWork.Object);

            var result = controllerClaimStates.Get();

            Assert.AreSame(result, stateCollection);
        }


        [TestMethod]
        public void GetClaimStateByCode()
        {
            string code = "STATE_NEW";
            var controllerClaimStates = new ClaimStatesController(mockUnitOfWork.Object);

            var result = controllerClaimStates.Get(code);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, stateCollection.Find(f => f.Code == code));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetClaimStateByCodeAndExpectException()
        {
            string code = "VeryWrongCode";
            var controllerClaimStates = new ClaimStatesController(mockUnitOfWork.Object);

            var result = controllerClaimStates.Get(code);

        }

        [TestMethod]
        public void GetClaimStateById()
        {
            int id = 1;
            var controllerClaimStates = new ClaimStatesController(mockUnitOfWork.Object);

            var result = controllerClaimStates.Get(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, stateCollection.Find(f => f.Id == id));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetClaimStateByIdAndExpectException()
        {
            int id = -99999;
            var controllerClaimStates = new ClaimStatesController(mockUnitOfWork.Object);

            var result = controllerClaimStates.Get(id);

        }

        [TestMethod]
        public void GetUpdatingClaimStateByLastUpdateDate()
        {
            DateTime lastUpdateDate = DateTime.Now;
            var actualObject = stateCollection.FindAll(f => f.UpdateAt >= lastUpdateDate);
            var controllerServices = new ClaimStatesController(mockUnitOfWork.Object);

            var expectedObject = controllerServices.Get(lastUpdateDate);

            Assert.IsNotNull(expectedObject);

            AssertUtils.IEnumerableAreEqual(expectedObject, actualObject);

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetUpdatingClaimStateByLastUpdateDateExpectException()
        {
            DateTime lastUpdateDate = DateTime.Now.AddDays(22);
            var actualObject = stateCollection.FindAll(f => f.UpdateAt >= lastUpdateDate);
            var controllerServices = new ClaimStatesController(mockUnitOfWork.Object);

            var expectedObject = controllerServices.Get(lastUpdateDate);

        }
    }
}
