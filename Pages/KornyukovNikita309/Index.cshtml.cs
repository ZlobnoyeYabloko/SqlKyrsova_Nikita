using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WebApplication2.Pages.KornyukovNikita309.Pharm;

namespace WebApplication2.Pages.KornyukovNikita309
{
    public class IndexModel : PageModel
    {
        public List<SalesInfo> listSale = new List<SalesInfo>();
        public String id = "";
        public int retuv = 0;
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=KornyukovNikita309;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select s.id, p.name, s.price, s.count, c.discount, ph.name " +
                        " from sales s inner join pharmaceuticals p on s.pharmaceuticals_id = p.id " +
                        " inner join cash_receipts c on s.cash_receipts_id=c.id " +
                        " inner join pharmacy_points ph on s.pharmacy_points_id = ph.id ";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SalesInfo salesInfo = new SalesInfo();
                                salesInfo.id = "" + reader.GetInt32(0);
                                salesInfo.pharmaceuticals_id = reader.GetString(1);
                                salesInfo.price = "" + reader.GetFloat(2);
                                salesInfo.count = "" + reader.GetInt32(3);
                                salesInfo.cash_receipts_id = "" + reader.GetInt32(4);
                                salesInfo.pharmacy_points_id = reader.GetString(5);
                                listSale.Add(salesInfo);
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
            String id1 = Request.Form["price"];
            id = id1;
            if (id.Length != 0)
            {
                listSale.Clear();
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=KornyukovNikita309;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    String sql2 = "declare @a int exec @a= dbo.myProc1 @price select @a";
                    using (SqlCommand cmd = new SqlCommand(sql2, connection))
                    {
                        connection.Open();
                        cmd.Parameters.AddWithValue("@price", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                retuv = reader.GetInt32(0);
                                if (retuv != 404)
                                {
                                    SalesInfo salesInfo = new SalesInfo();
                                    salesInfo.id = "" + reader.GetInt32(0);
                                    salesInfo.pharmaceuticals_id = "" + reader.GetInt32(1);
                                    salesInfo.price = "" + reader.GetFloat(2);
                                    salesInfo.count = "" + reader.GetInt32(3);
                                    salesInfo.cash_receipts_id = "" + reader.GetInt32(4);
                                    salesInfo.pharmacy_points_id = "" + reader.GetInt32(5);
                                    listSale.Add(salesInfo);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public class SalesInfo
    {
        public String id;
        public String pharmaceuticals_id;
        public String price;
        public String count;
        public String cash_receipts_id;
        public String pharmacy_points_id;
    }
}
