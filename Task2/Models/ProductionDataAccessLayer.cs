using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Task2.Models
{
    public class ProductionDataAccessLayer
    {
        string connectionString = "Server=ATHARVA\\SQLEXPRESS;Database=Store;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

        public int GetItemData(string? item, int quantity)
        {
            int storedQuantity = 0;

            if (!string.IsNullOrEmpty(item)) // Check if item is not null or empty before proceeding with the query
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM storage WHERE item = @item";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@item", item);
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Process each row fetched from the database
                            Store store = new Store();
                            Production prod = new Production();

                            store.itemID = Convert.ToInt32(reader["itemID"]);
                            store.item = reader["item"].ToString();
                            prod.item = reader["item"].ToString();
                            store.available = Convert.ToBoolean(reader["available"]);
                            store.quantity = Convert.ToInt32(reader["quantity"]);
                            prod.quantity = Convert.ToInt32(reader["quantity"]);

                            // Assign the last fetched quantity to 'storedQuantity'
                            storedQuantity = store.quantity;
                        }
                    }

                    connection.Close();
                }
            }

            return storedQuantity;
        }


        public bool CheckAvailability(string? item)
        {
            bool check = false;

            if (!string.IsNullOrEmpty(item)) // Check if item is not null or empty before proceeding with the query
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT available FROM storage WHERE item = @item";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@item", item);
                    connection.Open();

                    // Use ExecuteScalar for retrieving a single value
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        // If the result is not null and not DBNull, consider the item exists
                        check = Convert.ToBoolean(result);
                    }

                    connection.Close();
                }
            }

            return check;
        }

        public bool checkItem(string item)
        {
            bool check = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spCheckItem", connection);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public bool InvalidItemID(int itemID)
        {
            bool check = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM storage WHERE itemID = @itemID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@itemID", itemID);
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

        public void UpdateQuantity(string? item, int quantity)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateItemQuantity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@item", item);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddStoreRequirement(Store store)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@itemID", store.itemID);
                cmd.Parameters.AddWithValue("@item", store.item);
                cmd.Parameters.AddWithValue("@availability", CheckAvailability(store.item));
                cmd.Parameters.AddWithValue("@quantity", store.quantity);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteStoreItem(string item)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@item", item);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void ItemUnavailable(string? item)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateUnAvailability", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@item", item);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public string ItemName(int itemID)
        {
            string item="";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT item FROM storage WHERE itemID=@itemID",connection);
                cmd.Parameters.AddWithValue("@itemID", itemID);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Process each row fetched from the database
                        Store store = new Store();
                        Production prod = new Production();

                        store.itemID = Convert.ToInt32(reader["itemID"]);
                        store.item = reader["item"].ToString();
                        prod.item = reader["item"].ToString();
                        store.available = Convert.ToBoolean(reader["available"]);
                        store.quantity = Convert.ToInt32(reader["quantity"]);
                        prod.quantity = Convert.ToInt32(reader["quantity"]);

                        // Assign the last fetched quantity to 'storedQuantity'
                        item = store.item;
                    }
                    connection.Close();
                }
            }
            return item;
        }
    }
}