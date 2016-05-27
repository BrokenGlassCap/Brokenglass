using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrokenGlassTests.WebApi;
using BrokenGlassWebApp.Infostracture;
using System.Threading.Tasks;
using System.Linq;

namespace BrokenGlassTests.WebApi
{

    [TestClass]
    public class EmailServiceTest
    {
        public EmailServiceTest()
        {
            
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
        public async Task SendMessageAsync()
        {
            var emailService = new EmailService();
            var emailTo = "muzychenkoaa@gmail.com";

            var expectedObject = await emailService.SendMessageAsync(emailTo,"Confirmation Eamil","Confirm your account <a href=\"http:\\localhost:13602\\qeqeq\">Here</a>");

            Assert.IsTrue(expectedObject.To.ToList().Exists(e => e.Address == emailTo));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SendMessageAsyncWithNullEmail()
        {
            var emailService = new EmailService();
            string emailTo = null;

            var expectedObject = await emailService.SendMessageAsync(emailTo, "Confirmation Eamil", "Confirm your account <a href=\"http:\\localhost:13602\\qeqeq\">Here</a>");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SendMessageAsyncWithEmptyEmail()
        {
            var emailService = new EmailService();
            string emailTo = string.Empty;

            var expectedObject = await emailService.SendMessageAsync(emailTo, "Confirmation Eamil", "Confirm your account <a href=\"http:\\localhost:13602\\qeqeq\">Here</a>");
        }
    }
}
