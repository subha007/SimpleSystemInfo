using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// The class to store file structure portion and navigate
    /// through
    /// </summary>
    public class COFFNavigator : ICOFFNavigator
    {
        /// <summary>
        /// Data mapping
        /// </summary>
        public Dictionary<EnumReaderLayoutType, object> FileData { get; set; }

        /// <summary>
        /// Store the layout order
        /// </summary>
        public List<EnumReaderLayoutType> LayoutOrder { get; set; }

        /// <summary>
        /// Get the data
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public LayoutModel<TLayoutModel> GetData<TLayoutModel>(EnumReaderLayoutType enumVal)
            where TLayoutModel : struct
        {
            LayoutModel<TLayoutModel> model = null;
            if(this.FileData.ContainsKey(enumVal) == true)
                model = this.FileData[enumVal] as LayoutModel<TLayoutModel>;

            return model;
        }

        /// <summary>
        /// Get the data
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public LayoutModel<TLayoutModel> GetData<TLayoutModel>(int index)
            where TLayoutModel : struct
        {
            if(!(index >= 0 && index < this.FileData.Count)) return null;

            EnumReaderLayoutType enumVal = this.LayoutOrder[index];

            LayoutModel<TLayoutModel> model = null;
            if(this.FileData.ContainsKey(enumVal) == true)
                model = this.FileData[enumVal] as LayoutModel<TLayoutModel>;

            return model;
        }

        /// <summary>
        /// Set the data
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public void SetData<TLayoutModel>(EnumReaderLayoutType enumVal, LayoutModel<TLayoutModel> model,
            int position = -1)
            where TLayoutModel : struct
        {
            if(this.FileData.ContainsKey(enumVal) == true)
                this.FileData[enumVal] = model;
            else
                this.FileData.Add(enumVal, model);

            int fIndex = this.LayoutOrder.FindIndex(e => e == enumVal);
            if(fIndex == position) return;
            if(fIndex >= 0)
                this.LayoutOrder.Insert(position, enumVal);
            else
                this.LayoutOrder.Add(enumVal);
        }

        /// <summary>
        /// Set the data list
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public void SetData<TLayoutModel>(EnumReaderLayoutType enumVal, List<LayoutModel<TLayoutModel>> modelList,
            int position = -1)
            where TLayoutModel : struct
        {
            if (this.FileData.ContainsKey(enumVal) == true)
                this.FileData[enumVal] = modelList;
            else
                this.FileData.Add(enumVal, modelList);

            int fIndex = this.LayoutOrder.FindIndex(e => e == enumVal);
            if (fIndex == position) return;
            if (fIndex >= 0)
                this.LayoutOrder.Insert(position, enumVal);
            else
                this.LayoutOrder.Add(enumVal);
        }

        /// <summary>
        /// Delete the model
        /// </summary>
        /// <param name="enumVal"></param>
        public void Delete(EnumReaderLayoutType enumVal)
        {
            this.LayoutOrder.Remove(enumVal);
            if (this.FileData.ContainsKey(enumVal) == true)
                this.FileData.Remove(enumVal);
        }
    }
}
