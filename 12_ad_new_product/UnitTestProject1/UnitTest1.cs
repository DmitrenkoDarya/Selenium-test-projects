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
        static void write_into_file(string text, string path)
        { 
            using (StreamWriter outputFile = File.AppendText(path))
            {
                outputFile.WriteLine(text);
                outputFile.Close();
            }
        }

        //ф-ция генерации случайных строк
        static string get_random_password(int pwdLength)
        {
            string alph = "qwertyuiopasdfghjklzxcvbnm";
            char[] letters = alph.ToCharArray();
            string s = String.Empty;
            Random rndGen = new Random();

            for (int i = 0; i < pwdLength; i++)
            {
                s += letters[rndGen.Next(letters.Length)].ToString();
            }
            return s;
        }

        [Test]
        public void FirstTest()
        {
            Random rand = new Random();

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

            driver.FindElement(By.CssSelector("[href$=catalog]")).Click();
            Thread.Sleep(rand.Next(1200, 1500));

            driver.FindElement(By.CssSelector("[href$=edit_product]")).Click();
            Thread.Sleep(rand.Next(1200, 1500));

            //заполнение вкладки General
            driver.FindElement(By.CssSelector("[type=radio]")).Click();
            Thread.Sleep(rand.Next(800, 1000));

            driver.FindElement(By.Name("name[en]")).Click();
            Thread.Sleep(rand.Next(800, 1000));
            driver.FindElement(By.Name("name[en]")).SendKeys("Punk Duck");
            Thread.Sleep(rand.Next(800, 1000));

            driver.FindElement(By.Name("code")).Click();
            Thread.Sleep(rand.Next(800, 1000));
            driver.FindElement(By.Name("code")).SendKeys(get_random_password(2) + rand.Next(100, 999).ToString());
            Thread.Sleep(rand.Next(800, 1000));

            driver.FindElement(By.CssSelector("[data-name^=Rubber]")).Click();
            Thread.Sleep(rand.Next(800, 1000));

            driver.FindElement(By.CssSelector("[data-name=Root]")).Click();
            Thread.Sleep(rand.Next(800, 1000));

            driver.FindElement(By.Name("quantity")).Clear();
            Thread.Sleep(rand.Next(800, 1000));
            driver.FindElement(By.Name("quantity")).SendKeys(rand.Next(10, 40).ToString());
            Thread.Sleep(rand.Next(800, 1000));

           // driver.FindElement(By.Name("new_images[]")).SendKeys(Directory.GetCurrentDirectory() + @"\duck.jpg");
            driver.FindElement(By.Name("new_images[]")).SendKeys(TestContext.CurrentContext.WorkDirectory + @"\duck.jpg");
            Thread.Sleep(rand.Next(1800, 2000));

            //вкладка Информация
            driver.FindElement(By.CssSelector("[href$=information]")).Click();
            Thread.Sleep(rand.Next(800, 1000));

            driver.FindElement(By.Name("manufacturer_id")).Click();
            Thread.Sleep(rand.Next(800, 1000));
            driver.FindElement(By.Name("manufacturer_id")).SendKeys(Keys.Down + Keys.Enter);
            Thread.Sleep(rand.Next(800, 1000));

            driver.FindElement(By.Name("short_description[en]")).SendKeys("Cool Duck for children who wont to be a rock-star " + @"\m/");
            Thread.Sleep(rand.Next(800, 1000));

            driver.FindElement(By.Name("description[en]")).SendKeys("While bathing the child, we recommend listening to the groups Sex Pistols, The Clash, Nirvana etc.");
            Thread.Sleep(rand.Next(800, 1000));

            //Вкладка Цены
            driver.FindElement(By.CssSelector("[href$=prices]")).Click();
            Thread.Sleep(rand.Next(800, 1000));

            driver.FindElement(By.Name("purchase_price")).Clear();
            Thread.Sleep(rand.Next(800, 1000));
            driver.FindElement(By.Name("purchase_price")).SendKeys(rand.Next(10, 30).ToString());
            Thread.Sleep(rand.Next(800, 1000));

            driver.FindElement(By.Name("prices[USD]")).Clear();
            Thread.Sleep(rand.Next(800, 1000));
            driver.FindElement(By.Name("prices[USD]")).SendKeys(rand.Next(30, 40).ToString());
            Thread.Sleep(rand.Next(800, 1000));

            //сохранить товар
            driver.FindElement(By.Name("save")).Click();
            Thread.Sleep(rand.Next(2800, 3000));

            //проверить наличие товара в каталоге
            for (int i = 0; i < driver.FindElements(By.CssSelector(".row a")).Count; i++)
            {
                if (driver.FindElements(By.CssSelector(".row a"))[i].GetAttribute("textContent") == "Punk Duck")
                {
                    write_into_file("Новая утка добавлена удачно и найдена в каталоге", path);
                    break;
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
