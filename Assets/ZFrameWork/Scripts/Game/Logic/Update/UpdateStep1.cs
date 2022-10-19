using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFrameWork
{
    /// <summary>
    /// 获取资源服务器
    /// </summary>
    public class UpdateStep1: StepAction
    {
        public override int StepId { get { return 1; } }
        public override System.Action<System.Action<StepResult, int>> GetExcureAction()
        {
            return (oFinish) => {
                ResServer.ResServerTools.GerServerInfo(() => {
                    oFinish.Invoke(StepResult.Next, 0);
                }, () => {
                    oFinish.Invoke(StepResult.Stop, 0);
                });
            };
        }
    }
}
