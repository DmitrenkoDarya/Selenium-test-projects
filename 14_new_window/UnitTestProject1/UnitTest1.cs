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

            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            Thread.Sleep(rand.Next(1200, 1500));

            //получить кол-во кликабеьных ссылок для редактирования и кликнуть по любой из них
            count = driver.FindElements(By.CssSelector(".row [href*=edit_country]")).Count;
            driver.FindElements(By.CssSelector(".row [href*=edit_country]"))[rand.Next(0, count)].Click();
            Thread.Sleep(rand.Next(1200, 1500));

            write_into_file("country: " + driver.FindElement(By.Name("name")).GetAttribute("value"), path);
            write_into_file("\r\nlinks:", path);

            string main_window = driver.CurrentWindowHandle;
          
            ICollection<string> identify = new List<string>();
            
            for (int i = 0; i < driver.FindElements(By.CssSelector(".fa.fa-external-link")).Count; i++)
            {
                driver.FindElements(By.CssSelector(".fa.fa-external-link"))[i].Click();
                //массив для конвертации коллекции
                string[] ident = new string[2];
                //коллекция открытых окон
                identify = driver.WindowHandles;
                //конвертация
                identify.CopyTo(ident, 0);

                //перебираем список и переходим в окно, которое не главное
                for (int j = 0; j < 2; j++)
                {
                    if (ident[j] != main_window)
                    {
                        try
                        {
                            driver.SwitchTo().Window(ident[j]);
                            Thread.Sleep(rand.Next(1700, 1900));
                            write_into_file(driver.Url.ToString(), path);
                            driver.Close();
                            driver.SwitchTo().Window(main_window);
                        }
                        catch (NoSuchWindowException ex)
                        {
                            write_into_file("Не смогли найти окно", path);
                        }
                        Thread.Sleep(rand.Next(1200, 1500));
                        break;
                    }
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
