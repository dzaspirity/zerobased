using System.Xml.Linq;
using System.Xml.Serialization;

namespace Prism
{
    public static class XElementExtensions
    {
        public static T ToObject<T>(this XElement xElement)
        {
            System.Xml.XmlReader reader = xElement.CreateReader();
            var serializer = new XmlSerializer(typeof(T), xElement.Name.NamespaceName);
            var result = (T)serializer.Deserialize(reader);

            return result;
        }

    }
}
