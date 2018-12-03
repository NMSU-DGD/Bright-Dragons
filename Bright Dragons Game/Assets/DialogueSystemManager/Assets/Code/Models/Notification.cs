using UnityEngine;
using System.Xml.Serialization;

public class Notification 
{
    [XmlAttribute("ID")]
    public int id { get; set; }
    [XmlElement("Name")]
    public string name { get; set; }
    [XmlElement("Description")]
    public string description { get; set; }
    [XmlElement("Text")]
    public string text { get; set; }
    [XmlElement("Starter")]
    public string starter { get; set; }
}
