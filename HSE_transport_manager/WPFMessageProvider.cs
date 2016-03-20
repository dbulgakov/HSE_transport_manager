using System.Windows;
using HSE_transport_manager.Common.Interfaces;

namespace HSE_transport_manager
{
    class WpfMessageProvider:IDialogProvider
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
