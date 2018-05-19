// Decompiled with JetBrains decompiler
// Type: Spacejock.clsFile
// Assembly: Spacejock, Version=1.0.5856.28163, Culture=neutral, PublicKeyToken=5bed04c17873f94a
// MVID: 4DD99659-F8AD-4207-88FF-99601D1C7EC7
// Assembly location: C:\Program Files (x86)\yWriter5\Spacejock.dll

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.FileIO;
using Spacejock.My;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Spacejock
{
  public class clsFile
  {
    private cDelay cDelay;
    private clsText cText;
    private Encoding VB6TextEncoding;
    private bool mvarStopFill;
    private bool mvarStopCopy;
    private long mvarCopyFileSize;
    private long mvarCopyFileProgress;
    private TimeSpan MINDIFF;
    private clsLog cLog;

    private bool StopFill
    {
      get
      {
        return this.mvarStopFill;
      }
      set
      {
        this.mvarStopFill = value;
      }
    }

    public string SepChar
    {
      get
      {
        return Conversions.ToString(modGlobals.SEPCHAR);
      }
    }

    public event clsFile.ProgressEventHandler Progress;

    public event clsFile.WarningEventHandler Warning;

    public event clsFile.CountEventHandler Count;

    public event clsFile.FillingFolderEventHandler FillingFolder;

    public clsFile(ref clsLog LoggingClass)
    {
      this.cDelay = new cDelay();
      this.VB6TextEncoding = Encoding.GetEncoding(28591);
      this.MINDIFF = new TimeSpan(0, 0, 1);
      this.cLog = LoggingClass;
      this.cText = new clsText(ref this.cLog);
    }

    public void StopFillfiles()
    {
      this.StopFill = true;
    }

    public void StopCopy()
    {
      this.mvarStopCopy = true;
    }

    public long ReadCopyFileSize()
    {
      return this.mvarCopyFileSize;
    }

    public long ReadCopyFileProgress()
    {
      return this.mvarCopyFileProgress;
    }

    public bool CopyFile(string Sourcefilename, string DestFilename)
    {
      return this.CopyFileWithRetCode(Sourcefilename, DestFilename) == 0;
    }

    public int CopyFileWithRetCode(string Sourcefilename, string DestFilename)
    {
      this.mvarStopCopy = false;
      int num;
      try
      {
        if (Operators.CompareString(this.FileOnly(DestFilename), "", false) == 0 && Operators.CompareString(DestFilename, "", false) != 0)
          DestFilename = Path.Combine(DestFilename, this.FileOnly(Sourcefilename));
        if (this.CopyFileWithProgress(Sourcefilename, DestFilename))
        {
          this.SetFileDateTime(DestFilename, this.FileDate(Sourcefilename));
          num = 0;
        }
      }
      catch (UnauthorizedAccessException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        num = 50;
        this.cLog.AddAction("Unable to copy " + Sourcefilename + " ... access denied", "", "");
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        num = 50;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return num;
    }

    private bool CopyFileWithProgress(string Sourcefilename, string DestFilename)
    {
      byte[] buffer = new byte[65000];
      this.mvarStopCopy = false;
      if (this.FileExists(DestFilename))
        this.MakeReadWrite(DestFilename);
      bool flag;
      try
      {
        FileStream fileStream1 = new FileStream(Sourcefilename, FileMode.Open, FileAccess.Read, FileShare.Read);
        BinaryReader binaryReader = new BinaryReader((Stream) fileStream1);
        FileStream fileStream2 = new FileStream(DestFilename, FileMode.Create, FileAccess.Write);
        BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream2);
        this.mvarCopyFileSize = fileStream1.Length;
        this.mvarCopyFileProgress = 0L;
        for (int count = binaryReader.Read(buffer, 0, 65000); count > 0; count = binaryReader.Read(buffer, 0, 65000))
        {
          binaryWriter.Write(buffer, 0, count);
          if (count == 65000)
            cDelay.RegularDoEvents();
          this.mvarCopyFileProgress = this.mvarCopyFileProgress + (long) count;
          clsFile.ProgressEventHandler progressEvent = this.ProgressEvent;
          if (progressEvent != null)
            progressEvent(Sourcefilename, this.mvarCopyFileProgress, this.mvarCopyFileSize);
          if (this.mvarStopCopy)
            break;
        }
        binaryReader.Close();
        binaryWriter.Close();
        fileStream1.Close();
        fileStream2.Close();
        if (this.mvarStopCopy)
        {
          File.Delete(DestFilename);
          flag = false;
          this.cLog.AddAction("Copying of file " + Sourcefilename + " stopped by user", "", "");
          clsFile.WarningEventHandler warningEvent = this.WarningEvent;
          if (warningEvent != null)
            warningEvent(this.cLog.GetLatest);
        }
        else
          flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw ex;
      }
      return flag;
    }

    public string[] ReadTextFileIntoArray(string Filename)
    {
      return this.TextFileIntoArray(Filename);
    }

    public string[] TextFileIntoArray(string Filename)
    {
      string TextString = "";
      this.LoadTextFile(Filename, ref TextString, false);
      return this.cText.TextStringToArray(TextString);
    }

    public string[] TextFileIntoArray(string Filename, string Delimiter)
    {
      string TextString = "";
      this.LoadTextFile(Filename, ref TextString, false);
      return this.cText.TextStringToArray(TextString, Delimiter);
    }

    public string[] TextFileIntoArray(string Filename, bool UseSystemEncoding)
    {
      string TextString = "";
      this.LoadTextFile(Filename, ref TextString, false);
      return this.cText.TextStringToArray(TextString);
    }

    public Collection TextFileIntoCollection(string Filename, string WithKey)
    {
      Collection collection = new Collection();
      string TextString = "";
      this.LoadTextFile(Filename, ref TextString, false);
      foreach (string Key in this.cText.TextStringToArray(TextString))
      {
        if (Operators.CompareString(Key.Trim(), "", false) != 0)
        {
          if (Conversions.ToBoolean(WithKey))
          {
            if (!collection.Contains(Key))
              collection.Add((object) Key, Key, (object) null, (object) null);
          }
          else
            collection.Add((object) Key, (string) null, (object) null, (object) null);
        }
      }
      return collection;
    }

    public void WriteCollectionToFile(string Filename, Collection cCol)
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        try
        {
          foreach (object obj in cCol)
          {
            string str = Conversions.ToString(obj);
            stringBuilder.AppendLine(str);
          }
        }
        finally
        {
          IEnumerator enumerator;
          if (enumerator is IDisposable)
            (enumerator as IDisposable).Dispose();
        }
        this.WriteTextFile(Filename, stringBuilder.ToString(), false);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public void WriteCollectionToUnicodeFile(string Filename, Collection cCol)
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        try
        {
          foreach (object obj in cCol)
          {
            string str = Conversions.ToString(obj);
            stringBuilder.AppendLine(str);
          }
        }
        finally
        {
          IEnumerator enumerator;
          if (enumerator is IDisposable)
            (enumerator as IDisposable).Dispose();
        }
        this.WriteUnicodeForced(Filename, stringBuilder.ToString());
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }

    public string ReadTextFile(string Filename)
    {
      string TextString = "";
      this.LoadTextFile(Filename, ref TextString, false);
      return TextString;
    }

    public bool ReadTextFile(string Filename, ref string TextString)
    {
      return this.LoadTextFile(Filename, ref TextString, false);
    }

    public bool ReadTextFile(string Filename, ref string TextString, Encoding TextEncoding)
    {
      TextString = "";
      bool flag = false;
      try
      {
        if (this.FileExists(Filename))
          TextString = File.ReadAllText(Filename, TextEncoding);
        flag = true;
      }
      catch (UnauthorizedAccessException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        this.cLog.AddAction("Access denied: " + Filename, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        this.cLog.AddAction("File not found: " + Filename, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool LoadTextFile(string Filename, ref string TextString, bool UseSystemEncoding = false)
    {
      TextString = "";
      bool flag = false;
      try
      {
        if (this.FileExists(Filename))
          TextString = !UseSystemEncoding ? File.ReadAllText(Filename, this.VB6TextEncoding) : File.ReadAllText(Filename, Encoding.Default);
        flag = true;
      }
      catch (UnauthorizedAccessException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        this.cLog.AddAction("Access denied: " + Filename, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        this.cLog.AddAction("File not found: " + Filename, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool ReadTextAuto(string Filename, ref string TextString, bool UseSysEncoding)
    {
      TextString = "";
      bool flag = false;
      try
      {
        if (this.FileExists(Filename))
          TextString = !this.IsUnicodeFile(Filename) ? (!UseSysEncoding ? File.ReadAllText(Filename, this.VB6TextEncoding) : File.ReadAllText(Filename, Encoding.Default)) : File.ReadAllText(Filename, Encoding.UTF8);
        flag = true;
      }
      catch (UnauthorizedAccessException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        this.cLog.AddAction("Access denied: " + Filename, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        this.cLog.AddAction("File not found: " + Filename, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool WriteTextFile(string Filename, string TextString, bool UseSystemEncoding = false)
    {
      bool flag;
      try
      {
        try
        {
          this.EraseFile(Filename);
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
        if (UseSystemEncoding)
          File.WriteAllText(Filename, TextString, Encoding.Default);
        else
          File.WriteAllText(Filename, TextString, this.VB6TextEncoding);
        flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool WriteTextFile(string Filename, string TextString, Encoding Encoding)
    {
      bool flag;
      try
      {
        try
        {
          this.EraseFile(Filename);
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
        File.WriteAllText(Filename, TextString, Encoding);
        flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool WriteAsciiForced(string Filename, string TextString)
    {
      bool flag;
      try
      {
        this.EraseFile(Filename);
        File.WriteAllText(Filename, TextString, Encoding.ASCII);
        flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool AppendToTextFile(string Filename, ref string TextString, bool UseSystemEncoding = false)
    {
      bool flag;
      try
      {
        if (UseSystemEncoding)
          File.AppendAllText(Filename, TextString + "\r\n", Encoding.Default);
        else
          File.AppendAllText(Filename, TextString + "\r\n", this.VB6TextEncoding);
        flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool PrintToTextFile(string Filename, ref string TextString, bool UseSystemEncoding = false)
    {
      string Filename1 = Filename;
      string TextString1 = TextString + "\r\n";
      int num = UseSystemEncoding ? 1 : 0;
      this.AppendToTextFile(Filename1, ref TextString1, num != 0);
      bool flag;
      return flag;
    }

    public bool SaveTextFile(string Filename, string TextString, bool UseSystemEncoding = false)
    {
      return this.WriteTextFile(Filename, TextString, UseSystemEncoding);
    }

    public bool WriteFileSysEncoded(string Filename, string TextString)
    {
      return this.WriteTextFile(Filename, TextString, true);
    }

    public bool WriteUnicodeForced(string Filename, string TextString)
    {
      bool flag;
      try
      {
        try
        {
          this.EraseFile(Filename);
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
        File.WriteAllText(Filename, TextString, Encoding.UTF8);
        flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool ReadUnicodeForced(string Filename, ref string TextString)
    {
      bool flag;
      try
      {
        TextString = File.ReadAllText(Filename, Encoding.UTF8);
        flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool ReadAsciiForced(string Filename, ref string TextString)
    {
      bool flag;
      try
      {
        TextString = File.ReadAllText(Filename, this.VB6TextEncoding);
        flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool IsUnicodeFile(string Filename)
    {
      byte[] buffer = new byte[3];
      bool flag = false;
      if (this.FileExists(Filename))
      {
        try
        {
          FileStream fileStream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
          BinaryReader binaryReader = new BinaryReader((Stream) fileStream);
          int num;
          if (fileStream.Length >= 3L)
            num = binaryReader.Read(buffer, 0, 3);
          else if (fileStream.Length == 2L)
            num = binaryReader.Read(buffer, 0, 2);
          if (num >= 2)
          {
            if (num == 3 && Operators.CompareString(Conversion.Hex(buffer[0]), "EF", false) == 0 & Operators.CompareString(Conversion.Hex(buffer[1]), "BB", false) == 0 & Operators.CompareString(Conversion.Hex(buffer[2]), "BF", false) == 0)
              flag = true;
            if (Operators.CompareString(Conversion.Hex(buffer[0]), "FF", false) == 0 & Operators.CompareString(Conversion.Hex(buffer[1]), "FE", false) == 0)
              flag = true;
            else if (Operators.CompareString(Conversion.Hex(buffer[0]), "FE", false) == 0 & Operators.CompareString(Conversion.Hex(buffer[1]), "FF", false) == 0)
              flag = true;
          }
          binaryReader.Close();
          fileStream.Close();
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
          ProjectData.ClearProjectError();
        }
      }
      return flag;
    }

    public bool FirstFileNewer(string Sourcefilename, string DestFilename)
    {
      return this.FileDate(Sourcefilename).Subtract(this.FileDate(DestFilename)) > this.MINDIFF;
    }

    public bool CopyFileIfNewer(string Sourcefilename, string DestFilename)
    {
      bool flag1 = false;
      try
      {
        if (this.FileExists(DestFilename))
        {
          if (this.FirstFileNewer(Sourcefilename, DestFilename))
            flag1 = true;
        }
        else
          flag1 = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag1 = true;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      bool flag2;
      if (flag1)
      {
        try
        {
          flag2 = this.CopyFile(Sourcefilename, DestFilename);
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          Exception Ex = ex;
          flag2 = false;
          this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
          ProjectData.ClearProjectError();
        }
      }
      return flag2;
    }

    public bool MoveFolder(string SourceFolder, string DestFolder)
    {
      bool flag;
      try
      {
        Directory.Move(SourceFolder, DestFolder);
        flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool MoveFile(string Sourcefilename, string DestFilename)
    {
      bool flag;
      try
      {
        if (this.FileExists(DestFilename))
          this.EraseFile(DestFilename);
        File.Move(Sourcefilename, DestFilename);
        flag = true;
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool isFolderWriteable(string Folder)
    {
      bool flag = false;
      if (this.FolderExists(Folder))
      {
        string Filename = Path.Combine(Folder, "abc");
        int num = 0;
        for (; this.FileExists(Filename); Filename = Path.Combine(Folder, "abc") + Conversions.ToString(num))
          ++num;
        try
        {
          this.WriteTextFile(Filename, "nothing", false);
          this.EraseFile(Filename);
          flag = true;
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
      }
      return flag;
    }

    public bool isWriteable(string Filename)
    {
      bool flag = false;
      if (this.isFolderWriteable(this.PathOnly(Filename)))
      {
        if (!this.FileExists(Filename))
        {
          flag = true;
        }
        else
        {
          try
          {
            new FileStream(Filename, FileMode.Append).Close();
            flag = true;
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            this.cLog.AddAction(ex.Message, MethodBase.GetCurrentMethod());
            ProjectData.ClearProjectError();
          }
        }
      }
      return flag;
    }

    public bool isReadonly(string Filename)
    {
      bool flag;
      try
      {
        flag = (File.GetAttributes(Filename) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool MakeReadWrite(string Filename)
    {
      bool flag = false;
      try
      {
        File.SetAttributes(Filename, FileAttributes.Normal);
        flag = true;
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool FileExists(string Filename)
    {
      bool flag;
      try
      {
        if (Filename != null)
        {
          if (Operators.CompareString(Filename, "", false) != 0)
          {
            string str = Path.GetDirectoryName(Filename);
            if (Operators.CompareString(str, "", false) == 0)
              str = "." + this.SepChar;
            if (Filename.Contains("?") | Filename.Contains("*"))
              flag = MyProject.Computer.FileSystem.GetFiles(str, Microsoft.VisualBasic.FileIO.SearchOption.SearchTopLevelOnly, Path.GetFileName(Filename)).Count > 0;
            else
              flag = MyProject.Computer.FileSystem.FileExists(Filename);
          }
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool FileExistsRecursed(string Folder, string Filename)
    {
      bool flag;
      try
      {
        string str = Path.GetDirectoryName(Folder);
        if (Operators.CompareString(str, "", false) == 0)
          str = "." + this.SepChar;
        flag = MyProject.Computer.FileSystem.GetFiles(str, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, Path.GetFileName(Filename)).Count > 0;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool FolderIsEmpty(string Foldername)
    {
      try
      {
        return ((MyProject.Computer.FileSystem.GetFiles(Foldername, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*").Count > 0 ? 1 : 0) | (MyProject.Computer.FileSystem.GetFiles(Foldername, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*.*").Count > 0 ? 1 : 0)) == 0;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw ex;
      }
    }

    public bool FolderExists(string Foldername)
    {
      bool flag;
      try
      {
        flag = Directory.Exists(Foldername);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool MkMissingDir(string Foldername)
    {
      bool flag = false;
      try
      {
        if (!this.FolderExists(Foldername))
        {
          if (Operators.CompareString(Strings.Trim(Foldername), "", false) != 0)
          {
            Directory.CreateDirectory(Foldername);
            flag = true;
          }
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public string DriveOnly(string DrivePlusFolder)
    {
      string str = "";
      if (DrivePlusFolder.Length == 1)
        str = DrivePlusFolder;
      else if (DrivePlusFolder.Contains(":"))
        str = Strings.Left(DrivePlusFolder, 1);
      else if (DrivePlusFolder.StartsWith("\\\\"))
        str = DrivePlusFolder;
      else if (DrivePlusFolder.StartsWith("/"))
        str = DrivePlusFolder;
      return str;
    }

    public string FolderExDrive(string DrivePlusFolder)
    {
      string str = DrivePlusFolder;
      if (DrivePlusFolder.StartsWith("\\\\"))
      {
        int num = Strings.InStr(3, DrivePlusFolder, this.SepChar, CompareMethod.Binary);
        if (num != 0)
          str = Strings.Mid(DrivePlusFolder, num + 1);
      }
      else if (Strings.InStr(DrivePlusFolder, ":", CompareMethod.Binary) > 0)
      {
        int num = Strings.InStr(DrivePlusFolder, ":" + Conversions.ToString(modGlobals.SEPCHAR), CompareMethod.Binary);
        if (num != 0)
          str = Strings.Mid(DrivePlusFolder, num + 2);
      }
      else
        str = DrivePlusFolder;
      return str;
    }

    public string PathOnly(string PathPlusFile)
    {
      int length = Strings.InStrRev(PathPlusFile, this.SepChar, -1, CompareMethod.Binary);
      return length != 0 ? PathPlusFile.Substring(0, length) : "." + this.SepChar;
    }

    public clsFileType FileObject(string Filename)
    {
      return new clsFileType(Filename);
    }

    public string ExtractPath(string PathPlusFile)
    {
      int Length = Strings.InStrRev(PathPlusFile, this.SepChar, -1, CompareMethod.Binary);
      return Length != 0 ? Strings.Left(PathPlusFile, Length) : "";
    }

    public string ExtOnly(string Filename)
    {
      string str = "";
      try
      {
        str = Path.GetExtension(Filename);
        str = str != null ? str.Replace(".", "") : "";
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public string FileOnly(string PathPlusFile)
    {
      return Path.GetFileName(PathPlusFile);
    }

    public DateTime FileDate(string Filename)
    {
      DateTime dateTime;
      try
      {
        dateTime = File.GetLastWriteTime(Filename);
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        dateTime = DateTime.FromOADate(0.0);
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        dateTime = DateTime.FromOADate(0.0);
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return dateTime;
    }

    public bool FillFolders(string BasePath, ref Collection Subfolders)
    {
      return this.LocalFillFolders(BasePath, ref Subfolders, false);
    }

    public bool FillFoldersRecursed(string BasePath, string BaseFolder, ref Collection Subfolders)
    {
      return this.LocalFillFolders(BasePath, ref Subfolders, true);
    }

    public bool FillFoldersRecursed(string BasePath, ref Collection Subfolders)
    {
      return this.LocalFillFolders(BasePath, ref Subfolders, true);
    }

    private bool LocalFillFolders(string BasePath, ref Collection Subfolders, bool Recurse)
    {
      bool flag;
      try
      {
        BasePath = this.AddSlashIfMissing(BasePath);
        if (!Path.IsPathRooted(BasePath))
          BasePath = Path.GetFullPath(BasePath);
        if (Strings.InStr(BasePath, "\\\\", CompareMethod.Binary) > 1)
          BasePath = Strings.Replace(BasePath, "\\\\", "\\", 2, -1, CompareMethod.Binary);
        int num1 = Strings.Len(BasePath);
        ReadOnlyCollection<string> directories;
        if (Recurse)
        {
          try
          {
            directories = MyProject.Computer.FileSystem.GetDirectories(BasePath, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories);
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
            ProjectData.ClearProjectError();
            goto label_23;
          }
        }
        else
        {
          try
          {
            directories = MyProject.Computer.FileSystem.GetDirectories(BasePath, Microsoft.VisualBasic.FileIO.SearchOption.SearchTopLevelOnly);
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
            ProjectData.ClearProjectError();
            goto label_23;
          }
        }
        Subfolders = new Collection();
        IEnumerator<string> enumerator;
        try
        {
          enumerator = directories.GetEnumerator();
          while (enumerator.MoveNext())
          {
            string current = enumerator.Current;
            int num2;
            ++num2;
            string Left = Strings.Mid(current, num1 + 1);
            if (Operators.CompareString(Left, ".", false) != 0 & Operators.CompareString(Left, "..", false) != 0)
            {
              string Key = Left + this.SepChar;
              Subfolders.Add((object) Key, Key, (object) null, (object) null);
            }
            if (num2 == 100)
            {
              num2 = 0;
              cDelay.RegularDoEvents();
              if (this.mvarStopFill)
                goto label_23;
            }
          }
        }
        finally
        {
          if (enumerator != null)
            enumerator.Dispose();
        }
        cDelay.RegularDoEvents();
        flag = true;
      }
      catch (DirectoryNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
label_23:
      return flag;
    }

    public void FillFiles(string BasePath, string ValidExtensionList, ref Collection AllFiles)
    {
      this.LocalFillFilesStart(BasePath, ValidExtensionList, ref AllFiles, false, false, DateTime.MinValue, false);
    }

    public void FillFilesAfterDate(string BasePath, string ValidExtensionList, ref Collection AllFiles, DateTime CutoffDate)
    {
      this.LocalFillFilesStart(BasePath, ValidExtensionList, ref AllFiles, true, false, CutoffDate, false);
    }

    public void FillFilesBeforeDate(string BasePath, string ValidExtensionList, ref Collection AllFiles, DateTime CutoffDate)
    {
      this.LocalFillFilesStart(BasePath, ValidExtensionList, ref AllFiles, false, true, CutoffDate, false);
    }

    public void FillFilesRecursed(string BasePath, string ValidExtensionList, ref Collection AllFiles)
    {
      this.LocalFillFilesRecursedStart(BasePath, ValidExtensionList, ref AllFiles, false, false, DateTime.MinValue, false);
    }

    public void FillFilesAfterDateRecursed(string BasePath, string ValidExtensionList, ref Collection AllFiles, DateTime CutoffDate)
    {
      this.LocalFillFilesRecursedStart(BasePath, ValidExtensionList, ref AllFiles, true, false, CutoffDate, false);
    }

    public void FillFilesBeforeDateRecursed(string BasePath, string ValidExtensionList, ref Collection AllFiles, DateTime CutoffDate)
    {
      this.LocalFillFilesRecursedStart(BasePath, ValidExtensionList, ref AllFiles, false, true, CutoffDate, false);
    }

    private void LocalFillFilesStart(string BasePath, string ValidExtensionList, ref Collection AllFiles, bool AfterDate, bool BeforeDate, DateTime CutoffDate, bool UseObjects)
    {
      if (AllFiles == null)
        AllFiles = new Collection();
      ValidExtensionList = this.ReformatExtensionList(ValidExtensionList);
      string[] ExtArr = Strings.Split(ValidExtensionList, ";", -1, CompareMethod.Binary);
      BasePath = this.AddSlashIfMissing(BasePath);
      if (!Path.IsPathRooted(BasePath))
        BasePath = Path.GetFullPath(BasePath);
      if (Strings.InStr(BasePath, "\\\\", CompareMethod.Binary) > 1)
        BasePath = Strings.Replace(BasePath, "\\\\", "\\", 2, -1, CompareMethod.Binary);
      int BasePathLen = Strings.Len(BasePath);
      this.StopFill = false;
      this.LocalFillFiles(BasePath, BasePathLen, ExtArr, ref AllFiles, AfterDate, BeforeDate, CutoffDate, UseObjects);
    }

    private void LocalFillFilesRecursedStart(string BasePath, string ValidExtensionList, ref Collection AllFiles, bool AfterDate, bool BeforeDate, DateTime CutoffDate, bool UseObjects)
    {
      string BaseFolder = "";
      if (AllFiles == null)
        AllFiles = new Collection();
      ValidExtensionList = this.ReformatExtensionList(ValidExtensionList);
      BasePath = this.AddSlashIfMissing(BasePath);
      this.StopFill = false;
      if (!Path.IsPathRooted(BasePath))
        BasePath = Path.GetFullPath(BasePath);
      if (Strings.InStr(BasePath, "\\\\", CompareMethod.Binary) > 1)
        BasePath = Strings.Replace(BasePath, "\\\\", "\\", 2, -1, CompareMethod.Binary);
      int BasePathLen = Strings.Len(BasePath);
      string[] ExtArr = Strings.Split(ValidExtensionList, ";", -1, CompareMethod.Binary);
      this.LocalFillFilesRecursed(BasePath, BasePathLen, BaseFolder, ref ExtArr, ref AllFiles, AfterDate, BeforeDate, CutoffDate, UseObjects);
    }

    private void LocalFillFiles(string BasePath, int BasePathLen, string[] ExtArr, ref Collection AllFiles, bool AfterDate, bool BeforeDate, DateTime CutoffDate, bool UseObjects)
    {
      if (UseObjects)
        this.cLog.AddAction("FIXME: Filling using objects not supported", "", "");
      if (!this.FolderExists(BasePath))
        return;
      try
      {
        if (this.mvarStopFill)
          return;
        ReadOnlyCollection<string> files = MyProject.Computer.FileSystem.GetFiles(BasePath, Microsoft.VisualBasic.FileIO.SearchOption.SearchTopLevelOnly, ExtArr);
        IEnumerator<string> enumerator;
        try
        {
          enumerator = files.GetEnumerator();
          while (enumerator.MoveNext())
          {
            string current = enumerator.Current;
            bool flag = true;
            if (AfterDate | BeforeDate)
            {
              DateTime t1 = this.FileDate(current);
              if (AfterDate)
              {
                if (DateTime.Compare(t1, CutoffDate) <= 0)
                  flag = false;
              }
              else if (BeforeDate && DateTime.Compare(t1, CutoffDate) >= 0)
                flag = false;
            }
            if (flag)
            {
              string Key = Strings.Mid(current, BasePathLen + 1);
              AllFiles.Add((object) Key, Key, (object) null, (object) null);
            }
            int num;
            if (num == 100)
            {
              num = 0;
              cDelay.RegularDoEvents();
              if (this.mvarStopFill)
                break;
            }
          }
        }
        finally
        {
          if (enumerator != null)
            enumerator.Dispose();
        }
      }
      catch (DirectoryNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
    }

    private void LocalFillFilesRecursed(string BasePath, int BasePathLen, string BaseFolder, ref string[] ExtArr, ref Collection AllFiles, bool AfterDate, bool BeforeDate, DateTime CutoffDate, bool UseObjects)
    {
      if (UseObjects)
        this.cLog.AddAction("FIXME: Filling using objects not supported", "", "");
      try
      {
        if (this.mvarStopFill)
          return;
        Collection AllFiles1 = new Collection();
        this.LocalFillFiles(Path.Combine(BasePath, BaseFolder), BasePathLen, ExtArr, ref AllFiles1, AfterDate, BeforeDate, CutoffDate, UseObjects);
        if (this.mvarStopFill)
          return;
        try
        {
          foreach (object obj in AllFiles1)
          {
            string Key = Conversions.ToString(obj);
            AllFiles.Add((object) Key, Key, (object) null, (object) null);
          }
        }
        finally
        {
          IEnumerator enumerator;
          if (enumerator is IDisposable)
            (enumerator as IDisposable).Dispose();
        }
        clsFile.CountEventHandler countEvent = this.CountEvent;
        if (countEvent != null)
          countEvent((long) AllFiles.Count);
        AllFiles1.Clear();
        Collection Subfolders = new Collection();
        if (this.mvarStopFill)
          return;
        clsFile.FillingFolderEventHandler fillingFolderEvent = this.FillingFolderEvent;
        if (fillingFolderEvent != null)
          fillingFolderEvent(Path.Combine(BasePath, BaseFolder));
        this.LocalFillFolders(Path.Combine(BasePath, BaseFolder), ref Subfolders, false);
        cDelay.RegularDoEvents();
        try
        {
          foreach (object obj in Subfolders)
          {
            string path2 = Conversions.ToString(obj);
            if (this.mvarStopFill)
              return;
            this.LocalFillFilesRecursed(BasePath, BasePathLen, Path.Combine(BaseFolder, path2), ref ExtArr, ref AllFiles, AfterDate, BeforeDate, CutoffDate, UseObjects);
            if (this.mvarStopFill)
              return;
          }
        }
        finally
        {
          IEnumerator enumerator;
          if (enumerator is IDisposable)
            (enumerator as IDisposable).Dispose();
        }
        Subfolders.Clear();
      }
      catch (ArgumentNullException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        this.cLog.AddAction("Fillfiles failed: One of the supplied extensions is blank", "", "");
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
    }

    private string ReformatExtensionList(string ExtensionList)
    {
      if (Operators.CompareString(ExtensionList, "", false) == 0)
        ExtensionList = "*.*";
      else if (Operators.CompareString(ExtensionList, "*.*", false) != 0)
      {
        ExtensionList = Strings.Replace(ExtensionList, "*.", " ", 1, -1, CompareMethod.Binary);
        ExtensionList = Strings.Replace(ExtensionList, ";", " ", 1, -1, CompareMethod.Binary);
        ExtensionList = Strings.Replace(ExtensionList, ".", " ", 1, -1, CompareMethod.Binary);
        ExtensionList = Strings.Replace(ExtensionList, ",", " ", 1, -1, CompareMethod.Binary);
        ExtensionList = Strings.Replace(ExtensionList, "*", " ", 1, -1, CompareMethod.Binary);
        ExtensionList = this.cText.RemoveDblSpaces(ExtensionList);
        string[] SourceArray = Strings.Split(ExtensionList, " ", -1, CompareMethod.Binary);
        if (SourceArray.Length == 0)
        {
          ExtensionList = "*." + ExtensionList;
        }
        else
        {
          ExtensionList = Strings.Join(SourceArray, ";*.");
          ExtensionList = Strings.Mid(ExtensionList, 2);
        }
      }
      return ExtensionList;
    }

    public string FileWithoutExt(string Filename)
    {
      string str;
      try
      {
        str = Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        str = Filename;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public string Fileroot(string Filename)
    {
      string str;
      try
      {
        str = this.FileWithoutExt(Filename);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        str = Filename;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public string FileExPath(string Filename)
    {
      return this.FileOnly(Filename);
    }

    public string StripBadFileChar(string ProposedFilename)
    {
      string str = this.PathOnly(ProposedFilename);
      string Expression;
      if (Operators.CompareString(str, "", false) == 0 | Operators.CompareString(str, "." + this.SepChar, false) == 0)
      {
        str = "";
        Expression = ProposedFilename;
      }
      else
        Expression = this.FileOnly(ProposedFilename);
      string path2 = Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Expression, "?", "", 1, -1, CompareMethod.Binary), "\\", "", 1, -1, CompareMethod.Binary), "/", "", 1, -1, CompareMethod.Binary), "*", "", 1, -1, CompareMethod.Binary), ":", "", 1, -1, CompareMethod.Binary), ">", "", 1, -1, CompareMethod.Binary), "<", "", 1, -1, CompareMethod.Binary), "|", "", 1, -1, CompareMethod.Binary), this.SepChar, "", 1, -1, CompareMethod.Binary), "\"", "", 1, -1, CompareMethod.Binary);
      return Operators.CompareString(str, "", false) != 0 ? Path.Combine(str, path2) : path2;
    }

    public void CleanString(ref string StringToClean)
    {
      try
      {
        StringToClean = Strings.Replace(StringToClean, "\\", " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, this.SepChar, " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, "/", " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, ":", " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, "*", " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, "?", " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, "<", " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, ">", " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, "|", " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, "&", " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, ",", " ", 1, -1, CompareMethod.Binary);
        StringToClean = Strings.Replace(StringToClean, "%", " ", 1, -1, CompareMethod.Binary);
        if (Operators.CompareString(StringToClean, "PRN", false) == 0)
          StringToClean = "PRN1";
        if (Operators.CompareString(StringToClean, "CON", false) == 0)
          StringToClean = "CON1";
        if (Operators.CompareString(StringToClean, "NUL", false) == 0)
          StringToClean = "NUL1";
        if (Operators.CompareString(StringToClean, "AUX", false) == 0)
          StringToClean = "AUX1";
        StringToClean = Strings.Trim(Strings.Replace(StringToClean, "\"", " ", 1, -1, CompareMethod.Binary));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
    }

    public long LinesInFile(string Filename)
    {
      long num1 = 0;
      try
      {
        if (this.FileExists(Filename))
        {
          StreamReader streamReader = this.OpenAutoFile(Filename);
          try
          {
            long num2;
            while (!streamReader.EndOfStream)
            {
              streamReader.ReadLine();
              ++num2;
            }
            num1 = num2;
          }
          catch (Exception ex)
          {
            ProjectData.SetProjectError(ex);
            this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
            ProjectData.ClearProjectError();
          }
          finally
          {
            streamReader.Close();
          }
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return num1;
    }

    public StreamWriter OpenOutputStream(string Filename)
    {
      return this.CreateOutputFile(Filename);
    }

    public bool CloseFile(object AnyFileStreamObject)
    {
      bool flag;
      try
      {
        NewLateBinding.LateCall(AnyFileStreamObject, (System.Type) null, "close", new object[0], (string[]) null, (System.Type[]) null, (bool[]) null, true);
        flag = true;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public StreamWriter CreateOutputFile(string Filename)
    {
      StreamWriter streamWriter;
      try
      {
        streamWriter = new StreamWriter(Filename, false, this.VB6TextEncoding);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        streamWriter = (StreamWriter) null;
        ProjectData.ClearProjectError();
      }
      return streamWriter;
    }

    public StreamWriter CreateOutputUnicodeFile(string Filename)
    {
      StreamWriter streamWriter;
      try
      {
        streamWriter = new StreamWriter(Filename, false);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        streamWriter = (StreamWriter) null;
        ProjectData.ClearProjectError();
      }
      return streamWriter;
    }

    public StreamReader OpenAsciiFile(string Filename)
    {
      StreamReader streamReader;
      try
      {
        streamReader = new StreamReader(Filename, this.VB6TextEncoding);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        streamReader = (StreamReader) null;
        ProjectData.ClearProjectError();
      }
      return streamReader;
    }

    public StreamReader OpenUnicodeFile(string Filename)
    {
      StreamReader streamReader;
      try
      {
        streamReader = new StreamReader(Filename, Encoding.UTF8);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        streamReader = (StreamReader) null;
        ProjectData.ClearProjectError();
      }
      return streamReader;
    }

    public StreamReader OpenAutoFile(string Filename)
    {
      StreamReader streamReader;
      try
      {
        streamReader = !this.IsUnicodeFile(Filename) ? new StreamReader(Filename, this.VB6TextEncoding) : new StreamReader(Filename, Encoding.UTF8);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        streamReader = (StreamReader) null;
        ProjectData.ClearProjectError();
      }
      return streamReader;
    }

    public StreamWriter OpenAppendFile(string Filename)
    {
      StreamWriter streamWriter;
      try
      {
        streamWriter = this.FileExists(Filename) ? new StreamWriter(Filename, true, this.VB6TextEncoding) : this.CreateOutputFile(Filename);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        this.cLog.AddAction("Unable to open file for append: " + Filename, "", "");
        streamWriter = (StreamWriter) null;
        ProjectData.ClearProjectError();
      }
      return streamWriter;
    }

    public long FileSize(string Filename)
    {
      long num = 0;
      try
      {
        num = Microsoft.VisualBasic.FileSystem.FileLen(Filename);
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return num;
    }

    public bool EraseMultiFiles(string Folder, string Wildcards)
    {
      IEnumerator<string> enumerator;
      try
      {
        enumerator = MyProject.Computer.FileSystem.GetFiles(Folder, Microsoft.VisualBasic.FileIO.SearchOption.SearchTopLevelOnly, Wildcards).GetEnumerator();
        while (enumerator.MoveNext())
          this.EraseFile(enumerator.Current);
      }
      finally
      {
        if (enumerator != null)
          enumerator.Dispose();
      }
      bool flag;
      return flag;
    }

    public bool EraseFile(string Filename)
    {
      bool flag;
      try
      {
        if (Filename.Contains("*") | Filename.Contains("?"))
          this.EraseMultiFiles(this.PathOnly(Filename), this.FileOnly(Filename));
        if (Operators.CompareString(Filename, "", false) != 0 && this.FileExists(Filename))
        {
          this.MakeReadWrite(Filename);
          MyProject.Computer.FileSystem.DeleteFile(Filename);
        }
        flag = true;
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        this.cLog.AddAction((Exception) ex, MethodBase.GetCurrentMethod());
        flag = true;
        ProjectData.ClearProjectError();
      }
      catch (AccessViolationException ex1)
      {
        ProjectData.SetProjectError((Exception) ex1);
        try
        {
          File.SetAttributes(Filename, FileAttributes.Normal);
          File.Delete(Filename);
          flag = true;
        }
        catch (Exception ex2)
        {
          ProjectData.SetProjectError(ex2);
          Exception Ex = ex2;
          this.cLog.AddAction("Unable to delete " + Filename, "", "");
          this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
          flag = false;
          ProjectData.ClearProjectError();
        }
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        this.cLog.AddAction(Ex.Message, MethodBase.GetCurrentMethod());
        this.cLog.AddAction("Unable to delete " + Filename, "", "");
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool EraseFolder(string Foldername, bool Recursive = false)
    {
      bool flag;
      try
      {
        if (Recursive)
        {
          MyProject.Computer.FileSystem.DeleteDirectory(Foldername, DeleteDirectoryOption.DeleteAllContents);
          flag = true;
        }
        else
        {
          MyProject.Computer.FileSystem.DeleteDirectory(Foldername, DeleteDirectoryOption.ThrowIfDirectoryNonEmpty);
          flag = true;
        }
      }
      catch (DirectoryNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        if (Recursive)
          this.cLog.AddAction("Unable to recursive delete folder " + Foldername, "", "");
        else
          this.cLog.AddAction("Unable to delete folder " + Foldername, "", "");
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        flag = false;
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public void RunFile(string Filename)
    {
      try
      {
        Process process = new Process();
        ProcessStartInfo startInfo = process.StartInfo;
        startInfo.WorkingDirectory = "";
        startInfo.FileName = Filename;
        startInfo.UseShellExecute = true;
        startInfo.Verb = "open";
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.DoWork += new DoWorkEventHandler(this.BGRun_DoWork);
        backgroundWorker.RunWorkerAsync((object) process);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
    }

    public void RunFile(string Filename, string WorkingDirectory)
    {
      try
      {
        Process process = new Process();
        ProcessStartInfo startInfo = process.StartInfo;
        startInfo.WorkingDirectory = WorkingDirectory;
        startInfo.FileName = Filename;
        startInfo.UseShellExecute = true;
        startInfo.Verb = "open";
        if (Operators.CompareString(WorkingDirectory, (string) null, false) == 0)
          ;
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.DoWork += new DoWorkEventHandler(this.BGRun_DoWork);
        backgroundWorker.RunWorkerAsync((object) process);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
    }

    public void RunFile(string Filename, string WorkingDirectoryOrBlank, string Arguments)
    {
      try
      {
        Process process = new Process();
        ProcessStartInfo startInfo = process.StartInfo;
        if (Operators.CompareString(WorkingDirectoryOrBlank, "", false) != 0)
          startInfo.WorkingDirectory = WorkingDirectoryOrBlank;
        startInfo.FileName = Filename;
        startInfo.Arguments = Arguments;
        startInfo.UseShellExecute = true;
        startInfo.Verb = "open";
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.DoWork += new DoWorkEventHandler(this.BGRun_DoWork);
        backgroundWorker.RunWorkerAsync((object) process);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
    }

    private void BGRun_DoWork(object sender, DoWorkEventArgs e)
    {
      Process process;
      try
      {
        process = (Process) e.Argument;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction("Could not start process in BGRun_DoWork", "", "");
        ProjectData.ClearProjectError();
        return;
      }
      try
      {
        process.Start();
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        this.cLog.AddAction("Cannot run " + process.StartInfo.FileName, "", "");
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
    }

    public void RunFileHidden(string Filename, string WorkingDirectory = null)
    {
      try
      {
        Process process = new Process();
        ProcessStartInfo startInfo = process.StartInfo;
        if (Operators.CompareString(WorkingDirectory, (string) null, false) != 0)
          startInfo.WorkingDirectory = WorkingDirectory;
        startInfo.FileName = Filename;
        startInfo.UseShellExecute = true;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.Verb = "open";
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.DoWork += new DoWorkEventHandler(this.BGRun_DoWork);
        backgroundWorker.RunWorkerAsync((object) process);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
    }

    public void RunFileWithWait(string Filename, string FilePath = null)
    {
      string Path = Microsoft.VisualBasic.FileSystem.CurDir();
      string pathRoot1 = Path.GetPathRoot(Microsoft.VisualBasic.FileSystem.CurDir());
      try
      {
        if (FilePath != null)
        {
          string pathRoot2 = Path.GetPathRoot(FilePath);
          Microsoft.VisualBasic.FileSystem.ChDir(FilePath);
          Microsoft.VisualBasic.FileSystem.ChDrive(pathRoot2);
        }
        if (Filename.Contains("\""))
          Interaction.Shell(Filename, AppWinStyle.NormalFocus, true, -1);
        else
          Interaction.Shell(this.cText.AddQuotes(Filename), AppWinStyle.NormalFocus, true, -1);
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      Microsoft.VisualBasic.FileSystem.ChDir(Path);
      Microsoft.VisualBasic.FileSystem.ChDrive(pathRoot1);
    }

    public void RunFileWithWait(string Filename, string FilePath, string Parameters)
    {
      string Path = Microsoft.VisualBasic.FileSystem.CurDir();
      string pathRoot1 = Path.GetPathRoot(Microsoft.VisualBasic.FileSystem.CurDir());
      try
      {
        if (FilePath != null)
        {
          string pathRoot2 = Path.GetPathRoot(FilePath);
          Microsoft.VisualBasic.FileSystem.ChDir(FilePath);
          Microsoft.VisualBasic.FileSystem.ChDrive(pathRoot2);
        }
        if (Filename.Contains("\""))
          Interaction.Shell(Filename + " " + Parameters, AppWinStyle.NormalFocus, true, -1);
        else
          Interaction.Shell(this.cText.AddQuotes(Filename) + " " + Parameters, AppWinStyle.NormalFocus, true, -1);
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      Microsoft.VisualBasic.FileSystem.ChDir(Path);
      Microsoft.VisualBasic.FileSystem.ChDrive(pathRoot1);
    }

    public string AddForwardSlashIfMissing(string URL)
    {
      if (Operators.CompareString(URL, "", false) != 0 && !URL.EndsWith("/"))
        URL += "/";
      return URL;
    }

    public string AddSlashIfMissing(string PathToCheck)
    {
      if (Operators.CompareString(PathToCheck.Trim(), "", false) != 0 && !PathToCheck.EndsWith(this.SepChar))
        PathToCheck += this.SepChar;
      return PathToCheck;
    }

    public string ExtractUNCShare(string StringToCheck)
    {
      int num = Strings.InStr(Strings.InStr(3, StringToCheck, this.SepChar, CompareMethod.Binary) + 1, StringToCheck, this.SepChar, CompareMethod.Binary);
      return num != 0 ? Strings.Left(StringToCheck, num - 1) : StringToCheck;
    }

    public string ExtractUNCPath(string StringToCheck)
    {
      int Start = Strings.InStr(Strings.InStr(3, StringToCheck, this.SepChar, CompareMethod.Binary) + 1, StringToCheck, this.SepChar, CompareMethod.Binary);
      return Start != 0 ? Strings.Mid(StringToCheck, Start) : "";
    }

    public string LastFolder(string StringToCheck)
    {
      int num = Strings.InStrRev(StringToCheck, this.SepChar, Strings.Len(StringToCheck) - 1, CompareMethod.Binary);
      return Strings.Replace(num != 0 ? Strings.Mid(StringToCheck, num + 1) : StringToCheck, this.SepChar, "", 1, -1, CompareMethod.Binary);
    }

    public string FirstFolder(string StringToCheck)
    {
      int num = Strings.InStr(4, StringToCheck, this.SepChar, CompareMethod.Binary);
      return num != 0 ? Strings.Left(StringToCheck, num - 1) : StringToCheck;
    }

    public string RemoveFirstFolder(string Folder)
    {
      string Expression = this.FirstFolder(Folder);
      return Strings.Len(Expression) <= 0 ? Folder : Strings.Mid(Folder, Strings.Len(Expression) + 1);
    }

    public string RemoveLastFolder(string Folder)
    {
      string str;
      if (Folder.Length <= 3)
      {
        str = Folder;
      }
      else
      {
        string Expression = this.LastFolder(Folder);
        str = Strings.Len(Expression) <= 0 ? Folder : Strings.Left(Folder, Strings.Len(Folder) - Strings.Len(Expression) - 1);
      }
      return str;
    }

    public string RemoveSlashFromEnd(string Folder)
    {
      return Operators.CompareString(Folder, "", false) != 0 ? (!Folder.EndsWith(this.SepChar) ? Folder : Strings.Left(Folder, Strings.Len(Folder) - 1)) : "";
    }

    public bool SetFileDateTime(string Filename, DateTime TheDate)
    {
      bool flag;
      try
      {
        File.SetLastWriteTime(Filename, TheDate);
        flag = true;
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        flag = false;
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        flag = false;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public bool CopyFolder(string SourceFolder, string DestFolder)
    {
      DateTime AfterDate = DateAndTime.DateSerial(0, 0, 0);
      string ExtensionList = "";
      return this.CopyFolder(SourceFolder, DestFolder, ExtensionList, AfterDate);
    }

    public bool CopyFolder(string SourceFolder, string DestFolder, string ExtensionList)
    {
      DateTime AfterDate = DateAndTime.DateSerial(0, 0, 0);
      return this.CopyFolder(SourceFolder, DestFolder, ExtensionList, AfterDate);
    }

    public bool CopyFolder(string SourceFolder, string DestFolder, string ExtensionList, DateTime AfterDate)
    {
      bool flag = false;
      if (Operators.CompareString(ExtensionList, "*.*", false) == 0 & DateTime.Compare(AfterDate, DateAndTime.DateSerial(0, 0, 0)) == 0)
        ExtensionList = "";
      try
      {
        if (Operators.CompareString(ExtensionList, "", false) == 0 & DateTime.Compare(AfterDate, DateAndTime.DateSerial(0, 0, 0)) == 0)
        {
          MyProject.Computer.FileSystem.CopyDirectory(SourceFolder, DestFolder, true);
        }
        else
        {
          Collection AllFiles = new Collection();
          this.FillFilesRecursed(SourceFolder, ExtensionList, ref AllFiles);
          try
          {
            foreach (object obj in AllFiles)
            {
              string path2 = Conversions.ToString(obj);
              string Foldername = this.PathOnly(Path.Combine(DestFolder, path2));
              if (!this.FolderExists(Foldername))
                this.MkMissingDir(Foldername);
              File.Copy(Path.Combine(SourceFolder, path2), Path.Combine(DestFolder, path2), true);
            }
          }
          finally
          {
            IEnumerator enumerator;
            if (enumerator is IDisposable)
              (enumerator as IDisposable).Dispose();
          }
        }
        flag = true;
      }
      catch (FileNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        ProjectData.ClearProjectError();
      }
      catch (DirectoryNotFoundException ex)
      {
        ProjectData.SetProjectError((Exception) ex);
        ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public long DiskFree(string DriveletterOrPath)
    {
      long num;
      try
      {
        num = (long) Math.Round((double) MyProject.Computer.FileSystem.GetDriveInfo(this.DriveOnly(DriveletterOrPath)).AvailableFreeSpace / 1000.0);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        num = -1L;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return num;
    }

    public long DiskSize(string DriveletterOrPath)
    {
      long num;
      try
      {
        num = (long) Math.Round((double) MyProject.Computer.FileSystem.GetDriveInfo(this.DriveOnly(DriveletterOrPath)).TotalSize / 1000.0);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        num = -1L;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return num;
    }

    public string DriveLabel(string DriveLetter)
    {
      DriveLetter = this.DriveOnly(DriveLetter);
      string str = "Drive " + DriveLetter;
      try
      {
        str = MyProject.Computer.FileSystem.GetDriveInfo(DriveLetter).VolumeLabel;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public DriveType DriveType(string DriveletterOrPath)
    {
      DriveType driveType;
      try
      {
        string drive = this.DriveOnly(DriveletterOrPath);
        driveType = drive.Length != 1 ? (!drive.StartsWith("\\\\") ? (!drive.StartsWith("/") ? DriveType.Unknown : DriveType.Fixed) : DriveType.Network) : MyProject.Computer.FileSystem.GetDriveInfo(drive).DriveType;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception Ex = ex;
        driveType = DriveType.Unknown;
        this.cLog.AddAction(Ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return driveType;
    }

    public string DriveTypeString(string DriveletterOrPath)
    {
      string str = "";
      try
      {
        switch (MyProject.Computer.FileSystem.GetDriveInfo(this.DriveOnly(DriveletterOrPath)).DriveType)
        {
          case DriveType.Unknown:
            str = "Unknown";
            break;
          case DriveType.NoRootDirectory:
            str = "NoRootDirectory";
            break;
          case DriveType.Removable:
            str = "Removable";
            break;
          case DriveType.Fixed:
            str = "Fixed";
            break;
          case DriveType.Network:
            str = "Network";
            break;
          case DriveType.CDRom:
            str = "CDRom";
            break;
          case DriveType.Ram:
            str = "Ram";
            break;
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return str;
    }

    public void MoveIfMissingAndDelete(string SourceFolder, string FileOnly, string Destfolder)
    {
label_0:
      int num1;
      int num2;
      try
      {
        ProjectData.ClearProjectError();
        num1 = 2;
label_1:
        int num3 = 2;
        if (Operators.CompareString(Strings.LCase(this.AddSlashIfMissing(SourceFolder)), Strings.LCase(this.AddSlashIfMissing(Destfolder)), false) != 0)
          goto label_3;
label_2:
        num3 = 3;
        this.cLog.AddAction("Source and dest folders the same in MoveIfMissingAndDelete", "", "");
        goto label_20;
label_3:
        num3 = 6;
        string str1 = Path.Combine(SourceFolder, FileOnly);
label_4:
        num3 = 7;
        string str2 = Path.Combine(Destfolder, FileOnly);
label_5:
        num3 = 8;
        this.MkMissingDir(Destfolder);
label_6:
        num3 = 9;
        if (!this.FileExists(str2))
          goto label_8;
label_7:
        num3 = 10;
        this.EraseFile(str1);
        goto label_20;
label_8:
        num3 = 12;
label_9:
        num3 = 13;
        if (!this.FileExists(str1))
          goto label_20;
label_10:
        num3 = 14;
        this.MoveFile(str1, str2);
        goto label_20;
label_15:
        num2 = num3;
        switch (num1)
        {
          case 1:
label_14:
            int num4 = num2 + 1;
            num2 = 0;
            switch (num4)
            {
              case 1:
                goto label_0;
              case 2:
                goto label_1;
              case 3:
                goto label_2;
              case 4:
              case 11:
              case 15:
              case 16:
              case 17:
              case 20:
                goto label_20;
              case 5:
              case 6:
                goto label_3;
              case 7:
                goto label_4;
              case 8:
                goto label_5;
              case 9:
                goto label_6;
              case 10:
                goto label_7;
              case 12:
                goto label_8;
              case 13:
                goto label_9;
              case 14:
                goto label_10;
              case 18:
                goto label_11;
              case 19:
                break;
              default:
                goto label_19;
            }
          case 2:
label_11:
            num3 = 18;
            this.cLog.AddAction(Information.Err(), MethodBase.GetCurrentMethod());
            break;
          default:
            goto label_19;
        }
        num3 = 19;
        ProjectData.ClearProjectError();
        if (num2 == 0)
          throw ProjectData.CreateProjectError(-2146828268);
        goto label_14;
      }
      catch (Exception ex) when (ex is Exception & (uint) num1 > 0U & num2 == 0)
      {
        ProjectData.SetProjectError(ex);
        goto label_15;
      }
label_19:
      throw ProjectData.CreateProjectError(-2146828237);
label_20:
      if (num2 == 0)
        return;
      ProjectData.ClearProjectError();
    }

    public string PathCombine(string Part1, string Part2)
    {
      if (Part2.StartsWith(this.SepChar))
        Part2 = Part2.Substring(1);
      return Path.Combine(Part1, Part2);
    }

    public void RemoveFilesMatchingExt(ref Collection FilesCol, string ExtensionsToZap)
    {
      int count = FilesCol.Count;
      while (count >= 1)
      {
        if (ExtensionsToZap.Contains(this.ExtOnly(Conversions.ToString(FilesCol[count])).ToLower()))
          FilesCol.Remove(count);
        count += -1;
      }
    }

    public void RemoveFilesEndingIn(ref Collection FilesCol, string EndingIn)
    {
      foreach (string Left in Strings.Split(EndingIn.ToLower(), " ", -1, CompareMethod.Binary))
      {
        if (Operators.CompareString(Left, "", false) != 0)
        {
          int count = FilesCol.Count;
          while (count >= 1)
          {
            if (Conversions.ToString(FilesCol[count]).ToLower().EndsWith(Left))
              FilesCol.Remove(count);
            count += -1;
          }
        }
      }
    }

    public string ConvertSlashes(string FileWithPath)
    {
      string Find = Operators.CompareString(this.SepChar, "/", false) != 0 ? "/" : "\\";
      return Strings.Replace(FileWithPath, Find, this.SepChar, 1, -1, CompareMethod.Binary);
    }

    public bool ExportListviewToCSV(string CSVFile, ListView Listview1, bool OpenContainingFolder)
    {
      bool flag = false;
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        try
        {
          foreach (ColumnHeader column in Listview1.Columns)
            stringBuilder.Append(this.cText.AddDoubleQuotes(column.Text) + ",");
        }
        finally
        {
          IEnumerator enumerator;
          if (enumerator is IDisposable)
            (enumerator as IDisposable).Dispose();
        }
        stringBuilder.AppendLine("N/A");
        try
        {
          foreach (ListViewItem listViewItem in Listview1.Items)
          {
            int num1 = 0;
            int num2 = listViewItem.SubItems.Count - 1;
            for (int index = num1; index <= num2; ++index)
              stringBuilder.Append(this.cText.AddDoubleQuotes(listViewItem.SubItems[index].Text) + ",");
            stringBuilder.AppendLine("-");
          }
        }
        finally
        {
          IEnumerator enumerator;
          if (enumerator is IDisposable)
            (enumerator as IDisposable).Dispose();
        }
        this.WriteTextFile(CSVFile, stringBuilder.ToString(), false);
        flag = true;
        if (OpenContainingFolder)
          this.RunFile(this.PathOnly(CSVFile));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        this.cLog.AddAction(ex, MethodBase.GetCurrentMethod());
        ProjectData.ClearProjectError();
      }
      return flag;
    }

    public delegate void ProgressEventHandler(string ProgressString, long Count, long Total);

    public delegate void WarningEventHandler(string Message);

    public delegate void CountEventHandler(long ItemCount);

    public delegate void FillingFolderEventHandler(string FolderName);
  }
}
