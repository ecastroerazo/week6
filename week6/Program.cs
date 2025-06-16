using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static System.Console;

namespace Exercise02
{
  public class Customer
  {
    public required string CustomerID { get; set; }
    public required string CompanyName { get; set; }
    public string? City { get; set; }
  }
  public class Northwind : DbContext
  {
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      string connectionString = {DbPath};
      optionsBuilder.UseSqlite(connectionString);
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        using (var db = new Northwind())
        {
          var distinctCities = db.Customers
            .Where(c => c.City != null)
            .Select(c => c.City)
            .Distinct()
            .ToList();

          WriteLine("A list of cities that at least one customer resides in:");
          WriteLine($"{string.Join(", ", distinctCities)}");
          WriteLine();

          Write("Enter the name of a city: ");
          var city = ReadLine();

          if (!string.IsNullOrWhiteSpace(city))
          {
            var customersInCity = db.Customers
              .Where(c => c.City == city)
              .ToList();

            WriteLine($"Company names for customers in {city}:");
            foreach (var customer in customersInCity)
            {
              WriteLine(customer.CompanyName);
            }
          }
          else
          {
            WriteLine("No city name entered.");
          }
        }
      }
      catch (Exception ex)
      {
        WriteLine($"An error occurred: {ex.Message}");
      }
    }
  }
}
