using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace tbfApp
{
    public partial class SettingsPage : ContentPage
    {
        private ScrollView scroll;
        private StackLayout stackExpand;
        private Entry entryPass;
        private Entry entryColor;
        private Entry entryServerAdress;
        private Entry entryServerPort;
        private Entry entryServerBufferlenght;
        public SettingsPage()
        {
            InitializeComponent();

            scroll = new ScrollView();
            Content = scroll;

            BoxView boxSpace = new BoxView()
            {
                
            };

            Label lablePass = new Label()
            {
                Text = "Bitte das Passwort für die Erweiterten Einstellungen eingeben",
                Font = Font.BoldSystemFontOfSize(20),
            };

            entryPass = new Entry()
            {
                Keyboard = Keyboard.Telephone,
                Placeholder = "Password",
                IsPassword = true,
            };
            //yourTextField.KeyboardType = UIKeyboardTypePhonePad

            Button buttonExpand = new Button()
            {
                Text = "Einstellungen erweitern"
            };
            buttonExpand.Clicked += OnButtonClicked;

            // Build the page1.        
            var stack = new StackLayout();
            stack.Spacing = 1;
            scroll.Content = stack;
            stack.Children.Add(boxSpace);
            stack.Children.Add(lablePass);
            stack.Children.Add(entryPass);
            stack.Children.Add(buttonExpand);

            BoxView boxSpace2 = new BoxView()
            {

            };

            Label lableColor = new Label()
            {
                Text = "Farbe in SECHS-Stelligem Hex-Farbcode eingeben z.B. 009acd",
                Font = Font.BoldSystemFontOfSize(15),
            };

            entryColor = new Entry()
            {
                Placeholder = App.getMenueColor(),
            };
            //yourTextField.KeyboardType = UIKeyboardTypePhonePad

            Label lableServerAdresse = new Label()
            {
                Text = "Serveradresse",
                Font = Font.BoldSystemFontOfSize(15),
            };

            entryServerAdress = new Entry()
            {
                //Placeholder = App.getServerAdress(),
            };

            Label lableServerPort = new Label()
            {
                Text = "Serverport",
                Font = Font.BoldSystemFontOfSize(15),
            };

            entryServerPort = new Entry()
            {
                //Placeholder = App.getServerPort()
            };

            Label lableServerBufferlenght = new Label()
            {
                Text = "Pufferlänge des Servers",
                Font = Font.BoldSystemFontOfSize(15),
            };

            entryServerBufferlenght = new Entry()
            {
                //Placeholder = App.getServerBufferlenght(),
            };

            Button buttonSaveChanges = new Button()
            {
                Text = "Einstellungen speichern"
            };
            buttonSaveChanges.Clicked += ChangeColorClicked;

            stackExpand = new StackLayout();
            stackExpand.Spacing = 1;
            stackExpand.Children.Add(boxSpace2);
            stackExpand.Children.Add(lableColor);
            stackExpand.Children.Add(entryColor);
            stackExpand.Children.Add(lableServerAdresse);
            stackExpand.Children.Add(entryServerAdress);
            stackExpand.Children.Add(lableServerPort);
            stackExpand.Children.Add(entryServerPort);
            stackExpand.Children.Add(lableServerBufferlenght);
            stackExpand.Children.Add(entryServerBufferlenght);

            stackExpand.Children.Add(buttonSaveChanges);
        }
        void OnButtonClicked(object sender, EventArgs e)
        {
            
            if (entryPass.Text.Equals("0815"))
            {
                scroll.Content = stackExpand;
            }
            else
            {
                DisplayAlert("Fehler", "Passwort falsch, wenden Sie sich an ihren Trainer", "OK");
            }
        }

        void ChangeColorClicked(object sender, EventArgs e)
        {
            if (entryColor.Text.Length == 6)
            {
                App.setMenueColor(entryColor.Text);
            }
            else
            {
                DisplayAlert("Fehler", "Farbcode ungültig", "OK");
            }
        }
    }
}
