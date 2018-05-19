using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;

namespace Synuit.Toolkit.Utilities
{
 
   public class Common
   {
      private const uint maxNameSize = 12;
      private const uint minNameSize = 3;
      private const string nameRange = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      private const string isEmptyTokens = "\t\r\n\f\v ";

      private const uint maxMessageSize = 1024;

      //
      public static string formatUsername(string name)
      {
         try
         {
            /*return name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 1)*/
            return name.ToLower();
         }
         catch (ArgumentOutOfRangeException)
         {
         }
         return name;
      }

      //$!!$
      ////public static string getSystemUsername()
      ////{
      ////   string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
      ////   int pos = username.IndexOf("\\");
      ////   if (pos > 0)
      ////   {
      ////      username = username.Substring(pos + 1, username.Length - pos - 1);
      ////   }
      ////   return formatUsername(username);
      ////}

      //
      public static string validateName(string sname)
      {
         if (sname.Length > maxNameSize || sname.Length < minNameSize)
         {
            string msg = "Client name must be between " + minNameSize + " and " + maxNameSize + " characters in length.";
            throw new Exception(msg);
         }
         /*if(in.find_last_not_of(nameRange) != string::npos)
         {
         throw string("Invalid character in name. Valid characters are letter and digits.");
         */

         //string out = in;
         //transform(out.begin(), out.end(), out.begin(), ::tolower);
         //if(out.begin() != out.end())
         //{
         //   transform(out.begin(), out.begin() + 1, out.begin(), ::toupper);
         //}
         return sname;
      }

      public static string validateMessage(string smsg)
      {
         if (smsg.Length > maxMessageSize)
         {
            //$!!$ PEACE.InvalidMessageException ex;
            //string msg;
            //msg = "Message length exceeded, maximum length is " + maxMessageSize + " characters.";
            //ex.reason = msg.str();
            //$!!$ throw new ex;
         }

         //if(smsg.find_last_not_of(isEmptyTokens) == string::npos)
         if (smsg == "")
         {
            string msg = "Your message is empty and was ignored.";
            //$!!$throw (new Exception )();
         }
         // Strip html codes in the message
         string sout;
         foreach (char chr in smsg)
         {
            switch (chr)
            {
               case '&':
                  {
                     //$!!$sout += "&amp;";
                     break;
                  }

               case '"':
                  {
                     //$!!$sout += "&quot;";
                     break;
                  }

               case '\'':
                  {
                     //$!!$ sout += "&#39;";
                     break;
                  }

               case '<':
                  {
                     //$!!$ sout += "&lt;";
                     break;
                  }

               case '>':
                  {
                     //$!!$sout += "&gt;";
                     break;
                  }

               case '\r':
               case '\n':
               case '\v':
               case '\f':
               case '\t':
                  {
                     //$!!$sout += " ";
                     break;
                  }

               default:
                  {
                     //sout.push_back(*it);
                     break;
                  }
            }
         }
         //$!!$return sout;
         return smsg;
      }

      ////////////////////////////////////////////////////////////////////////////////

      public static void SaveObject(object Object, Type type, string filepath /*, Type[] types*/  )
      {
         System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(type);

         System.IO.StreamWriter file = new System.IO.StreamWriter(filepath);
         writer.Serialize(file, Object);
         file.Close();
      }

      //
      public static void SaveObject<T>(T Object, string filepath)
      {
         System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(T));

         System.IO.StreamWriter file = new System.IO.StreamWriter(filepath);
         writer.Serialize(file, Object);
         file.Close();
      }

      //
      public static object LoadObject(Type type, string filepath /*, Type[] types*/  )
      {
         System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(type);
         System.IO.StreamReader file = new System.IO.StreamReader(filepath);

         object obj = reader.Deserialize(file);
         file.Close();
         return obj;
      }

      //
      public static T LoadObject<T>(string filepath) where T : class
      {
         System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(T));
         System.IO.StreamReader file = new System.IO.StreamReader(filepath);

         T obj = (T)reader.Deserialize(file);
         file.Close();
         return obj;
      }

      public static string Serialize(object Object, Type type)
      {
         System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(type);

         //System.IO.StreamWriter file = new System.IO.StreamWriter(filepath);
         StringWriter tw = new StringWriter();

         writer.Serialize(tw, Object);
         return tw.ToString();
      }

      public static string Serialize<T>(T Object)
      {
         System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(T));
         StringWriter tw = new StringWriter();
         writer.Serialize(tw, Object);
         return tw.ToString();
      }

      public static T Deserialize<T>(string str) where T : class
      {
         System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(T));
         XmlReader xml = XmlReader.Create(new StringReader(str));
         return (T)reader.Deserialize(xml);
      }

      public static T Deserialize<T>(Type type, string str) where T : class
      {
         System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(type);
         //System.IO.StreamReader file = new System.IO.StreamReader(new MemoryStream(objec));

         XmlReader xml = XmlReader.Create(new StringReader(str));

         return (T)reader.Deserialize(xml);
      }

      //
      public static bool DirectoryExists(string path)
      {
         return (Directory.Exists(path));
      }

      //
      public static bool FileExists(string filepath)
      {
         return (File.Exists(filepath));
      }

      //
      public static string FormatFilePath(string filepath)
      {
         return (filepath.Replace("\\\\", "\\"));
      }
   }
}

