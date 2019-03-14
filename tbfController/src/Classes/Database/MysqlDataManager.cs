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
    class DBMysqlDataManager: DBEngine
    { 
        public DBMysqlDataManager(string host_ip, string sql_user, string sql_pass, short sql_port, string sql_db_default) : 
            base(host_ip, sql_user, sql_pass, sql_port, sql_db_default)
        {

        }

        protected override SqlDataReader executeQuery(SqlConnection mssqlConnection, string query)
        {
            throw new NotImplementedException();
        }

        protected override MySqlDataReader executeQuery(MySqlConnection MysqlConnection, string query)
        {
            MySqlCommand MysqlCommand = null;
            MysqlCommand = new MySqlCommand(query, MysqlConnection);
            return MysqlCommand.ExecuteReader();
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

                MysqlData = executeQuery(MysqlConn, "Select user_id from tbf_users where nickname = '"+ sUserName +"' and password = MD5('"+ sPassword +"')");
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

        public override int signUpRegisterUser(string sUserName, string sSecondName, string sForeName, string sPassword, string sEmail, short iIsTrainer = 0)
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
                    return 4;
                }
                //Check, if user is existing + email 
                MysqlData = executeQuery(MysqlConn, "Select user_id from tbf_users where nickname='"+ sUserName +"'");
                while (MysqlData.Read())
                {
                    return 2;
                }
                MysqlData.Close();

                //Check, if user is existing + email 
                MysqlData = executeQuery(MysqlConn, "Select email from tbf_users where email='" + sEmail + "'");
                while (MysqlData.Read())
                {
                    return 3;
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
                    + iIsTrainer + "');");
                MysqlData.Close();
            }
            return 1;
        }

        //Content

        public override List<List<string>> getRoomOverViewData()
        {            
            using (MySqlConnection MysqlConn =
                new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
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
                return createDataMatrix(executeQuery(MysqlConn, "Select * from tbf_rooms"));
            }
        }

        public override List<List<string>> getRoomOverViewData2(int iUserID)
        {
            using (MySqlConnection MysqlConn =
                new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
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
                return createDataMatrix(executeQuery(MysqlConn, "Select DISTINCT tbfr.room_id, tbfr.name, tbfr.description, tbfr.is_private, tbfr.room_icon_url  from tbf_rooms as tbfr "
                                        + "INNER JOIN tbf_user_room_relation ON tbfr.room_id = tbf_user_room_relation.room_id "
                                        + "WHERE tbf_user_room_relation.user_id = " + iUserID + " OR tbfr.is_private = 0" ));
            }
        }

        public override List<List<string>> getWorkoutOverViewData(int iRoomID)
        {
            using (MySqlConnection MysqlConn =
                new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
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
                return createDataMatrix(executeQuery(MysqlConn, "SELECT tbf_workouts.workout_id, tbf_workouts.name, tbf_workouts.description, tbf_workouts.workout_icon_url from tbf_workouts"
                                                                + " WHERE tbf_workouts.room_id = " + iRoomID ));
            }
        }

        public override List<List<string>> getLevelOverviewData(int iWorkoutID)
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
                    return null;
                }
                //Get level matrix               
                return createDataMatrix(executeQuery(MysqlConn, "SELECT tbf_level.level_id, tbf_level.level_grade, tbf_level.description FROM tbf_level WHERE tbf_level.workout_id = " + iWorkoutID));
            }
        }

        public override List<List<string>> getFullExerciseData(int iLevelID)
        {
            using (MySqlConnection MysqlConn =
               new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return null;
                }
                //Get level matrix               
                return createDataMatrix(executeQuery(MysqlConn, "SELECT tbf_exercise.exercise_id, tbf_exercise.name, tbf_exercise.description, tbf_exercise.media_url FROM tbf_exercise " +
                                                                "INNER JOIN tbf_level_exercise_relation ON tbf_exercise.exercise_id = tbf_level_exercise_relation.exercise_id " +
                                                                "WHERE tbf_level_exercise_relation.level_id = " + iLevelID));
            }
        }

        public override List<List<string>> getUserList()
        {
            using (MySqlConnection MysqlConn =
                new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return null;
                }
                //Get tbl_users matrix
                return createDataMatrix(executeQuery(MysqlConn, "Select user_id, nickname, name, forename, email, is_trainer from tbf_users"));
            }
        }

        public override List<List<string>> getUserMemberData(int iRoomID)
        {
            using (MySqlConnection MysqlConn =
                new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return null;
                }
                //Get tbl_users matrix
                return createDataMatrix(executeQuery(MysqlConn, "Select u.user_id, nickname, email, ur.room_id " +
                    "from tbf_users u left join (Select * from tbf_user_room_relation where tbf_user_room_relation.room_id = " +
                    iRoomID + ") ur on u.user_id = ur.user_id;"));
            }
        }

        public override int addNewRoom(int iUserID, string sName, string sDecription, short iIsPrivate, string sIconURL)
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

                //Check, if roomname already exist
                MysqlData = executeQuery(MysqlConn, "SELECT room_id from tbf_rooms where Name = '" + sName + "'");
                while (MysqlData.Read())
                {
                    return 2;
                }
                MysqlData.Close();

                //Add new room
                MysqlData = executeQuery(MysqlConn, "INSERT INTO `" + sql_db_default + "`.`tbf_rooms` (`name`, `description`, `is_private`, `room_icon_url`) VALUES ('"
                    + sName + "', '"
                    + sDecription + "', b'" + iIsPrivate + "', '"
                    + sIconURL + "')");
                MysqlData.Close();

                //Add user to room
                MysqlData = executeQuery(MysqlConn, "INSERT INTO `"
                    + sql_db_default + "`.`tbf_user_room_relation` (`room_id`, `user_id`) VALUES((SELECT room_id from tbf_rooms where Name = '"
                    + sName + "'), '"
                    + iUserID.ToString() + "') ");
                MysqlData.Close();

                return 1;

            }


        }

        public override int updateRoom(int iRoomID,  string sName, string sDecription, short bIs_Private, string sIconURL)
        {
            using (MySqlConnection MysqlConn =
               new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return 2;
                }
                //update existing room              
                executeQuery(MysqlConn, "UPDATE tbf_rooms SET tbf_rooms.name = '" + sName + "', " +
                                            "tbf_rooms.description = '" + sDecription + "', tbf_rooms.is_private = " + bIs_Private + ", " +
                                            "tbf_rooms.room_icon_url = '" + sIconURL + "' WHERE tbf_rooms.room_id = " + iRoomID);
                return 1;
            }
        }

        public override int deleteRoom(int iRoomID)
        {
            using (MySqlConnection MysqlConn =
               new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return 2;
                }
                //Delete also the workouts + levels in the room
                executeQuery(MysqlConn, "DELETE FROM tbf_rooms WHERE tbf_rooms.room_id = " + iRoomID);
                return 1;
            }
        }

        public override int addNewWorkout(int iRoomID, string sName, string sDescription, string sIconURL)
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

                //Check, if workout name already exist
                MysqlData = executeQuery(MysqlConn, "SELECT workout_id from tbf_workouts where Name = '" + sName + "'");
                while (MysqlData.Read())
                {
                    return 2;
                }
                MysqlData.Close();

                //Add new workout
                MysqlData = executeQuery(MysqlConn, "INSERT INTO `" + sql_db_default + "`.`tbf_workouts` (`room_id`, `name`, `description`, `workout_icon_url`) VALUES ("
                    + iRoomID + ", '"
                    + sName + "', '"
                    + sDescription + "', '"
                    + sIconURL + "')");
                MysqlData.Close();
                return 1;

            }
        }

        public override int updateWorkout(int iWorkoutID, int iRoomID, string sName, string sDescription, string sIconURL)
        {
            using (MySqlConnection MysqlConn =
               new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return 2;
                }
                //update existing room              
                executeQuery(MysqlConn, "UPDATE tbf_workouts SET tbf_workouts.name = '" + sName + "', " +
                                            "tbf_workouts.room_id = " + iRoomID + ", " +
                                            "tbf_workouts.description = '" + sDescription + "', " +
                                            "tbf_workouts.workout_icon_url = '" + sIconURL + "' WHERE tbf_workouts.workout_id = " + iWorkoutID);
                return 1;
            }
        }

        public override int deleteWorkout(int iWorkoutID)
        {
            using (MySqlConnection MysqlConn =
               new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return 2;
                }
                //Delete also the workouts + levels in the room
                executeQuery(MysqlConn, "DELETE FROM tbf_workouts WHERE tbf_workouts.workout_id = " + iWorkoutID);
                return 1;
            }
        }

        public override int addNewExercise(int iWorkoutID, int iLevelNumber, string sName, string sLevelDescription, string sExerciseDescription, string sMediaURL)
        {
            using (MySqlConnection MysqlConn =
                new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                MySqlDataReader MysqlData = null;
                int iExerciseID = 0;
                int iLevelID = 0;
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return 3;
                }

                //Add new exercise
                MysqlData = executeQuery(MysqlConn, "INSERT INTO tbf_exercise (name, media_url, description) " +
                                                    "VALUES('"+ sName +"', '"+sMediaURL+"', '"+ sExerciseDescription +"');");
                MysqlData.Close();
                //Add new level
                MysqlData = executeQuery(MysqlConn, "INSERT INTO tbf_level (workout_id, level_grade, description) " +
                                                    "VALUES(" + iWorkoutID + ", " + iLevelNumber + ", '" + sLevelDescription + "');");
                MysqlData.Close();

                //get exercise id
                MysqlData = executeQuery(MysqlConn, "SELECT exercise_id from tbf_exercise where name = '"+ sName + "' AND media_url = '"+ sMediaURL +"' AND description = '"+ sExerciseDescription +"'");
                while (MysqlData.Read())
                {
                    iExerciseID = Convert.ToInt32(MysqlData.GetValue(0));
                }
                MysqlData.Close();

                //get level id
                MysqlData = executeQuery(MysqlConn, "SELECT level_id from tbf_level where workout_id = " + iWorkoutID + " AND level_grade = " + iLevelNumber + " AND description = '" + sLevelDescription + "'");
                while (MysqlData.Read())
                {
                    iLevelID = Convert.ToInt32(MysqlData.GetValue(0));
                }
                MysqlData.Close();

                // add level - exercise relation
                MysqlData = executeQuery(MysqlConn, "INSERT INTO tbf_level_exercise_relation (exercise_id, level_id) " +
                                                    "VALUES(" + iExerciseID + ", " + iLevelID + ");");
                MysqlData.Close();

                return 1;

            }
        }

        public override int updateExercise(int iLevelID, int iExerciseID, int iWorkoutID, int iLevelNumber, string sName, string sLevelDescription, string sExerciseDescription, string sMediaURL)
        {
            using (MySqlConnection MysqlConn =
               new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return 2;
                }
                //update exercise + level data        
                executeQuery(MysqlConn, "UPDATE tbf_exercise SET tbf_exercise.name = '" + sName + "', " +
                                            "tbf_exercise.description = '" + sExerciseDescription + "', " +
                                            "tbf_exercise.media_url = '" + sMediaURL + "' WHERE tbf_exercise.exercise_id = " + iExerciseID);
                executeQuery(MysqlConn, "UPDATE tbf_level SET tbf_level.workout_id = " + iWorkoutID + ", " +
                                            "tbf_level.level_grade = " + iLevelID + ", " +
                                            "tbf_level.description = '" + sLevelDescription + "' WHERE tbf_level.level_id = " + iLevelID);
                return 1;
            }
        }

        public override int deleteExercise(int iExerciseID)
        {
            using (MySqlConnection MysqlConn =
               new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return 2;
                }
                //Delete also the workouts + levels in the room
                executeQuery(MysqlConn, "DELETE FROM tbf_exercise WHERE tbf_exercise.exercise_id = " + iExerciseID);
                return 1;
            }
        }

        public override int deleteLevel(int iLevelID)
        {
            using (MySqlConnection MysqlConn =
               new MySqlConnection("server=" + host_ip + ";database=" + sql_db_default + ";uid=" + sql_user + ";pwd=" + sql_pass + ";"))
            {
                //Connect
                try
                {
                    MysqlConn.Open();
                }
                catch (Exception e)
                {
                    return 2;
                }
                //Delete also the workouts + levels in the room
                executeQuery(MysqlConn, "DELETE FROM tbf_level WHERE tbf_level.level_id = " + iLevelID);
                return 1;
            }
        }


        #region Support functions
        private List<List<string>> createDataMatrix(MySqlDataReader MysqlData)
        {
            //This is a list of lists. 
            //Each list in the list stands for 1 room entry. The global list contains all rooms in form of a list
            List<List<string>> llMatrixContainer = new List<List<string>>();
            //Save the data in the list<list<string>> variable
            int iRowCounter = 0;
            //Each iteration = 1 row
            while (MysqlData.Read())
            {
                List<string> lDataRow = new List<string>();
                //Each iteration = 1 field in the current row
                for (int i = 0; i < MysqlData.FieldCount; i++)
                {
                    lDataRow.Add(MysqlData.GetValue(i).ToString());
                }
                llMatrixContainer.Add(lDataRow);
                iRowCounter++;
            }
            return llMatrixContainer;
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
        #endregion
    }
}
