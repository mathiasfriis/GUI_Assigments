using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Agent_Assignments
{
    public class Repository
    {
        static public bool ReadFile(string fileName, out List<Agent> agents)
        {
            agents = new List<Agent>();
            // Create an instance of the XmlSerializer class and specify the type of object to deserialize.
            XmlSerializer serializer = new XmlSerializer(typeof(List<Agent>));

            TextReader reader = new StreamReader(fileName);
            // Deserialize all the agents.
            agents = (List<Agent>)serializer.Deserialize(reader);
            reader.Close();

            return true;
        }

        internal static void SaveFile(string fileName, List<Agent> agents)
        {
            // Create an instance of the XmlSerializer class and specify the type of object to serialize.
            XmlSerializer serializer = new XmlSerializer(typeof(List<Agent>));
            TextWriter writer = new StreamWriter(fileName);
            // Serialize all the agents.
            serializer.Serialize(writer, agents);
            writer.Close();
        }
    }
}
