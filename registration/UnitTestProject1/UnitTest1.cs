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

        //ф-ция генерации случайных строк
        static string GetRandomPassword(int pwdLength)
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
            string name = GetRandomPassword(rand.Next(4, 10));
            string surname = GetRandomPassword(rand.Next(4, 10));
            string address1 = GetRandomPassword(rand.Next(4, 10)) + " " + rand.Next(1, 25).ToString();
            string city = GetRandomPassword(rand.Next(4, 10));
            string email = GetRandomPassword(rand.Next(4, 10)) + @"@" + GetRandomPassword(rand.Next(4, 7)) + ".com";
            string post_code = rand.Next(10000, 99999).ToString();
            string phone = "+1" + rand.Next(100000000, 999999999).ToString();
            string pass = GetRandomPassword(rand.Next(4, 10)) + rand.Next(2, 6).ToString();

            string path = @"D:\MyTest.txt";
            using (FileStream fs = File.Create(path));

            driver.Url = "http://localhost/litecart/en/";
            Thread.Sleep(rand.Next(1200, 2200));
            driver.FindElement(By.CssSelector("a[href$= create_account]")).Click();
            Thread.Sleep(rand.Next(1200, 2200));

            //сгенерировать имя, клик на поле и заполнение
            driver.FindElement(By.Name("firstname")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.Name("firstname")).SendKeys(name);
            Thread.Sleep(rand.Next(1200, 1500));

            //сгенерировать фамилию, клик на поле и заполнение
            driver.FindElement(By.Name("lastname")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.Name("lastname")).SendKeys(surname);
            Thread.Sleep(rand.Next(1200, 1500));

            //сгенерировать адрес, клик на поле и заполнение
            driver.FindElement(By.Name("address1")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.Name("address1")).SendKeys(address1);
            Thread.Sleep(rand.Next(1200, 1500));

            //клик на поле и заполнение почтового кода
            driver.FindElement(By.Name("postcode")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.Name("postcode")).SendKeys(post_code);
            Thread.Sleep(rand.Next(1200, 1500));

            //сгенерировать город, клик на поле и заполнение
            driver.FindElement(By.Name("city")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.Name("city")).SendKeys(city);
            Thread.Sleep(rand.Next(1200, 1500));

            //раазвернуть список и выбрать США
            driver.FindElement(By.CssSelector("span[id^=select2]")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.CssSelector(".select2-search__field")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.CssSelector(".select2-search__field")).SendKeys("United States" + Keys.Enter);
            Thread.Sleep(rand.Next(1200, 1500));

            //сгенерировать email, клик на поле и заполнение
            driver.FindElement(By.Name("email")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.Name("email")).SendKeys(email);
            Thread.Sleep(rand.Next(1200, 1500));

            //сгенерировать телефон, клик на поле и заполнение
            driver.FindElement(By.Name("phone")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.Name("phone")).SendKeys(phone);
            Thread.Sleep(rand.Next(1200, 1500));

            //сгенерировать пароль, клик на поле и заполнение
            driver.FindElement(By.Name("password")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.Name("password")).SendKeys(pass);
            Thread.Sleep(rand.Next(1200, 1500));

            //подтвердить пароль, клик на поле и заполнение
            driver.FindElement(By.Name("confirmed_password")).Click();
            Thread.Sleep(rand.Next(1200, 1500));
            driver.FindElement(By.Name("confirmed_password")).SendKeys(pass);
            Thread.Sleep(rand.Next(1200, 1500));

            //зарегистрироваться
            try
            {
                driver.FindElement(By.Name("create_account")).Click();
                Thread.Sleep(rand.Next(1200, 1500));
                WriteIntoFile("email: " + email, path);
                WriteIntoFile("password: " + pass, path);
                WriteIntoFile("name: " + name, path);
                WriteIntoFile("lastname: " + surname, path);
            }
            catch (NoSuchElementException ex)
            {
                WriteIntoFile("Не удалось нажать на кнопку Зарегистрироваться", path);
            }

            //выйти
            try
            {
                driver.FindElement(By.CssSelector("a[href$= logout]")).Click();
                Thread.Sleep(rand.Next(1200, 1500));
                WriteIntoFile("--- Logout ---", path);
            }
            catch (NoSuchElementException ex)
            {
                WriteIntoFile("Не удалось нажать на кнопку Logout", path);
            }

            //войти
            try
            {
                driver.FindElement(By.Name("email")).Click();
                Thread.Sleep(rand.Next(1000, 1200));
                driver.FindElement(By.Name("email")).SendKeys(email);
                Thread.Sleep(rand.Next(1000, 1200));

                driver.FindElement(By.Name("password")).Click();
                Thread.Sleep(rand.Next(1000, 1200));
                driver.FindElement(By.Name("password")).SendKeys(pass);
                Thread.Sleep(rand.Next(1000, 1200));

                driver.FindElement(By.Name("login")).Click();
                WriteIntoFile("--- Login ---", path);
                Thread.Sleep(rand.Next(2000, 2200));
            }
            catch (NoSuchElementException ex)
            {
                WriteIntoFile("Не удалось Войти", path);
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
