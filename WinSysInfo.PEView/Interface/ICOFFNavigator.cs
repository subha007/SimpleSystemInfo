using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Interface
{
    public interface ICOFFNavigator
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
        LayoutModel<TLayoutModel> GetData<TLayoutModel>(EnumReaderLayoutType enumVal)
            where TLayoutModel : struct;

        /// <summary>
        /// Get the data
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        LayoutModel<TLayoutModel> GetData<TLayoutModel>(int index)
            where TLayoutModel : struct;

        /// <summary>
        /// Set the data
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        void SetData<TLayoutModel>(EnumReaderLayoutType enumVal, LayoutModel<TLayoutModel> model,
            int position = -1)
            where TLayoutModel : struct;

        /// <summary>
        /// Set the data list
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        void SetData<TLayoutModel>(EnumReaderLayoutType enumVal, List<LayoutModel<TLayoutModel>> modelList,
            int position = -1)
            where TLayoutModel : struct;
    }
}
