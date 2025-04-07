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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentsApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            using (var context = new Entities())
            {
                var res = context.student.ToList();
                DataGrid.ItemsSource = res;
            }
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(FilterTextBox.Text))
            {
                using (var context = new Entities())
                {
                    var res = context.student.Where(s => s.LastName.Contains(FilterTextBox.Text)).ToList();
                    DataGrid.ItemsSource = res;   
                }
            }
            else
            {
                MessageBox.Show("Введите значение");
                using (var context = new Entities())
                {
                    var res = context.student.ToList();
                    DataGrid.ItemsSource = res;
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem is student selectedStudent)
            {
                var result = MessageBox.Show($"Удалить студента {selectedStudent.LastName}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new Entities())
                    {
                        var studentToDelete = context.student.Find(selectedStudent.Id);
                        if (studentToDelete != null)
                        {
                            context.student.Remove(studentToDelete);
                            context.SaveChanges();
                            DataGrid.ItemsSource = context.student.ToList();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите студента для удаления", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditStudentWindow();
            if (editWindow.ShowDialog() == true)
            {
                using (var context = new Entities()) 
                {
                    var student = new student() 
                    {
                        FirstName = editWindow.FirstName,
                        LastName = editWindow.LastName,
                        AverageScore = editWindow.AverageScore
                    };
                    context.student.Add(student);
                    context.SaveChanges();
                    DataGrid.ItemsSource = context.student.ToList();
                }
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem is student selectedStudent)
            {
                var editWindow = new EditStudentWindow(selectedStudent);
                if (editWindow.ShowDialog() == true)
                {
                    try
                    {
                        using (var context = new Entities())
                        {
                            var studentToUpdate = context.student.Find(selectedStudent.Id);

                            if (studentToUpdate == null)
                            {
                                MessageBox.Show("Студент не найден в базе данных", "Ошибка",
                                              MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            studentToUpdate.FirstName = editWindow.FirstName;
                            studentToUpdate.LastName = editWindow.LastName;
                            studentToUpdate.AverageScore = editWindow.AverageScore;

                            int changes = context.SaveChanges();

                            if (changes > 0)
                            {
                                DataGrid.ItemsSource = context.student.ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении студента: {ex.Message}", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите студента для редактирования", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
