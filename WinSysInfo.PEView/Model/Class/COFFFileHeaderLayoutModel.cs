namespace WinSysInfo.PEView.Model
{
    public class COFFFileHeaderLayoutModel : LayoutModel<COFFFileHeader>
    {
        public COFFFileHeaderLayoutModel(LayoutModel<COFFFileHeader> baseObj)
            :base(baseObj)
        {
        }

        public bool IsImportLibrary()
        {
            return this.actualData.NumberOfSections == 0XFFFF;
        }

        public uint GetNumberOfSections()
        {
            return this.IsImportLibrary() ? 0 : (uint)this.actualData.NumberOfSections;
        }

        public uint GetPointerToSymbolTable()
        {
            return this.IsImportLibrary() ? 0 : (uint)this.actualData.PointerToSymbolTable;
        }

        public uint GetNumberOfSymbols()
        {
            return this.IsImportLibrary() ? 0 : (uint)this.actualData.NumberOfSymbols;
        }
    }
}
