using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFrameWork
{
    public abstract class FilePlatform
    {
        //持久化路径
        public abstract string GetPersistencePath();
        //获取StreamingAsset
        public abstract byte[] LoadBytesAtStreamingAsstetsFolder(string relatedPath);
        //从持久目录获取，没有从StreamingAssets 目录获取
        public abstract byte[] LoadBytesSafeAtPersistencePath(string relatedPath);

    }
}
