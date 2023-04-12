using System;
using System.Data;
using System.Data.Common;
using Npgsql;

// using LINQ (allows you to perform queries on collections of data in a similar way to SQL queries for databases.
//worked with Said
class Sample2
{
    static void Main(string[] args)
    {
        // Connect to a PostgreSQL database
        NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1:5432;User Id=postgres; " +
           "Password=heyDas;Database=prods;");
        conn.Open();

        NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM customer", conn);

        NpgsqlDataReader reader = command.ExecuteReader();

        DataTable dtable = new DataTable();

        dtable.Load(reader);

        assignment20(dtable);

        conn.Close();
    }

    static void assignment20(DataTable dtable)
    {
        var collection = dtable.AsEnumerable()  //retrives all the rows of 'dtable' as an enumerable collection
            .GroupBy(row => row.Field<string>("rep_id"))  //group the rows by the value of 'rep_id'
            .Select(repGroup => new
            {
                repId = repGroup.Key,  //value of 'rep_id'for that group
                TotalBalance = repGroup.Sum(row => row.Field<decimal>("cust_balance")) //calculate TotalBalance as the sum of 'cust_balance' for all rows in the group
            })
             .Where(answer => answer.TotalBalance >= 12000) //filter the results
             .OrderBy(answer => answer.repId); //sort by 'rep_id'


        Console.WriteLine("rep_id   TotalBalance"); //show the name of the columns

        foreach (var result in collection) //loop for the groups in the collection
        {
            Console.WriteLine(result.repId + "          " + result.TotalBalance); //write a line with the 'rep_id'and 'TotalBalance' of the selected data
        }

    }
}