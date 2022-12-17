using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace WebApplication2.Pages.KornyukovNikita309.Pharm
{
    public class EditModel : PageModel
    {
        public PharmInfo pharmInfo = new PharmInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public List<IdsToPharm> idList = new List<IdsToPharm>();
        public List<IdsToPharm> idList2 = new List<IdsToPharm>();
        String name1 = "";
        String Type = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=KornyukovNikita309;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql3 = " SELECT p.id, p.name, p.packing, p.price, m.name, pt.type " +
                        " FROM pharmaceuticals p inner join manufacturers m on p.manufactures_id=m.id " +
                        " inner join pharmaceutical_types pt on p.pharmaceutical_types_id=pt.id WHERE p.id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql3, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pharmInfo.id = "" + reader.GetInt32(0);
                                pharmInfo.name = reader.GetString(1);
                                pharmInfo.packing = "" + reader.GetInt32(2);
                                pharmInfo.price = "" + reader.GetFloat(3);
                                pharmInfo.manufactures_id = reader.GetString(4);
                                pharmInfo.pharmaceuticals_types_id = reader.GetString(5);
                            }
                        }
                    }
                    String sql = "SELECT name FROM manufacturers";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IdsToPharm idsInfo = new IdsToPharm();
                                idsInfo.manufacturesId = reader.GetString(0);
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
                                idsInfo.PharmTypeId = reader.GetString(0);
                                idList2.Add(idsInfo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
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
            //save the new sale into the database
            try
            {
                String connectionString = "Data Source =.\\sqlexpress; Initial Catalog = KornyukovNikita309; Integrated Security = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
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
                    String sql = "UPDATE PHARMACEUTICALS " + "SET name=@name, packing=@packing, price=@price, manufactures_id=@manufactures_id, pharmaceutical_types_id=@pharmaceutical_types_id"
                        + " WHERE id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", pharmInfo.name);
                        cmd.Parameters.AddWithValue("@packing", pharmInfo.packing);
                        cmd.Parameters.AddWithValue("@price", pharmInfo.price);
                        cmd.Parameters.AddWithValue("@manufactures_id", name1);
                        cmd.Parameters.AddWithValue("@pharmaceutical_types_id", Type);
                        cmd.Parameters.AddWithValue("@id", pharmInfo.id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            Response.Redirect("/KornyukovNikita309/Pharm/Index");
        }
    }
}
