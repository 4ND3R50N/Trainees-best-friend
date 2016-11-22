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

        public override object executeQuery(object mysqlConnection, string query)
        {
            throw new NotImplementedException();
        }

        public override bool testDBConnection()
        {
            MySqlDataReader mysqlDataReader = null;

            using (MySqlConnection mysqlConnection =
                   new MySqlConnection("Server=" + host_ip + ";Database=" + sql_db_default + ";User Id=" + sql_user + ";Password=" + sql_pass + ";MultipleActiveResultSets=True;"))
            {
                try
                {
                    mysqlConnection.Open();
                }
                catch (Exception) { 
                
                    return false;
                }
                return true;
            }
        }
    }
}
