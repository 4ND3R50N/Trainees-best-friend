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
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WCDatabaseEngine
{
    class DBMssqlDataManager : DBEngine
    {      

        //Queries


        public DBMssqlDataManager(string host_ip, string sql_user, string sql_pass, short sql_port, string sql_db_default)
             : base(host_ip, sql_user, sql_pass, sql_port, sql_db_default)
        {    
        }

        public override int addNewRoom(int iUserID, string sName, string sDecription, short bIs_Private, string sIconURL)
        {
            throw new NotImplementedException();
        }

        protected override SqlDataReader executeQuery(SqlConnection mssqlConnection, string query)
        {
            SqlCommand mssqlCommand = null;
            mssqlCommand = new SqlCommand(query, mssqlConnection);
            return mssqlCommand.ExecuteReader();
        }

        protected override MySql.Data.MySqlClient.MySqlDataReader executeQuery(MySql.Data.MySqlClient.MySqlConnection mysqlConnection, string query)
        {
            throw new NotImplementedException();
        }

        public override List<List<string>> getFullExerciseData(int iLevelID)
        {
            throw new NotImplementedException();
        }

        public override List<List<string>> getLevelOverviewData(int iWorkoutID)
        {
            throw new NotImplementedException();
        }

        public override List<List<string>> getRoomOverViewData()
        {
            throw new NotImplementedException();
        }

        public override List<List<string>> getRoomOverViewData2(int iUserID)
        {
            throw new NotImplementedException();
        }

        public override List<List<string>> getWorkoutOverViewData(int iRoomID)
        {
            throw new NotImplementedException();
        }

        public override int loginUser(string sUserName, string sPassword, ref int iUserID)
        {
            throw new NotImplementedException();
        }

        public override int signUpRegisterUser(string sUserName, string sSecondName, string sForeName, string sPassword, string sEmail, short isTrainer = 0)
        {
            throw new NotImplementedException();
        }

        public override bool testDBConnection()
        {
            using (SqlConnection mssqlConnection =
              new SqlConnection("Server=" + host_ip + ";Database=" + sql_db_default + ";User Id=" + sql_user + ";Password=" + sql_pass + ";MultipleActiveResultSets=True;"))
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

        public override int updateRoom(int iRoomID,  string sName, string sDecription, short bIs_Private, string sIconURL)
        {
            throw new NotImplementedException();
        }

        public override int deleteRoom(int iRoomID)
        {
            throw new NotImplementedException();
        }
    }
}
