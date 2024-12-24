using WilliamsRacingTechnicalAssessment.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace WilliamsRacingTechnicalAssessment.ViewModels
{
    public class DriverViewModel : NotifyBase
    {
        public DriverViewModel(string rootPath)
        {
            _rootPath = rootPath;
            ClearCommand = new RelayCommand<object>(OnClear, CanClear);

            LoadData();
            ExtractRelevantData();
            SelectedDriver = Drivers[0];
        }

        // Hard-coded parameters relevant to this viewmodel.
        private readonly string _driversName = "drivers.json";
        private readonly string _driverStandingsName = "driver_standings.csv";
        private readonly int _idxDriverId = 2;
        private readonly int _idxPosition = 4;
        private readonly int _podiumLimit = 3;

        private readonly string _rootPath;
        private string _forenameText;
        private string _surnameText;
        private string _nationalityText;
        private Driver _selectedDriver;
        private int _numTimesOnPodium;
        private int _numRacesEntered;
        private readonly List<int> _driverIds = new List<int>();
        private readonly List<int> _positions = new List<int>();

        public ObservableCollection<Driver>? Drivers { get; private set; }
        public RelayCommand<object> ClearCommand { get; private set; }

        public Driver SelectedDriver
        {
            get
            {
                return _selectedDriver;
            }
            set
            {
                _selectedDriver = value;
                OnPropertyChanged(nameof(SelectedDriver));
                UpdateNumRacesEntered();
                UpdateNumTimesOnPodium();
            }
        }
        
        public string? ForenameText
        {
            get
            {
                return _forenameText;
            }
            set
            {
                _forenameText = value;
                OnPropertyChanged(nameof(ForenameText));
                ClearCommand.RaiseCanExecuteChanged();
            }
        }
        
        public string? SurnameText
        {
            get
            {
                return _surnameText;
            }
            set
            {
                _surnameText = value;
                OnPropertyChanged(nameof(SurnameText));
                ClearCommand.RaiseCanExecuteChanged();
            }
        }
        
        public string? NationalityText
        {
            get
            {
                return _nationalityText;
            }
            set
            {
                _nationalityText = value;
                OnPropertyChanged(nameof(NationalityText));
                ClearCommand.RaiseCanExecuteChanged();
            }
        }

        public int NumTimesOnPodium
        {
            get
            {
                return _numTimesOnPodium;
            }
            set
            {
                _numTimesOnPodium = value;
                OnPropertyChanged(nameof(NumTimesOnPodium));
            }
        }

        public int NumRacesEntered
        {
            get
            {
                return _numRacesEntered;
            }
            set
            {
                _numRacesEntered = value;
                OnPropertyChanged(nameof(NumRacesEntered));
            }
        }

        /// <summary>
        /// Method bound to property for clearing UI textboxes.
        /// </summary>
        /// <param name="obj"></param>
        private void OnClear(object obj)
        {
            ForenameText = null;
            SurnameText = null;
            NationalityText = null;

            ClearCommand.RaiseCanExecuteChanged();
        }

        private bool CanClear(object obj)
        {
            return ForenameText != null || SurnameText != null || NationalityText != null;
        }

        /// <summary>
        /// Serialise the driver data.
        /// </summary>
        private void LoadData()
        {
            string driversJson = File.ReadAllText(Path.Combine(_rootPath, _driversName));
            ObservableCollection<Driver>? drivers = JsonSerializer.Deserialize<ObservableCollection<Driver>>(driversJson);
            Drivers = new ObservableCollection<Driver>
            (
                drivers.OrderBy(driver => driver.driverId)
            );
        }

        /// <summary>
        /// Extract relevant information from the data files.
        /// </summary>
        private void ExtractRelevantData()
        {
            // Get a list of all driver IDs and a list of positions.
            // Can't use a dict due to repeated driverIds as keys.
            List<string[]> driverStandingsLines = File.ReadAllLines(Path.Combine(_rootPath, _driverStandingsName))
                .Skip(1)
                .Select(line => line.Split(','))
                .ToList();

            foreach (string[] line in driverStandingsLines)
            {
                _driverIds.Add(int.Parse(line[_idxDriverId]));
                _positions.Add(int.Parse(line[_idxPosition]));
            }
        }

        /// <summary>
        /// Update the number of races entered for the selected driver.
        /// </summary>
        private void UpdateNumRacesEntered()
        {
            // For all driverIds, when equal to the selected driverId, add it
            // to a list and get the count.
            NumRacesEntered = _driverIds
                .Where(driverId => driverId == SelectedDriver.driverId)
                .Count();
        }

        /// <summary>
        /// Update the number of podium finishes for the selected driver.
        /// </summary>
        private void UpdateNumTimesOnPodium()
        {
            // For each driverId in _driverIds with a corresponding index, add it to a list 
            // when it equals the selected driverId and the corresponding position is <= 3.
            NumTimesOnPodium = _driverIds
                .Where((driverId, idx) => driverId == SelectedDriver.driverId && _positions[idx] <= _podiumLimit)
                .Count();
        }
    }
}
