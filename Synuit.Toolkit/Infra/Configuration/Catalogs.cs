using System.Collections.Generic;

namespace Synuit.Toolkit.Infra.Configuration
{

   public class Catalogs : List<Catalog> { }
   public class Catalog: MonikerRegistry 
   {
      public string Name { get; set; }
      public string Mask { get; set; } // i.e. *.api.dll
      public string Type { get; set; }
      public string Path { get; set; }
      public Namespaces Namespaces { get; set; }
   }
 
}