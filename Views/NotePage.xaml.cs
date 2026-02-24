using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp.Views
{
    [QueryProperty(nameof(Note), "note")]
    [QueryProperty(nameof(IsNew), "isNew")]
    public partial class NotePage : ContentPage
    {
        private readonly DatabaseService _database;
        private Note _note;
        private bool _isNew;

        public Note Note
        {
            get => _note;
            set
            {
                _note = value;
                LoadNote();
            }
        }

        public bool IsNew
        {
            get => _isNew;
            set => _isNew = value;
        }

        public NotePage(DatabaseService database)
        {
            InitializeComponent();
            _database = database;
        }

        private void LoadNote()
        {
            if (_note != null)
            {
                TitleLabel.Text = _note.DisplayTitle;
                ContentEditor.Text = _note.Content;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (_note == null) return;

            _note.Content = ContentEditor.Text;

            if (_isNew)
            {
                await _database.AddNoteAsync(_note.Date, _note.Content);
            }
            else
            {
                await _database.UpdateNoteAsync(_note);
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}