using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain.ServiceUtils;

namespace BrokenGlassTests.Domain
{

    [TestClass]
    public class NinjectServiceTest
    {
        private TestContext testContextInstance;
        public NinjectServiceTest()
        {
            
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
        public void NinjectServiceSingeltonGetInstance()
        {
            var ninjectInstance = NinjectService.Instance;

            Assert.IsNotNull(ninjectInstance);
            Assert.IsInstanceOfType(ninjectInstance, typeof(NinjectService));

        }

        [TestMethod]
        public void ReturnInstanceTest()
        {
            var ninjectInstance = NinjectService.Instance;

            var instance = ninjectInstance.GetService<IUnitOfWork>();

            Assert.IsInstanceOfType(instance, typeof(UnitOfWork));
        }
    }
}
