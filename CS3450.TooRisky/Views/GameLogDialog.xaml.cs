using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CS3450.TooRisky.Utils;
using System.Linq;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CS3450.TooRisky.Views
{
    public sealed partial class GameLogDialog : ContentDialog
    {
        /// <summary>
        /// Initializes the game log dialog
        /// </summary>
        public GameLogDialog()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Sets content of the game log dialog to the last handful of events.
        /// </summary>
        public void SetContent()
        {
            LogEntries.Text = string.Join("\r\n", GameLog.Events);
        }
    }
}
