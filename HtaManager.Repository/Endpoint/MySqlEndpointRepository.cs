using HtaManager.Infrastructure.Domain;
using HtaManager.Repository.Base;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace HtaManager.Repository.Endpoint
{
    public class MySqlEndpointRepository : IEndpointRepository
    {
        private MySqlConnection connection;

        private readonly string connectionString;
        private List<EndpointDescriptor> endpointDescriptorList;

        public MySqlEndpointRepository(IUnityContainer container, string connectionString)
        {
            this.endpointDescriptorList = container.Resolve<List<EndpointDescriptor>>();
            this.connectionString = connectionString;
        }

        public List<EndpointDescriptor> GetEndpointDescriptorList()
        {
            endpointDescriptorList.Clear();
            List<EndpointDescriptor> buffer = new List<EndpointDescriptor>();

            Connect();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM endpoint_descriptor ORDER BY endpoint_descriptor_id";

            HtaMySqlDataReader reader = null;
            try
            {
                reader = new HtaMySqlDataReader(cmd.ExecuteReader());
            }
            catch
            {
                MessageBox.Show("Datenbankfehler.", "", MessageBoxButton.OK);
                return buffer;
            }

            while (reader.Read())
            {
                EndpointDescriptor newEndpointType = ReadEndpointDescriptor(reader);
                buffer.Add(newEndpointType);
            }

            Disconnect();

            foreach (EndpointDescriptor ep in buffer)
            {
                EndpointDescriptor elementToAdd = null;

                if (ep.ParentId > 0)
                {
                    if (buffer.Any(item => item.Id == ep.ParentId))
                    {
                        EndpointDescriptor parent = buffer.First(item => item.Id == ep.ParentId);
                        parent.ChildList.Add(ep);
                        ep.Parent = parent;
                        elementToAdd = parent;
                    }
                }
                else
                {
                    elementToAdd = ep;
                }

                if (!endpointDescriptorList.Any(item => item.Id == elementToAdd.Id))
                {
                    endpointDescriptorList.Add(elementToAdd);
                }
            }

            return endpointDescriptorList;
        }

        public int SaveEndpointDescriptor(EndpointDescriptorViewModel endpointDescriptorViewModel)
        {
            return SaveEndpointDescriptor(EndpointDescriptor.FromViewModel(endpointDescriptorViewModel));
        }

        public int SaveEndpointDescriptor(EndpointDescriptor EndpointDescriptor)
        {
            Connect();

            string cmdPrefix = "";
            string cmdSuffix = "";
            string cmdText = "";
            if (EndpointDescriptor.Id > 0)
            {
                cmdPrefix = "UPDATE endpoint_descriptor SET";
                cmdSuffix = $" WHERE endpoint_descriptor_id = {EndpointDescriptor.Id}";
            }
            else
            {
                cmdPrefix = "INSERT INTO endpoint_descriptor SET";
            }

            cmdText = cmdPrefix + " endpoint_descriptor_name = ?endpoint_descriptor_name, abbreviation = ?abbreviation, parent_id = ?parent_id, endpoint_descriptor_dimension = ?endpoint_descriptor_dimension" + cmdSuffix;

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = cmdText;

            cmd.Parameters.AddWithValue("?endpoint_descriptor_name", EndpointDescriptor.Name);
            cmd.Parameters.AddWithValue("?abbreviation", EndpointDescriptor.Abbreviation);
            if (EndpointDescriptor.ParentId > 0)
            {
                cmd.Parameters.AddWithValue("?parent_id", EndpointDescriptor.ParentId);
            }
            else
            {
                cmd.Parameters.AddWithValue("?parent_id", null);

            }
            cmd.Parameters.AddWithValue("?endpoint_descriptor_dimension", EndpointDescriptor.Dimension.ToString().ToLower());

            int rowCount = cmd.ExecuteNonQuery();
            Disconnect();

            if (rowCount > 0)
            {
                if (cmd.LastInsertedId > 0)
                {
                    return Convert.ToInt32(cmd.LastInsertedId);
                }
                else
                {
                    return EndpointDescriptor.Id;
                }
            }
            else
            {
                return EndpointDescriptor.Id;
            }         
        }

        public bool DeleteEndpointDescriptor(EndpointDescriptor EndpointDescriptor)
        {
            Connect();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"DELETE FROM endpoint_descriptor WHERE endpoint_descriptor_id={EndpointDescriptor.Id}";
            int rowCount = cmd.ExecuteNonQuery();

            Disconnect();

            return rowCount > 0;
        }

        public EndpointDescriptor ReadEndpointDescriptor(HtaMySqlDataReader reader)
        {
            EndpointDescriptor result = new EndpointDescriptor();

            result.Id = reader.GetInt32("endpoint_descriptor_id");
            result.Name = reader.GetString("endpoint_descriptor_name");
            result.Abbreviation = reader.GetString("abbreviation");
            result.Dimension = (EndpointDimensionType)Enum.Parse(typeof(EndpointDimensionType), reader.GetString("endpoint_descriptor_dimension").ToUpper());
            result.ParentId = reader.GetInt32("parent_id");
            result.NameEN = reader.GetString("endpoint_descriptor_shorthand");
            result.AbbreviationEN = reader.GetString("abbreviation_en");

            return result;
        }

        public void Connect()
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                if (!(connection is object))
                {
                    connection = new MySqlConnection(connectionString);
                }

                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public void Disconnect()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
