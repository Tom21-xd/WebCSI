using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebCSI.Data
{
    public class Conexion
    {
        protected MySqlConnection? connection;
        MySqlCommand? cmd;


        protected void Conectar()
        {
            try
            {
                //connection = new MySqlConnection("Server=web-bd.mysql.database.azure.com;UserID = tom;Password=Johan.Sebastian;Database=bd_web;SslMode=Required;SslCa={path_to_CA_cert};");
                connection = new MySqlConnection("Server=localhost;Port=3306;Database=app_dengue;Uid=root;Pwd=0518");
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        protected void Desconectar()
        {
            try
            {
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public DataTable ProcedimientosSelect(string[] Parametros, string NombreProcedimiento, string[] valores)
        {
            DataTable dt = new DataTable();
            Conectar();
            try
            {
                cmd = new MySqlCommand(NombreProcedimiento, connection);
                if (Parametros != null)
                {
                    for (int i = 0; i < Parametros.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@" + Parametros[i], valores[i]);
                    }
                }

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Desconectar();
            }
            return dt;
        }

        public void procedimientosInEd(string[] Parametros, string NombreProcedimiento, string[] valores)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand(NombreProcedimiento, connection);
                if (Parametros != null)
                {
                    for (int i = 0; i < Parametros.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@" + Parametros[i], valores[i]);
                    }
                }

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Desconectar();
            }
        }

    }
}
