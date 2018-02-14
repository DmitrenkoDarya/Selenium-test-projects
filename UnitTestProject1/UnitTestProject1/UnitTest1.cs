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
            driver.Url = "http://www.google.com/";
            driver.FindElement(By.Name("q")).SendKeys("webdriver");

            //в моей версии Google нет кнопки "поиск", нужно 
            //нажать Enter => кнопка "Поиск" не находится по заданным условиям,
            //и тест не успешен. Поэтому поместила действия, которые могут быть не 
            //выполнены, в обработку исключений. В качестве альтернативы - ввод Enter
            try {
                driver.FindElement(By.Name("btnG")).Click();
            }
            catch (ElementNotVisibleException e) {
                driver.FindElement(By.Name("q")).SendKeys("\n");
            }

            wait.Until(ExpectedConditions.TitleIs("webdriver - Поиск в Google"));
            Thread.Sleep(2500); //пауза, чтобы самостоятельно увидеть title вкладки
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
