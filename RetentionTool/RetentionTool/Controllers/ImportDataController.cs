using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;

namespace RetentionTool.Controllers
{
    public class ImportDataController : Controller
    {
        // GET: ImportData
        public ActionResult Index()
        {
            return View();
        }

        SqlConnection sqlcon = new SqlConnection("workstation id=RetentionTool.mssql.somee.com;packet size=4096;user id=dineshrays_SQLLogin_1;pwd=dinesh@123;data source=RetentionTool.mssql.somee.com;persist security info=False;initial catalog=RetentionTool");
        public ActionResult Upload()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string Upload(HttpPostedFileBase uploadFile)
        {//
            StringBuilder strValidations = new StringBuilder(string.Empty);
            try
            {
                if (uploadFile.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads"),
                   Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(filePath);
                    DataSet ds = new DataSet();

                    //A 32-bit provider which enables the use of

                    //string ConnectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 xml;HDR=Yes;IMEX=1\"");
                    // string ConnectionString = "Provider=Microsoft.ACE.OLEDB.4.0;DataSource=" + filePath + ";Extended Properties=Excel 8.0;HDR=YES;";


                    //foreach (string ConnectionString in new[]
                    // {
                    //     "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;",
                    //        "Provider=Microsoft.Jet.OLEDB.4.0;Data source={0};Extended Properties=Excel 8.0;"
                    //})

                    string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;";
                    {
                        string connectionstr = String.Format(ConnectionString, filePath);
                        using (OleDbConnection conn = new OleDbConnection(connectionstr))
                        {
                            conn.Open();
                            using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
                            {
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                string query = "SELECT * FROM [" + sheetName + "]";
                                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                                //DataSet ds = new DataSet();
                                adapter.Fill(ds, "Items");
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        sqlcon.Open();
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            //Now we can insert this data to database...
                                            string empid = ds.Tables[0].Rows[i]["Employee ID"].ToString();
                                            string empname = ds.Tables[0].Rows[i]["First Name"].ToString() + " " + ds.Tables[0].Rows[i]["Last Name"].ToString();
                                            //string managerid = ds.Tables[0].Rows[i]["Manager ID"].ToString();
                                            // int employeeexperience = (int)ds.Tables[0].Rows[i]["Employee Experience"];


                                            SqlCommand sqlCommand = new SqlCommand("insert into PersonalInfo(EmpCode,Name,IsActive) values('" + empid + "','" + empname + "', '1'); select SCOPE_IDENTITY();", sqlcon);

                                            string PersonalInfoIdstr = sqlCommand.ExecuteScalar().ToString();
                                            int PersonalInfoId = Convert.ToInt32(PersonalInfoIdstr);

                                            string skills = ds.Tables[0].Rows[i]["Worker Skills as Text"].ToString();

                                            if (String.IsNullOrEmpty(skills))
                                            {

                                            }
                                            else
                                            {
                                                string[] skill = skills.Split(',');


                                                for (int j = 0; j < skill.Length; j++)
                                                {
                                                    string skillName = skill[j];
                                                    SqlDataAdapter cmd = new SqlDataAdapter("Select  id  from Skills where Name='" + skillName + "'", sqlcon);
                                                    //    string skillid = cmd.ExecuteScalar().ToString();
                                                    DataTable dt = new DataTable();
                                                    cmd.Fill(dt);
                                                    //Int64.Parse(cmd.ExecuteScalar().ToString());
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        string SkillIdstr = dt.Rows[0]["Id"].ToString();
                                                        int SkillId = Convert.ToInt32(SkillIdstr);
                                                        sqlCommand = new SqlCommand("insert into EmployeeSkills(P_Id,CommonField_Id,Skills_Id,IsActive) values(" + PersonalInfoId + ",'3'," + SkillId + " ,'1')", sqlcon);
                                                        sqlCommand.ExecuteNonQuery();


                                                    }
                                                    else
                                                    {
                                                        SqlCommand cmd1 = new SqlCommand("insert into Skills(Name,CommonField_Id,IsActive) " +
                                                            "values('" + skill[j] + "', '3', '1'); select SCOPE_IDENTITY();", sqlcon);
                                                        //cmd1.ExecuteNonQuery();
                                                        string SkillIdstr = cmd1.ExecuteScalar().ToString();
                                                        int SkillId = Convert.ToInt32(SkillIdstr);
                                                        sqlCommand = new SqlCommand("insert into EmployeeSkills(P_Id,CommonField_Id,Skills_Id,IsActive) values(" + PersonalInfoId + ",'3'," + SkillId + " ,'1')", sqlcon);
                                                        sqlCommand.ExecuteNonQuery();
                                                    }
                                                }
                                            }


                                            // sqlcon.Open();
                                            //SqlCommand cmd = new SqlCommand("insert into tblName(empid,empname) values('"+empid+"','"+empname+"')", sqlcon);
                                            //// SqlCommand cmd = new SqlCommand("Select top 5 id  from Skills", sqlcon);
                                            // cmd.ExecuteNonQuery();
                                            ////SqlDataReader dr = cmd.ExecuteReader();
                                            ////while (dr.Read())
                                            ////{
                                            ////    int skillid = int.Parse(dr["Id"].ToString());
                                            ////}

                                        }

                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            string empid = ds.Tables[0].Rows[i]["Employee ID"].ToString();
                                            string managerid = ds.Tables[0].Rows[i]["Manager ID"].ToString();
                                            // string managerid = ds.Tables[0].Rows[i]["Manager ID"].ToString();
                                            string doj = ds.Tables[0].Rows[i]["DOJ"].ToString();
                                            string department = ds.Tables[0].Rows[i]["Department"].ToString();
                                            string designation = ds.Tables[0].Rows[i]["Designation"].ToString();
                                            string worklocation = ds.Tables[0].Rows[i]["Work Location"].ToString();

                                            //SqlDataAdapter cmd = new SqlDataAdapter("Select  id  from Skills where Name='" + skillName + "'", sqlcon);
                                            SqlCommand sqlcmd = new SqlCommand("Select  id  from PersonalInfo where EmpCode='" + empid + "'", sqlcon);
                                            string pid_empidstr = sqlcmd.ExecuteScalar().ToString();
                                            Int32 pid_empid = Convert.ToInt32(pid_empidstr);


                                            sqlcmd = new SqlCommand("Select  id  from PersonalInfo where EmpCode='" + managerid + "'", sqlcon);
                                            string pid_mgridstr = sqlcmd.ExecuteScalar().ToString();
                                            Int32 pid_mgrid = Convert.ToInt32(pid_mgridstr);


                                            sqlcmd = new SqlCommand("insert into CurrentInfo(P_Id,Designation,DOJ,ReportingManager,WorkLocation,Department)" +
                                                " values(" + pid_empid + ",'" + designation + "'," + doj + ",'" + pid_mgridstr + "','" + worklocation + "','" + department + "')", sqlcon);
                                            sqlcmd.ExecuteNonQuery();
                                        }
                                    }
                                }


                            }
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return "";
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
    
