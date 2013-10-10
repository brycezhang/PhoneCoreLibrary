using System;
using GalaSoft.MvvmLight;

namespace Cdel.PhoneFramework
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class EntityBase<TKey> : ObservableObject
    {
        public EntityBase()
        {
        }

        public virtual TKey Id
        {
            get;
            set;
        }
    }
}
