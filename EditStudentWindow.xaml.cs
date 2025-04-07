using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentsApp
{
    /// <summary>
    /// Логика взаимодействия для EditStudentWindow.xaml
    /// </summary>
    public partial class EditStudentWindow : Window
    {
        public string FirstName => FirstNameTextBox.Text;
        public string LastName => LastNameTextBox.Text;
        public decimal AverageScore => decimal.Parse(AverageScoreTextBox.Text);
        public EditStudentWindow()
        {
            InitializeComponent();
        }
        public EditStudentWindow(student student) : this()
        {
            FirstNameTextBox.Text = student.FirstName;
            LastNameTextBox.Text = student.LastName;
            AverageScoreTextBox.Text = student.AverageScore.ToString();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("Введите имя студента", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Введите фамилию студента", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!decimal.TryParse(AverageScoreTextBox.Text, out decimal score) || score < 0 || score > 5)
            {
                MessageBox.Show("Введите корректный средний балл (0-5)", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
