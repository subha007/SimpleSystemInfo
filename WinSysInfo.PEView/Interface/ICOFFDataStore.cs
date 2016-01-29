using System.Collections.Generic;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Interface
{
    public interface ICOFFDataStore
    {
        /// <summary>
        /// Data mapping
        /// </summary>
        Dictionary<EnumReaderLayoutType, object> FileData { get; set; }

        /// <summary>
        /// Store the layout order
        /// </summary>
        List<EnumReaderLayoutType> LayoutOrder { get; set; }

        /// <summary>
        /// Get the data
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        TLayoutModel GetData<TLayoutModel>(EnumReaderLayoutType enumVal)
            where TLayoutModel : class;

        /// <summary>
        /// Get the data
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        TLayoutModel GetData<TLayoutModel>(int index)
            where TLayoutModel : class;

        /// <summary>
        /// Set the data
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="enumVal"></param>
        /// <param name="modelList"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        void SetData<TLayoutModel>(EnumReaderLayoutType enumVal, TLayoutModel modelList,
            int position = -1)
            where TLayoutModel : class;

        /// <summary>
        /// Delete the model
        /// </summary>
        /// <param name="enumVal"></param>
        void Delete(EnumReaderLayoutType enumVal);
    }
}
