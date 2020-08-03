using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace tweet_pi_number_bot.Bot
{
    class TwitterBot
    {
        public string LoginUrl { get; set; }
        public IWebDriver Driver { get; set; }
        public List<string> Tweets { get; set; }

        public void Login()
        {
            Driver.Navigate().GoToUrl(LoginUrl);

            //Gets the username input
            var usernameInput = Driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/main/div/div/div[1]/form/div/div[1]/label/div/div[2]/div/input"));
            //Types the username input
            usernameInput.SendKeys(ConfigurationManager.AppSettings["username"]);

            //Gets the password input
            var passwordInput = Driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/main/div/div/div[1]/form/div/div[2]/label/div/div[2]/div/input"));
            //Types the password
            passwordInput.SendKeys(ConfigurationManager.AppSettings["password"]);

            //Gets the "Login" button
            var loginButton = Driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/main/div/div/div[1]/form/div/div[3]/div/div"));
            loginButton.Click();
        }

        public void StartThread()
        {
            var tweetContent = "(2,5 horas pra eu programar, 2 segundos pra você curtir)\n(https://github.com/gabriel21henrique/tweet-pi-number-bot)\n\nAs 6718 primeiras casas decimais do número Pi, a thread";

            //Gets the "Tweet" button
            var tweetButton = Driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/header/div/div/div/div[1]/div[3]/a/div"));
            tweetButton.Click();

            //Gets the Tweet input
            var tweetInput = Driver.FindElement(By.XPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div[3]/div/div/div/div[1]/div/div/div/div/div[2]/div[1]/div/div/div/div/div/div/div/div/div/div[1]/div/div/div/div[2]/div"));
            //Types the Tweet
            tweetInput.SendKeys(tweetContent);
        }

        public void SetTweets(string pi)
        {
            foreach (var tweet in SplitPiNumberInTweets(pi, 280))
            {
                Tweets.Add(tweet);
            }

            static IEnumerable<string> SplitPiNumberInTweets(string str, int maxLength)
            {
                for (int index = 0; index < str.Length; index += maxLength)
                {
                    yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
                }
            }
        }

        public string CalculatePiUpToNDecimal(int n)
        {
            n++;

            uint[] x = new uint[n * 10 / 3 + 2];
            uint[] r = new uint[n * 10 / 3 + 2];
            uint[] pi = new uint[n];

            for (int j = 0; j < x.Length; j++)
                x[j] = 20;

            for (int i = 0; i < n; i++)
            {
                uint carry = 0;
                for (int j = 0; j < x.Length; j++)
                {
                    uint num = (uint)(x.Length - j - 1);
                    uint dem = num * 2 + 1;

                    x[j] += carry;

                    uint q = x[j] / dem;
                    r[j] = x[j] % dem;

                    carry = q * num;
                }

                pi[i] = x[x.Length - 1] / 10;

                r[x.Length - 1] = x[x.Length - 1] % 10; ;

                for (int j = 0; j < x.Length; j++)
                    x[j] = r[j] * 10;
            }

            var result = "";

            uint c = 0;

            for (int i = pi.Length - 1; i >= 0; i--)
            {
                pi[i] += c;
                c = pi[i] / 10;

                result = (pi[i] % 10).ToString() + result;
            }

            result = result.Insert(1, ",");

            return result;
        }

        public void AddTweets()
        {
            for (int i = 0; i < Tweets.Count; i++)
            {
                AddTweet(i + 2, Tweets[i]);
            }
        }

        void AddTweet(int tweetNumber, string tweetContent)
        {
            //Gets the add Tweet button
            var addTweetButton = Driver.FindElement(By.XPath($"/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div[3]/div/div/div/div[{tweetNumber - 1}]/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[3]/div"));
            addTweetButton.Click();

            //Gets the Tweet input
            var tweetInput = Driver.FindElement(By.XPath($"/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div[3]/div/div/div/div[{tweetNumber}]/div/div/div/div/div[2]/div/div/div/div/div/div/div/div/div/div/div[1]/div/div/div/div[2]/div"));
            //Types the Tweet
            tweetInput.SendKeys(tweetContent);
        }

        public void TweetAll()
        {
            //Gets the "Tweet All" button
            var tweetAllButton = Driver.FindElement(By.XPath($"/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div[3]/div/div/div/div[{Tweets.Count + 1}]/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[4]/div"));
            tweetAllButton.Click();
        }
    }
}
