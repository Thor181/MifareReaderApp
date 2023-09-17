using MifareReaderApp.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    public class PortWorker : IDisposable
    {
        private SerialPort Port { get; set; }
        private bool _disposed = false;

        public delegate void PortDataReceivedEventHandler(string data);
        public event PortDataReceivedEventHandler? OnPortDataReceived;

        public delegate void PortOpenedEventHandler(bool result);
        public event PortOpenedEventHandler OnPortOpened;

        private bool _portIsOpen;
        public bool PortIsOpen
        {
            get
            {
                return _portIsOpen;
            }
            set
            {
                _portIsOpen = value;
                OnPortOpened?.Invoke(value);
            }
        }

        private string PortName => AppConfig.Instance.PortName;

        public PortWorker()
        {
            Port = new SerialPort(PortName, 9600, Parity.None, 8, StopBits.One);

            Port.DataReceived += Port_DataReceived;
            Port.ErrorReceived += Port_ErrorReceived;

            OpenPort();
        }

        private void OpenPort()
        {
            _ = Retry.Do(() => { Port.PortName = PortName; Port.Open(); return true; }, OnPortOpen, TimeSpan.FromSeconds(5), 99, "Port");
        }

        private bool OnPortOpen(bool result)
        {
            if (result == true)
            {
                PortIsOpen = true;
                return false;
            }

            return true;
        }

        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageDialog.ShowDialog($"С портом {PortName} возникла ошибка\n" + e.EventType);
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = Port.ReadExisting();
            OnPortDataReceived?.Invoke(data);
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    Port.Dispose();
            }
        }
        #endregion

    }
}
