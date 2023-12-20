using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public static class XmlSerializerManager<T>
{
    public static void Save(string path, T t)
    {
        var serializer = new XmlSerializer(typeof(T));
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Encoding = Encoding.UTF8;
        settings.Indent = true;

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(fs, settings))
            {
                serializer.Serialize(xmlWriter, t);
            }
        }
    }


    public static T Load(string path)
    {
        var serializer = new XmlSerializer(typeof(T));

        using (var stream = new FileStream(path, FileMode.Open))
        {
            return (T)serializer.Deserialize(stream);
        }
    }
}