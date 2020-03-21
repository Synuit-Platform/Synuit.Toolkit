//
//  Synuit.Toolkit.Utilities - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System;
using System.Net;
using System.Threading;

namespace Synuit.Toolkit.Utils.Net
{
   public class ApiWebClient : WebClient
   {
      protected override WebRequest GetWebRequest(Uri uri)
      {
         WebRequest w = base.GetWebRequest(uri);
         //default is 100 secs - too long for crypto API calls
         //if something is wrong with API prevent calls from taking 100s to timeout
         //set to 10 secs
         w.Timeout = 10000; // --> ten secs
         return w;
      }

      //
      public string DownloadAsString(string address)
      {
         return DownloadAsString(new Uri(address));
      }

      //
      public string DownloadAsString(Uri address)
      {
         string response;

         try
         {
            response = DownloadString(address);
         }
         catch (WebException)
         {
            //try again if Exception
            Thread.Sleep(750);
            response = DownloadString(address);
         }
         return response;
      }
   }
}