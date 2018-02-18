using System;
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
        static void WriteIntoFile(string text, string path)
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
            String login = "admin";
            String h1 = String.Empty;
            Random rand = new Random();
            int count_sections = 0;
            int count_presections = 0;
            string path = @"D:\MyTest.txt";
            using (FileStream fs = File.Create(path));
            
            driver.Url = "http://localhost/litecart/admin";

            driver.FindElement(By.Name("username")).SendKeys(login);
            Thread.Sleep(rand.Next(1000,2000));

            driver.FindElement(By.Name("password")).SendKeys(login);
            Thread.Sleep(rand.Next(1000, 2000));

            driver.FindElement(By.Name("login")).Click();
            Thread.Sleep(rand.Next(1000, 2000));

            //количество пунктов меню
            count_sections = driver.FindElements(By.Id("app-")).Count;
 
            for (int i = 0; i < count_sections; i++)
            {
                //кликнуть по текущему пункту меню
                driver.FindElements(By.CssSelector("#app-"))[i].Click();
                Thread.Sleep(rand.Next(1000, 2000));
                //получить количество заголовков и сразу перевести в строку, т.к. она нужна для отчёта
                h1 = driver.FindElements(By.TagName("h1")).Count.ToString();
                //сделать запись в файл-отчёт
                WriteIntoFile("Пункт " + (i + 1).ToString() + " - кликнули, h1: " + h1 + " шт -- OK", path);
                //попытаться кликнуть по подпунктам. Да, обработчик ошибок только на подпункты, я понимаю
                try
                {
                    //количество подпунктов в текущем пункте
                    count_presections = driver.FindElement(By.CssSelector("#app-.selected")).FindElements(By.CssSelector("li")).Count;
                    //если их больше 0, то делать всё то же, что и с пунктами. 
                    //Первый подпункт специально игнорируется, т.к. он тождественен пункту-родителю
                    if (count_presections > 0)
                    {
                        for (int j = 1; j < count_presections; j++)
                        {
                            driver.FindElement(By.CssSelector("#app-.selected")).FindElements(By.CssSelector("li"))[j].Click();
                            Thread.Sleep(rand.Next(1000, 2000));
                            h1 = driver.FindElements(By.TagName("h1")).Count.ToString();
                            WriteIntoFile("Пункт " + (i + 1).ToString() + ", Подпункт " + (j + 1).ToString() + " - кликнули, h1: " + h1 + " шт -- OK", path);
                        }
                    }
                }
                catch (StaleElementReferenceException ex)
                {
                    WriteIntoFile("Не нашли Подпункт в Пункте " + (i + 1).ToString() + " -- NOT OK!!!", path);
                }              
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
