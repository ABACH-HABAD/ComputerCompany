using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerCompanyClasses
{
    public class Review
    {
        public static List<Review> Reviews = new List<Review>();

        private readonly string senderEmail;
        private readonly string sender;
        private readonly string message;
        private readonly short stars;
        private readonly BitmapImage bitmap;

        private static readonly string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "DataBase", "ComputerCompany.db");

        public string Sender { get => sender; }
        public string Message { get => message; }
        public string Stars => SelectStars(stars); 

        public BitmapImage Bitmap { get => bitmap; }

        public string SenderEmail => senderEmail;

        public Review(string senderEmail, string sender, string message, long stars, byte[] bitmap = null)
        {
            if (stars > 5) this.stars = 5;
            else this.stars = Convert.ToInt16(stars);
            this.senderEmail = senderEmail;
            this.sender = sender;
            this.message = message;

            BitmapImage bitmapImage = new BitmapImage();
            if (bitmap != null)
            {
                using (MemoryStream stream = new MemoryStream(bitmap))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    this.bitmap = bitmapImage;
                }
            }
            else
            {
                try
                {
                    Uri imageUri = new Uri("pack://application:,,,/ComputerCompany;component/Resources/Images/NoImage.png", UriKind.Absolute);

                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = imageUri;
                    bitmapImage.EndInit();

                    this.bitmap = bitmapImage;
                }
                catch { }
            }
        }

        public static void LoadReviews()
        {
            Reviews.Clear();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand()
                {
                    Connection = connection,
                    CommandText = "SELECT Account.name, Account.email, Account.image, Review.stars, Review.message FROM Review JOIN Account ON Review.sender = Account.id_account"
                };

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Reviews.Add(
                            new Review(
                                (string)reader["email"],
                                (string)reader["name"],
                                (string)reader["message"],
                                (long)reader["stars"],
                                bitmap: reader["image"] == DBNull.Value ? null : (byte[])reader["image"]
                                ));
                        }
                    }
                }
            }
        }

        public static string SelectStars(short starCount)
        {
            string star = string.Empty;
            for (int i = 0; i < starCount; i++) star += "★";
            for (int i = star.Length; i < 5; i++) star += "☆";
            return star;
        }

        public static string FindReviewByLogin(string login)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand()
                {
                    Connection = connection,
                    CommandText = "SELECT Review.message FROM Review JOIN Account ON Account.id_account = Review.sender WHERE Account.email = @email"
                };
                command.Parameters.AddWithValue("@email", login);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return (string)reader["message"];
                    }
                    else return string.Empty;
                }
            }
        }

        public static void SendReview(Review review)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand()
                {
                    Connection = connection,
                    CommandText = "SELECT Review.sender FROM Review JOIN Account ON Account.id_account = Review.sender WHERE Account.email = @email"
                };
                command.Parameters.AddWithValue("@email", review.senderEmail);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        long senderID = (long)reader["sender"];

                        using (SqliteTransaction transaction = connection.BeginTransaction()) 
                        {
                            command = new SqliteCommand()
                            {
                                Connection = connection,
                                Transaction = transaction,
                                CommandText = "UPDATE Review SET message = @message, stars = @stars WHERE sender = @sender"
                            };
                            command.Parameters.AddWithValue("@sender", senderID);
                            command.Parameters.AddWithValue("@message", review.Message);
                            command.Parameters.AddWithValue("@stars", (long)review.stars);

                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                    else
                    {
                        command = new SqliteCommand()
                        {
                            Connection = connection,
                            CommandText = "SELECT id_account FROM Account WHERE Account.email = @email"
                        };
                        command.Parameters.AddWithValue("@email", review.senderEmail);

                        using (SqliteDataReader reader2 = command.ExecuteReader())
                        {
                            if (reader2.HasRows)
                            {
                                reader2.Read();
                                long senderID = (long)reader2["id_account"];

                                using (SqliteTransaction transaction = connection.BeginTransaction())
                                {
                                    command = new SqliteCommand()
                                    {
                                        Connection = connection,
                                        Transaction = transaction,
                                        CommandText = "INSERT INTO Review (sender, message, stars) VALUES (@sender, @message, @stars);"
                                    };
                                    command.Parameters.AddWithValue("@sender", senderID);
                                    command.Parameters.AddWithValue("@message", review.Message);
                                    command.Parameters.AddWithValue("@stars", review.stars);

                                    command.ExecuteNonQuery();
                                    transaction.Commit();
                                }
                            }
                        }
                    }

                    LoadReviews();
                }
            }
        }
    }
}
