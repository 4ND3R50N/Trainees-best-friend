using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

            Title = "Einstellungen";

            scroll = new ScrollView();
            Content = scroll;

            BoxView boxSpace = new BoxView()
            {
                
            };

            Label lablePass = new Label()
            {
                Text = "Bitte das Passwort für die erweiterten Einstellungen eingeben",
                Font = Font.BoldSystemFontOfSize(20),
            };

            entryPass = new Entry()
            {
                //Keyboard = Keyboard.Telephone,
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
                Placeholder = App.GetMenueColor(),
            };
            //yourTextField.KeyboardType = UIKeyboardTypePhonePad

            Label lableServerAdresse = new Label()
            {
                Text = "Server IP Adresse",
                Font = Font.BoldSystemFontOfSize(15),
            };

            entryServerAdress = new Entry()
            {
                Placeholder = App.GetServerAdress(),
            };

            Label lableServerPort = new Label()
            {
                Text = "Serverport",
                Font = Font.BoldSystemFontOfSize(15),
            };

            entryServerPort = new Entry()
            {
                Placeholder = Convert.ToString(App.GetServerPort())
            };

            Label lableServerBufferlenght = new Label()
            {
                Text = "Pufferlänge des Servers",
                Font = Font.BoldSystemFontOfSize(15),
            };

            entryServerBufferlenght = new Entry()
            {
                Placeholder = Convert.ToString(App.GetServerBufferlenght()),
            };

            Button buttonSaveChanges = new Button()
            {
                Text = "Einstellungen speichern"
            };
            buttonSaveChanges.Clicked += SaveChangesClicked;

            Button buttonStandardSettings = new Button()
            {
                Text = "Standard Einstellungen"
            };
            buttonStandardSettings.Clicked += StandardSettingsClicked;

            // Build the page1.
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
            stackExpand.Children.Add(buttonStandardSettings);
        }
        async void OnButtonClicked(object sender, EventArgs e)
        {
            if (entryPass.Text != null && entryPass.Text.Equals("0815"))
            {
                scroll.Content = stackExpand;
            }
            else
            {
                await DisplayAlert("Fehler", "Passwort falsch, wenden Sie sich an ihren Trainer", "OK");
            }
        }

        async void SaveChangesClicked(object sender, EventArgs e)
        {
            if (entryColor.Text != null )
            {
                //if (entryColor.Text.Length == 6)
                if(Regex.IsMatch(entryColor.Text, @"\A\b[0-9a-fA-F]+\b\Z") && entryColor.Text.Length == 6)
                {
                    App.SetMenueColor(entryColor.Text);
                }
                else if(entryColor.Text.Equals(""))
                {
                    
                }
                else
                {
                    await DisplayAlert("Fehler", "Farbcode ungültig", "OK");
                    return;
                }
            }

            if (entryServerAdress.Text != null)
            {
                int punkteAnzahl = Regex.Matches(entryServerAdress.Text, Regex.Escape(".")).Count;
                if (entryServerAdress.Text.Length >= 6 && entryServerAdress.Text.Length <= 15 && punkteAnzahl == 3)
                {
                    App.SetServerAdress(entryServerAdress.Text);
                }
                else if (entryServerAdress.Text.Equals(""))
                {

                }
                else
                {
                    await DisplayAlert("Fehler", "Serveradresse ungültig", "OK");
                    return;
                }
            }

            if (entryServerPort.Text != null)
            {
                int port;
                if (entryServerPort.Text.Length >= 1 && entryServerPort.Text.Length <= 5 && int.TryParse(entryServerPort.Text, out port))
                {
                    App.SetServerPort(port);
                }
                else if (entryServerPort.Text.Equals(""))
                {

                }
                else
                {
                    await DisplayAlert("Fehler", "Serverport ungültig", "OK");
                    return;
                }
            }

            if (entryServerBufferlenght.Text != null)
            {
                int bufferlenght;
                if (entryServerBufferlenght.Text.Length >= 1 && entryServerBufferlenght.Text.Length <= 5 && int.TryParse(entryServerBufferlenght.Text, out bufferlenght))
                {
                    App.SetServerBufferlenght(bufferlenght);
                }
                else if (entryServerBufferlenght.Text.Equals(""))
                {

                }
                else
                {
                    await DisplayAlert("Fehler", "Pufferlänge des Servers ungültig", "OK");
                    return;
                }
            }
            var answer = await DisplayAlert("Erfolgreich gespeichert", "App muss neu gestartet werden", "Später", "App beenden");
            if (!answer)
            {
                //Restart App TODO
            }
        }

        void StandardSettingsClicked(object sender, EventArgs e)
        {
            //TODO
        }
    }
}
