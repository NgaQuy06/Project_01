using System.Data;
using Npgsql;

namespace Server_Project_01
{
    public class Npg
    {
        public static string str = "";

        public async static Task<LoginRequest?> Login(string username, string password)
        {
            try
            {
                using var conn = new NpgsqlConnection(str);
                await conn.OpenAsync();
                string sql = "select \"id\", \"email\" from public.\"Account\" where \"username\" = @u and \"password\" = @p";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("u", username.Trim());
                    cmd.Parameters.AddWithValue("p", password.Trim());
                    using var reader = await cmd.ExecuteReaderAsync();
                    if (reader != null && await reader.ReadAsync())
                    {
                        return new LoginRequest
                        {
                            id = reader.GetInt32("id"),
                            email = reader.GetString("email")
                        };
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đăng nhập: {ex.Message} ({DateTime.Now})");
                return null;
            }
        }

        public async static Task<bool> UsernameExists(string username)
        {
            try
            {
                using var conn = new NpgsqlConnection(str);
                await conn.OpenAsync();
                string sql = "select count(*) from public.\"Account\" where \"username\" = @u";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("u", username.Trim());
                    int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi kiểm tra tên đăng nhập: {ex.Message} ({DateTime.Now})");
                return false;
            }
        }

        public async static Task<bool> Register(string username, string password, string email)
        {
            try
            {
                using var conn = new NpgsqlConnection(str);
                await conn.OpenAsync();
                string sql = "insert into public.\"Account\" (\"username\", \"password\", \"email\", \"created_at\") values (@u, @p, @e, @ca)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("u", username.Trim());
                    cmd.Parameters.AddWithValue("p", password.Trim());
                    cmd.Parameters.AddWithValue("e", email.Trim());
                    cmd.Parameters.AddWithValue("ca", DateTime.Now);
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đăng ký: {ex.Message} ({DateTime.Now})");
                return false;
            }
        }
    }

    public class LoginRequest
    {
        public int id { get; set; }
        public string email { get; set; }
    }
}
