using HtaManager.Infrastructure.Domain;
using HtaManager.Repository.Base;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            List<EndpointDescriptor> buffer = new List<EndpointDescriptor>();

            Connect();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM endpoint_type ORDER BY endpoint_type_id";

            HtaMySqlDataReader reader = new HtaMySqlDataReader(cmd.ExecuteReader());

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

        public bool SaveEndpointDescriptor(EndpointDescriptor EndpointDescriptor)
        {
            Connect();

            string cmdPrefix = "";
            string cmdSuffix = "";
            string cmdText = "";
            if (EndpointDescriptor.Id > 0)
            {
                cmdPrefix = "UPDATE endpoint_type SET";
                cmdSuffix = $" WHERE endpoint_type_id = {EndpointDescriptor.Id}";
            }
            else
            {
                cmdPrefix = "INSERT INTO endpoint_type SET";
            }


            cmdText = cmdPrefix + " endpoint_type_name = ?endpoint_type_name, abbreviation = ?abbreviation, parent_id = ?parent_id, endpoint_type_dimension = ?endpoint_type_dimension" + cmdSuffix;

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = cmdText;

            cmd.Parameters.AddWithValue("?endpoint_type_name", EndpointDescriptor.Name);
            cmd.Parameters.AddWithValue("?abbreviation", EndpointDescriptor.Abbreviation);
            if (EndpointDescriptor.Parent != null)
            {
                cmd.Parameters.AddWithValue("?parent_id", EndpointDescriptor.Parent.Id);
            }
            else
            {
                cmd.Parameters.AddWithValue("?parent_id", null);

            }
            cmd.Parameters.AddWithValue("?endpoint_type_dimension", EndpointDescriptor.Dimension.ToString().ToLower());

            int rowCount = cmd.ExecuteNonQuery();

            EndpointDescriptor.Id = Convert.ToInt32(cmd.LastInsertedId);
            Disconnect();

            return rowCount > 0;
        }

        public bool DeleteEndpointDescriptor(EndpointDescriptor EndpointDescriptor)
        {
            Connect();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"DELETE FROM endpoint_type WHERE endpoint_type_id={EndpointDescriptor.Id}";
            int rowCount = cmd.ExecuteNonQuery();

            Disconnect();

            return rowCount > 0;
        }

        public EndpointDescriptor ReadEndpointDescriptor(HtaMySqlDataReader reader)
        {
            EndpointDescriptor result = new EndpointDescriptor();

            result.Id = reader.GetInt32("endpoint_type_id");
            result.Name = reader.GetString("endpoint_type_name");
            result.Abbreviation = reader.GetString("abbreviation");
            result.Dimension = (EndpointDimensionType)Enum.Parse(typeof(EndpointDimensionType), reader.GetString("endpoint_type_dimension").ToUpper());
            result.ParentId = reader.GetInt32("parent_id");
            result.NameEN = reader.GetString("endpoint_type_name_en");
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
                    connection.Open();
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
