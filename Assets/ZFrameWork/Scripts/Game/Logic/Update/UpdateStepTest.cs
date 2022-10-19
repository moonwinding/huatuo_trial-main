using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFrameWork
{
    /// <summary>
    /// 测试步骤直接测试下载
    /// </summary>
    public class UpdateStepTest : StepAction
    {
        private string downLoadResPath = "common";
        public override int StepId { get { return 4; } }
        public override System.Action<System.Action<StepResult, int>> GetExcureAction()
        {
            return (oFinish) =>
            {
                TestDownLoad(() => {
                    oFinish?.Invoke(StepResult.Next, 0);
                }, () => {
                    oFinish?.Invoke(StepResult.Stop, 0);
                });
                
            };
        }
        private void TestDownLoad(System.Action onFinish,System.Action onFail)
        {

            ResServer.ResServerTools.DownLoadOneResByRelatedPath(downLoadResPath, (oBytes) => {
                if (oBytes != null && oBytes.Length > 0)
                {
                    var resPath = FileHelper.CombinePath(FileUtility.FilePlatform.GetPersistencePath(), downLoadResPath);
                    FileHelper.FileWriteAllBytes(resPath, oBytes);
                    onFinish?.Invoke();
                }
                else
                {
                    onFail?.Invoke();
                }
            });
        }
    }
}
