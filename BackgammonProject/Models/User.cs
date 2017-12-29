using System.ComponentModel;
using System.Xml.Linq;

namespace BackgammonProject.Models
{
    public class User : AbstractXmlSerializable, INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public User()
        {
            this.Name = null;
        }

        public User(string currentUser)
        {
            this.Name = currentUser;
        }

        public override bool FromXml(XElement xmlElement)
        {
            if (!valid(xmlElement))
            {
                return false;
            }
            this.Name = xmlElement.Element("Name").Value;
            this.Password= xmlElement.Element("Password").Value;
            return true;
        }

        public override XElement ToXml()
        {
            XElement serialized = new XElement(GetStringType(),
                new XElement("Type", GetStringType()),
                new XElement("Name", Name),
                new XElement("Password", Password));
            return serialized;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
