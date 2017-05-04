using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhiteCode.Network;
namespace tbfContentManager.Classes
{
    static public class RoomManager
    {
        public static bool AddRoomSend(ref simpleNetwork_Client TCPClient, int iUserId, string sTrennzeichen, string sBeschreibung,
            string sPicURL, bool isPrivate, string sRoomName)
        {
            int i_isPrivate_room = 0;
            if (isPrivate)
            {
                i_isPrivate_room = 1;
            }
            else
            {
                i_isPrivate_room = 0;
            }
            if (sRoomName.Length > 0)
            {
                /*WICHTIG FUER SPAETER!
                    Bild muss vorher auf DB geschickt, der schickt dann URL zurueck, dass ist dann die txt_url_room 
                 */
                //MessageBox.Show("#203;" + iUserId + sTrennzeichen + txt_name_room.Text + sTrennzeichen + txt_beschreibung_room.Text + sTrennzeichen + i_isPrivate_room + sTrennzeichen + txt_url_pic_room.Text + sTrennzeichen);
                TCPClient.sendMessage("#203;" + iUserId + sTrennzeichen + sRoomName + sTrennzeichen
                    + sBeschreibung + sTrennzeichen + i_isPrivate_room + sTrennzeichen + sPicURL + sTrennzeichen, true);
                return true;
            }
            return false;
        }

        public static void AddRoomReceive(List<string> tmp)
        {
            if (tmp[1] == "1")
            {
                MessageBox.Show("Der Raum wurde erfolgreich angelegt!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }else if (tmp[1] == "2")
            {
                MessageBox.Show("Der Raumname existiert bereits!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }else if (tmp[1] == "3")
            {
                MessageBox.Show("Der Server hat einen internen Fehler! Bitte kontaktieren Sie einen Administrator!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Unbekannter Protokolfehler!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetAllRoomSend(ref simpleNetwork_Client TCPClient) {
            //TCPClient.changeProtocolFunction(Server_response);
            TCPClient.sendMessage("#201", true);
        }

        public static void GetAllRoomReceive() {

        }

        
    }
}
