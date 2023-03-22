using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CeasarApp.Infrastructure;

namespace CeasarApp.Models
{
    class Ceasar
    {
        private string EncodedMessage;
        private bool IsRussian;
        /// <summary>
        /// Декодирование сообщения на вход на русском языке
        /// </summary>
        /// <returns></returns>
        public string Decrypt()
        {
            if (string.IsNullOrEmpty(EncodedMessage))
                return EncodedMessage;

            string CorrectedSentence;
            string ShiftedMessage = EncodedMessage;

            if (IsRussian)
            {
                for (int i = 0; i < 33; i++)
                {
                    CorrectedSentence = CorrectSentence(ShiftedMessage);
                    if(CorrectedSentence != null)
                    {
                        return CorrectedSentence;
                    }
                    ShiftedMessage = ShiftRussian(ShiftedMessage);
                }
            }
            else
            {
                for (int i = 0; i < 26; i++)
                {
                    ShiftedMessage = ShiftEnglish(ShiftedMessage);
                    CorrectedSentence = CorrectSentence(ShiftedMessage);
                    if (CorrectedSentence != null)
                    {
                        return CorrectedSentence;
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Сдвиг сообщения на уровень назад на русском языке
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string ShiftRussian(string message)
        {
            string Alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            string Message = message.ToLower();
            string result = "";
            foreach (char c in Message)
            {
                if (!Alphabet.Contains(c))
                {
                    result += c;
                }
                else
                {
                    if (c == 'а')
                    {
                        result += 'я';
                    }
                    else
                    {
                        result += Alphabet[Alphabet.IndexOf(c) - 1];
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Сдвиг сообщения на уровень назад на английском языке
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        static string ShiftEnglish(string message)
        {
            string Alphabet = "abcdefghijklmnopqrstuvwxyz";
            string Message = message.ToLower();
            string result = "";
            foreach (char c in Message)
            {
                if (!Alphabet.Contains(c))
                {
                    result += c;
                }
                else
                {
                    if (c == 'a')
                    {
                        result += 'z';
                    }
                    else
                    {
                        result += Alphabet[Alphabet.IndexOf(c) - 1];
                    }
                }
            }
            return result;
        }

        public string DecryptEnglish()
        {
            return "";
        }
        /// <summary>
        /// Исправление ошибок в слове
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns>null если неудалось преобразовать какое-либо слово или исправленное предложение</returns>
        private string CorrectSentence(string sentence)
        {
            string[] Words = sentence.Split(' ');
            string CorrectedSentence = "";
            string temp;
            SpellCheck spellCheck = new SpellCheck(IsRussian);

            foreach (string word in Words)
            {
                temp = spellCheck.FindMatch(word);
                if(temp != null)
                {
                    CorrectedSentence += " " + temp;
                }
                else
                {
                    return null;
                }
            }
            return CorrectedSentence;
        }

        public Ceasar(string Message, bool isRussian)
        {
            EncodedMessage = Message;
            IsRussian = isRussian;
        }
    }
}
