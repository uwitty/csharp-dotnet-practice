using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marshal = System.Runtime.InteropServices.Marshal;

namespace OfficeOp
{
    public class ExcelOp : IDisposable
    {
        dynamic excel_ = null;
        dynamic books_ = null;
        dynamic book_ = null;
        dynamic sheet_ = null;
        dynamic cells_ = null;

        public ExcelOp(string filename = null, string sheetname = null)
        {
            try
            {
                Type t = Type.GetTypeFromProgID("excel.application");
                excel_ = Activator.CreateInstance(t);
                excel_.Visible = true;

                books_ = excel_.Workbooks;
                if (filename == null)
                {
                    book_ = books_.Add();
                    sheet_ = book_.ActiveSheet();
                    if (sheetname != null)
                    {
                        sheet_.Name = sheetname;
                    }
                }
                else
                {
                    book_ = books_.Open(filename);
                    if (sheetname == null)
                    {
                        sheet_ = book_.ActiveSheet();
                    }
                    else
                    {
                        ActivateSheet(sheetname);
                    }
                }

                cells_ = sheet_.Cells;
            }
            catch (Exception)
            {
                ReleaseObjects();
                throw;
            }
        }

        ~ExcelOp() {
            Dispose(false);
        }

        public void ActivateSheet(string name)
        {
            if (sheet_ != null)
            {
                Marshal.ReleaseComObject(sheet_);
            }
            dynamic sheets = null;
            try
            {
                sheets = book_.WorkSheets;
                sheet_ = sheets.Item[name];
                sheet_.Activate();
            }
            finally
            {
                Marshal.ReleaseComObject(sheets);
            }
        }

        public string this[string addr]
        {
            get
            {
                dynamic cell = null;
                try
                {
                    cell = cells_.Range(addr);
                    return cell.Value;
                }
                finally
                {
                    if (cell != null)
                    {
                        Marshal.ReleaseComObject(cell);
                    }
                }
            }
            set
            {
                dynamic cell = null;
                try
                {
                    cell = cells_.Range(addr);
                    cell.Value = value;
                }
                finally
                {
                    if (cell != null)
                    {
                        Marshal.ReleaseComObject(cell);
                    }
                }
            }
        }

        private void ReleaseObjects()
        {
            if (cells_ != null)
            {
                Marshal.ReleaseComObject(cells_);
                cells_ = null;
            }
            if (sheet_ != null)
            {
                Marshal.ReleaseComObject(sheet_);
                sheet_ = null;
            }
            if (book_ != null)
            {
                Marshal.ReleaseComObject(book_);
                book_ = null;
            }
            if (books_ != null)
            {
                Marshal.ReleaseComObject(books_);
                books_ = null;
            }
            if (excel_ != null)
            {
                Marshal.ReleaseComObject(excel_);
                excel_ = null;
            }
        }

        private static void SetExcelValue(dynamic cells, int row, int col, string value)
        {
            dynamic cell = null;

            try
            {
                cell = cells[row, col];
                cell.Value = value;
            }
            finally
            {
                if (cell != null)
                {
                    Marshal.ReleaseComObject(cell);
                }
            }

        }

        private static void SetExcelValue(dynamic cells, string range, string value)
        {
            dynamic cell = null;

            try
            {
                cell = cells.Range(range);
                cell.Value = value;
            }
            finally
            {
                if (cell != null)
                {
                    Marshal.ReleaseComObject(cell);
                }
            }

        }

        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)。
                }

                // Release unmanaged objects
                ReleaseObjects();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
