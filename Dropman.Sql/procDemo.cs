using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.IO;
using System.Text;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void procSimpleDemo()
    {
        var pipe = SqlContext.Pipe;
        pipe.Send("Jednoduchy vypis");
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void procDataDemo(string sql, string path)
    {
        var csv = new StringBuilder();
        using (var con = new SqlConnection("Context Connection=true"))
        {
            con.Open();
            using (var cmd = new SqlCommand(sql, con))
            {
                using (var rdr = cmd.ExecuteReader())
                {
                    var header = "";
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        header += rdr.GetName(i).ToString() + ",";
                    }
                    header = header.Substring(0, header.Length - 1);
                    csv.AppendLine(header);


                    while (rdr.Read())
                    {
                        var row = "";
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            row += rdr[i].ToString() + ",";
                        }
                        row = row.Substring(0, row.Length - 1);
                        csv.AppendLine(row);

                    }
                }
            }
        }
        
        File.WriteAllText(path, csv.ToString());  
    }
}
