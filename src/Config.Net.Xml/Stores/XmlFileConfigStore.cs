using System;
using System.IO;
using System.Text;
using System.Xml;
using Config.Net.Core;

namespace Config.Net.Xml.Stores
{
   /// <summary>
   /// Simple XML storage.
   /// </summary>
   public class XmlFileConfigStore : IConfigStore
   {
      private readonly string _rootElementName;
      private readonly string _pathName;
      private readonly XmlDocument _doc;

      /// <summary>
      /// Create XML storage in the file specified in <paramref name="name"/>.
      /// </summary>
      /// <param name="name">Name of the file, either path to XML storage file, or xml file content.</param>
      /// <param name="isFilePath">Set to true if <paramref name="name"/> specifies file name, otherwise false. </param>
      /// <param name="rootElementName">The name of the root element of the config xml</param>
      /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
      /// <exception cref="IOException">Provided path is not valid.</exception>
      /// <remarks>Storage file does not have to exist, however it will be created as soon as first write performed.</remarks>
      public XmlFileConfigStore(string name, bool isFilePath, string rootElementName)
      {
         if (name == null) throw new ArgumentNullException(nameof(name));
         _rootElementName = rootElementName;

         if (isFilePath)
         {
            _pathName = Path.GetFullPath(name);   // Allow relative path to JSON file
            _doc = ReadXmlFile(_pathName);
            CanWrite = true;
         }
         else
         {
            _doc = ReadXmlString(name);
            CanWrite = false;
         }

         CanRead = true;
      }

      public void Dispose()
      {
         // nothing to dispose.
      }

      public string Name => "xml";

      public bool CanRead { get; }

      public bool CanWrite { get; }

      public string Read(string key)
      {
         if (key == null || _doc == null) return null;

         bool isLength = OptionPath.TryStripLength(key, out key);
         key = key.Replace('.', '/');

         string path = $"/{_rootElementName}/{key}";

         XmlNodeList valueTokens = _doc.SelectNodes(path);

         if (valueTokens == null)
         {
            return null;
         }

         if (isLength)
         {
            return valueTokens?.Count.ToString();
         }

         if (valueTokens.Count == 0)
         {
            return null;
         }

         if (valueTokens.Count > 1)
         {
            throw new ArgumentException("Key resolves to a list of elements");
         }

         return GetStringValue(valueTokens[0]);
      }

      private string GetStringValue(XmlNode node)
      {
         return node?.InnerText;
      }

      public void Write(string key, string value)
      {
         throw new NotSupportedException($"No write action supported in {nameof(XmlFileConfigStore)} store");
      }

      private static XmlDocument ReadXmlFile(string fileName)
      {
         if (File.Exists(fileName))
         {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
               var xmlDocument = new XmlDocument();
               xmlDocument.Load(fs);
               return xmlDocument;
            }
         }

         return new XmlDocument();
      }

      private static XmlDocument ReadXmlString(string xmlString)
      {
         var xmlDocument = new XmlDocument();
         xmlDocument.LoadXml(xmlString);
         return xmlDocument;
      }
   }
}
