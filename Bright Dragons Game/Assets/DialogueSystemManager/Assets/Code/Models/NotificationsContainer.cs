using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("NotificationContainer")]
public class NotificationsContainer : IXmlObject
{
    [XmlArray("Notifications")]
    [XmlArrayItem("Notification")]
    public List<Notification> notifications { get; set; }
}
