using WilliamsRacingTechnicalAssessment.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace WilliamsRacingTechnicalAssessment.ViewModels
{
    public class CircuitViewModel : NotifyBase
    {
        public CircuitViewModel(string rootPath)
        {
            _rootPath = rootPath;
            ClearCommand = new RelayCommand<object>(OnClear, CanClear);

            LoadData();
            ExtractRelevantData();
            SelectedCircuit = Circuits[0];
        }

        // Hard-coded parameters relevant to this viewmodel.
        private readonly string _circuitsName = "circuits.json";
        private readonly string _racesName = "races.csv";
        private readonly string _lapTimesName = "lap_times.csv";
        private readonly int _idxCircuitId = 3;
        private readonly int _idxRaceId = 0;
        private readonly int _idxMilliseconds = 5;
        private readonly string _notApplicableString = "N/A";
        private readonly string _lapTimeFormatString = @"mm\:ss\:fff";

        private readonly string _rootPath;
        private string _nameText;
        private string _locationText;
        private string _countryText;
        private Circuit _selectedCircuit;
        private string _fastestLapTime;
        private int _totalRacesCompleted;
        private readonly Dictionary<int, int> _raceDict = new Dictionary<int, int>();
        private readonly List<int> _raceIds = new List<int>();
        private readonly List<int> _lapTimes = new List<int>();
        private List<int> _matchingRaceIds;
        private List<int> _matchingLapTimes;

        public ObservableCollection<Circuit>? Circuits { get; private set; }
        public RelayCommand<object> ClearCommand { get; private set; }

        public Circuit SelectedCircuit
        {
            get 
            { 
                return _selectedCircuit; 
            }
            set
            {
                _selectedCircuit = value;
                OnPropertyChanged(nameof(SelectedCircuit));
                UpdateFastestLapTime();
                UpdateTotalRacesCompleted();
            }
        }
        
        public string? NameText
        {
            get
            {
                return _nameText;
            }
            set
            {
                _nameText = value;
                OnPropertyChanged(nameof(NameText));
                ClearCommand.RaiseCanExecuteChanged();
            }
        }

        public string? LocationText
        {
            get
            {
                return _locationText;
            }
            set
            {
                _locationText = value;
                OnPropertyChanged(nameof(LocationText));
                ClearCommand.RaiseCanExecuteChanged();
            }
        }
   
        public string? CountryText
        {
            get
            {
                return _countryText;
            }
            set
            {
                _countryText = value;
                OnPropertyChanged(nameof(CountryText));
                ClearCommand.RaiseCanExecuteChanged();
            }
        }

        public string FastestLapTime
        {
            get
            {
                return _fastestLapTime;
            }
            set
            {
                _fastestLapTime = value;
                OnPropertyChanged(nameof(FastestLapTime));
            }
        }

        public int TotalRacesCompleted
        {
            get
            {
                return _totalRacesCompleted;
            }
            set
            {
                _totalRacesCompleted = value;
                OnPropertyChanged(nameof(TotalRacesCompleted));
            }
        }

        /// <summary>
        /// Method bound to property for clearing UI textboxes.
        /// </summary>
        /// <param name="obj"></param>
        private void OnClear(object obj)
        {
            NameText = null;
            CountryText = null;
            LocationText = null;

            ClearCommand.RaiseCanExecuteChanged();
        }

        private bool CanClear(object obj)
        {
            return NameText != null || CountryText != null || LocationText != null;
        }

        /// <summary>
        /// Serialise the circuit data.
        /// </summary>
        private void LoadData()
        {
            string circuitJson = File.ReadAllText(Path.Combine(_rootPath, _circuitsName));
            ObservableCollection<Circuit>? circuits = JsonSerializer.Deserialize<ObservableCollection<Circuit>>(circuitJson);
            Circuits = new ObservableCollection<Circuit>
            (
                circuits.OrderBy(circuit => circuit.circuitId)
            );
        }

        /// <summary>
        /// Get a dict of all race IDs and their corresponding circuit IDs.
        /// </summary>
        private void GetRaceDict()
        {
            List<string[]> raceLines = File.ReadAllLines(Path.Combine(_rootPath, _racesName))
                .Skip(1)
                .Select(line => line.Split(','))
                .ToList();

            foreach (string[] raceLine in raceLines)
            {
                _raceDict.Add(int.Parse(raceLine[_idxRaceId]), int.Parse(raceLine[_idxCircuitId]));
            }
        }

        /// <summary>
        /// Get a list of all race IDs and a list of lap times.
        /// </summary>
        private void GetLapTimes()
        {
            List<string[]> lapTimeLines = File.ReadAllLines(Path.Combine(_rootPath, _lapTimesName))
                .Skip(1)
                .Select(line => line.Split(','))
                .ToList();

            // Can't use a dict due to repeated raceIds as keys.
            foreach (string[] lapTimeLine in lapTimeLines)
            {
                _raceIds.Add(int.Parse(lapTimeLine[_idxRaceId]));
                _lapTimes.Add(int.Parse(lapTimeLine[_idxMilliseconds]));
            }
        }

        /// <summary>
        /// Extract relevant information from the data files.
        /// </summary>
        private void ExtractRelevantData()
        {
            GetRaceDict();
            GetLapTimes();
        }

        /// <summary>
        /// Update the list of the race IDs that match the selected circuit ID.
        /// </summary>
        private void GetMatchingRaceIds()
        {
            // For all keys (raceIds), when the corresponding dict value (circuitId)
            // equals the selected circuitId, then add that key to a list.
            _matchingRaceIds = _raceDict
                .Keys
                .Where(raceId => _raceDict[raceId] == SelectedCircuit.circuitId)
                .ToList();
        }

        /// <summary>
        /// Update the list of lap times that have a race ID that matches the selected circuit ID.
        /// </summary>
        private void GetMatchingLapTimes()
        {
            // For each lapTime in lapTimes with a corresponding index, when
            // the corresponding raceId is contained, then add the lapTime to a list.
            _matchingLapTimes = _lapTimes
                .Where((lapTime, idx) => _matchingRaceIds.Contains(_raceIds[idx]))
                .ToList();
        }

        /// <summary>
        /// Update the fastest lap time for a selected circuit.
        /// </summary>
        /// <returns></returns>
        private void GetFastestLapTime()
        {
            // The lap_times file might not contain lap data for a certain race ID.
            // In this case, the list of lap times will be empty.
            FastestLapTime = _matchingLapTimes.Count == 0 ? _notApplicableString : 
                TimeSpan.FromMilliseconds(_matchingLapTimes.Min()).ToString(_lapTimeFormatString);
        }

        /// <summary>
        /// Get the fastest lap time from all laps from all race IDs for a selected circuit ID.
        /// </summary>
        private void UpdateFastestLapTime()
        {
            GetMatchingRaceIds();
            GetMatchingLapTimes();
            GetFastestLapTime();
        }

        private void UpdateTotalRacesCompleted()
        {
            // For all values (circuitIds) in the race dict, when equal to the 
            // selected circuitId, add it to a list and finally get the count.
            TotalRacesCompleted = _raceDict
                .Values
                .Where(circuitId => circuitId == SelectedCircuit.circuitId)
                .Count();
        }
    }
}
