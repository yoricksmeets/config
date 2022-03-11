using System;
using System.IO;
using Config.Net.Xml.Stores;
using Xunit;

namespace Config.Net.Tests.Stores
{
   public class XmlFileConfigStoreTest : AbstractTestFixture, IDisposable
   {
      private string _path;
      private XmlFileConfigStore _store;

      public XmlFileConfigStoreTest()
      {
         _path = Path.Combine(BuildDir.FullName, "TestData", "sample.xml");
         _store = new XmlFileConfigStore(_path, true, "config");
      }

      [Fact]
      public void Read_inline_property()
      {
         string value = _store.Read("ApplicationInsights.InstrumentationKey");

         Assert.NotNull(value);
         Assert.Equal("c75aaedd-7e93-4f67-b7b7-526f7924ccaa", value);
      }

      [Fact]
      public void Read_nested_inline_property()
      {
         string key = "Logging.LogLevel.Default";
         string value = _store.Read(key);

         Assert.Equal("Debug", value);
      }

      [Fact]
      public void Read_collection_length()
      {
         string key = "Numbers.$l";
         string value = _store.Read(key);

         Assert.Equal("3", value);
      }

      [Fact]
      public void Read_collection_single_item_length()
      {
         string key = "Numbers[1]";
         string value = _store.Read(key);

         Assert.Equal("1", value);
      }

      public void Dispose()
      {
         _store.Dispose();
      }
   }
}
