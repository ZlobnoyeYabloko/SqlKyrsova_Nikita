using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebApplication2.Pages.KornyukovNikita309.Pharm
{
    public class CreateModel : PageModel
    {
        public PharmInfo pharmInfo = new PharmInfo();
        public String errorMessage = "";
        public String succesMessage = "";
        public List<IdsToPharm> idList = new List<IdsToPharm>();
        public List<IdsToPharm> idList2 = new List<IdsToPharm>();
        String name1 = "";
        String Type = "";
        String connectionString1 = "Data Source =.\\sqlexpress; Initial Catalog = KornyukovNikita309; Integrated Security = True";
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=KornyukovNikita309;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT name FROM manufacturers";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IdsToPharm idsInfo = new IdsToPharm();
                                idsInfo.manufacturesId = "" + reader.GetString(0);
                                idList.Add(idsInfo);
                            }
                        }
                    }
                    String sql1 = "select Type from pharmaceutical_types";
                    using (SqlCommand cmd = new SqlCommand(sql1, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IdsToPharm idsInfo = new IdsToPharm();
                                idsInfo.PharmTypeId = "" + reader.GetString(0);
                                idList2.Add(idsInfo);
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
            pharmInfo.name = Request.Form["name"];
            pharmInfo.packing = Request.Form["packing"];
            pharmInfo.price = Request.Form["price"];
            pharmInfo.manufactures_id = Request.Form["manufactures_id"];
            pharmInfo.pharmaceuticals_types_id = Request.Form["pharmaceuticals_types_id"];
            if (pharmInfo.name.Length == 0 || pharmInfo.packing.Length == 0 || pharmInfo.price.Length == 0 ||
                pharmInfo.manufactures_id.Length == 0 || pharmInfo.pharmaceuticals_types_id.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }
            try
            {
               
                using (SqlConnection connection = new SqlConnection(connectionString1))
                {
                    connection.Open();
                    String sql1 = "select m.id, p.id from manufacturers m, pharmaceutical_types p " +
                        " where name=@name and Type=@Type";
                    using (SqlCommand cmd = new SqlCommand(sql1, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", pharmInfo.manufactures_id);
                        cmd.Parameters.AddWithValue("@Type", pharmInfo.pharmaceuticals_types_id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                name1 = "" + reader.GetInt32(0);
                                Type = "" + reader.GetInt32(1);
                            }
                        }
                    }
                    String sql = "insert into pharmaceuticals " + "(name, packing, price, manufactures_id, pharmaceutical_types_id) " +
                        "values (@name, @packing, @price, @manufactures_id, @pharmaceutical_types_id); ";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {                        
                        cmd.Parameters.AddWithValue("@name", pharmInfo.name);
                        cmd.Parameters.AddWithValue("@packing", pharmInfo.packing);
                        cmd.Parameters.AddWithValue("@price", pharmInfo.price);
                        cmd.Parameters.AddWithValue("@manufactures_id", name1);
                        cmd.Parameters.AddWithValue("@pharmaceutical_types_id", Type);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            pharmInfo.name = "";
            pharmInfo.packing = "";
            pharmInfo.price = "";
            succesMessage = "New sale added";
            Response.Redirect("/KornyukovNikita309/Pharm/Index");
        }
    }
    public class IdsToPharm
    {
        public String manufacturesId;
        public String PharmTypeId;
    }
}
