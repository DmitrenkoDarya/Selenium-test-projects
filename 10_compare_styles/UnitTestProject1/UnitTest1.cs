using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace UnitTestProject1
{
    [TestFixture]
    public class MyFirstTest
    {
        public IWebDriver driver;
        public WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //driver2 = new InternetExplorerDriver();
            //wait2 = new WebDriverWait(driver1, TimeSpan.FromSeconds(10));
            Random rand = new Random();            
        }

        //ф-ция дозаписи в файл
        public static void write_into_file(string text, string path)
        { 
            using (StreamWriter outputFile = File.AppendText(path))
            {
                outputFile.WriteLine(text);
                outputFile.Close();
            }
        }

        //какой цвет?
        public string What_color (string rgba)
        {
            string regular_red = @"(?<=\().*?(?=,)";
            string regular_green = @"(?<=,\ ).*?(?=,\ \d+,)";
            string regular_blue = @"(?<=,\ \d+,\ ).*?(?=,\ \d+\))";

            string red = Regex.Match(rgba, regular_red).Value.ToString();
            string green = Regex.Match(rgba, regular_green).Value.ToString();
            string blue = Regex.Match(rgba, regular_blue).Value.ToString();

            if (red != "0" && green == "0" && blue == "0")
                return "red";
            else
                if (red == "0" && green != "0" && blue == "0")
                return "green";
            else
                if (red == "0" && green == "0" && blue != "0")
                return "blue";
            else
                if (red == green && green == blue)
                return "gray";
            else
                return "color is diff";
        }

        //сравнение атрибутов
        public void compare_attr(IWebDriver driver, string path)
        {
            Random rand = new Random();
            string result = String.Empty;

            driver.Url = "http://localhost/litecart/en/";
            Thread.Sleep(rand.Next(1200, 1500));

            IWebElement duck = driver.FindElement(By.CssSelector("#box-campaigns .link"));

            string name_main = duck.FindElement(By.CssSelector(".name")).GetAttribute("textContent");
            string price_main = duck.FindElement(By.CssSelector(".regular-price")).GetAttribute("textContent");
            string sale_price_main = duck.FindElement(By.CssSelector(".campaign-price")).GetAttribute("textContent");

            int price_height = Convert.ToInt32(duck.FindElement(By.CssSelector(".regular-price")).GetAttribute("offsetHeight"));
            int sale_price_height = Convert.ToInt32(duck.FindElement(By.CssSelector(".campaign-price")).GetAttribute("offsetHeight"));

            bool height;

            if (price_height < sale_price_height)
                height = true;
            else
                height = false;

            string clr_sale_price_main = duck.FindElement(By.CssSelector(".campaign-price")).GetCssValue("color");

            clr_sale_price_main = What_color(clr_sale_price_main);
            string clr_price_main = duck.FindElement(By.CssSelector(".regular-price")).GetCssValue("color");
            clr_price_main = What_color(clr_price_main);

            bool strike_price;

            if (duck.FindElement(By.CssSelector(".regular-price")).TagName == "s" || duck.FindElement(By.CssSelector(".regular-price")).TagName == "strike")
                strike_price = true;
            else
                strike_price = false;

            bool strong_price;

            if (duck.FindElement(By.CssSelector(".campaign-price")).TagName == "b" || duck.FindElement(By.CssSelector(".campaign-price")).TagName == "strong")
                strong_price = true;
            else
                strong_price = false;

            //перейти в товар
            duck.Click();
            Thread.Sleep(rand.Next(1200, 1600));

            //определение названия на стр.товара, сравнение с названием в карточке товара и запись в файл
            string name_full = driver.FindElement(By.CssSelector("h1.title")).GetAttribute("textContent");

            if (name_main == name_full)
                result = "Названия товара совпадают: " + name_main;
            else
                result = "Названия товара не совпадают: " + name_main + " - " + name_full;

            write_into_file(result, path);

            //определение цены без скидки на стр.товара, сравнение с ценой в карточке товара и запись в файл
            string prise_full = driver.FindElement(By.CssSelector(".regular-price")).GetAttribute("textContent");

            if (price_main == prise_full)
                result = "Основная цена товара совпадает: " + price_main;
            else
                result = "Основная цена товара не совпадает: " + price_main + " - " + prise_full;

            write_into_file(result, path);

            //определение цены со скидкой на стр.товара, сравнение со скидочной ценой в карточке товара и запись в файл
            string sale_price_full = driver.FindElement(By.CssSelector(".campaign-price")).GetAttribute("textContent");

            if (sale_price_main == sale_price_full)
                result = "Скидочная цена товара совпадает: " + sale_price_main ;
            else
                result = "Скидчная цена товара не совпадает: " + sale_price_main + " - " + sale_price_full;

            write_into_file(result, path);

            //определение цветов цен на ст ранице товара
            string clr_sale_price_full = driver.FindElement(By.CssSelector(".campaign-price")).GetCssValue("color");
            clr_sale_price_full = What_color(clr_sale_price_full);
            string clr_price_full = driver.FindElement(By.CssSelector(".regular-price")).GetCssValue("color");
            clr_price_full = What_color(clr_price_full);

            //сравнение между собой цвета цен со скидкой и запись в файл
            if (clr_price_main == clr_price_full)
                result = "Цвет скидочной цены совпадает: " + clr_price_main;
            else
                result = "Цвет скидочной цены не совпадает: " + clr_price_main + " - " + clr_price_full;

            write_into_file(result, path);

            //сравнение между собой цвета цен без скидки и запись в файл
            if (clr_sale_price_main == clr_sale_price_full)
                result = "Цвет основной цены совпадает: " + clr_sale_price_main;
            else
                result = "Цвет основной цены не совпадает: " + clr_sale_price_main + " - " + clr_sale_price_full;

            write_into_file(result, path);

            //вывод в файл форматирования текста цен на карточке товара из списка товаров
            if (strong_price == true)
                write_into_file("\r\nШрифт скидочной цены в карточке товара жирный", path);
            else
                write_into_file("Шрифт скидочной цены в карточке товара не жирный", path);

            if (strike_price == true)
                write_into_file("Шрифт обычной цены в карточке товара зачёркнутый", path);
            else
                write_into_file("Шрифт обычной цены в карточке товара не зачёркнутый", path);

            if (height == true)
                write_into_file("Шрифт обычной цены в карточке товара меньше, чем у скидочной", path);
            else
                write_into_file("Шрифт обычной цены в карточке товара не меньше, чем у скидочной\r\n", path);

            //переопрееление параметров форматирования текста цен для страницы товара, сравнение, запись в файл
            price_height = Convert.ToInt32(driver.FindElement(By.CssSelector(".regular-price")).GetAttribute("offsetHeight"));
            sale_price_height = Convert.ToInt32(driver.FindElement(By.CssSelector(".campaign-price")).GetAttribute("offsetHeight"));

            if (price_height < sale_price_height)
                height = true;
            else
                height = false;

            if (driver.FindElement(By.CssSelector(".regular-price")).TagName == "s" || driver.FindElement(By.CssSelector(".regular-price")).TagName == "strike")
                strike_price = true;
            else
                strike_price = false;
            
            if (driver.FindElement(By.CssSelector(".campaign-price")).TagName == "b" || driver.FindElement(By.CssSelector(".campaign-price")).TagName == "strong")
                strong_price = true;
            else
                strong_price = false;


            if (strong_price == true)
                write_into_file("\r\nШрифт скидочной цены на странице товара жирный", path);
            else
                write_into_file("Шрифт скидочной цены на странице товара не жирный", path);

            if (strike_price == true)
                write_into_file("Шрифт обычной цены на странице товара зачёркнутый", path);
            else
                write_into_file("Шрифт обычной цены на странице товара не зачёркнутый", path);

            if (height == true)
                write_into_file("Шрифт обычной цены на странице товара меньше, чем у скидочной", path);
            else
                write_into_file("Шрифт обычной цены на странице товара не меньше, чем у скидочной", path);
            //write_into_file(clr_sale_price_main + " " + clr_price_main, path);
        }
              
        [Test]
        public void CHROME_Test()
        {
            string path = TestContext.CurrentContext.TestDirectory + @"\MyTest_CHROME.txt";
            using (FileStream fs = File.Create(path)) ;

            write_into_file("chrome", path);
            compare_attr(driver, path);    
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }        
    }

    public class MySecondTest : MyFirstTest
    {
        [SetUp]
        new public void start()
        {
            driver = new InternetExplorerDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Random rand = new Random();
        }

        [Test]
        public void IE_Test()
        {
            string path = TestContext.CurrentContext.TestDirectory + @"\MyTest_IE.txt";
            using (FileStream fs = File.Create(path)) ;

            write_into_file("IE", path);
            compare_attr(driver, path);
        }

        [TearDown]
        new public void Stop()
        {
            driver.Quit();
            driver = null;
        } 
    }
}
