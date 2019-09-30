using SQLite;

namespace CORMobileApp.Database.Entities
{
    public class SettingsStorage
    {
        [PrimaryKey, AutoIncrement]
        public int Inernal_settingsID { get; set; }
        public string Inernal_ProfileName{ get; set; }
        public bool Apperance_Darkmode { get; set; }
        public bool Notifications_ReciveAll { get; set; }
    }
}
