using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Threading;
using tweet_pi_number_bot.Bot;

namespace tweet_pi_number_bot
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--start-maximized");

            TwitterBot twitterBot = new TwitterBot()
            {
                LoginUrl = "https://twitter.com/login?lang=pt",
                Driver = new ChromeDriver(chromeOptions),
                Tweets = new List<string>()
            };

            Thread thead = new Thread(() => CalculatePiAndSetTweets(twitterBot));
            thead.Start();

            twitterBot.Login();
            twitterBot.StartThread();

            thead.Join();

            twitterBot.AddTweets();
            twitterBot.TweetAll();
        }

        static void CalculatePiAndSetTweets(TwitterBot twitterBot)
        {
            twitterBot.SetTweets(twitterBot.CalculatePiUpToNDecimal(6718));
        }
    }
}
