using System.IO;
using System.Collections.Generic;

namespace CeasarApp.Models
{
    /// <summary>
    /// Проверка одного слова для поиска совпадения или замены.
    /// </summary>
    class SpellCheck
    {
        private List<string> Dict = new List<string>(); // Список всех слов русского или английского словаря
        private int WordsAmount = 93479; // Длина словаря

        public SpellCheck(bool IsRussian)
        {
            InitializeDictionary(IsRussian);
        }
        /// <summary>
        /// Заполнение списка слов
        /// </summary>
        /// <param name="IsRussian"></param>
        private void InitializeDictionary(bool IsRussian)
        {
            string path = IsRussian ? "rus.txt" : "en.txt";
            if (!IsRussian) WordsAmount = 9999;
            using (StreamReader sr = new StreamReader(path))
            {
                for (int i = 0; i < WordsAmount; i++)
                {
                    Dict.Add(sr.ReadLine());
                }
            }
        }
        /// <summary>
        /// Неточный поиск слова в словаре
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string FindMatch(string word)
        {
            if (Dict.Contains(word))
                return word;

            int MatchScore;
            int counter = 0;
            string result = null;

            while (counter != WordsAmount - 1)
            {
                MatchScore = LevenshteinDistance(word, Dict[counter++]);
                if (MatchScore <= 2) //!!
                {
                    return Dict[counter - 1];
                }
            }
            return result;
        }

        private static int Minimum(int a, int b, int c) => (a = a < b ? a : b) < c ? a : c;
        /// <summary>
        /// Поиск минимального количество перестановок для получения слова через расстояние Левенштейна
        /// </summary>
        /// <param name="FirstWord"></param>
        /// <param name="SecondWord"></param>
        /// <returns></returns>
        private static int LevenshteinDistance(string FirstWord, string SecondWord)
        {
            var n = FirstWord.Length + 1;
            var m = SecondWord.Length + 1;
            var MatrixD = new int[n, m];

            const int DeletionCost = 1;
            const int InsertionCost = 1;

            for (var i = 0; i < n; i++)
            {
                MatrixD[i, 0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                MatrixD[0, j] = j;
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var SubstitutionCost = FirstWord[i - 1] == SecondWord[j - 1] ? 0 : 1;

                    MatrixD[i, j] = Minimum(MatrixD[i - 1, j] + DeletionCost,          // удаление
                                            MatrixD[i, j - 1] + InsertionCost,         // вставка
                                            MatrixD[i - 1, j - 1] + SubstitutionCost); // замена
                }
            }

            return MatrixD[n - 1, m - 1];
        }
    }
}
