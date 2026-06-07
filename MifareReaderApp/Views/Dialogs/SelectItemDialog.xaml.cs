using MifareReaderApp.Models;
using System.Collections.Generic;
using System.Windows;

namespace MifareReaderApp.Views.Dialogs
{
    public partial class SelectItemDialog : Window
    {
        public string Message { get; private set; }
        public IEnumerable<object> Items { get; set; }
        public string DisplayMemberName { get; private set; }
        public object SelectedItem { get; set; }

        /// <summary>
        /// Диалог выбора элемента из списка.
        /// </summary>
        /// <param name="items">Список элементов для выбора.</param>
        /// <param name="message">Поясняющий текст (опционально).</param>
        public SelectItemDialog(IEnumerable<object> items, string displayMemberName, string message = "Выберите элемент:")
        {
            DisplayMemberName = displayMemberName;
            Items = items;
            Message = message;

            InitializeComponent();

            Owner = App.Current.MainWindow;
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите элемент из списка.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}