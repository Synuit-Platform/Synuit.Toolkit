using System.Net;
using Xunit;
using Synuit.Toolkit.Utils.Net;
//
namespace Synuit.Toolkit.Tests
{
   public class IpifyTestFixture
   {
      [Fact]
      public void GetAddress_ReturnsStringContainingAnIPAddress()
      {
         string ip = Ipify.GetPublicAddress();
         Assert.True(IPAddress.TryParse(ip, out IPAddress ipAddress));
      }

      [Fact]
      public void GetIPAddress_ReturnsStringContainingAnIPAddressUsingHttps()
      {
         string ip = Ipify.GetPublicAddress(true);
         Assert.True(IPAddress.TryParse(ip, out IPAddress ipAddress));
      }

      [Fact]
      public void GetIPAddress_ReturnsIPAddressInstance()
      {
         IPAddress ipAddress = Ipify.GetPublicIPAddress();
         Assert.NotNull(ipAddress);
         Assert.NotEqual(IPAddress.None, ipAddress);
      }

      [Fact]
      public void GetIPAddress_ReturnsIPAddressInstanceUsingHttps()
      {
         IPAddress ipAddress = Ipify.GetPublicIPAddress(true);
         Assert.NotNull(ipAddress);
         Assert.NotEqual(IPAddress.None, ipAddress);
      }
   }
}
