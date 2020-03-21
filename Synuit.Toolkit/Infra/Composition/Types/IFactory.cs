namespace Synuit.Toolkit.Infra.Composition.Types
{
   public interface IFactory<T>
   {
      T Create();
   }
}