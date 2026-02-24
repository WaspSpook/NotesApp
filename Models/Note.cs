using SQLite;

namespace NotesApp.Models
{
    [Table("Notes")]
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Version { get; set; }
        public string Content { get; set; } = "";

        [Ignore]
        public string DisplayTitle => $"{Date:yyyy-MM-dd} V{Version}";
    }
}