using System.Collections.Generic;

namespace PhoneCoreLibrary
{
    /// <summary>
    /// 数据转换适配器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAdapter<T> where T : class
    {
        T Adapt(string xmlOrJsonStr);

        IList<T> AdaptToList(string xmlOrJsonStr);

        string AdaptBack(T model);

        string AdaptBack(IList<T> list);
    }
}
