using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteHttpUtil
{
    /// <summary>
    /// HTTP请求入口
    /// </summary>
    public class HttpEntrance : EntranceBase
    {
        #region properties & constructors

        /// <summary>
        /// 功能开关(如果为false, 入口将敞开不作任何控制)
        /// </summary>
        private bool _switch = true;

        private int _closeTriggerCount = 4;
        /// <summary>
        /// 触发通道关闭的最大次数
        /// </summary>
        public int CloseTriggerCount
        {
            get { return _closeTriggerCount; }
        }

        /// <summary>
        /// 请求是否成功记录
        /// </summary>
        public List<bool> requestRsList { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="closedLastTime"></param>
        public HttpEntrance(string id, TimeSpan closedLastTime, int closeTriggerCount) 
            : base(id, closedLastTime)
        {
            _closeTriggerCount = (closeTriggerCount > 0) ? closeTriggerCount : _closeTriggerCount;
            requestRsList = new List<bool>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="closedLastMinutes"></param>
        public HttpEntrance(string id, int closedLastMinutes, int closeTriggerCount)
            : base(id, TimeSpan.FromMinutes(closedLastMinutes))
        {
            _closeTriggerCount = (closeTriggerCount > 0) ? closeTriggerCount : _closeTriggerCount;
            requestRsList = new List<bool>();
        }

        #endregion

        /// <summary>
        /// 查看httpEntrance是否做控制
        /// </summary>
        /// <returns></returns>
        public bool Switch()
        {
            return _switch;
        }

        /// <summary>
        /// 开启或关闭httpEntrance控制
        /// </summary>
        /// <param name="swch">true：如果本来就控制不做任何操作, 从不控制转为控制相当于初始化一次  false：不做控制，任何询问都将返回成功，任何操作请求都无效</param>
        public void Switch(bool swch)
        {
            if (swch)
            {
                if (_switch) //本来就开启的
                {
                    return; //不做任何操作
                }
                else //从关闭转为开启, 相当于初始化
                {
                    _switch = true;
                    ResetAll();
                }
            }
            else
            {
                _switch = false;
            }
        }

        /// <summary>
        /// 重新开启入口
        /// </summary>
        public override void ReOpen()
        {
            base.ReOpen();
            requestRsList = new List<bool>();
        }

        /// <summary>
        /// 重置所有参数到初始状态
        /// </summary>
        public override void ResetAll()
        {
            if (!_switch)
                return;

            _closeTriggerCount = 3;
            requestRsList = new List<bool>();
            base.ResetAll();
        }

        /// <summary>
        /// 判断HTTP请求入口是否开启
        /// </summary>
        /// <returns></returns>
        public override bool IsOpen()
        {
            if (!_switch)
                return true;

            if (base.IsOpen()) //如果开启在，就直接返回开启
            {
                return true;
            }
            else //如果处于关闭状态
            {
                if (DateTime.Now - this.LastCloseDtm >= this.CloseDuration) //判断关闭时间是否已达到上限
                {
                    ReOpen(); //达到了则重新开启
                }
                return base.IsOpen();
            }
        }

        /// <summary>
        /// 判断HTTP请求入口是否开启
        /// </summary>
        /// <returns></returns>
        public bool IsOpen(Action ifClosedAction)
        {
            if (!_switch)
                return true;

            var result = IsOpen();
            if (result == false && ifClosedAction != null) //执行关闭状态下要做的action
            {
                ifClosedAction();
            }
            return result;
        }

        /// <summary>
        /// 添加一次请求结果是否成功的记录
        /// </summary>
        /// <param name="val">请求成功与否</param>
        /// <param name="isTryClose">添加结果后是否判断是否达到关闭入口的标准，如果达到将立即关闭</param>
        public void AddResult(bool val, bool isTryClose = true)
        {
            if (!_switch)
                return;

            this.requestRsList.Add(val);
            if (this.requestRsList.Count > CloseTriggerCount)
            {
                try
                {
                    requestRsList.RemoveRange(0, this.requestRsList.Count - CloseTriggerCount);
                }
                catch { }
            }
            if(isTryClose)
            {
                if(GetReqFailedRate() == 1)
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// 获取失败请求结果占比(少于maxHttpFailCount次记录不统计直接返回0)
        /// </summary>
        /// <returns></returns>
        public decimal GetReqFailedRate()
        {
            if (this.requestRsList.Count < CloseTriggerCount || _switch == false)
                return 0;

            return Decimal.Divide(this.requestRsList.Where(aa => aa == false).Count(), this.requestRsList.Count);
        }

        /// <summary>
        /// 修改触发通道关闭的最大次数
        /// </summary>
        /// <param name="newVal">允许为0, 0或负数则不控制</param>
        public void ChangeCloseTriggerCount(int newVal)
        {
            _closeTriggerCount = newVal;
        }

        /// <summary>
        /// 获取“距通道重新开启还有多久”时长
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetClosedRemaining()
        {
            if (IsOpen())
                return TimeSpan.FromSeconds(0);
            
            //(关闭时刻+关闭持续时间) - 当前时间
            var tspan = this.LastCloseDtm.Add(this.CloseDuration) - DateTime.Now;
            return tspan;
        }
    }
}
