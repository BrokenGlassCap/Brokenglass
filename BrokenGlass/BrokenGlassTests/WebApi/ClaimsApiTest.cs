using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain;
using BrokenGlassWebApp.Controllers.api;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Hosting;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace BrokenGlassTests.WebApi
{

    [TestClass]
    public class ClaimsApiTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IRepository<Claim>> mockGenricRepository;
        private List<Claim> stubClaims;
        private List<User> stubUsers;
        private TestContext testContextInstance;
        private Mock<IRepository<User>> mockUsersRepository;

        public ClaimsApiTest()
        {
            InitStubClaimsObject();
            InitStubUsersObject();
            InitMockObjects();

            SetupUserMockObject();

            SetupClaimObject();
            SetupUnitOfWorkMockObject();
        }

        private void SetupUnitOfWorkMockObject()
        {
            mockUnitOfWork.Setup(p => p.ClaimRepository).Returns(() => mockGenricRepository.Object);
            mockUnitOfWork.Setup(p => p.UserRepository).Returns(() => mockUsersRepository.Object);
        }

        private void SetupClaimObject()
        {
            mockGenricRepository.Setup(p => p.GetAll()).Returns(() => stubClaims);
            mockGenricRepository.Setup(p => p.GetAllAsync()).Returns(() => Task.Factory.StartNew(() => (IEnumerable<Claim>)stubUsers));

            mockGenricRepository.Setup(p => p.Insert(It.IsAny<Claim>())).Callback((Claim claim) => { stubClaims.Add(claim); });
            mockGenricRepository.Setup(p => p.GetByIdAsync(It.IsAny<int>())).Returns<int>(r =>
            {
                return Task.Factory.StartNew(() => stubClaims.AsQueryable<Claim>().SingleOrDefault(o => o.Id == r));
            });

            mockGenricRepository.Setup(p => p.FindAsync(It.IsAny<Expression<Func<Claim, bool>>>()))
                                   .Returns<Expression<Func<Claim, bool>>>(r =>
                                   {
                                       return Task.Factory.StartNew(() => stubClaims.AsQueryable<Claim>().SingleOrDefault(r));
                                   });
            mockGenricRepository.Setup(p => p.FindAllAsync(It.IsAny<Expression<Func<Claim, bool>>>()))
                                .Returns<Expression<Func<Claim, bool>>>(r =>
                                {
                                    return Task.Factory.StartNew(() => stubClaims.AsQueryable<Claim>().Where(r).AsEnumerable());
                                });
        }

        private void SetupUserMockObject()
        {
            mockUsersRepository.Setup(m => m.GetAll()).Returns(() => stubUsers);
            mockUsersRepository.Setup(p => p.GetAllAsync()).Returns(() => Task.Factory.StartNew(() => (IEnumerable<User>)stubUsers));

            mockUsersRepository.Setup(p => p.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .Returns<Expression<Func<User, bool>>>(r =>
                           {
                               return Task.Factory.StartNew(() => stubUsers.AsQueryable<User>().SingleOrDefault(r));
                           });
            mockUsersRepository.Setup(p => p.FindAllAsync(It.IsAny<Expression<Func<User, bool>>>()))
                                .Returns<Expression<Func<User, bool>>>(r =>
                                {
                                    return Task.Factory.StartNew(() => stubUsers.AsQueryable<User>().Where(r).AsEnumerable());
                                });
        }

        private void InitMockObjects()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockGenricRepository = new Mock<IRepository<Claim>>();
            mockUsersRepository = new Mock<IRepository<User>>();
        }

        private void InitStubUsersObject()
        {
            stubUsers = new List<User>(){
                new User() {
                    Id = 1,
                    Email = "UserTest@mail.ru",
                    FirstName = "Bob",
                    LastName = "Martin",
                    UpdateAt = DateTime.UtcNow,
                    Claim = stubClaims.FindAll(f => f.UserId == 1)
                },
                new User() {
                    Id = 2,
                    Email = "connorj@gmail.com",
                    FirstName = "John",
                    LastName = "Connor",
                    UpdateAt = DateTime.UtcNow,
                    Claim = stubClaims.FindAll(f => f.UserId == 2)
                }
            };
        }

        private void InitStubClaimsObject()
        {
            stubClaims = new List<Claim>()
            {
                new Claim() {Id = 1, AdressId = 1, ClaimStateId = 1, ServiceId = 1, CreateAt = DateTime.Now, UserId = 1, Photo = new List<Photo>() {
                    new Photo() {ClaimId =1, UpdateAt = DateTime.UtcNow } } },
                new Claim() {Id = 2, AdressId = 2, ClaimStateId = 2, ServiceId = 2, CreateAt = DateTime.Now, UserId = 2, Photo = new List<Photo>() {
                    new Photo() {ClaimId =2, UpdateAt = DateTime.UtcNow } } },
                new Claim() {Id = 3, AdressId = 3, ClaimStateId = 3, ServiceId = 3, CreateAt = DateTime.Now, UserId = 3, Photo = new List<Photo>() {
                    new Photo() {ClaimId =3, UpdateAt = DateTime.UtcNow } }  },
                new Claim() {Id = 4, AdressId = 4, ClaimStateId = 4, ServiceId = 4, CreateAt = DateTime.Now, UserId = 4, Photo = new List<Photo>() {
                    new Photo() {ClaimId =1, UpdateAt = DateTime.UtcNow } } },
                new Claim() {Id = 5, AdressId = 5, ClaimStateId = 5, ServiceId = 5, CreateAt = DateTime.Now, UserId = 1, Photo = new List<Photo>() {
                    new Photo() {ClaimId =1, UpdateAt = DateTime.UtcNow } }}
            };
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
        public async Task GetClaimsByUserId()
        {
            int userID = 1;
            var claimController = new ClaimsController(mockUnitOfWork.Object);

            var expectedObject = await claimController.Get(userID);
            
            AssertUtils.IEnumerableAreEqual(expectedObject, stubClaims.FindAll(f => f.UserId == userID));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task GetClaimsByUserIdExpecteException()
        {
            int userID = -1;
            var claimController = new ClaimsController(mockUnitOfWork.Object);

            var expectedObject = await claimController.Get(userID);
    
        }

        [TestMethod]
        public async Task GetClaimsByUserEmail()
        {
            var userEmail = "UserTest@mail.ru";
            var claimController = new ClaimsController(mockUnitOfWork.Object);

            var expectedObject = await claimController.Get(userEmail);

            AssertUtils.IEnumerableAreEqual(expectedObject, stubUsers.Find(f => f.Email == userEmail).Claim);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task GetClaimsByWrongUserEmailExpecteException()
        {
            var userEmail = "WrongEmailt@mail.ru";
            var claimController = new ClaimsController(mockUnitOfWork.Object);

            var expectedObject = await claimController.Get(userEmail);

        }

        [TestMethod]
        public async Task GetClaimsByUserEmailWithoutPhotos()
        {
            var userEmail = "UserTest@mail.ru";
            var claimController = new ClaimsController(mockUnitOfWork.Object);

            var expectedObject = await claimController.Get(userEmail);
            var enumeratroObject =  expectedObject.GetEnumerator();
            enumeratroObject.MoveNext();
            Assert.AreEqual(enumeratroObject.Current.Photo.Count,0);
        }

        [TestMethod]
        public async Task GetClaimById()
        {
            var claimId = 1;
            var controller = new ClaimsController(mockUnitOfWork.Object);

            var expectedObject = await controller.GetClaimById(claimId);

            Assert.AreEqual(expectedObject, stubClaims.Find(f => f.Id == claimId));

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task GetClaimByIdExpectedException()
        {
            var claimId = -1;
            var controller = new ClaimsController(mockUnitOfWork.Object);

            var expectedObject = await controller.GetClaimById(claimId);
        }

        public async Task PostClaim()
        {
            var controller = new ClaimsController(mockUnitOfWork.Object);
            var claim = new Claim()
            {
                Id = 10,
                AdressId = 1,
                ClaimStateId = 1,
                ServiceId = 1,
                CreateAt = DateTime.Now,
                UserId = 1,
                Photo = new List<Photo>() { new Photo() { ClaimId = 10, UpdateAt = DateTime.UtcNow } }
            };
            //TestUtils.SetApiControllerContextAndRequest(controller, "http://localhost/api/Claims");
            await controller.PostClaim(claim);

            Assert.IsTrue(stubClaims.Contains(claim));
        }


    }
}
