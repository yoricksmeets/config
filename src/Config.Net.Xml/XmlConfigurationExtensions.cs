using System;
using Config.Net.Xml.Stores;

namespace Config.Net
{
   /// <summary>
   /// Configuration extensions
   /// </summary>
   public static class XmlConfigurationExtensions
   {
      /// <summary>
      /// Uses XML file as a builder storage.
      /// </summary>
      /// <param name="builder">Configuration object.</param>
      /// <param name="xmlFilePath">Full path to xml storage file.</param>
      /// <param name="rootElementName">The name of the root element of the config xml</param>
      /// <returns>Changed builder.</returns>
      /// <remarks>Storage file does not have to exist, however it will be created as soon as first write performed.</remarks>
      public static ConfigurationBuilder<TInterface> UseXmlFile<TInterface>(this ConfigurationBuilder<TInterface> builder, string xmlFilePath, string rootElementName = "config") where TInterface : class
      {
         builder.UseConfigStore(new XmlFileConfigStore(xmlFilePath, true, rootElementName));
         return builder;
      }

      /// <summary>
      /// Uses XML file as a builder storage.
      /// </summary>
      /// <param name="builder">Configuration object.</param>
      /// <param name="xmlString">Xml document.</param>
      /// <param name="rootElementName">The name of the root element of the config xml</param>
      /// <returns>Changed builder.</returns>
      /// <remarks>Storage file does not have to exist, however it will be created as soon as first write performed.</remarks>
      public static ConfigurationBuilder<TInterface> UseXmlString<TInterface>(this ConfigurationBuilder<TInterface> builder, string xmlString, string rootElementName = "config") where TInterface : class
      {
         builder.UseConfigStore(new XmlFileConfigStore(xmlString, false, rootElementName));
         return builder;
      }
   }
}
