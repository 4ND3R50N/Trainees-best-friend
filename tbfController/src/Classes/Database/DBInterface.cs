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
using System.Collections.Generic;

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
        protected abstract MySqlDataReader executeQuery(MySqlConnection MysqlConnection, string query);
        protected abstract SqlDataReader executeQuery(SqlConnection MssqlConnection, string query);
        public abstract bool testDBConnection();

        //Content
        public abstract int signUpRegisterUser(string sUserName, string sSecondName, string sForeName, string sPassword, string sEmail, short bIs_Trainer = 0);
        public abstract int loginUser(string sUserName, string sPassword, ref int iUserID);

        public abstract List<List<string>> getRoomOverViewData();
        public abstract List<List<string>> getRoomOverViewData2(int iUserID);
        public abstract List<List<string>> getWorkoutOverViewData(int iRoomID);
        public abstract List<List<string>> getLevelOverviewData(int iWorkoutID);
        public abstract List<List<string>> getFullExerciseData(int iLevelID);
        public abstract List<List<string>> getUserList();
        public abstract List<List<string>> getUserMemberData(int iRoomID);

        public abstract int addNewRoom(int iUserID, string sName, string sDecription,short bIs_Private, string sIconURL);
        public abstract int updateRoom(int iRoomID, string sName, string sDecription, short bIs_Private, string sIconURL);
        public abstract int deleteRoom(int iRoomID);

        public abstract int addNewWorkout(int iRoomID, string sName, string sDescription, string sIconURL);
        public abstract int updateWorkout(int iWorkoutID, int iRoomID, string sName, string sDescription, string sIconURL);
        public abstract int deleteWorkout(int iWorkoutID);

        public abstract int addNewExercise(int iWorkoutID, int iLevelNumber, string sName, string sLevelDescription, string sExerciseDescription, string sMediaURL);
        public abstract int updateExercise(int iLevelID, int iExerciseID, int iWorkoutID, int iLevelNumber, string sName, string sLevelDescription, string sExerciseDescription, string sMediaURL);
        public abstract int deleteExercise(int iExerciseID);
        public abstract int deleteLevel(int iLevelID);
}
    

}
