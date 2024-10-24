using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hianyzasok
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<Absence> absences;

        public class Absence
        {
            public string Name { get; set; }
            public string Class { get; set; }
            public int StartDate { get; set; }
            public int EndDate { get; set; }
            public int MissedHours { get; set; }
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                absences = new List<Absence>();

                foreach (var line in lines.Skip(1))
                {
                    var data = line.Split(';');
                    absences.Add(new Absence
                    {
                        Name = data[0],
                        Class = data[1],
                        StartDate = int.Parse(data[2]),
                        EndDate = int.Parse(data[3]),
                        MissedHours = int.Parse(data[4])
                    });
                }

                MessageBox.Show("Data loaded successfully.");
            }
        }

        // 2. Feladat
        private void btnTotalHours_Click(object sender, RoutedEventArgs e)
        {
            int totalHours = absences.Sum(a => a.MissedHours);
            txtTotalHours.Text = $"Total Hours: {totalHours} hours.";
        }

        // 4. Feladat
        private void btnCheckAbsence_Click(object sender, RoutedEventArgs e)
        {
            int day;
            if (int.TryParse(txtDay.Text, out day))
            {
                string studentName = txtStudentName.Text;

                bool wasAbsent = absences.Any(a => a.Name.Equals(studentName, StringComparison.OrdinalIgnoreCase) &&
                                                   a.StartDate <= day && a.EndDate >= day);

                if (wasAbsent)
                    txtAbsenceResult.Text = "The student was absent in September.";
                else
                    txtAbsenceResult.Text = "The student was not absent in September.";
            }
            else
            {
                MessageBox.Show("Please enter a valid day between 1 and 30.");
            }
        }

        // 5. Feladat
        private void btnListAbsences_Click(object sender, RoutedEventArgs e)
        {
            int day;
            if (int.TryParse(txtDay.Text, out day))
            {
                var absentStudents = absences
                    .Where(a => a.StartDate <= day && a.EndDate >= day)
                    .Select(a => $"{a.Name} ({a.Class})")
                    .ToList();

                if (absentStudents.Any())
                    txtAbsenceList.Text = "Absentees:\n" + string.Join("\n", absentStudents);
                else
                    txtAbsenceList.Text = "No students were absent on this day.";
            }
            else
            {
                MessageBox.Show("Please enter a valid day between 1 and 30.");
            }
        }

        //Placeholder functions
        private void txtDay_GotFocus(object sender, RoutedEventArgs e)
            {
                if (txtDay.Text == "Enter Day")
                {
                    txtDay.Text = "";
                    txtDay.Foreground = Brushes.Black;
                }
            }

        private void txtDay_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDay.Text))
            {
                txtDay.Text = "Enter Day";
                txtDay.Foreground = Brushes.Gray;
            }
        }

        private void txtStudentName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtStudentName.Text == "Enter Student Name")
            {
                txtStudentName.Text = "";
                txtStudentName.Foreground = Brushes.Black;
            }
        }

        private void txtStudentName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                txtStudentName.Text = "Enter Student Name";
                txtStudentName.Foreground = Brushes.Gray;
            }
        }
    }
}