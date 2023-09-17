using MifareReaderApp.Models;

namespace MifareReaderApp.Stuff.Constants
{
    public class Constants
    {
        public const string AdministratorTabName = "AdministratorTab";
        public const string OperatorTabName = "OperatorTab";

        public static Dictionary<string, bool> YesOrNotValues = new Dictionary<string, bool>()
        {
            {"Да", true},
            {"Нет", false},
        };

        public static Dictionary<string, string> DatabaseTablesLocalizedNames = new()
        {
            { nameof(MfRADbContext.Users), "Пользователи" }
        };

        public static double MaxScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 200;
    }
}
