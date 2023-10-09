using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsVowel
{
    internal static class CharExtentions
    {
        private static readonly HashSet<char> LettersForCount = ParseLettersFromConfig();

        private static HashSet<char> ParseLettersFromConfig()
        {
            var letters = ConfigurationManager.AppSettings.Get(nameof(LettersForCount));

            if (string.IsNullOrWhiteSpace(letters))
                throw new ConfigurationErrorsException($"Invalid value for key: {nameof(LettersForCount)}");

            var parsedLetters = new HashSet<char>();

            foreach (var c in letters)
            {
                if (char.IsLetter(c))
                {
                    parsedLetters.Add(char.ToLower(c));
                    parsedLetters.Add(char.ToUpper(c));
                }
            }

            return parsedLetters;
        }

        /// <summary>
        /// Check if the letter(ignore case) is in the LettersForCount value in App.config.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetterForCount(this char c)
        {
            return LettersForCount.Contains(c);
        }
    }
}
