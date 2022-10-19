using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFrameWork
{
    /// <summary>
    /// 本地大版本和线上大版本进行对比
    /// </summary>
    public class UpdateStep2: StepAction
    {
        private ResTotalVersion _localTotalVersion;//本地总版本号
        private ResTotalVersion _lineTotalVersion;//线上的总版本号
        public override int StepId { get { return 2; } }
        public override System.Action<System.Action<StepResult, int>> GetExcureAction()
        {
            return (oFinish) => {
                LoadLocalTotalVersion();
                if (_localTotalVersion == null)
                {
                    oFinish?.Invoke(StepResult.Stop, 0);
                    return;
                }
                LoadLineTotalVersion(() => {
                    if (_lineTotalVersion == null)
                        oFinish?.Invoke(StepResult.Stop, 0);
                    else if (_lineTotalVersion.VersionCode > _localTotalVersion.VersionCode)
                        oFinish?.Invoke(StepResult.Next, 0);
                    else
                        oFinish?.Invoke(StepResult.Jump, StepId + 2);
                });
            };
        }
        private void LoadLocalTotalVersion() {
            var localTotalVersionPath = FileHelper.CombinePath(FileUtility.FilePlatform.GetPersistencePath(), GameDefine.TotalVersion_Local);
            if (FileHelper.FileExist(localTotalVersionPath))
            {
                _localTotalVersion = new ResTotalVersion(localTotalVersionPath);
            }
            else {
                UnityEngine.Debug.LogError("local total version is not exsit : "+ localTotalVersionPath);
            }
        }
        private void LoadLineTotalVersion(System.Action onFinish) {

            ResServer.ResServerTools.DownLoadOneResByRelatedPath(GameDefine.TotalVersion, (bytes) =>
            {
                var lineTotalVersionPath = FileHelper.CombinePath(FileUtility.FilePlatform.GetPersistencePath(),GameDefine.TotalVersion_Line);
                FileHelper.FileWriteAllBytes(lineTotalVersionPath, bytes);
                _lineTotalVersion = new ResTotalVersion(lineTotalVersionPath);
                _lineTotalVersion.Load();
                onFinish?.Invoke();
            });
        }
    }
}
