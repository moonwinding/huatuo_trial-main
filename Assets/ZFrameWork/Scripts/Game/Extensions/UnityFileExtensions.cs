using System.IO;
public static class UnityFileExtensions
{
    public static void SaveTo(this string data,string path) { 
        
        if(string.IsNullOrEmpty(data) || string.IsNullOrEmpty(path)) return;
        path.CreateDirectoryIfNotExists();
        File.WriteAllText(path, data);
    }
    public static void CreateDirectoryIfNotExists(this string folder) {
        string path = Path.GetDirectoryName(folder);
        if (string.IsNullOrEmpty(path))
            return;
        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}