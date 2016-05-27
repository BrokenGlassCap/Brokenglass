using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrokenGlassDomain.Entities;
using BrokenGlassDomain;
using Moq;
using Moq.Protected;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using BrokenGlassDomain.DataLayer;

namespace BrokenGlassTests.Domain
{
    [TestClass]
    public class UserFactoryTest
    {
        private TestContext testContextInstance;
        private Mock<IUnitOfWork> mockUnitOfWork;
        private List<User> stubUsers;
        private Mock<UserFactory> mockUserFactory;
        private Mock<UserManager<IdentityUser>> mockUserManager;
        private Mock<AuthDBContext> mockAuthContext;
        private Mock<IRepository<User>> mockUsersRepository;

        public UserFactoryTest()
        {
            InitStubUserList();

        }

        private void InitMockUnitOfWork()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUsersRepository = new Mock<IRepository<User>>();
            mockUnitOfWork.Setup(p => p.UserRepository).Returns(() => mockUsersRepository.Object);
        }
        private void InitMockAuthContext()
        {
            mockAuthContext = new Mock<AuthDBContext>();
        }
        private void InitMockUserFactory()
        {
            InitMockUserManager();
            mockUserFactory = new Mock<UserFactory>();
            mockUserFactory.Protected().SetupGet<UserManager<IdentityUser>>("UserManager").Returns(mockUserManager.Object);
        }
        private void InitMockUserManager()
        {
            mockUserManager = new Mock<UserManager<IdentityUser>>();
            //mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).Callback((IdentityUser i, string pass) => { Task.Factory.StartNew(() => { stubUsers.Add(new User() { Email = i.Email });}); });
            mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).Callback((IdentityUser i, string pass) => { stubUsers.Add(new User() { Email = i.Email }); });
        }

        private void InitStubUserList()
        {
            stubUsers = new List<User>()
            {
                 new User() {
                    Id = 1,
                    Email = "UserTest@mail.ru",
                    FirstName = "Bob",
                    LastName = "Martin",
                    UpdateAt = DateTime.UtcNow
                    
                },
                new User() {
                    Id = 2,
                    Email = "connorj@gmail.com",
                    FirstName = "John",
                    LastName = "Connor",
                    UpdateAt = DateTime.UtcNow
                }
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
    
        public async Task Registration()
        {
            var userFactory = UserFactory.GetUser(mockUserManager.Object, mockAuthContext.Object,mockUnitOfWork.Object);
            var userRegistrationModel = new UserRegistrationModel()
            {
                Email = "ivan@sberbank.ru",
                Password = "123123",
                ConfirmPassword = "123123"
            };

            await userFactory.RegistrationAsync(userRegistrationModel);
            mockUserManager.Verify(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()),Times.Once);
            Assert.IsTrue(stubUsers.Exists(e => e.Email == "ivan@sberbank.ru"));

        }
    }
}
