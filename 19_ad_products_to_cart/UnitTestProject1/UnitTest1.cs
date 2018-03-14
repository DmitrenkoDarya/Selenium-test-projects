using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
 
namespace UnitTestProject1
{
    internal class Page
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        public string path = TestContext.CurrentContext.TestDirectory + @"\MyTest.txt";

        public Page(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void write_into_file(string text, string path)
        {
            using (StreamWriter outputFile = File.AppendText(path))
            {
                outputFile.WriteLine(text);
                outputFile.Close();
            }
        }
    }

    internal class MainPage : Page
    {
        public MainPage(IWebDriver driver) : base(driver) { }

        internal MainPage Open()
        {
            driver.Url = "http://localhost/litecart/en/";
            return this;
        }

        internal void ClickOnDuck()
        {
            driver.FindElements(By.CssSelector(".link:nth-child(1)"))[0].Click();
        }
    }

    internal class InDuckPage : Page
    {
        public InDuckPage(IWebDriver driver) : base(driver) { }

        internal void AddToCart()
        {
            driver.FindElement(By.Name("add_cart_product")).Click();
        }

        internal string CounterValue()
        {
            return driver.FindElement(By.CssSelector(".quantity")).GetAttribute("textContent");
        }
    }

    internal class CartPage : Page
    {
        public CartPage(IWebDriver driver) : base(driver) { }

        internal void Open()
        {
            driver.FindElement(By.CssSelector(".link[href$=checkout]")).Click();
        }

        internal bool Delete()
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
                    return false;
                }

                if (wait.Until(ExpectedConditions.StalenessOf(table)) == true)
                    write_into_file("Удалили утку", path);
                else
                    write_into_file("Не смогли удалить утку", path);

                return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }
    }

    public class Application
    {
        private IWebDriver driver;

        private MainPage main_page;
        private InDuckPage in_duck_page;
        private CartPage cart_page;

        public Application()
        {
            driver = new ChromeDriver();
            main_page = new MainPage(driver);
            in_duck_page = new InDuckPage(driver);
            cart_page = new CartPage(driver);
        }

        public void Quit()
        {
            driver.Quit();
        }

        internal void ChoiseDuck()
        {
            main_page.Open().ClickOnDuck();
        }

        internal void AddDuckToCart()
        {
            string counter_before = in_duck_page.CounterValue();
            int i = 0;
            in_duck_page.AddToCart();

            while (counter_before == in_duck_page.CounterValue() && i < 10)
            {
                Thread.Sleep(200);
                i++;
            }
        }

        internal void DeleteFromCard()
        {
            cart_page.Open();

            while (cart_page.Delete() == true)
            {
                cart_page.Delete();
            }
        }
    }

    public class TestBase
    {
        public Application app;
        public string path = TestContext.CurrentContext.TestDirectory + @"\MyTest.txt";

        [SetUp]
        public void start()
        {
            app = new Application();
        }

        [TearDown]
        public void stop()
        {
            app.Quit();
            app = null;
        }
    }

    [TestFixture]
    public class MainTest : TestBase
    {
        [Test]
        public void Test()
        {
            using (FileStream fs = File.Create(path)) ;

            for (int i = 0; i < 3; i++)
            {
                app.ChoiseDuck();
                app.AddDuckToCart();
            }

            app.DeleteFromCard();
        }
    }
}
