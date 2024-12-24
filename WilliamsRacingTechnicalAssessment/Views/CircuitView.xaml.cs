using System.Windows;
using System.Windows.Controls;
using WilliamsRacingTechnicalAssessment.Models;
using WilliamsRacingTechnicalAssessment.ViewModels;

namespace WilliamsRacingTechnicalAssessment.Views
{
    /// <summary>
    /// Interaction logic for CircuitView.xaml
    /// </summary>
    public partial class CircuitView : UserControl
    {
        public CircuitView()
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
            Circuit filteredObj = obj as Circuit;

            _dict.Clear();
            _dict.Add(nameTextBox, filteredObj.name);
            _dict.Add(locationTextBox, filteredObj.location);
            _dict.Add(countryTextBox, filteredObj.country);
        }

        private bool CompoundFilter(object obj)
        {
            UpdateDict(obj);

            bool res = true;
            foreach(KeyValuePair<TextBox, string> pair in _dict) 
            {
                res &= IndividualFilter(pair.Key, pair.Value);
            }
            return res;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            circuitsListView.Items.Filter = CompoundFilter;
        }
    }
}
