using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace Task2.Models
{
    public class PurchaseDataAccessLayer
    {
        string connectionString = "Server=ATHARVA\\SQLEXPRESS;Database=Purchase;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
        string connectionString2 = "Server=ATHARVA\\SQLEXPRESS;Database=Store;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
        public IEnumerable<Purchase> GetAllOrders()
        {
            List<Purchase> lstItems = new List<Purchase>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllOrders", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Purchase order = new Purchase();
                    order.itemID = Convert.ToInt32(reader["itemID"]);
                    order.item = reader["item"].ToString();
                    order.quantity = Convert.ToInt32(reader["quantity"]);
                    lstItems.Add(order);
                }
                connection.Close();
            }
            return lstItems;
        }

        public void AddtoStore(string? item)
        {
            using (SqlConnection con = new SqlConnection(connectionString2))
            {
                SqlCommand cmd = new SqlCommand("spUpdateAvailability", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@item", item);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeletePurchaseOrder(int itemID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Orders WHERE itemID=@itemID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@itemID", itemID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public bool InvalidItemID(int itemID,string item)
        {
            bool check = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Orders WHERE itemID = @itemID AND item=@item";
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