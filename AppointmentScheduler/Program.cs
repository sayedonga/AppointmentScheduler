
using AppointmentScheduler.AppointmentScheduler;
using System;
using System.Linq;

namespace AppointmentScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Database.EnsureCreated();
            }

            while (true)
            {
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void Register()
        {
            using (var db = new ApplicationDbContext())
            {
                Console.Write("Enter username: ");
                var username = Console.ReadLine();

                Console.Write("Enter password: ");
                var password = Console.ReadLine();

                var user = new User { Username = username, Password = password };

                db.Users.Add(user);
                db.SaveChanges();
                Console.WriteLine("User registered successfully.");
            }
        }

        static void Login()
        {
            using (var db = new ApplicationDbContext())
            {
                Console.Write("Enter username: ");
                var username = Console.ReadLine();

                Console.Write("Enter password: ");
                var password = Console.ReadLine();

                var user = db.Users.SingleOrDefault(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    Console.WriteLine("Login successful.");
                    ShowMenu(user);
                }
                else
                {
                    Console.WriteLine("Invalid credentials.");
                }
            }
        }

        static void ShowMenu(User user)
        {
            while (true)
            {
                Console.WriteLine("1. Add Appointment");
                Console.WriteLine("2. View Appointments");
                Console.WriteLine("3. Update Appointment");
                Console.WriteLine("4. Delete Appointment");
                Console.WriteLine("5. Search Appointments");
                Console.WriteLine("6. Logout");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddAppointment(user);
                        break;
                    case "2":
                        ViewAppointments(user);
                        break;
                    case "3":
                        UpdateAppointment(user);
                        break;
                    case "4":
                        DeleteAppointment(user);
                        break;
                    case "5":
                        SearchAppointments(user);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void AddAppointment(User user)
        {
            using (var db = new ApplicationDbContext())
            {
                Console.Write("Enter date (yyyy-mm-dd): ");
                var date = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter time (hh:mm): ");
                var time = TimeSpan.Parse(Console.ReadLine());

                Console.Write("Enter title: ");
                var title = Console.ReadLine();

                Console.Write("Enter description (optional): ");
                var description = Console.ReadLine();

                Console.Write("Enter location (optional): ");
                var location = Console.ReadLine();

                var appointment = new Appointment
                {
                    Date = date.Add(time),
                    Title = title,
                    Description = description,
                    Location = location,
                    UserId = user.UserId
                };

                db.Appointments.Add(appointment);
                db.SaveChanges();
                Console.WriteLine("Appointment added successfully.");
            }
        }

        static void ViewAppointments(User user)
        {
            using (var db = new ApplicationDbContext())
            {
                var appointments = db.Appointments.Where(a => a.UserId == user.UserId).ToList();

                foreach (var appointment in appointments)
                {
                    Console.WriteLine($"ID: {appointment.AppointmentId}, Date: {appointment.Date}, Title: {appointment.Title}, Description: {appointment.Description}, Location: {appointment.Location}");
                }
            }
        }

        static void UpdateAppointment(User user)
        {
            using (var db = new ApplicationDbContext())
            {
                Console.Write("Enter appointment ID to update: ");
                var appointmentId = int.Parse(Console.ReadLine());

                var appointment = db.Appointments.SingleOrDefault(a => a.AppointmentId == appointmentId && a.UserId == user.UserId);

                if (appointment != null)
                {
                    Console.Write("Enter new date (yyyy-mm-dd): ");
                    appointment.Date = DateTime.Parse(Console.ReadLine()).Add(appointment.Date.TimeOfDay);

                    Console.Write("Enter new time (hh:mm): ");
                    appointment.Date = appointment.Date.Date.Add(TimeSpan.Parse(Console.ReadLine()));

                    Console.Write("Enter new title: ");
                    appointment.Title = Console.ReadLine();

                    Console.Write("Enter new description (optional): ");
                    appointment.Description = Console.ReadLine();

                    Console.Write("Enter new location (optional): ");
                    appointment.Location = Console.ReadLine();

                    db.SaveChanges();
                    Console.WriteLine("Appointment updated successfully.");
                }
                else
                {
                    Console.WriteLine("Appointment not found.");
                }
            }
        }

        static void DeleteAppointment(User user)
        {
            using (var db = new ApplicationDbContext())
            {
                Console.Write("Enter appointment ID to delete: ");
                var appointmentId = int.Parse(Console.ReadLine());

                var appointment = db.Appointments.SingleOrDefault(a => a.AppointmentId == appointmentId && a.UserId == user.UserId);

                if (appointment != null)
                {
                    db.Appointments.Remove(appointment);
                    db.SaveChanges();
                    Console.WriteLine("Appointment deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Appointment not found.");
                }
            }
        }

        static void SearchAppointments(User user)
        {
            using (var db = new ApplicationDbContext())
            {
                Console.Write("Enter title to search: ");
                var title = Console.ReadLine();

                var appointments = db.Appointments.Where(a => a.UserId == user.UserId && a.Title.Contains(title)).ToList();

                foreach (var appointment in appointments)
                {
                    Console.WriteLine($"ID: {appointment.AppointmentId}, Date: {appointment.Date}, Title: {appointment.Title}, Description: {appointment.Description}, Location: {appointment.Location}");
                }
            }
        }
    }
}
