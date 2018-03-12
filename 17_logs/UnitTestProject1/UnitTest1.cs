using System;
using System.Collections.Generic;
using System.IO;
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

        //ф-ция дозаписи в файл
        static void write_into_file(string text, string path)
        { 
            using (StreamWriter outputFile = File.AppendText(path))
            {
                outputFile.WriteLine(text);
                outputFile.Close();
            }
        }
               
        [Test]
        public void FirstTest()
        {
            Random rand = new Random();
            int count = 0;

            string path = TestContext.CurrentContext.TestDirectory + @"\MyTest.txt";
            using (FileStream fs = File.Create(path));

            string login = "admin";

            driver.Url = "http://localhost/litecart/admin/";
            Thread.Sleep(rand.Next(1200, 1500));

            //логинимся
            driver.FindElement(By.Name("username")).SendKeys(login);
            Thread.Sleep(rand.Next(1000, 2000));

            driver.FindElement(By.Name("password")).SendKeys(login);
            Thread.Sleep(rand.Next(1000, 2000));

            driver.FindElement(By.Name("login")).Click();
            Thread.Sleep(rand.Next(1000, 2000));

            driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1";
            Thread.Sleep(rand.Next(1200, 1500));
                        
            for (int i = 0; i < driver.FindElements(By.CssSelector("[href*=product_id]")).Count; i = i + 2)
            {
                driver.FindElements(By.CssSelector("[href*=product_id]"))[i].Click();

                write_into_file(driver.Url + "\r\nlogs:", path);

                foreach (LogEntry l in driver.Manage().Logs.GetLog("browser"))
                {
                    write_into_file(l.ToString(), path);
                   // Console.WriteLine(l);
                }

                write_into_file("_________\r\n", path);

                Thread.Sleep(rand.Next(800, 1000));
                driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1";
                Thread.Sleep(rand.Next(800, 1000));
            }           
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
