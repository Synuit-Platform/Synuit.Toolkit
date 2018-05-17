//
using Synuit.Toolkit.Metadata.Models;

namespace Synuit.Toolkit.Types.Metadata
{
   public interface IWorkspace { }

   public interface IWorkspace<T, I> : IWorkspace where T : Context where I : Item
   {
      IContext Context { get; set; }

      //
      void WriteItem(string path, string value, string name = "", string description = "");

      // -->
      string ReadItem(string path, string defaultValue = "");
   }
}