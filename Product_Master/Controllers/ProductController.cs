using Product_Master.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product_Master.Controllers
{
    public class ProductController : Controller
    {
        public string sqlConn = ConfigurationManager.AppSettings["Connstr"];
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Addproduct()
        {
            ViewBag.Categories = new SelectList(getcategory(), "CategoryId", "CategoryName");
            return View();
        }

        public ActionResult Product(int Pagenumber = 1, int Pagesize= 5)
        {
            List<clsProduct> products = new List<clsProduct>();

            using (SqlConnection con = new SqlConnection(sqlConn))
            {
                string sp = "sp_getproductpages";
                SqlCommand cmd = new SqlCommand(sp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pagenumber", Pagenumber);
                cmd.Parameters.AddWithValue("@Pagesize", Pagesize);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    products.Add(new clsProduct
                    {
                        ProductId = Convert.ToInt32(row["ProductId"]),
                        ProductName = row["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(row["CategoryId"]),
                        CategoryName = row["CategoryName"].ToString(),
                    });
                }
            }
            return View(products);

            // return View();
        }

        [HttpPost]
        public ActionResult Addproduct(clsProduct product)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(sqlConn))
                {
                    try
                    {
                        string insQuery = "insert into tbl_ProductMaster(ProductName,CategoryId) values(@product,@categoryid);";
                        SqlCommand cmd = new SqlCommand(insQuery, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@product", product.ProductName);
                        cmd.Parameters.AddWithValue("@categoryid", product.CategoryId);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return RedirectToAction("Addproduct");
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error in Addproduct :- " + ex.Message);
                    }
                }
            }
            return View(product);
        }

        public List<clsCategory> getcategory()
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
                    });
                }
            }
            return category;
        }

        public ActionResult EditProd(int id)
        {
            using (SqlConnection con = new SqlConnection(sqlConn))
            {
                
                string query = @"SELECT * FROM tbl_ProductMaster WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", id);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sda.Fill(dt);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                clsProduct product = new clsProduct
                {
                    ProductId = Convert.ToInt32(dt.Rows[0]["ProductId"]),
                    ProductName = dt.Rows[0]["ProductName"].ToString()
                };

                return View(product);
            }
        }

        [HttpPost]
        public ActionResult EditProd(clsProduct product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(sqlConn))
                    {
                        string uptQuery = "update tbl_ProductMaster set ProductName = @ProductName  where ProductId = @ProductId ;";
                        SqlCommand cmd = new SqlCommand(uptQuery, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                        cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        //return RedirectToAction("Edit");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error in Edit Product :- " + ex.Message);
                }
            }
            return RedirectToAction("Product");
        }

        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(sqlConn))
                    {
                        string delQuery = "delete from tbl_ProductMaster where ProductId = @Product ;";
                        SqlCommand cmd = new SqlCommand(delQuery, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@Product", id);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        //return RedirectToAction("Category");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error in delete :- " + ex.Message);
                }
            }
            return RedirectToAction("Product");
            //return View();
        }
    }
}