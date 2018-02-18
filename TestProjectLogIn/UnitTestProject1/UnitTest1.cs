using System;
using System.Threading;
using System.Collections.Generic;
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
            int count_sections = 0;
            int count_presections = 0;
           // List<IWebElement> sections = new List<IWebElement>;

            driver.Url = "http://localhost/litecart/en/";
          
            driver.FindElement(By.Name("email")).SendKeys(log_pass);
            Thread.Sleep(r.Next(1500, 3000)); //рандомная пауза для эмуляции поведения пользователя

            driver.FindElement(By.Name("password")).SendKeys(log_pass);
            Thread.Sleep(r.Next(1500, 3000)); //рандомная пауза для эмуляции поведения пользователя

            driver.FindElement(By.Name("login")).Click();
            Thread.Sleep(r.Next(1500, 3000)); //рандомная пауза для эмуляции поведения пользователя

            List<WebElement> sections = new List<WebElement>();
            sections = driver.FindElements(By.Id("app-"));
            // Console.WriteLine(sections.ToString());
            driver.FindElement(By.Id("app-")).Click();
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
