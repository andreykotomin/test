using System.Configuration;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json;
using static NewsVowel.ANSIEscapeCode;

namespace NewsVowel
{
    public class Program
    {
        private static readonly string NewsApiKey = ConfigurationManager.AppSettings.Get(nameof(NewsApiKey))
            ?? throw new ConfigurationErrorsException($"Invalid value for key: {nameof(NewsApiKey)}");
        private static readonly string RequestQ = ConfigurationManager.AppSettings.Get(nameof(RequestQ))
            ?? "космос";

        private static void Main()
        {
            var newsApiClient = new NewsApiClient(NewsApiKey);

            Task.Run(() => GetEverythingAsync(newsApiClient, RequestQ, Languages.RU, OnGetArticlesAsync)).Wait();
            Task.Run(() => GetEverythingAsync(newsApiClient, RequestQ, Languages.EN, OnGetArticlesAsync)).Wait();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task GetEverythingAsync(NewsApiClient newsApiClient, string requestQ, Languages language, Func<List<Article>, Languages, Task> onGetArcticles)
        {
            var articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
            {
                Q = requestQ,
                Language = language
            });

            if (articlesResponse.Status != Statuses.Ok)
            {
                await WriteLineAsync($"{RED}Error:\t{articlesResponse.Error.Message}\nwith code:\t{articlesResponse.Error.Code}{NORMAL}");
                return;
            }

            await onGetArcticles.Invoke(articlesResponse.Articles, language);
        }

        private static async Task OnGetArticlesAsync(List<Article> articles, Languages language)
        {
            const string lr = "\n____________________________________________________________________________\n";

            foreach (var article in articles)
            {
                var title = ArticleView(nameof(article.Title), article.Title);
                var author = ArticleView(nameof(article.Author), article.Author);
                var description = ArticleView(nameof(article.Description), article.Description);
                var content = ArticleView(nameof(article.Content), article.Content); //TODO parse full content from news page article.Url

                await WriteLineAsync($"{RED}{lr}{language}:\n{NORMAL}{title}{author}{description}{content}");
            }

            static string ArticleView(string name, string text)
            {
                var wordWithMostVowels = new FindWordWithMostGivenLettersCountBuilder();
                return $"{name}:\t{text}\n{YELLOW}Word with the most vowels:\t{GREEN}{wordWithMostVowels.Add(text)}{NORMAL}{lr}";
            }
        }

        private static async Task WriteLineAsync(string value)
        {
            await Console.Out.WriteLineAsync(value);
        }
    }
}