using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace DialogueSystem.Models
{
    public class Conversation
    {
        [XmlAttribute("Id")]
        public int id { get; set; }

        [XmlElement("Name")]
        public string name { get; set; }

        [XmlElement("Description")]
        public string description { get; set; }

        [XmlElement("Starter")]
        public string starter { get; set; }

        [XmlElement("Repeatable")]
        public bool isRepeatable { get; set; }

        [XmlElement("Completed")]
        public bool isCompleted { get; set; }

        [XmlElement("Locked")]
        public bool isLocked { get; set; }

        [XmlElement("Phase")]
        public int phase { get; set; }

        [XmlArray("Dialogues")]
        [XmlArrayItem("Dialogue")]
        public List<Dialogue> dialogues { get; set; }
    }
}