using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace DialogueSystem.Models
{
    [XmlRoot("Dialogue")]
    public class Dialogue
    {

        [XmlAttribute("Id")]
        public int id { get; set; }

        [XmlElement("Text")]
        public string text { get; set; }

        [XmlArray("Responses")]
        [XmlArrayItem("Response")]
        public List<Response> responses { get; set; }
    }
}