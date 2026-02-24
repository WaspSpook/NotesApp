using System.Resources;

namespace NotesApp.Resources.Localization
{
    public static class AppResources
    {
        private static readonly ResourceManager ResourceManager = 
            new ResourceManager("NotesApp.Resources.Localization.AppResources", typeof(AppResources).Assembly);

        public static string AppTitle => ResourceManager.GetString(nameof(AppTitle)) ?? "Notes";
        public static string AddButton => ResourceManager.GetString(nameof(AddButton)) ?? "+";
        public static string Delete => ResourceManager.GetString(nameof(Delete)) ?? "Delete";
        public static string NotePlaceholder => ResourceManager.GetString(nameof(NotePlaceholder)) ?? "Enter note text...";
        public static string SaveButton => ResourceManager.GetString(nameof(SaveButton)) ?? "Save";
    }
}