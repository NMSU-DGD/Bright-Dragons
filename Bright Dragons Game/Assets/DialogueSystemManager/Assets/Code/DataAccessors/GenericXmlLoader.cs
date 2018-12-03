using DialogueSystem.Models;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace DialogueSystem.Data
{
    public class GenericXmlLoader<T> where T : class, IXmlObject, new()
    {
        public T LoadXMLFromExternalFolder(string path, string message)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T xmlContainer = new T();
            if (path != string.Empty && File.Exists(path))
            {
                FileStream file = File.Open(path, FileMode.Open);
                xmlContainer = serializer.Deserialize(file) as T;

                file.Close();
            }
            else
                Debug.Log(message);

            return xmlContainer;
        }

        public T LoadXMLFromResourcesFolder(string path, string message)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T xmlContainer = new T();
            TextAsset _xml = Resources.Load<TextAsset>(path);
            if (_xml != null)
            {
                StringReader reader = new StringReader(_xml.text);
                xmlContainer = serializer.Deserialize(reader) as T;

                reader.Close();
            }
            else
                Debug.Log(message);

            return xmlContainer;
        }

        public void SaveXMLFile(T xmlContainer, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                serializer.Serialize(writer, xmlContainer);
            }
        }
    }
}