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

        public delegate void PortDataReceived(string data);
        public event PortDataReceived? OnPortDataReceived;

        public bool PortIsOpen => Port.IsOpen;
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
            try
            {
                Port.Open();
            }
            catch (Exception e)
            {
                MessageDialog.ShowDialog($"При открытии соединения к порту {PortName} возникла ошибка.\n\n" + e.Message);
            }
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
