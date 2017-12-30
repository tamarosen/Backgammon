using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BackgammonProject.Models
{
    public class ModelXmlMapper
    {
        enum MappedType
        {
            UNDEFINED,
            MESSAGE,
            CONTACT,
            USER,
            CHAT_REQUEST,
            CHAT_REQUEST_RESPONSE,
            LOGIN,
            LOGIN_RESPONSE
        };

        static IDictionary<string, MappedType> map = new Dictionary<string, MappedType>
        {
            { AbstractXmlSerializable.GetStringType(typeof(Message)), MappedType.MESSAGE },
            { AbstractXmlSerializable.GetStringType(typeof(Contact)), MappedType.CONTACT },
            { AbstractXmlSerializable.GetStringType(typeof(User)), MappedType.USER },
            { AbstractXmlSerializable.GetStringType(typeof(ChatRequest)), MappedType.CHAT_REQUEST },
            { AbstractXmlSerializable.GetStringType(typeof(ChatRequestResponse)), MappedType.CHAT_REQUEST_RESPONSE },
            { AbstractXmlSerializable.GetStringType(typeof(Login)), MappedType.LOGIN },
            { AbstractXmlSerializable.GetStringType(typeof(LoginResponse)), MappedType.LOGIN_RESPONSE }
        };

        public static AbstractXmlSerializable FromXmlString(string xmlDoc)
        {
            XDocument serialized = XDocument.Load(xmlDoc);
            string type = serialized.Element("Type").Value;
            MappedType t = MappedType.UNDEFINED;
            if (ModelXmlMapper.map.TryGetValue(type, out t)) {
                switch (t)
                {
                    case MappedType.MESSAGE:
                        Message msg = new Message();
                        msg.FromXml(serialized.Root);
                        return msg;
                    case MappedType.USER:
                        User user = new User();
                        user.FromXml(serialized.Root);
                        return user;
                    case MappedType.CONTACT:
                        Contact contact = new Contact();
                        contact.FromXml(serialized.Root);
                        return contact;
                    case MappedType.CHAT_REQUEST:
                        ChatRequest req = new ChatRequest();
                        req.FromXml(serialized.Root);
                        return req;
                    case MappedType.CHAT_REQUEST_RESPONSE:
                        ChatRequestResponse resp = new ChatRequestResponse();
                        resp.FromXml(serialized.Root);
                        return resp;
                    case MappedType.LOGIN:
                        Login login = new Login();
                        login.FromXml(serialized.Root);
                        return login;
                    case MappedType.LOGIN_RESPONSE:
                        LoginResponse loginResp = new LoginResponse();
                        loginResp.FromXml(serialized.Root);
                        return loginResp;
                    case MappedType.UNDEFINED:
                        throw new Exception("Don't know how to parse this type");
                }
            }
            return null;
        }

        public static string GetAsXmlString(AbstractXmlSerializable obj)
        {
            return obj.ToXml().ToString();
        }

        public static string GetAsXmlString(IList<AbstractXmlSerializable> list)
        {
            XElement array = new XElement("Array");
            foreach (var obj in list)
            {
                array.Add(obj.ToXml().ToString());
            }
            return array.ToString();
        }
    }
}
