using Precept.Architecture;
using Precept.Architecture.DI;
using Precept.Architecture.Types.DI;

//
namespace Precept.Metadata.Composition
{
   /*
   Dependency injection container that is responsible for resolving
   all of the requested dependencies at run time.
   */

   public class Container : AbstractContainer
   {
      private static readonly IContainer Instance = new Container();

      // --> Responsible for configuring the container and prepare it to resolve dependency requests
      override protected void Setup()
      {
         //RegisterType<Session.Types.ISessionManager, Session.SessionManager>();
      }

      // --> Get reference to the static container instance
      public static IContainer Get { get { return Instance; } }

      // -->
      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            base.Dispose(disposing);
         }
      }

      public void initialize()
      {
         //throw new NotImplementedException();
      }

      public void destroy()
      {
         //throw new NotImplementedException();
      }
   }
}