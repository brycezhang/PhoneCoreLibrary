using System;

namespace PhoneCoreLibrary.Utils
{
    /// <summary>
    /// 泛型ID类型转换(int/Guid)
    /// </summary>
    public class ConvertGenericId
    {
        /// <summary>
        /// 转换为int类型
        /// </summary>
        /// <typeparam name="TGeneric"></typeparam>
        /// <param name="tGeneric"></param>
        /// <returns>转换失败返回-1</returns>
        public static int ToInt<TGeneric>(TGeneric tGeneric)
        {
            try
            {
                var obj = Convert.ChangeType(tGeneric, typeof(int), null);
                return obj != null ? int.Parse(obj.ToString()) : -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        /// <summary>
        /// 转换为Guid
        /// </summary>
        /// <typeparam name="TGeneric"></typeparam>
        /// <param name="tGeneric"></param>
        /// <returns>转换失败返回Guid.Empty</returns>
        public static Guid ToGuid<TGeneric>(TGeneric tGeneric)
        {
            try
            {
                var obj = Convert.ChangeType(tGeneric, typeof(Guid), null);
                return obj != null ? Guid.Parse(obj.ToString()) : Guid.Empty;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }
    }
}
