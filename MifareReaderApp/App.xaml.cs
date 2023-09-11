using System.Configuration;
using System.Data;
using System.Windows;

namespace MifareReaderApp
{
    public partial class App : Application
    {
        public static void DispatcherInvoke(Action action)
        {
            App.Current.Dispatcher.Invoke(action);
        }
    }


}
