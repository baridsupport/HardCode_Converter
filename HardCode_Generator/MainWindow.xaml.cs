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
using System.Text.RegularExpressions;
using System.IO;

namespace HardCode_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private string persianPattern = @"[(\u0600-\u06FF)]+[(\u0600-\u06FF\s)]*";

        private string persianPattern = @">[\w|\W]{1,20}</option>";
        private string numberPattern = $"value=\"{@"[\w\W]{0,9}\d{1,3}"}\"";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string result = "";
            try
            {
                MatchCollection name = Regex.Matches(inputTextBox.Text, persianPattern);
                string[] nameArray = new string[name.Count];
                for (int i = 0; i < name.Count; i++)
                {
                    nameArray[i] = name[i].Value.Substring(1, name[i].Value.IndexOf('<') - 1);
                }

                MatchCollection number = Regex.Matches(inputTextBox.Text, numberPattern);
                numberPattern = @"\d{1,3}";
                string numbersString = "";
                for (int i = 0; i < number.Count; i++)
                {
                    numbersString += number[i].Value;
                }
                number = Regex.Matches(numbersString, numberPattern);

                result = @"ParameterRecipes = new List<ParameterRecipe>()
                    {";
                for (int i = 0; i < nameArray.Length; i++)
                {
                    result += "\n" + $"                        new ParameterRecipe({number[i].Value}, \"{nameArray[i]}\"),";
                }
                result = result.Remove(result.LastIndexOf(','), 1);
                result += "\n" + @"                    }";
            }
            catch (Exception er)
            {
                MessageBox.Show($"Error:\n{er.Message}");
            }
            
            inputTextBox.Text = result;
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(inputTextBox.Text);
        }

        private void pasteButton_Click(object sender, RoutedEventArgs e)
        {
            inputTextBox.Text = Clipboard.GetText();
        }
    }
}
