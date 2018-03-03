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
            int count_ducks = 0;
            Random rand = new Random();
            string path = Directory.GetCurrentDirectory() + @"\MyTest.txt";
            using (FileStream fs = File.Create(path));
            
            driver.Url = "http://localhost/litecart/en/";

            //количество уточек на стр
            count_ducks = driver.FindElements(By.CssSelector(".sticker")).Count;
 
            for (int i = 0; i < count_ducks; i++)
            {
                try
                {
                    if (driver.FindElements(By.CssSelector(".content .link"))[i].FindElements(By.CssSelector("[class^=sticker]")).Count == 1)
                        WriteIntoFile("У уточки №" + (i + 1).ToString() + " есть стикер\r\n", path);
                    else
                        WriteIntoFile("У уточки №" + (i + 1).ToString() + " НЕТ стикера\r\n", path);
                }
                catch (NoSuchElementException ex)
                {
                    WriteIntoFile("Нам так жаль, мы не смогли найти уточку №" + (i + 1).ToString(),path);
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
