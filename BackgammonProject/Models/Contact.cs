﻿using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace BackgammonProject.Models
{
    public class Contact : AbstractXmlSerializable, INotifyPropertyChanged
    {
        public string Name { get; set; }
        public bool IsOnline { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public override bool FromXml(XElement xmlElement)
        {
            if (!valid(xmlElement))
            {
                return false;
            }
            this.Name = xmlElement.Element("Name").Value;
            this.IsOnline = Boolean.Parse(xmlElement.Element("IsOnline").Value);
            return true;
        }

        public override XElement ToXml()
        {
            XElement serialized = new XElement(GetStringType(),
                new XElement("Type", GetStringType()),
                new XElement("Name", Name),
                new XElement("IsOnline", IsOnline.ToString()));
            return serialized;
        }
    }
}
