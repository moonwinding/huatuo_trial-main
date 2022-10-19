using UnityEngine;

namespace ZFrameWork
{
    public class FilePlatformStandalone: FilePlatform
    {
        //持久化路径
        public override string GetPersistencePath() {

            return FileHelper.CombinePath(Application.streamingAssetsPath,GameDefine.BuildRoot);
        }
        //获取StreamingAsset
        public override byte[] LoadBytesAtStreamingAsstetsFolder(string relatedPath) {

            var path = FileHelper.CombinePath(Application.streamingAssetsPath, relatedPath);
            if (FileHelper.FileExist(path))
            {
                return FileHelper.FileReadAllBytes(path);
            }
            return null;
        }
        //从持久目录获取，没有从StreamingAssets 目录获取
        public override byte[] LoadBytesSafeAtPersistencePath(string relatedPath)
        {
            var filePath = FileHelper.CombinePath(GameDefine.BuildRoot, relatedPath);
            return LoadBytesAtStreamingAsstetsFolder(filePath);
        }
    }
}
