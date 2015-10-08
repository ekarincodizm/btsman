using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data;
using System.Data.Odbc;
using System.Text;
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

    public class Promotion : CommonEntity
    {

        public int _promotionID;
        public string _promotionName;
        public string _promotionDesc;
        public Course[] _courses;
        public int _cost; // promotion cost
        public bool _isActive;

        // helper info
        public int _fullCost; // summarize of all courses full cost;
        public int _discountedCost; // discount from _cost

        public Promotion()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static Promotion CreateForm(OdbcDataReader reader)
        {
            Promotion promotion = new Promotion();
            Promotion.CreateForm(reader, promotion);
            return promotion;
        }

        
        // This loads only promotion info not course list
        public static bool CreateForm(OdbcDataReader reader, Promotion promotion)
        {                       
            int fCount = reader.FieldCount;
            for (int i = 0; i < fCount; i++)
            {
                string name = reader.GetName(i);

                // Map to DB field. Need to change if db changed
                switch (name) {
                    case "promotion_id": promotion._promotionID = reader.GetInt32(i);
                                      break;
                    case "promotion_name": promotion._promotionName = reader.GetString(i);
                                      break;
                    case "promotion_desc": promotion._promotionDesc = reader.GetString(i);
                                      break;
                    case "cost": promotion._cost = reader.GetInt32(i);
                                 promotion._discountedCost = promotion._cost;
                                      break;    
                    case "is_active": promotion._isActive = reader.GetInt32(i) > 0 ? true : false;
                                      break;

                }
            }
            return reader.HasRows;
        }

        public int GetFullCost()
        {
            if (_courses == null) { return -1; }
            int fullCost = 0;
            for (int i = 0; i < _courses.Length; i++)
            {
                fullCost += _courses[i]._cost;
            }
            return fullCost;
        }

        public static int GetMaxPromotionID(DBManager db)
        {
            OdbcDataReader reader = db.Query("SELECT MAX(promotion_id) from promotion");
            if (reader.Read())
            {
                if (reader.IsDBNull(0)) return 1;
                int maxID = (int)reader.GetInt64(0);
                return maxID;
            }
            return 0;
        }

        public static bool LoadCourseList(DBManager db, Promotion promotion)
        {
            if (promotion==null) return false;
            if (promotion._promotionID <= 0) return false;

            promotion._courses = Course.LoadListFromDBCustom(db, "SELECT c.* FROM promotion_course_mapping pcm, course c WHERE pcm.promotion_id="+promotion._promotionID 
                                            + " AND pcm.course_id=c.course_id");
            promotion.summarizeFullCost(promotion._courses);

            return true;
        }

        public int summarizeFullCost(Course[] courses)
        {
            this._fullCost = 0;
            for (int i = 0; i < courses.Length; i++)
            {
                this._fullCost += courses[i]._cost;
            }
            return this._fullCost;
        }

        public static string GetQSearchSQL(string qsearch)
        {
            if (qsearch == null) return "";
            return String.Format(" (promotion_id LIKE '%{0}%' or promotion_name LIKE '%{0}%' or promotion_desc LIKE '%{0}%' or cost LIKE '%{0}%') ", qsearch);
        }


        public override bool AddToDB(DBManager db)
        {

            try
            {
                db.BeginTransaction(IsolationLevel.ReadCommitted);

                // Add to promotion table
                String[] key = { "promotion_name", "promotion_desc", "cost", "is_active" };
                String[] val = { _promotionName, _promotionDesc, _cost.ToString(), _isActive ? "1" : "0" };
                bool result = (db.Insert("promotion", key, val) > 0) ? true : false;

                // Get Latest inserted ID
                this._promotionID = GetMaxPromotionID(db);

                // Add to promotion_course_mapping table
                if (_courses != null)
                {
                    String[] pckey = { "promotion_id", "course_id" };
                    for (int i = 0; i < _courses.Length; i++)
                    {
                        String[] pcval = { _promotionID.ToString(), _courses[i]._courseID.ToString() };
                        result = result & (db.Insert("promotion_course_mapping", pckey, pcval) > 0 ? true : false);
                    }
                }
                db.Commit();
                return result;
            }
            catch (Exception e)
            {
                db.Rollback();
                return false;
            }
        }

        public override bool UpdateToDB(DBManager db)
        {
            if (_promotionID <= 0) return false;
            
            // Update to promotion table
            String[] key = { "promotion_name", "promotion_desc", "cost", "is_active" };
            String[] val = { _promotionName, _promotionDesc, _cost.ToString(), _isActive ? "1" : "0" };
            bool result = (db.Update("promotion", key, val, "promotion_id=" + _promotionID) > 0) ? true : false;
            
            // Add to promotion_course_mapping table
            if (_courses != null)
            {
                // Delete existing first
                db.Delete("promotion_course_mapping", " promotion_id=" + _promotionID);
                // Add
                String[] pckey = { "promotion_id", "course_id" };
                for (int i = 0; i < _courses.Length; i++)
                {
                    String[] pcval = { _promotionID.ToString(), _courses[i]._courseID.ToString() };
                    result = result & (db.Insert("promotion_course_mapping", pckey, pcval) > 0 ? true : false);
                }
            }
            return result;
        }

        public override bool DeleteToDB(DBManager db)
        {
            if (_promotionID <= 0) return false;
            // First delete from promotion_course_mapping
            db.Delete("promotion_course_mapping", " promotion_id=" + _promotionID);
            // Then delete from promotion
            return (db.Delete("promotion", " promotion_id=" + _promotionID) > 0) ? true : false;
        }

        public override bool LoadFromDB(DBManager db, string sqlCriteria)
        {
            OdbcDataReader reader = db.Query("SELECT * FROM promotion WHERE " + sqlCriteria);
            if (!reader.Read()) return false;
            bool result = Promotion.CreateForm(reader, this);
            // Load course list

            result = Promotion.LoadCourseList(db, this);
            return result;
        }

        public static PromotionMatcher LoadFromDBByMatchingCourses(DBManager db, LinkedList<Course> list)
        {
            Course[] courses = new Course[list.Count];
            int i = 0;
            foreach (Course t in list)
            {
                courses[i++] = t;
            }
            return LoadFromDBByMatchingCourses(db, courses);
        }

        public static PromotionMatcher LoadFromDBByMatchingCourses(DBManager db, Course[] courses)
        {
            /*
                select * from
                (
                    select tmp2.promotion_id,tmp2.promotion_name, count( pcm.course_id) numall, nummatch   from
                        (
                        select promotion_id, promotion_name, count(*) as nummatch from 
                            (
                            select p.*,pcm.course_id from  promotion p,  promotion_course_mapping pcm where p.promotion_id=pcm.promotion_id and (pcm. course_id=1 or pcm. course_id=2 or pcm.course_id=82 ) 
                            ) as tmp1  group by promotion_id
                        ) as tmp2
                        ,
                        (select * from promotion_course_mapping ) as pcm
                         where tmp2.promotion_id = pcm.promotion_id
                         group by tmp2.promotion_id
                ) as match_pro
                where nummatch = numall
                order by numall desc, promotion_id            
            */
            if (courses.Length == 0) return new PromotionMatcher();

            LinkedList<Promotion> result = new LinkedList<Promotion>();
            // Generate where courses list
            StringBuilder sqlCourses = new StringBuilder();
            for (int i = 0; i < courses.Length; i++)
            {
                if (i > 0) sqlCourses.Append(" or ");
                sqlCourses.Append(" pcm.course_id=" + courses[i]._courseID);                
            }
            string sql1 = "select * from "
                        + "( "
                        + "    select tmp2.promotion_id, tmp2.promotion_name, nummatch, count( pcm.course_id) numall from "
                        + "    ( "
                        + "         select promotion_id, promotion_name, count(*) as nummatch from "
                        + "         ( "
                        + "             select p.*,pcm.course_id from  promotion p, promotion_course_mapping pcm "
                        + "             where p.is_active=1 and p.promotion_id=pcm.promotion_id and ( " + sqlCourses + " ) "
                        + "         ) as tmp1  group by promotion_id "
                        + "     ) as tmp2 "
                        + "     , "
                        + "     (select * from promotion_course_mapping ) as pcm "
                        + "     where tmp2.promotion_id = pcm.promotion_id group by tmp2.promotion_id "
                        + ") as match_pro "
                        + "where nummatch = numall "
                        + "order by numall desc, promotion_id ";

            OdbcDataReader reader = db.Query(sql1);
            while (reader.Read())
            {
                int promotionID = reader.GetInt32(0);
                Promotion p = new Promotion();
                p.LoadFromDB(db, " promotion_id=" + promotionID);
                result.AddLast(p);
            }
            /*
            if (!filterSubsetPromotion)
            {
                // convert to array
                Promotion[] proArray = new Promotion[result.Count];
                result.CopyTo(proArray, 0);
                return proArray;
            }*/

            /*
            // filter out subset promotion
            LinkedList<Promotion> filterResult = new LinkedList<Promotion>();
            foreach (Promotion p1 in result)
            {
                bool isSubset = false;
                foreach (Promotion p2 in result)
                {
                    if (p2._courses.Length < p1._courses.Length) continue;
                    if (p2._promotionID == p1._promotionID) continue;
                    
                    bool isSubsetOfThis = true;
                    for (int i = 0; i < p1._courses.Length; i++)
                    {
                        bool found = false;
                        for (int j = 0; j < p2._courses.Length; j++)
                        {
                            if (p1._courses[i]._courseID == p2._courses[j]._courseID)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found) {
                            isSubsetOfThis = false;
                            break;
                        }
                    }
                    // found all courses of p1 in p2 so p1 is subset of p2
                    if (isSubsetOfThis)
                    {
                        isSubset = true;
                    }
                }
                if (!isSubset)
                {
                    filterResult.AddLast(p1);
                }
            }
             */
            //if (courses.Length == 3)
            PromotionMatcher matcher = new PromotionMatcher(new LinkedList<Course>(courses), result);
            matcher.Match();

            // convert to array
          //  Promotion[] filterProArray = new Promotion[matcher._matchedPromotions.Count];
          //  matcher._matchedPromotions.CopyTo(filterProArray, 0);

            return matcher;
        }

        public static LinkedList<Promotion> CopyList(LinkedList<Promotion> plist)
        {
            Promotion[] tmpArray = new Promotion[plist.Count];
            plist.CopyTo(tmpArray, 0);
            return new LinkedList<Promotion>(tmpArray);
        }

        public static Promotion[] LoadListFromDB(DBManager db, string sqlCriteria)
        {
            return LoadListFromDBCustom(db, "SELECT * FROM promotion " + sqlCriteria);
        }

        public static Promotion[] LoadListFromDBCustom(DBManager db, string sqlAll)
        {
            OdbcDataReader reader = db.Query(sqlAll);
            LinkedList<Promotion> list = new LinkedList<Promotion>();
            while (reader.Read())
            {
                list.AddLast(Promotion.CreateForm(reader));
            }

            Promotion[] entities = new Promotion[list.Count];
            int i = 0;
            foreach (Promotion t in list)
            {
                entities[i++] = t;
            }
            return entities;
        }

        public static string GetPromotionID(int id)
        {
            return "P" + StringUtil.FillString(id.ToString(), "0", 4, true);
        }

        public static string GetPromotionID(string id)
        {
            return Config.PROMOTION_CODE_PREFIX + StringUtil.FillString(id, "0", Config.PROMOTION_CODE_LENGTH, true);
        }

        public override string ToString()
        {
            string courseList = "";
            if (_courses != null)
            {
                foreach (Course c in _courses)
                {
                    courseList = courseList + c._courseID + ",";
                }
            }

            return String.Format("Promotion [promotion_id={0}, promotion_name={1}, promotion_desc={2}, cost={3}, courseList={4}, is_active={5}]"
                                , _promotionName, _promotionDesc, _cost.ToString(), courseList, _isActive ? "0" : "1");
        }
    }

    public class PromotionMatcher
    {

        public bool _debug = false;
        protected LinkedList<Course> _clist;
        protected LinkedList<Promotion> _plist;

        public int _cost = -1;
        public LinkedList<Promotion> _matchedPromotions;
        public LinkedList<Course> _matchedCourses;

        public PromotionMatcher()
        {
        }

        public PromotionMatcher(LinkedList<Course> clist, LinkedList<Promotion> plist)
        {
            _clist = clist;
            _plist = plist;
        }

        // Each promotion must match
        public int Match()
        {
            // copy list
            LinkedList<Course> clist_tmp = Course.CopyList(_clist);
            LinkedList<Promotion> plist_tmp = Promotion.CopyList(_plist);
            // result
            LinkedList<Promotion> presult = new LinkedList<Promotion>();
            _cost = -1;
            
            // recursive method. may take a long time
            RunMatch(clist_tmp, plist_tmp, presult);
            return _cost;
        }



        protected void RunMatch(LinkedList<Course> clist, LinkedList<Promotion> plist, LinkedList<Promotion> presult)
        {
            if (plist.Count == 0)
            {
                // end matching
                // calculate cost
                int cost = 0;
                foreach (Promotion p in presult)
                {
                    cost += p._cost;
                }
                foreach (Course c in clist)
                {
                    cost += c._cost;
                }
                // debug
                if (_debug) { PrintCPDetail(cost, clist, presult); }

                // store result if cheaper
                if ((cost < _cost) || (_cost < 0))
                {
                    _cost = cost;
                    _matchedPromotions = Promotion.CopyList(presult);
                    _matchedCourses = Course.CopyList(clist);
                }

                return;
            }

            foreach (Promotion p in plist)
            {
                LinkedList<Course> clist_tmp = Course.CopyList(clist);
                LinkedList<Promotion> presult_tmp = Promotion.CopyList(presult);

                if (TryMath2Promotion(clist_tmp, p))
                {
                    presult_tmp.AddLast(p);
                }

                // copy
                LinkedList<Promotion> plist_tmp = Promotion.CopyList(plist);
                plist_tmp.Remove(p);
                // recursive
                RunMatch(clist_tmp, plist_tmp, presult_tmp);
            }
        }

        // remove courses from list too
        protected bool TryMath2Promotion(LinkedList<Course> clist, Promotion p)
        {
            foreach (Course c1 in p._courses)
            {
                bool found = false;
                foreach (Course c2 in clist)
                {
                    if (c2._courseID == c1._courseID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return false;
            }
            // found
            // remove matched courses from list
            foreach (Course c1 in p._courses)
            {
                foreach (Course c2 in clist)
                {
                    if (c2._courseID == c1._courseID)
                    {
                        clist.Remove(c2);
                        break;
                    }
                }
            }
            return true;
        }

        protected static void PrintCPDetail(int sum, LinkedList<Course> clist, LinkedList<Promotion> plist)
        {

            System.Diagnostics.Debug.WriteLine("-----------------------------------" + sum.ToString());
            foreach (Promotion p in plist)
            {
                System.Diagnostics.Debug.WriteLine("P" + p._promotionID.ToString() + " - " + p._cost.ToString());
                foreach (Course c0 in p._courses)
                {
                    System.Diagnostics.Debug.WriteLine("  C" + c0._courseID.ToString() + " - " + c0._cost.ToString());
                }
            }
            foreach (Course c in clist)
            {
                System.Diagnostics.Debug.WriteLine("C" + c._courseID.ToString() + " - " + c._cost.ToString());
            }
        }
    }


}
