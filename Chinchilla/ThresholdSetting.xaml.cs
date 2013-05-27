using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chinchilla
{
    /// <summary>
    /// Interaction logic for ThresholdSetting.xaml
    /// </summary>
    public partial class ThresholdSetting : Window
    {
        public ThresholdSetting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Window2 main = (Window2)Application.Current.MainWindow;
            main.threValue.Clear();
            main.threValue.Add(this.textBox1.Text);
            main.threValue.Add(this.textBox2.Text);
            main.threValue.Add(this.textBox3.Text);
            this.Hide();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
