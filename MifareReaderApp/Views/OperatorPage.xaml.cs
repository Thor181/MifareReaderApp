using MifareReaderApp.ViewModels;
using MifareReaderApp.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MifareReaderApp.Views
{
    public partial class OperatorPage : UserControl, IPage
    {

        public UIElement CustomChild
        {
            get { return (UIElement)this.GetValue(CustomChildProp); }
            set { SetValue(CustomChildProp, value); }
        }

        public static readonly DependencyProperty CustomChildProp 
            = DependencyProperty.Register("CustomChild", typeof(UIElement), typeof(OperatorPage), new PropertyMetadata(new PropertyChangedCallback(CustomChildChanged)));

        public OperatorPageViewModel ViewModel { get; set; }

        public OperatorPage()
        {
            ViewModel = new OperatorPageViewModel();
            InitializeComponent();
        }

        public void BeforeOpen()
        {
            
        }

        public static void CustomChildChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is OperatorPage operatorPage)
            {
                operatorPage.CustomChildGrid.Children.Clear();
                operatorPage.CustomChildGrid.Children.Add(e.NewValue as UIElement);
            }
        }
    }
}
