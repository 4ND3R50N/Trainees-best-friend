/**
 * WhiteCode
 *
 * A selfmade Interface controller who shares global functions to subclasses
 *
 * @author		Anderson from WhiteCode
 * @copyright		Copyright (c) 2016
 * @link		http://white-code.org
 * @since		Version 1.0
 */
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace WCDatabaseEngine
{
 
    abstract class DBEngine
    {

        protected string host_ip;
        protected string sql_user;
        protected string sql_pass;
        protected short sql_port;
        protected string sql_db_default;
        

        public DBEngine(string host_ip, string sql_user, string sql_pass, short sql_port, string sql_db_default)
        {
            this.host_ip = host_ip;
            this.sql_user = sql_user;
            this.sql_pass = sql_pass;
            this.sql_port = sql_port;
            this.sql_db_default = sql_db_default;

        }

        //Support

        public abstract MySqlDataReader executeQuery(MySqlConnection MysqlConnection, string query);
        public abstract SqlDataReader executeQuery(SqlConnection MssqlConnection, string query);
        public abstract bool testDBConnection();

        //Content

        public abstract int signUpRegisterUser(string sUserName, string sSecondName, string sForeName, string sPassword, string sEmail, bool isTrainer = false);

    }
    

}
