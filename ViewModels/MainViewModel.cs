using System.ComponentModel;
using System.Windows.Input;
using NotesApp.Models;
using NotesApp.Services;
using NotesApp.Views;

namespace NotesApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _database;
        private List<Note> _todayNotes;
        private DateTime _currentDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Note> TodayNotes
        {
            get => _todayNotes;
            set
            {
                _todayNotes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TodayNotes)));
            }
        }

        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDate)));
            }
        }

        public ICommand AddNoteCommand { get; }
        public ICommand EditNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }

        public MainViewModel(DatabaseService database)
        {
            _database = database;
            CurrentDate = DateTime.Today;
            AddNoteCommand = new Command(AddNote);
            EditNoteCommand = new Command<Note>(EditNote);
            DeleteNoteCommand = new Command<Note>(DeleteNote);

            LoadTodayNotes();
        }

        private async void LoadTodayNotes()
        {
            TodayNotes = await _database.GetNotesByDateAsync(CurrentDate);
        }

        private async void AddNote()
        {
            await Shell.Current.GoToAsync(nameof(NotePage), new Dictionary<string, object>
            {
                { "note", new Note { Date = CurrentDate, Content = "" } },
                { "isNew", true }
            });
        }

        private async void EditNote(Note note)
        {
            if (note == null) return;
            await Shell.Current.GoToAsync(nameof(NotePage), new Dictionary<string, object>
            {
                { "note", note },
                { "isNew", false }
            });
        }

        private async void DeleteNote(Note note)
        {
            if (note == null) return;
            bool confirm = await Application.Current.MainPage.DisplayAlert("Удалить", $"Удалить заметку {note.DisplayTitle}?", "Да", "Нет");
            if (confirm)
            {
                await _database.DeleteNoteAsync(note);
                LoadTodayNotes();
            }
        }

        public void OnAppearing()
        {
            LoadTodayNotes();
        }
    }
}