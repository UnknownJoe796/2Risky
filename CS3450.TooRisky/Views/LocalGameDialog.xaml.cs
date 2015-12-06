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
using CS3450.TooRisky.Model;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CS3450.TooRisky.Views
{
    public sealed partial class LocalGameDialog : ContentDialog
    {
        public List<TextBox> PlayerNames { get; private set; }

        public LocalGameDialog()
        {
            this.InitializeComponent();
            PlayerNames = new List<TextBox>();

            for (var i = 0; i < 6; i++)
            {

                var block = new TextBlock()
                {
                    Text = "Player " + (i + 1),
                    Foreground = ((PlayerNumber) (i + 1)).Color()
                   
                };

                var box = new TextBox();

                PlayerGrid.Children.Add(block);
                Grid.SetColumn(block, 0);
                Grid.SetRow(block, i);

                PlayerGrid.Children.Add(box);
                Grid.SetColumn(box, 1);
                Grid.SetRow(box,i);

                PlayerNames.Add(box);
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
