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
    public class ClaimsApiTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IRepository<Claim>> mockGenricRepository;
        private List<Claim> stubClaims;
        private TestContext testContextInstance;

        public ClaimsApiTest()
        {
            stubClaims = new List<Claim>()
                                {
                                    new Claim() {Id = 1, AdressId = 1, ClaimStateId = 1, ServiceId = 1, CreateAt = DateTime.Now, UserId = 1 },
                                    new Claim() {Id = 2, AdressId = 2, ClaimStateId = 2, ServiceId = 2, CreateAt = DateTime.Now, UserId = 2 },
                                    new Claim() {Id = 3, AdressId = 3, ClaimStateId = 3, ServiceId = 3, CreateAt = DateTime.Now, UserId = 3  },
                                    new Claim() {Id = 4, AdressId = 4, ClaimStateId = 4, ServiceId = 4, CreateAt = DateTime.Now, UserId = 4 },
                                    new Claim() {Id = 5, AdressId = 5, ClaimStateId = 5, ServiceId = 5, CreateAt = DateTime.Now, UserId = 1}
                                };

            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockGenricRepository = new Mock<IRepository<Claim>>();

            mockGenricRepository.Setup(p => p.GetAll()).Returns(() => stubClaims);
            mockUnitOfWork.Setup(p => p.ClaimRepository).Returns(() => mockGenricRepository.Object);
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
        public void GetClaimsByUserId()
        {
            int userID = 1;
            var claimController = new ClaimsController(mockUnitOfWork.Object);

            var expectedObject = claimController.Get(userID);
            
            AssertUtils.IEnumerableAreEqual(expectedObject, stubClaims.FindAll(f => f.UserId == userID));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetClaimsByUserIdExpecteException()
        {
            int userID = -1;
            var claimController = new ClaimsController(mockUnitOfWork.Object);

            var expectedObject = claimController.Get(userID);

            
        }
    }
}
