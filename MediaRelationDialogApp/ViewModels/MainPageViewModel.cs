﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using MediaRelationDialogApp.Services;
using CommonClassLibrary.DTO;
using Microsoft.Maui.Storage;

namespace MediaRelationDialogApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        public ObservableCollection<RundownDTO> Records { get; set; } = new ObservableCollection<RundownDTO>();

        private RundownDTO _selectedRecord;
        public RundownDTO SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                _selectedRecord = value;
                OnPropertyChanged(nameof(SelectedRecord));
                OnPropertyChanged(nameof(IsRecordSelected));
                UpdateCommandStates();
                SelectedItem = null;
                SelectedDetail = null;

                if (_selectedRecord == null)
                {
                    StatusMessage = "Vælg venligst en rækkefølge først.";
                }
                else
                {
                    StatusMessage = $"Valgt rækkefølge: {_selectedRecord.Name}";
                }
            }
        }

        private RundownItemDTO _selectedItem;
        public RundownItemDTO SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(IsItemSelected));
                UpdateCommandStates();
                SelectedDetail = null;

                if (_selectedItem == null)
                {
                    StatusMessage = "Vælg venligst en historie";
                }
                else
                {
                    StatusMessage = $"Valgt historie: {_selectedRecord.Name} \u2192 {_selectedItem.Name}";
                }
            }
        }

        private DetailDTO _selectedDetail;
        public DetailDTO SelectedDetail
        {
            get => _selectedDetail;
            set
            {
                _selectedDetail = value;
                OnPropertyChanged(nameof(SelectedDetail));
                OnPropertyChanged(nameof(IsDetailSelected));
                UpdateCommandStates();

                if (_selectedDetail == null)
                {
                    StatusMessage = "Vælg venligst et element.";
                }
                else
                {
                    StatusMessage = $"Valgt element: {_selectedRecord.Name} \u2192 {_selectedItem.Name} \u2192 {_selectedDetail.Title}";
                }
            }
        }

        public bool IsRecordSelected => SelectedRecord != null;
        public bool IsItemSelected => SelectedItem != null;
        public bool IsDetailSelected => SelectedDetail != null;
        public bool IsFileSelected => _selectedFile != null;

        private FileResult _selectedFile;

        public ICommand PickFileCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ReloadCommand { get; }

        public MainPageViewModel(ApiService apiService)
        {
            _apiService = apiService;
            PickFileCommand = new Command(async () => await PickFileAsync(), () => IsDetailSelected);
            SaveCommand = new Command(async () => await SaveDetailAsync(), () => CanSaveDetail);
            ReloadCommand = new Command(async () => await ReloadDataAsync());

            LoadDataAsync();
        }

        // Property for at tjekke om knappen skal kunne aktiveres
        public bool CanSaveDetail => IsRecordSelected && IsItemSelected && IsDetailSelected && IsFileSelected;

        // Hent data fra API'et
        private async Task LoadDataAsync()
        {
            try
            {
                StatusMessage = "Henter rækkefølger fra databasen...";
                var rundowns = await _apiService.GetRecordsAsync();

                Records.Clear();
                if (rundowns != null && rundowns.Any())
                {
                    foreach (var rundown in rundowns)
                    {
                        Records.Add(rundown);
                    }
                    StatusMessage = "Rækkefølger hentet!";
                }
                else
                {
                    StatusMessage = "Ingen rækkefølger fundet.";
                }

                UpdateCommandStates();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fejl ved hentning af rækkefølger: {ex.Message}";
            }
        }

        // Nulstil udvalgte elementer ved genindlæsning
        private async Task ReloadDataAsync()
        {
            SelectedRecord = null;
            Records.Clear();
            _selectedFile = null;
            OnPropertyChanged(nameof(IsFileSelected));
            OnPropertyChanged(nameof(IsRecordSelected));
            OnPropertyChanged(nameof(IsItemSelected));
            OnPropertyChanged(nameof(IsDetailSelected));
            StatusMessage = "Data nulstillet og genindlæses...";
            await LoadDataAsync();
        }

        // Filvælger
        private async Task PickFileAsync()
        {
            if (!IsDetailSelected)
            {
                StatusMessage = "Du skal vælge en Detail først, før du kan vælge en fil.";
                return;
            }
            try
            {
                _selectedFile = await FilePicker.Default.PickAsync();

                if (_selectedFile != null)
                {
                    StatusMessage = $"Valgte fil: {_selectedFile.FileName}";
                    UpdateCommandStates(); // Aktiverer Gem knappen, hvis både fil og detail er valgt
                }
                else
                {
                    StatusMessage = "Ingen fil valgt. Prøv igen.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fejl ved filvalg: {ex.Message}";
            }
        }

        public async Task SaveDetailAsync()
        {
            if (!CanSaveDetail)
            {
                StatusMessage = "Fejl: Du skal vælge både en Rækkefølge, en historie, et element og en fil før du kan gemme.";
                return;
            }

            try
            {
                SelectedDetail.VideoPath = _selectedFile.FullPath;

                var response = await _apiService.UpdateDetailAsync(SelectedRecord.UUID, SelectedDetail);

                if (response != null)
                {
                    StatusMessage = "Historie opdateret succesfuldt!";
                }
                else
                {
                    StatusMessage = "Fejl ved opdatering af historie.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fejl ved opdatering af historie: {ex.Message}";
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateCommandStates()
        {
            OnPropertyChanged(nameof(CanSaveDetail));
            ((Command)PickFileCommand).ChangeCanExecute();
            ((Command)SaveCommand).ChangeCanExecute();
        }
    }
}