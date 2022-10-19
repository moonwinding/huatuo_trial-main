using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public static class FileHelper
{
    public static void CopyDirectory(string srcDir, string tgtDir)
    {
        DirectoryInfo source = new DirectoryInfo(srcDir);
        DirectoryInfo target = new DirectoryInfo(tgtDir);
        if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new Exception("父目录不能拷贝到子目录！");
        }
        if (!source.Exists)
        {
            return;
        }
        if (!target.Exists)
        {
            target.Create();
        }
        FileInfo[] files = source.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
        }
        DirectoryInfo[] dirs = source.GetDirectories();
        for (int j = 0; j < dirs.Length; j++)
        {
            CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
        }
    }
    public static void CopyDirectoryAndSuffix(string srcDir, string tgtDir, string pSuffix, string pNewSuffix = null, string pNeedSpecPath = null,
           string pDontNeedSpecPath = null, string pDontNeedSpecPathEx = null)
    {
        DirectoryInfo source = new DirectoryInfo(srcDir);
        DirectoryInfo target = new DirectoryInfo(tgtDir);
        if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase)) { throw new Exception("父目录不能拷贝到子目录！"); }
        if (!source.Exists) { return; }

        FileInfo[] files = source.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            var fixFullName = FormatFilePath(files[i].FullName);
            if (pNeedSpecPath != null && !fixFullName.Contains(pNeedSpecPath))
            {
                continue;
            }
            if (pDontNeedSpecPath != null && fixFullName.Contains(pDontNeedSpecPath))
            {
                continue;
            }
            if (pDontNeedSpecPathEx != null && fixFullName.Contains(pDontNeedSpecPathEx))
            {
                continue;
            }
            if (files[i].FullName.EndsWith(pSuffix))
            {
                if (!target.Exists) { target.Create(); }
                string newFilePath = FormatFilePath(target.FullName + @"\" + files[i].Name);
                if (pNewSuffix != null)
                {
                    if (!newFilePath.EndsWith(pNewSuffix))
                    {
                        int lastIndex = newFilePath.LastIndexOf('.');
                        newFilePath = newFilePath.Substring(0, lastIndex);
                        newFilePath += pNewSuffix;
                    }
                }
                File.Copy(files[i].FullName, newFilePath, true);
            }
        }

        DirectoryInfo[] dirs = source.GetDirectories();

        for (int j = 0; j < dirs.Length; j++)
        {
            CopyDirectoryAndSuffix(dirs[j].FullName, FormatFilePath(target.FullName + @"\" + dirs[j].Name), pSuffix, pNewSuffix, pNeedSpecPath, pDontNeedSpecPath, pDontNeedSpecPathEx);
        }
    }

    /// <summary>
    /// 删除并创建新的目录
    /// </summary>
    /// <param name="dir"></param>
    public static void DeleteCreateNewDirectory(string dir)
    {
        if (Directory.Exists(dir))
            Directory.Delete(dir, true);
        Directory.CreateDirectory(dir);
    }
    public static string FormatFilePath(string filePath) {
        var path = filePath.Replace('\\','/');
        path = path.Replace("//","/");
        return path;
    }
    public static string ToFormatPath(this string filePath){ filePath = FormatFilePath(filePath);return filePath;}
    public static void DeleteDirectory(string target_dir)
    {
        try
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    public static void CreatDirectoryByFile(string filePath) {
        CreatDirectory(Path.GetDirectoryName(filePath));
    }
    public static void CreatDirectory(string dir) {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }
    public static void FileWriteAllBytes(string filePath, byte[] bytes) {
        CreatDirectoryByFile(filePath);
        File.WriteAllBytes(filePath, bytes);
    }
    public static void FileWriteAllTexts(string filePath,string text) {
        CreatDirectoryByFile(filePath);
        File.WriteAllText(filePath, text);
    }
    public static string FileReadAllText(string filePath) {
        if (File.Exists(filePath))
            return File.ReadAllText(filePath);
        return null;
    }
    public static byte[] FileReadAllBytes(string filePath) {
        if (File.Exists(filePath))
            return File.ReadAllBytes(filePath);
        return null;
    }
    public static string CombinePath(string path1,string path2) {
        var path = Path.Combine(path1, path2);
        return ToFormatPath(path);
    }
    public static bool FileExist(string filePath) {
        return File.Exists(filePath);
    }
}
