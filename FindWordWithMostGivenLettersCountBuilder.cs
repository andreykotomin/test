using System.Text;

namespace NewsVowel
{
    internal class FindWordWithMostGivenLettersCountBuilder
    {
        public string Word => currentWord;

        private readonly StringBuilder wordBuilder = new();
        private string currentWord = string.Empty;
        private int maxCount = 0;
        private int currentCount = 0;

        public FindWordWithMostGivenLettersCountBuilder Add(string text)
        {
            if (string.IsNullOrEmpty(text))
                return this;

            foreach (var c in text)
            {
                if (char.IsLetter(c))
                {
                    wordBuilder.Append(c);
                    if (c.IsLetterForCount())
                        currentCount++;
                    continue;
                }

                OnEndWord();
            }
            OnEndWord();

            return this;
        }

        private void OnEndWord()
        {
            if (wordBuilder.Length == 0)
                return;

            if (currentCount > maxCount)
            {
                maxCount = currentCount;
                currentWord = wordBuilder.ToString();
            }

            currentCount = 0;
            wordBuilder.Clear();
        }

        public override string ToString() => currentWord;
    }
}
