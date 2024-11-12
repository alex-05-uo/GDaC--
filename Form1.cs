using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace GDKlaba3WordSearcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(ClosingFormClosing);
        }
        private void ClosingFormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        // Объявление списка для хранения строк из файла
        List<string> FileLines = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            // Открытие диалога выбора файла
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                // Установка фильтра для диалога на файлы только с расширением *.txt или все файлы
                openDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                // Разрешение выбора только одного файла
                openDialog.Multiselect = false;
                // Убедиться, что пользователь не нажал кнопку "Отмена"
                if (openDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Обновление метки текущего файла только именем файла (без полного пути)
                    label_CurrentFile.Text = $"Текущий файл: {Path.GetFileName(openDialog.FileName)}";
                    // Добавление каждой строки текстового файла в список
                    foreach (string line in File.ReadAllLines(openDialog.FileName, Encoding.UTF8))
                        FileLines.Add(line);
                    textBox1.Text = File.ReadAllText(openDialog.FileName);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Очистка списка результатов
            list_SearchResults.Items.Clear();
            // Получение поискового запроса из первого TextBox
            string searchTerm = text_SearchTerm.Text;
            // Получение текста для поиска из второго TextBox
            string textToSearch = textBox1.Text;
            // Если поисковый запрос или текст для поиска пусты, возврат
            if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(textToSearch))
                return;
            // Счетчик номера строк, чтобы позже представить их в списке результатов
            int iLineNumber = 1;
            // Флаг для проверки наличия заданного слова
            bool termFound = false;
            // Счетчик количества найденных слов
            int wordCount = 0;
            // Обработка текста для поиска
            ProcessTextForSearch(textToSearch, searchTerm, iLineNumber, ref termFound, ref wordCount);
            // Если заданное слово не найдено, отображение сообщения
            if (!termFound)
            {
                MessageBox.Show("Заданное слово не найдено в тексте.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Вывод количества найденных слов в textBox2
            textBox2.Text = $"{wordCount}";
        }
        private void ProcessTextForSearch(string text, string searchTerm, int lineNumber, ref bool termFound, ref int wordCount)
        {
            // Создаем регулярное выражение для поиска слова с учетом регистра
            Regex regex = new Regex("\\b" + Regex.Escape(searchTerm) + "\\b");
            // Разделение текста на строки
            string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            // Для каждой строки в тексте
            foreach (var line in lines)
            {
                // Ищем все вхождения слова в строке
                MatchCollection matches = regex.Matches(line);
                // Если найдено хотя бы одно вхождение слова
                if (matches.Count > 0)
                {
                    // Установка флага в true, если поисковый запрос найден
                    termFound = true;
                    // Создание нового элемента ListViewItem, который будет добавлен позже в список результатов
                    ListViewItem lvi = new ListViewItem(line);
                    // Добавление номера строки во второй столбец
                    lvi.SubItems.Add(lineNumber.ToString());
                    // Добавление элемента ListViewItem в список результатов
                    list_SearchResults.Items.Add(lvi);
                    // Увеличение счетчика найденных слов на количество вхождений в строке
                    wordCount += matches.Count;
                }
                // Увеличение переменной номера строки
                lineNumber++;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            // Получение слова для удаления из TextBox
            string wordToDelete = text_SearchTerm.Text;
            // Если слово для удаления пусто, возврат
            if (string.IsNullOrEmpty(wordToDelete))
                return;
            // Получение текста для поиска из TextBox
            string textToSearch = textBox1.Text;
            // Если текст для поиска пуст, возврат
            if (string.IsNullOrEmpty(textToSearch))
                return;
            // Создаем регулярное выражение для поиска слова с учетом регистра
            Regex regex = new Regex("\\b" + Regex.Escape(wordToDelete) + "\\b");
            // Заменяем все вхождения заданного слова на пустую строку
            string newText = regex.Replace(textToSearch, string.Empty);
            // Если в тексте не найдено слово для удаления
            if (newText == textToSearch)
            {
                MessageBox.Show($"Слово '{wordToDelete}' не найдено в тексте.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Обновляем текст в TextBox
                textBox1.Text = newText;
                MessageBox.Show($"Слово '{wordToDelete}' успешно удалено из текста.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            // Получение заданного слова для поиска из TextBox
            string searchTerm = text_SearchTerm.Text;
            // Получение нового слова из TextBox
            string newWord = textBox3.Text;
            // Если заданное слово или новое слово пусты, возврат
            if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(newWord))
                return;
            // Получение текста для поиска из TextBox
            string textToSearch = textBox1.Text;
            // Если текст для поиска пуст, возврат
            if (string.IsNullOrEmpty(textToSearch))
                return;
            // Создаем регулярное выражение для поиска заданного слова с учетом регистра
            Regex regex = new Regex("\\b" + Regex.Escape(searchTerm) + "\\b");
            // Заменяем все вхождения заданного слова на новое слово
            string newText = regex.Replace(textToSearch, newWord);
            // Если в тексте не найдено заданное слово
            if (newText == textToSearch)
            {
                MessageBox.Show($"Слово '{searchTerm}' не найдено в тексте.", "Изменение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Обновляем текст в TextBox
                textBox1.Text = newText;
                MessageBox.Show($"Слово '{searchTerm}' успешно заменено на '{newWord}'.", "Изменение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Получение текста из textBox1
            string textToSave = textBox1.Text;
            // Если текст для сохранения пуст, возврат
            if (string.IsNullOrEmpty(textToSave))
            {
                MessageBox.Show("Нет данных для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Показываем диалоговое окно сохранения файла
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveDialog.FilterIndex = 1;
                saveDialog.RestoreDirectory = true;
                // Если пользователь выбрал файл для сохранения
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Записываем текст в выбранный файл
                        File.WriteAllText(saveDialog.FileName, textToSave);
                        MessageBox.Show("Файл успешно сохранен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Произошла ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void label3_Click(object sender, EventArgs e)
        {
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
