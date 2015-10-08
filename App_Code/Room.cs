using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BTS.DB;
using BTS.Util;

/// <summary>
/// Summary description for User
/// </summary>
/// 

namespace BTS.Entity
{

    public class Room : CommonEntity
    {

        public int _roomID;
        public int _branchID;
        public string _name;
        public int _seatNo;
        public string _img;
        public string _description;

        // helper info
        public string _branchName;

        public Room()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static Room CreateForm(OdbcDataReader reader)
        {
            Room room = new Room();
            Room.CreateForm(reader, room);
            return room;
        }

        
        public static bool CreateForm(OdbcDataReader reader, Room room)
        {           
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);

                // Map to DB field. Need to change if db changed
                switch (name) {
                    case "room_id": room._roomID = reader.GetInt32(i);
                                      break;
                    case "branch_id": room._branchID = reader.GetInt32(i);
                                      break;
                    case "name": room._name = reader.GetString(i);
                                      break;
                    case "seat_no": room._seatNo = reader.GetInt32(i);
                                      break;
                    case "img": room._img = reader.GetString(i);
                                      break;
                    case "description": room._description = reader.GetString(i);
                                      break;

                    // helper info
                    case "branch_name": room._branchName = reader.GetString(i);
                                      break;

                }
            }
            return reader.HasRows;

        }

        public static string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            return String.Format(" ( room_id LIKE '%{0}%' or name LIKE '%{0}%' or seat_no LIKE '%{0}%' or description LIKE '%{0}%' ) ", qsearch);
        }


        public override bool AddToDB(DBManager db)
        {
            String[] key = { "branch_id", "name", "seat_no", "img", "description" };
            String[] val = { _branchID.ToString(), _name, _seatNo.ToString(), _img, _description};
            return (db.Insert("room", key, val) > 0) ? true : false;
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (_roomID <= 0) return false;
            String[] key = { "branch_id", "name", "seat_no", "img", "description" };
            String[] val = { _branchID.ToString(), _name, _seatNo.ToString(), _img, _description};
            return (db.Update("room", key, val, "room_id="+_roomID) > 0) ? true : false;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (_roomID <= 0) return false;
            return (db.Delete("room", "room_id=" + _roomID) > 0) ? true : false;
        }

        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM room WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            return Room.CreateForm(reader, this);
        }


        public static Room[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            return LoadListFromDBCustom(db, "SELECT * FROM room " + sqlCriteria);
        }

        public static Room[] LoadListFromDBCustom(DBManager db, string sqlAll)
        {
            OdbcDataReader reader = db.Query(sqlAll);
            LinkedList<Room> list = new LinkedList<Room>();
            while (reader.Read())
            {
                list.AddLast(Room.CreateForm(reader));
            }

            Room[] entities = new Room[list.Count];
            int i = 0;
            foreach (Room r in list)
            {
                entities[i++] = r;
            }
            return entities;
        }


        public static string GetRoomID(int id)
        {
            return GetRoomID(id.ToString());
        }

        public static string GetRoomID(string id)
        {
            return Config.ROOM_CODE_PREFIX + StringUtil.FillString(id, "0", Config.ROOM_CODE_LENGTH, true);
        }



        public override string ToString()
        {
            return String.Format("Room [room_id={0}, branch_id={1}, name={2}, seat_no={3}, img={4}, description={5} ]"
                                , _roomID, _branchID, _name, _seatNo, _img, _description);
        }
    }
}
