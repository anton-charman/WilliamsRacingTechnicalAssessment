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
using WilliamsRacingTechnicalAssessment.Models;

namespace WilliamsRacingTechnicalAssessment.Views
{
    /// <summary>
    /// Interaction logic for DriverView.xaml
    /// </summary>
    public partial class DriverView : UserControl
    {
        public DriverView()
        {
            InitializeComponent();
        }

        private Dictionary<TextBox, string> _dict = new Dictionary<TextBox, string>();

        private bool IndividualFilter(TextBox textBox, string prop)
        {
            return string.IsNullOrEmpty(textBox.Text) ||
                prop.Contains(textBox.Text, StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateDict(object obj)
        {
            Driver filteredObj = obj as Driver;

            _dict.Clear();
            _dict.Add(forenameTextBox, filteredObj.forename);
            _dict.Add(surnameTextBox, filteredObj.surname);
            _dict.Add(nationalityTextBox, filteredObj.nationality);
        }

        private bool CompoundFilter(object obj)
        {
            UpdateDict(obj);

            bool res = true;
            foreach (KeyValuePair<TextBox, string> pair in _dict)
            {
                res &= IndividualFilter(pair.Key, pair.Value);
            }

            return res;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            driversListView.Items.Filter = CompoundFilter;
        }
    }
}
