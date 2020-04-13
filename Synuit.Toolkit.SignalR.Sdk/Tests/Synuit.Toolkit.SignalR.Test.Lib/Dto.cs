
namespace ModelLib
{
    public class Dto
    {
        public string ClientId { get; set; }
        public int Data { get; set; }

        public override string ToString()
        {
            return $"{ClientId}     {Data}";
        }
    }
}
