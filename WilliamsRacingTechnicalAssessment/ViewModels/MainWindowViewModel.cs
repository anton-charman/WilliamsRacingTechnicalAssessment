namespace WilliamsRacingTechnicalAssessment.ViewModels
{
    public class MainWindowViewModel : NotifyBase
    {
        public MainWindowViewModel()
        {
            _circuitViewModel = new CircuitViewModel(_rootPath);
            _driverViewModel = new DriverViewModel(_rootPath);
        }

        // TODO:
        // Change this to the location of the *.json and *.csv data files.
        //.........................................................................
        private string _rootPath = @"C:\Users\User\Documents\Temp\WilliamsDataset";
        //.........................................................................

        private NotifyBase _currentViewModel;
        private CircuitViewModel _circuitViewModel;
        private DriverViewModel _driverViewModel;
        private int _selectedIndex;

        public NotifyBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                OnSelectTab(_selectedIndex);
            }
        }

        private void OnSelectTab(int destination)
        {
            CurrentViewModel = destination switch
            {
                0 => _circuitViewModel,
                1 => _driverViewModel,
                _ => throw new ArgumentException()
            };
        }
    }
}
