using CeasarApp.Infrastructure.Commands;
using CeasarApp.Models;
using CeasarApp.ViewModels.Base;
using Microsoft.Win32;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CeasarApp.ViewModels
{
    /// <summary>
    /// ViewModel главного окна программы
    /// </summary>
    class MainWindowViewModel : ViewModel
    {
        #region Переменные
        private string _Title = "Шифр Цезаря";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private string _DecodedText = "";
        public string DecodedText
        {
            get => _DecodedText;
            set
            {
                Set(ref _DecodedText, value);
            }
        }

        private string _EncodedText = "";
        public string EncodedText
        {
            get => _EncodedText;
            set => Set(ref _EncodedText, value);
        }

        private string _SelectedLanguage = "Сменить язык: en";
        public string SelectedLanguage
        {
            get => _SelectedLanguage;
            set => Set(ref _SelectedLanguage, value);
        }

        #endregion

        #region Команды
        #region DecodeMessageCommand
        public ICommand DecodeMessageCommand { get; }
        private void OnDecodeMessageCommandExecuted(object p)
        {
            Ceasar ceasar = new Ceasar(EncodedText, _SelectedLanguage == "Сменить язык: ru");
            string temp = ceasar.Decrypt();

            if (temp != null)
            {
                DecodedText = temp;
            }
            else
            {
                DecodedText = "Сдвиг не найден";
            }
        }
        private bool CanDecodeMessageCommandExecute(object p) => true;
        #endregion

        #region LanguageChangeCommand
        public ICommand LanguageChangeCommand { get; }
        private void OnLanguageChangeCommandExecuted(object p)
        {
            if (SelectedLanguage == "Сменить язык: en")
                SelectedLanguage = "Сменить язык: ru";
            else
                SelectedLanguage = "Сменить язык: en";
        }
        private bool CanLanguageChangeCommandExecute(object p) => true;
        #endregion

        #region ReadFileCommand
        public ICommand ReadFileCommand { get; }
        private void OnReadFileCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовый файл (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
                EncodedText = File.ReadAllText(openFileDialog.FileName);
                OnDecodeMessageCommandExecuted(null);
        }
        private bool CanReadFileCommandExecute(object p) => true;
        #endregion
        #endregion

        public MainWindowViewModel()
        {
            // расшифровать сообщение
            DecodeMessageCommand = new LambdaCommand(OnDecodeMessageCommandExecuted, CanDecodeMessageCommandExecute);
            // изменение языка
            LanguageChangeCommand = new LambdaCommand(OnLanguageChangeCommandExecuted, CanLanguageChangeCommandExecute);
            // прочитать из файла
            ReadFileCommand = new LambdaCommand(OnReadFileCommandExecuted, CanReadFileCommandExecute);
        }
    }
}
