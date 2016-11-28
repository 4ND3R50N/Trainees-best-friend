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
                    + sForeName + "', '"
                    + sPassword + "', '"
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
