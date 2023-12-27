using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace Task2.Models
{
    public class StoreDataAccessLayer
    {
        string connectionString = "Server=ATHARVA\\SQLEXPRESS;Database=Store;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
        string connectionString2 = "Server=ATHARVA\\SQLEXPRESS;Database=Purchase;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
        public IEnumerable<Store> GetAllItems()
        {
            List<Store> lstItems = new List<Store>();
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllItems", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Store store = new Store();
                    store.itemID = Convert.ToInt32(reader["itemID"]);
                    store.item = reader["item"].ToString();
                    store.available = (bool)reader["available"];
                    store.quantity = Convert.ToInt32(reader["quantity"]);
                    lstItems.Add(store);
                }
                connection.Close();
            }
            return lstItems;
        }

        public void AddRequirement(Purchase order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString2))
            {
                SqlCommand command = new SqlCommand("spAddRequirement", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@itemID", order.itemID);
                command.Parameters.AddWithValue("@item", order.item);
                command.Parameters.AddWithValue("@quantity", order.quantity);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public bool InvalidItemID(int itemID, string item)
        {
            bool check = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM storage WHERE itemID = @itemID AND item=@item";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@itemID", itemID);
                cmd.Parameters.AddWithValue("@item", item);
                connection.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    // If the result is not null and not DBNull, consider the item exists
                    check = Convert.ToBoolean(result);
                }
                connection.Close();
            }
            return check;
        }
    }
}