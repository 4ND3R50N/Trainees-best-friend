/**
 * WhiteCode
 *
 * A selfmade subclass with an global database controller
 *
 * @author		Anderson from WhiteCode
 * @copyright		Copyright (c) 2016
 * @link		http://white-code.org
 * @since		Version 1.0
 */

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace WCDatabaseEngine
{
    class DBMysqlManager: DBEngine
    { 
        public DBMysqlManager(string host_ip, string sql_user, string sql_pass, short sql_port, string sql_db_default) : 
            base(host_ip, sql_user, sql_pass, sql_port, sql_db_default)
        {

        }

        public override SqlDataReader executeQuery(SqlConnection mssqlConnection, string query)
        {
            throw new NotImplementedException();
        }

        public override MySqlDataReader executeQuery(MySqlConnection MysqlConnection, string query)
        {
            MySqlCommand MysqlCommand = null;
            MysqlCommand = new MySqlCommand(query, MysqlConnection);
            return MysqlCommand.ExecuteReader();
        }

        public override List<List<string>> getRoomOverViewData()
        {
            //This is a list of lists. 
            //Each list in the list stands for 1 room entry. The global list contains all rooms in form of a list
            List<List<string>> llRoomOverViewData = new List<List<string>>();

            using (MySqlConnection MysqlConn =
                new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                MySqlDataReader MysqlData = null;
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return null;
                }
                //Get tbl_rooms matrix
                MysqlData = executeQuery(MysqlConn, "Select room_id, name, description, is_private, room_icon_url from tbf_rooms");
                //Save the data in the list<list<string>> variable
                int iRowCounter = 0;
                //Each iteration = 1 row
                while(MysqlData.Read())
                {
                    List<string> lRoom = new List<string>();
                    //Each iteration = 1 field in the current row
                    for (int i = 0; i < MysqlData.FieldCount; i++)
                    {
                        lRoom.Add(MysqlData.GetValue(i).ToString());
                    }
                    llRoomOverViewData.Add(lRoom);
                    iRowCounter++;
                }
                return llRoomOverViewData;
                
            }
        }
        public override int loginUser(string sUserName, string sPassword, ref int iUserID)
        {
            using (MySqlConnection MysqlConn =
                new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                MySqlDataReader MysqlData = null;
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return 3;
                }

                MysqlData = executeQuery(MysqlConn, "Select user_id from tbf_users where nickname = '"+ sUserName +"' and password = MD5('"+ sPassword +"') ");
                //Check, if the data is correct
                while (MysqlData.Read())
                {
                    iUserID = Convert.ToInt32(MysqlData.GetValue(0));
                    return 1;
                }
                MysqlData.Close();
                return 2;

            }
        }

        public override int signUpRegisterUser(string sUserName, string sSecondName, string sForeName, string sPassword, string sEmail, bool isTrainer = false)
        {
            using (MySqlConnection MysqlConn =
                  new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                MySqlDataReader MysqlData = null;
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return 3;
                }
                //Check, if user is existing
                MysqlData = executeQuery(MysqlConn, "Select user_id from tbf_users where nickname='"+ sUserName +"'");
                while (MysqlData.Read())
                {
                    return 2;
                }
                MysqlData.Close();

                //Create new user
                MysqlData = executeQuery(MysqlConn,
                    "INSERT INTO `" + sql_db_default + "`.`tbf_users` (`nickname`, `name`, `forename`, `password`, `email`, `is_trainer`) VALUES ('"
                    + sUserName + "', '"
                    + sSecondName + "', '"
                    + sForeName + "', MD5('"
                    + sPassword + "'), '"
                    + sEmail + "', b'"
                    + Convert.ToString((isTrainer) ? 1 : 0) + "');");
            }
            return 1;
        }

        public override bool testDBConnection()
        {
            using (MySqlConnection MysqlConn =
                   new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e) {                    
                    return false;
                }
                return true;
            }
        }
    }
}
