using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace WebApplication2.Pages.KornyukovNikita309
{
    public class CreateModel : PageModel
    {
        public SalesInfo salesInfo = new SalesInfo();
        public String errorMessage = "";
        public String succesMessage = "";
        public List<IdsToSales> idList = new List<IdsToSales>();
        public List<IdsToSales> idList2 = new List<IdsToSales>();
        public List<IdsToSales> idList3 = new List<IdsToSales>();
        String pName = "";
        String cDiscount = "";
        String ppName = "";

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=KornyukovNikita309;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT name FROM pharmaceuticals";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IdsToSales idsInfo = new IdsToSales();
                                idsInfo.pharmId = reader.GetString(0);
                                idList.Add(idsInfo);
                            }
                        }
                    }
                    String sql1 = "select discount from cash_receipts";
                    using (SqlCommand cmd = new SqlCommand(sql1, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IdsToSales idsInfo = new IdsToSales();
                                idsInfo.cashRpId = "" + reader.GetInt32(0);
                                idList2.Add(idsInfo);
                            }
                        }
                    }
                    String sql2 = "select name from pharmacy_points";
                    using (SqlCommand cmd = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IdsToSales idsInfo = new IdsToSales();
                                idsInfo.pharmPointId = reader.GetString(0);
                                idList3.Add(idsInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception" + ex.ToString());
            }
        }
        public void OnPost()
        {
            OnGet();
            salesInfo.pharmaceuticals_id = Request.Form["pharmaceuticals_id"];
            salesInfo.price = Request.Form["price"];
            salesInfo.count = Request.Form["count"];
            salesInfo.cash_receipts_id = Request.Form["cash_receipts_id"];
            salesInfo.pharmacy_points_id = Request.Form["pharmacy_points_id"];
            if (salesInfo.pharmaceuticals_id.Length == 0 || salesInfo.price.Length == 0 || salesInfo.count.Length == 0 ||
                salesInfo.cash_receipts_id.Length == 0 || salesInfo.pharmacy_points_id.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }
            //save the new sale into the database
            try
            {
                String connectionString = "Data Source =.\\sqlexpress; Initial Catalog = KornyukovNikita309; Integrated Security = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql1 = "select p.id, c.id, pp.id from pharmaceuticals p, cash_receipts c,  pharmacy_points pp " +
                    " where p.name=@name and c.discount=@discount and pp.name=@namepp";
                    using (SqlCommand cmd = new SqlCommand(sql1, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", salesInfo.pharmaceuticals_id);
                        cmd.Parameters.AddWithValue("@discount", salesInfo.cash_receipts_id);
                        cmd.Parameters.AddWithValue("@namepp", salesInfo.pharmacy_points_id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pName = "" + reader.GetInt32(0);
                                cDiscount = "" + reader.GetInt32(1);
                                ppName = "" + reader.GetInt32(2);
                            }
                        }
                    }
                    String sql = "insert into sales " + "(pharmaceuticals_id, price, count, cash_receipts_id, pharmacy_points_id) " +
                        "values (@pharmaceuticals_id, @price, @count, @cash_receipts_id, @pharmacy_points_id); ";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@pharmaceuticals_id", pName);
                        cmd.Parameters.AddWithValue("@price", salesInfo.price);
                        cmd.Parameters.AddWithValue("@count", salesInfo.count);
                        cmd.Parameters.AddWithValue("@cash_receipts_id", cDiscount);
                        cmd.Parameters.AddWithValue("@pharmacy_points_id", ppName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            salesInfo.price = "";
            salesInfo.count = "";
            succesMessage = "New sale added";
            Response.Redirect("/KornyukovNikita309/Index");
        }
    }
    public class IdsToSales
    {
        public String pharmId;
        public String cashRpId;
        public String pharmPointId;
    }
}
