using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFrameWork
{
    /// <summary>
    /// 下载线上的热更新资源 对比出差异文件并且更新
    /// </summary>
    public class UpdateStep3:StepAction
    {
        public override int StepId { get { return 3; } }
        public override System.Action<System.Action<StepResult, int>> GetExcureAction()
        {
            return (oFinish) => {
                
            };
        }
    }
}
