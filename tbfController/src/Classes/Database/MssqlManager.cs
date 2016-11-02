/**
 * WhiteCode
 *
 * A selfmade subclass with an global database controller
 *
 * @author		Anderson from WhiteCode
 * @copyright		Copyright (c) 2016
 * @link		http://whitecode.org
 * @since		Version 1.0
 */
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Database
{
    class DBMssqlProtesManager : DBEngine
    {

        string host_ip;
        string sql_user;
        string sql_pass;
        short sql_port;
        string sql_db_protes;
        string sql_db_game;

        //Queries


        public DBMssqlProtesManager(string host_ip, string sql_user, string sql_pass, short sql_port, string sql_db_protes, string sql_db_game)
        {
            this.host_ip = host_ip;
            this.sql_user = sql_user;
            this.sql_pass = sql_pass;
            this.sql_port = sql_port;
            this.sql_db_protes = sql_db_protes;
            this.sql_db_game = sql_db_game;
        }

        public override SqlDataReader executeQuery(SqlConnection mssqlConnection, string query)
        {
            SqlCommand mssqlCommand = null;
            mssqlCommand = new SqlCommand(query, mssqlConnection);
            return mssqlCommand.ExecuteReader();
        }

        public override object executeQuery(object mysqlConnection, string query)
        {
            throw new NotImplementedException();
        }

        public override bool testDBConnection()
        {
            using (SqlConnection mssqlConnection =
              new SqlConnection("Server=" + host_ip + ";Database=" + sql_db_protes + ";User Id=" + sql_user + ";Password=" + sql_pass + ";MultipleActiveResultSets=True;"))
            {
                try
                {
                    mssqlConnection.Open();
                }
                catch (Exception)
                {
                    //dataHandler.writeInMainlog("MSSQL Connect failed. [testDBConnection]", true);
                    return false;
                }

            }
            return true;
        }

       
    }
}
