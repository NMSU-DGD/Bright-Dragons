using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace DialogueSystem.Models
{
    [XmlRoot("Response")]
    public class Response
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("Text")]
        public string text { get; set; }

        [XmlElement("ActionTaken")]
        public string actionTaken { get; set; }

        [XmlElement("NextDialogueId")]
        public int nextDialogueId { get; set; }

        [XmlElement("ConversationId")]
        public int conversationParentId { get; set; }
    }
}