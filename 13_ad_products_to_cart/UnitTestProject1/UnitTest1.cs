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

        //ф-ция добавления товара и проверки изменения счётчика корзины
        private void Add_to_cart(IWebDriver driver, int i, string path)
        {
            Random rand = new Random();

            driver.Url = "http://localhost/litecart/en/";
            Thread.Sleep(rand.Next(1200, 1500));

            driver.FindElements(By.CssSelector(".link:nth-child(1)"))[i].Click();
            Thread.Sleep(rand.Next(1200, 1500));

            string value_before = driver.FindElement(By.CssSelector(".quantity")).GetAttribute("textContent");

            driver.FindElement(By.Name("add_cart_product")).Click();
            Thread.Sleep(rand.Next(1200, 1500));

            string value_after = driver.FindElement(By.CssSelector(".quantity")).GetAttribute("textContent");

            if (value_before != value_after)
                write_into_file("Утка №" + (i + 1).ToString() + " добавлена в корзину.", path);
            else
                write_into_file("Утка №" + (i + 1).ToString() + " почему-то не добавлена в корзину. Наверное, она продана.", path);
        }

        private string Delete_from_cart(IWebDriver driver, string path)
        {
            Random rand = new Random();

            try
            {
                IWebElement table = driver.FindElement(By.CssSelector("#order_confirmation-wrapper"));
           
                try
                {
                    driver.FindElement(By.Name("remove_cart_item")).Click();
                }
                catch (NoSuchElementException ex)
                {
                    return "not ok";
                }

                if (wait.Until(ExpectedConditions.StalenessOf(table)) == true)
                    write_into_file("Удалили утку", path);
                else
                    write_into_file("Не смогли удалить утку", path);

                return "ok";
            }
            catch (NoSuchElementException ex)
            {
                return "not ok";
            }
        }

        [Test]
        public void FirstTest()
        {
            Random rand = new Random();

            string path = TestContext.CurrentContext.TestDirectory + @"\MyTest.txt";
            using (FileStream fs = File.Create(path));

            //добавление уток в корзину
            for (int i = 0; i < 3; i++)
                Add_to_cart(driver, i, path);

            //переход в корзину
            driver.FindElement(By.CssSelector(".link[href$=checkout]")).Click();
            Thread.Sleep(rand.Next(1000, 1500));

            //удаление из корзины
            string identify = "ok";

            do
            {
                identify = Delete_from_cart(driver, path);
            }
            while (identify == "ok");
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
