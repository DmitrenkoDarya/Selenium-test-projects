using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace UnitTestProject1
{
    [TestFixture]
    public class MyFirstTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void FirstTest()
        {
            String log_pass = "admin";
            Random r = new Random();

            driver.Url = "http://localhost/litecart/en/";

            driver.FindElement(By.Name("email")).SendKeys(log_pass);
            Thread.Sleep(r.Next(1500, 3000)); //рандомная пауза для эмуляции поведения пользователя

            driver.FindElement(By.Name("password")).SendKeys(log_pass);
            Thread.Sleep(r.Next(1500, 3000)); //рандомная пауза для эмуляции поведения пользователя

            driver.FindElement(By.Name("login")).Click();
            Thread.Sleep(r.Next(1500, 3000)); //рандомная пауза для эмуляции поведения пользователя
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
