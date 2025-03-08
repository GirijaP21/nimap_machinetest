using Product_Master.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer;
using System.Web.UI.WebControls;

namespace Product_Master.Controllers
{
    public class CategoryController : Controller
    {
        public string sqlConn = ConfigurationManager.AppSettings["Connstr"];
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCat()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            using (SqlConnection con = new SqlConnection(sqlConn))
            {

                string query = @"SELECT * FROM tbl_CategoryMaster WHERE CategoryId = @CategoryId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CategoryId", id);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sda.Fill(dt);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                clsCategory category = new clsCategory
                {
                    CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]),
                    CategoryName = dt.Rows[0]["CategoryName"].ToString()
                };

                return View(category);
            }

            //return View(category);
        }

        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(sqlConn))
                    {
                        string delQuery = "delete from tbl_CategoryMaster where CategoryId = @category ;";
                        SqlCommand cmd = new SqlCommand(delQuery, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@category", id);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        //return RedirectToAction("Category");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error in Category Add :" + ex.Message);
                }
            }
            return RedirectToAction("Category");
            //return View();
        }

        public ActionResult Category()
        {
            List<clsCategory> category = new List<clsCategory>();

            using (SqlConnection con = new SqlConnection(sqlConn))
            {
                string selectQuery = "select * from tbl_CategoryMaster;";
                SqlCommand cmd = new SqlCommand(selectQuery, con);
                //cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    category.Add(new clsCategory
                    {
                        CategoryId = Convert.ToInt32(row["CategoryId"]),
                        CategoryName = row["CategoryName"].ToString()
                    }
                        );
                }
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult AddCat(clsCategory category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(sqlConn))
                    {
                        string insQuery = "insert into tbl_CategoryMaster(CategoryName) values(@category) ;";
                        SqlCommand cmd = new SqlCommand(insQuery, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@category", category.CategoryName);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return RedirectToAction("AddCat");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error in Category Add :" + ex.Message);
                }
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(clsCategory category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(sqlConn))
                    {
                        string uptQuery = "update tbl_CategoryMaster set CategoryName = @categoryName  where CategoryId = @categoryId ;";
                        SqlCommand cmd = new SqlCommand(uptQuery, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@categoryId", category.CategoryId);
                        cmd.Parameters.AddWithValue("@categoryName", category.CategoryName);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        //return RedirectToAction("Edit");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error in Category edit :"+ ex.Message);
                }
            }
            return RedirectToAction("Edit");
            //return View(category);
        }

        //[HttpPost]
        //public ActionResult Delete(clsCategory category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (SqlConnection con = new SqlConnection(sqlConn))
        //        {
        //            string delQuery = "delete from tbl_CategoryMaster where CategoryName = @category ;";
        //            SqlCommand cmd = new SqlCommand(delQuery, con);
        //            con.Open();
        //            cmd.Parameters.AddWithValue("@category", category.CategoryName);
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //            return RedirectToAction("Category");
        //        }
        //    }
        //    return View(category);
        //}

    }
}