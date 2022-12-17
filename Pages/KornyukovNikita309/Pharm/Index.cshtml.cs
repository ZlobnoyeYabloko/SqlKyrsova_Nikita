using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace WebApplication2.Pages.KornyukovNikita309.Pharm
{
    public class IndexModel : PageModel
    {
        public List<PharmInfo> listPharm = new List<PharmInfo>();
        public String id = "";
        int retuv = 0;
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=KornyukovNikita309;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = " SELECT p.id, p.name, p.packing, p.price, m.name, pt.type " +
                        " FROM pharmaceuticals p inner join manufacturers m on p.manufactures_id=m.id " +
                        " inner join pharmaceutical_types pt on p.pharmaceutical_types_id=pt.id ";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PharmInfo pharmInfo = new PharmInfo();
                                pharmInfo.id = "" + reader.GetInt32(0);
                                pharmInfo.name = reader.GetString(1);
                                pharmInfo.packing = "" + reader.GetInt32(2);
                                pharmInfo.price = "" + reader.GetFloat(3);
                                pharmInfo.manufactures_id = reader.GetString(4);
                                pharmInfo.pharmaceuticals_types_id = reader.GetString(5);
                                listPharm.Add(pharmInfo);
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
            String id1 = Request.Form["name"];
            id = id1;
            if (id.Length != 0) {
                listPharm.Clear();
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=KornyukovNikita309;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    String sql2 = "declare @a int exec @a= dbo.myProc2 @name select @a";
                    using (SqlCommand cmd = new SqlCommand(sql2, connection))
                    {
                        connection.Open();
                        cmd.Parameters.AddWithValue("@name", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                retuv = reader.GetInt32(0);
                                if (retuv != 404)
                                {
                                    PharmInfo pharmInfo = new PharmInfo();
                                    pharmInfo.id = "" + reader.GetInt32(0);
                                    pharmInfo.name = reader.GetString(1);
                                    pharmInfo.packing = "" + reader.GetInt32(2);
                                    pharmInfo.price = "" + reader.GetFloat(3);
                                    pharmInfo.manufactures_id = "" + reader.GetInt32(4);
                                    pharmInfo.pharmaceuticals_types_id = "" + reader.GetInt32(5);
                                    listPharm.Add(pharmInfo);
                                }
                             }
                        }
                    }             
                }
            }
        }
    }
    public class PharmInfo
    {
        public String id;
        public String name;
        public String packing;
        public String price;
        public String manufactures_id;
        public String pharmaceuticals_types_id;
    }
}
