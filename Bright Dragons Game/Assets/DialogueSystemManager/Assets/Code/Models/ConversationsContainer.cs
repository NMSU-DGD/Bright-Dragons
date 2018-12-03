using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace DialogueSystem.Models
{
    [XmlRoot("ConversationsCollection")]
    public class ConversationsContainer : IXmlObject
    {
        public ConversationsContainer()
        {

        }

        [XmlArray("Conversations")]
        [XmlArrayItem("Conversation")]
        public List<Conversation> Conversations { get; set; }
    }
}