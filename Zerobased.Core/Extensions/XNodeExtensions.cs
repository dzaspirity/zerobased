using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Prism
{
    public static class XNodeExtensions
    {
        public static string XPathElementValue(this XNode node, string expression, IXmlNamespaceResolver resolver)
        {
            XElement element = node.XPathSelectElement(expression, resolver);
            string value = element == null ? null : element.Value;
            return value;
        }

        public static T XPathElementValue<T>(this XNode node, string expression, IXmlNamespaceResolver resolver)
        {
            string valueStr = node.XPathElementValue(expression, resolver);

            if (valueStr == null)
            {
                return default(T);
            }
            else
            {
                var typeConverter = typeof(T).GetConverter();
                T value = (T)typeConverter.ConvertFromString(valueStr);
                return value;
            }
        }
    }
}
