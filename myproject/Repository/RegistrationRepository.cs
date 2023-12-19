using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using myproject.Models;


namespace myproject.Repository
{
    public class RegistrationRepository
    {
        private SqlConnection sqlconnect;

        private void Connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ToString();
            sqlconnect = new SqlConnection(connectionString);
        }

        //Add the new user details to database
        public (bool, string) AddDetails(Registration registration)
        {
            Connection();
            SqlCommand command = new SqlCommand("SPI_UserRegistration", sqlconnect);
            command.CommandType = CommandType.StoredProcedure;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registration.Password);
            command.Parameters.AddWithValue("@Password", hashedPassword);
            command.Parameters.AddWithValue("@FirstName", registration.FirstName);
            command.Parameters.AddWithValue("@LastName", registration.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", registration.DateOfBirth);
            command.Parameters.AddWithValue("@Gender", registration.Gender);
            command.Parameters.AddWithValue("@Email", registration.Email);
            command.Parameters.AddWithValue("@Phone", registration.Phone);
            command.Parameters.AddWithValue("@State", registration.State);
            command.Parameters.AddWithValue("@City", registration.City);
            command.Parameters.AddWithValue("@Username", registration.Username);
            command.Parameters.AddWithValue("@ConfirmPassword", registration.ConfirmPassword);

            try
            {
                sqlconnect.Open();

                // Check if the username already exists
                SqlCommand checkUsernameCommand = new SqlCommand("SELECT COUNT(*) FROM Signup WHERE Username = @Username", sqlconnect);
                checkUsernameCommand.Parameters.AddWithValue("@Username", registration.Username);
                int existingUserCount = (int)checkUsernameCommand.ExecuteScalar();

                if (existingUserCount > 0)
                {
                    return (false, "Username already exists. Please choose a different username.");
                }

                // Check if the email already exists
                SqlCommand checkEmailCommand = new SqlCommand("SELECT COUNT(*) FROM Signup WHERE Email = @Email", sqlconnect);
                checkEmailCommand.Parameters.AddWithValue("@Email", registration.Email);
                int existingEmailCount = (int)checkEmailCommand.ExecuteScalar();

                if (existingEmailCount > 0)
                {
                    return (false, "Email already exists. Please use a different email address.");
                }

                // Check if the phone number already exists
                SqlCommand checkPhoneCommand = new SqlCommand("SELECT COUNT(*) FROM Signup WHERE Phone = @Phone", sqlconnect);
                checkPhoneCommand.Parameters.AddWithValue("@Phone", registration.Phone);
                int existingPhoneCount = (int)checkPhoneCommand.ExecuteScalar();

                if (existingPhoneCount > 0)
                {
                    return (false, "Phone number already exists. Please use a different phone number.");
                }

                // Username and email are both unique, proceed with inserting the new registration
                int rowsAffected = command.ExecuteNonQuery();
                return (rowsAffected > 0, null);
            }
            finally
            {
                sqlconnect.Close();
            }
        }

        //get the username
        public List<string> GetAllUsernames()
        {
            Connection();
            SqlCommand command = new SqlCommand("SP_Signup", sqlconnect);
            command.CommandType = CommandType.StoredProcedure;

            List<string> usernames = new List<string>();

            try
            {
                sqlconnect.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string username = reader["Username"].ToString();
                    usernames.Add(username);
                }

                reader.Close();
            }
            finally
            {
                sqlconnect.Close();
            }

            return usernames;
        }
    }
}