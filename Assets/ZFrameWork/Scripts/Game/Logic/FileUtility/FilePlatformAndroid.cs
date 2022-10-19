using UnityEngine;


namespace ZFrameWork
{
    public class FilePlatformAndroid: FilePlatform
    {
        //持久化路径
        public override string GetPersistencePath()
        {
            return FileHelper.CombinePath(Application.persistentDataPath, GameDefine.BuildRoot);
        }
        //获取StreamingAsset
        public override byte[] LoadBytesAtStreamingAsstetsFolder(string relatedPath)
        {
            return BetterStreamingAssets.ReadAllBytes(relatedPath);
        }
        //从持久目录获取，没有从StreamingAssets 目录获取
        public override byte[] LoadBytesSafeAtPersistencePath(string relatedPath)
        {
            var persistencePath = GetPersistencePath();
            var persistenceFilePath = FileHelper.CombinePath(persistencePath, relatedPath);
            if (FileHelper.FileExist(persistenceFilePath))
                return FileHelper.FileReadAllBytes(persistenceFilePath);
            else
                return LoadBytesAtStreamingAsstetsFolder(relatedPath);
        }
    }
}
