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
        string login = "admin";
        //список под хранение гео
        List<string> country = new List<string>();
        //список под хранение адресов стран, у которых больше 0 гео
        List<string> urls = new List<string>();
        //счётчик неудачных сравнений
        int wrong_counter = 0;
        Random rand = new Random();
        int count = 0;

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

        //ф-ция логина
        static void login_admin(string login, IWebDriver driver, Random rand)
        {
            driver.Url = "http://localhost/litecart/admin/";
            Thread.Sleep(rand.Next(1200, 1500));

            //логинимся
            driver.FindElement(By.Name("username")).SendKeys(login);
            Thread.Sleep(rand.Next(1000, 2000));

            driver.FindElement(By.Name("password")).SendKeys(login);
            Thread.Sleep(rand.Next(1000, 2000));

            driver.FindElement(By.Name("login")).Click();
            Thread.Sleep(rand.Next(1000, 2000));
        }

        [Test]
        public void FirstTest()
        {       
            string path = Directory.GetCurrentDirectory() + @"\MyTest1.txt";

            using (FileStream fs = File.Create(path));

            login_admin(login, driver, rand);

            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            Thread.Sleep(rand.Next(1200, 1500));            
            
            //количество строк в таблице
            int count_links = driver.FindElements(By.CssSelector("tr.row")).Count;

            for (int i = 0; i < count_links; i++)
            {
                //добавить в список Гео названия стран
                country.Add(driver.FindElements(By.CssSelector("tr.row td:nth-child(5)"))[i].GetAttribute("textContent"));

                //параллельно проверять кол-во зон. Если оно отлично от нуля - добавлять ссылку на страну в др.список
                if (driver.FindElements(By.CssSelector("tr.row td:nth-child(6)"))[i].GetAttribute("textContent") != "0")
                {
                    urls.Add(driver.FindElements(By.CssSelector("tr.row td:nth-child(5) a"))[i].GetAttribute("href"));
                }
            }            

            //попарное сравнение стран "предыдущий со следующим"
            for (int i = 0; i < country.Count - 1; i++)
            {                
                if (String.Compare(country[i], country[i + 1]) != -1)
                {
                    write_into_file("Страны " + country[i] + " и " + country[i + 1] + " в неправильном порядке", path);
                    wrong_counter++;
                }
            }

            if (wrong_counter == 0)
                write_into_file("Страны в правильном порядке\r\n_________\r\n", path);

            //кол-во ссылок на страны, у которых есть зоны
            int urls_count = urls.Count;

            //если список ссылок не пустой, начать проверку зон внутри стран
            if (urls_count > 0)
            {
                write_into_file("Количество стран с гео-зонами: " + urls_count.ToString() + " \r\n_________\r\n", path);
           
                for (int i = 0; i < urls_count; i++)
                {
                    //очистим список гео и снова используем
                    country.Clear();
                    driver.Url = urls[i];
                    Thread.Sleep(rand.Next(1200, 1700));

                    //кол-во зон в табличке
                    int zones_count = driver.FindElements(By.CssSelector("#table-zones [name*=name]")).Count - 1;
                    write_into_file(zones_count.ToString() + " zones", path);
                    //обнулим неудачные сравнения и снова используем
                    wrong_counter = 0;

                    //добавим в список гео названия зон
                    for (int j = 0; j < zones_count; j++)
                        country.Add(driver.FindElements(By.CssSelector("#table-zones [name*=name]"))[j].GetAttribute("value"));

                    //сравним зоны попарно
                    for (int j = 0; j < zones_count - 1; j++)
                    {
                        if (String.Compare(country[j], country[j + 1]) != -1)
                        {
                            write_into_file("Геозоны " + country[i] + " и " + country[i + 1] + " в неправильном порядке", path);
                            wrong_counter++;
                        }   
                        else
                            write_into_file(country[j] + " + " + country[j + 1], path);
                    }
                   if (wrong_counter == 0)
                       write_into_file("Геозоны в стране №" + (i + 1).ToString() + " в правильном порядке\r\n_________\r\n", path);
                }
            }           
        }
        
        [Test]
        public void SecondTest()
        {
            string path_2 = Directory.GetCurrentDirectory() + @"\MyTest2.txt";

            using (FileStream fs = File.Create(path_2));

            login_admin(login, driver, rand);

            driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";
            Thread.Sleep(rand.Next(1200, 1900));

            count = driver.FindElements(By.CssSelector("a[href*=geo_zone_id]")).Count;

            for (int i = 0; i < count; i = i + 2)
                urls.Add(driver.FindElements(By.CssSelector("a[href*=geo_zone_id]"))[i].GetAttribute("href"));

            for (int i = 0; i < urls.Count; i++)
            {
                driver.Url = urls[i];
                Thread.Sleep(rand.Next(1200, 1900));
                country.Clear();

                count = driver.FindElements(By.CssSelector("[name*=zone_code] [selected=selected]")).Count;

                for (int j = 0; j < count; j++)
                    country.Add(driver.FindElements(By.CssSelector("[name*=zone_code] [selected=selected]"))[j].GetAttribute("textContent"));

                //обнулим неудачные сравнения и снова используем
                wrong_counter = 0;

                //сравним зоны попарно
                for (int j = 0; j < country.Count - 1; j++)
                {
                    if (String.Compare(country[j], country[j + 1]) != -1)
                    {
                        write_into_file("Геозоны " + country[j] + " и " + country[j + 1] + " в неправильном порядке", path_2);
                        wrong_counter++;
                    }
                    else
                        write_into_file(country[j] + " + " + country[j + 1], path_2);
                }
                if (wrong_counter == 0)
                    write_into_file("Геозоны в стране №" + (i + 1).ToString() + " в правильном порядке\r\n_________\r\n", path_2);
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
