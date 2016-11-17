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

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCDatabaseEngine
{
 
    abstract class DBEngine
    {
        //Support

        public abstract object executeQuery(object mysqlConnection, string query);
        public abstract SqlDataReader executeQuery(SqlConnection mssqlConnection, string query);
        public abstract bool testDBConnection();
        
       


    }
    

}
