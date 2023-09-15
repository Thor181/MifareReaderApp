using MifareReaderApp.Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.DataLogic.Stuff
{
    public class HealthLogic : BaseLogic
    {
        public delegate void OnConnectedEventHandler(bool result);
        public event OnConnectedEventHandler OnConnected;

        public void CheckDbAvailable()
        {
            _ = Retry.Do(() => DbAvailable, OnConnectedHandler, TimeSpan.FromSeconds(5), int.MaxValue, "Database"); ;
        }
        
        public bool OnConnectedHandler(bool result)
        {
            OnConnected?.Invoke(result);
            return true;
        }
    }
}
