using SQLite;
using NotesApp.Models;

namespace NotesApp.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Note>().Wait();
        }

        public Task<List<Note>> GetNotesByDateAsync(DateTime date)
        {
                var start = date.Date;
				var end = date.Date.AddDays(1);
				return _database.Table<Note>()
					.Where(n => n.Date >= start && n.Date < end)
					.OrderBy(n => n.Version)
					.ToListAsync();
        }

        public async Task<int> AddNoteAsync(DateTime date, string content)
        {
            var existingNotes = await GetNotesByDateAsync(date);
            int nextVersion = existingNotes.Any() ? existingNotes.Max(n => n.Version) + 1 : 1;

            var note = new Note
            {
                Date = date.Date,
                Version = nextVersion,
                Content = content
            };
            return await _database.InsertAsync(note);
        }

        public Task<int> UpdateNoteAsync(Note note)
        {
            return _database.UpdateAsync(note);
        }

        public Task<int> DeleteNoteAsync(Note note)
        {
            return _database.DeleteAsync(note);
        }
    }
}