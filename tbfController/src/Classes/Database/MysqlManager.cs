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

using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace Database
{
    class clsDBMysqlProtesManager: DBEngine
    {

        

        public clsDBMysqlProtesManager(string host_ip, string sql_user, string sql_pass, short sql_port, string sql_db_protes, string sql_db_game)
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
            throw new NotImplementedException();
        }
    }
}
