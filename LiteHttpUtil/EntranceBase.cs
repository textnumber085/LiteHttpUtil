using System;

namespace LiteHttpUtil
{
    /// <summary>
    /// 通道入口的基类
    /// </summary>
    public class EntranceBase
    {
        #region private properties

        /// <summary>
        /// Id
        /// </summary>
        private string _id = "";

        /// <summary>
        /// 通道入口是否开启
        /// </summary>
        private bool _isOpen = true;

        /// <summary>
        /// 上次入口关闭时间
        /// </summary>
        private DateTime _lastCloseDtm = DateTime.MinValue;

        /// <summary>
        /// 最大关闭持续时间
        /// </summary>
        private TimeSpan _closeDuration = TimeSpan.MinValue;

        #endregion

        /// <summary>
        /// Id
        /// </summary>
        public string ID
        {
            get { return _id; }
        }
        
        /// <summary>
        /// 上次入口关闭时间
        /// </summary>
        public DateTime LastCloseDtm
        {
            get { return _lastCloseDtm; }
        }
        
        /// <summary>
        /// 最大关闭持续时间
        /// </summary>
        public TimeSpan CloseDuration
        {
            get { return _closeDuration; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="closedLastTime"></param>
        public EntranceBase(string id, TimeSpan closedLastTime)
        {
            this._id = id;
            this._isOpen = true;
            this._lastCloseDtm = DateTime.MinValue;
            this._closeDuration = closedLastTime;
        }

        /// <summary>
        /// 判断入口是否打开（不做任何处理直接返回私有属性isOpen的状态）
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOpen()
        {
            return this._isOpen;
        }

        /// <summary>
        /// 开启入口
        /// </summary>
        public virtual void Open()
        {
            this._isOpen = true;
        }

        /// <summary>
        /// 关闭入口
        /// </summary>
        public virtual void Close()
        {
            this._isOpen = false;
            this._lastCloseDtm = DateTime.Now;
        }

        /// <summary>
        /// 重新开启入口 
        /// </summary>
        public virtual void ReOpen()
        {
            Open();
        }

        /// <summary>
        /// 重置所有参数到初始状态
        /// </summary>
        public virtual void ResetAll()
        {
            this._lastCloseDtm = DateTime.MinValue;
            this._isOpen = true;
        }

        /// <summary>
        /// 重置关闭持续时间
        /// </summary>
        /// <param name="lastTime"></param>
        public virtual void ResetClosedLastTime(TimeSpan lastTime)
        {
            this._closeDuration = lastTime;
        }
    }
}
