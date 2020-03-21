using MessagePack;
using MessagePack.AspNetCoreMvcFormatter;

namespace Microsoft.Extensions.DependencyInjection
{
   //
   // Summary:
   //     Extensions methods for configuring MVC via an Microsoft.Extensions.DependencyInjection.IMvcBuilder.
   public static class MessagePackMvcBuilderExtensions
   {
      //
      // Summary:
      //     Configures Newtonsoft.Json specific features such as input and output formatters.
      //
      // Parameters:
      //   builder:
      //     The Microsoft.Extensions.DependencyInjection.IMvcBuilder.
      //
      // Returns:
      //     The Microsoft.Extensions.DependencyInjection.IMvcBuilder.
      public static IMvcBuilder AddMessagePackFormatters(this IMvcBuilder builder)
      {
         MessagePackSerializerOptions LZ4Standard = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);

         builder.AddMvcOptions(option =>
         {
            option.OutputFormatters.Add(new MessagePackOutputFormatter(LZ4Standard));
            option.InputFormatters.Add(new MessagePackInputFormatter(LZ4Standard));
         });

         return builder;
      }

      ////////
      //////// Summary:
      ////////     Configures Newtonsoft.Json specific features such as input and output formatters.
      ////////
      //////// Parameters:
      ////////   builder:
      ////////     The Microsoft.Extensions.DependencyInjection.IMvcBuilder.
      ////////
      ////////   setupAction:
      ////////     Callback to configure Microsoft.AspNetCore.Mvc.MvcNewtonsoftJsonOptions.
      ////////
      //////// Returns:
      ////////     The Microsoft.Extensions.DependencyInjection.IMvcBuilder.
      //////public static IMvcBuilder AddMessagePackFormatters(this IMvcBuilder builder, Action<MvcNewtonsoftJsonOptions> setupAction);
   }
}