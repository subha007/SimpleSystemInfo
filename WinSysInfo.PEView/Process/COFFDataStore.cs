using System.Collections.Generic;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// The class to store file structure portion and navigate
    /// through
    /// </summary>
    public class COFFDataStore : ICOFFDataStore
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
        /// Base construction
        /// </summary>
        public COFFDataStore()
        {
            this.FileData = new Dictionary<EnumReaderLayoutType, object>();
            this.LayoutOrder = new List<EnumReaderLayoutType>();
        }

        /// <summary>
        /// Get the data
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public TLayoutModel GetData<TLayoutModel>(EnumReaderLayoutType enumVal)
            where TLayoutModel : class
        {
            TLayoutModel model = null;
            if(this.FileData.ContainsKey(enumVal) == true)
                model = this.FileData[enumVal] as TLayoutModel;

            return model;
        }

        /// <summary>
        /// Get the data
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public TLayoutModel GetData<TLayoutModel>(int index)
            where TLayoutModel : class
        {
            if(!(index >= 0 && index < this.FileData.Count)) return null;

            EnumReaderLayoutType enumVal = this.LayoutOrder[index];

            TLayoutModel model = null;
            if(this.FileData.ContainsKey(enumVal) == true)
                model = this.FileData[enumVal] as TLayoutModel;

            return model;
        }

        /// <summary>
        /// Set the data list
        /// </summary>
        /// <typeparam name="TLayoutModel"></typeparam>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public void SetData<TLayoutModel>(EnumReaderLayoutType enumVal, TLayoutModel modelList,
            int position = -1)
            where TLayoutModel : class
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
