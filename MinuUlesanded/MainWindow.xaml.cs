
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MinuUlesanded
{
    public partial class MainWindow : Window
    {
        private List<TaskItem> tasks = new List<TaskItem>();

        private readonly string filePath = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            "MinuUlesanded",
            "tasks.json");

        public MainWindow()
        {
            InitializeComponent();
            LoadTasks();
            RefreshTasks();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = TaskTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                    "Palun sisesta ülesande nimi!",
                    "Viga",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            string subject =
                ((ComboBoxItem)SubjectComboBox.SelectedItem)
                .Content.ToString() ?? "";

            string priority =
                ((ComboBoxItem)PriorityComboBox.SelectedItem)
                .Content.ToString() ?? "";

            TaskItem task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Name = name,
                Subject = subject,
                Priority = priority,
                DueDate = TaskDatePicker.SelectedDate,
                IsCompleted = false
            };

            tasks.Add(task);

            SaveTasks();
            RefreshTasks();

            TaskTextBox.Clear();
            TaskDatePicker.SelectedDate = null;
            SubjectComboBox.SelectedIndex = 0;
            PriorityComboBox.SelectedIndex = 1;
        }

        private void RefreshTasks()
        {
            TaskPanel.Children.Clear();

            string filter = "Kõik";

            if (FilterComboBox.SelectedItem is ComboBoxItem item)
            {
                filter = item.Content.ToString() ?? "Kõik";
            }

            IEnumerable<TaskItem> filteredTasks = tasks;

            if (filter == "Tehtud")
            {
                filteredTasks = tasks.Where(t => t.IsCompleted);
            }
            else if (filter == "Tegemata")
            {
                filteredTasks = tasks.Where(t => !t.IsCompleted);
            }

            foreach (TaskItem task in filteredTasks)
            {
                Border taskBorder = new Border
                {
                    Background = new SolidColorBrush(
                        Color.FromRgb(255, 245, 249)),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(12),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                Grid row = new Grid();

                row.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });

                row.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = new GridLength(
                            1, GridUnitType.Star)
                    });

                row.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });

                CheckBox checkBox = new CheckBox
                {
                    IsChecked = task.IsCompleted,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 12, 0)
                };

                checkBox.Checked += (s, e) =>
                {
                    task.IsCompleted = true;
                    SaveTasks();
                    RefreshTasks();
                };

                checkBox.Unchecked += (s, e) =>
                {
                    task.IsCompleted = false;
                    SaveTasks();
                    RefreshTasks();
                };

                Grid.SetColumn(checkBox, 0);
                row.Children.Add(checkBox);

                StackPanel details = new StackPanel();

                TextBlock nameText = new TextBlock
                {
                    Text = task.Name,
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.Black
                };

                if (task.IsCompleted)
                {
                    nameText.TextDecorations =
                        TextDecorations.Strikethrough;
                }

                string date = task.DueDate.HasValue
                    ? task.DueDate.Value.ToString("dd.MM.yyyy")
                    : "Tähtaeg puudub";

                TextBlock infoText = new TextBlock
                {
                    Text = task.Subject + " | " +
                           task.Priority + " | " + date,
                    FontSize = 12,
                    Foreground = Brushes.Gray,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 0)
                };

                details.Children.Add(nameText);
                details.Children.Add(infoText);

                Grid.SetColumn(details, 1);
                row.Children.Add(details);

                Button deleteButton = new Button
                {
                    Content = "Kustuta",
                    Background = new SolidColorBrush(
                        Color.FromRgb(231, 138, 179)),
                    Foreground = Brushes.White,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(12, 7, 12, 7),
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };

                deleteButton.Click += (s, e) =>
                {
                    tasks.Remove(task);
                    SaveTasks();
                    RefreshTasks();
                };

                Grid.SetColumn(deleteButton, 2);
                row.Children.Add(deleteButton);

                taskBorder.Child = row;
                TaskPanel.Children.Add(taskBorder);
            }

            if (!filteredTasks.Any())
            {
                TaskPanel.Children.Add(new TextBlock
                {
                    Text = "Ülesandeid ei leitud.",
                    Foreground = Brushes.Gray,
                    FontSize = 14,
                    HorizontalAlignment =
                        HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 20)
                });
            }

            UpdateStatistics();
        }

        private void FilterComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (TaskPanel != null)
            {
                RefreshTasks();
            }
        }

        private void UpdateStatistics()
        {
            int total = tasks.Count;
            int completed = tasks.Count(t => t.IsCompleted);
            int pending = total - completed;

            TotalText.Text = total.ToString();
            CompletedText.Text = completed.ToString();
            PendingText.Text = pending.ToString();

            StatusText.Text =
                "Ülesandeid kokku: " + total;
        }

        private void SaveTasks()
        {
            try
            {
                string? directory = Path.GetDirectoryName(filePath);

                if (directory != null)
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(
                    tasks,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                File.WriteAllText(filePath, json);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Ülesannete salvestamine ebaõnnestus!",
                    "Viga",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadTasks()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);

                    tasks = JsonSerializer.Deserialize<List<TaskItem>>(
                        json) ?? new List<TaskItem>();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Ülesannete laadimine ebaõnnestus!",
                    "Viga",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Priority { get; set; } = "";
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
