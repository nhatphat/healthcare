using Aspose.Cells;
using Home.models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Home.Utils
{
    public class DBManager
    {
        private static string DATA_PATH = "/data/master_data.xlsx";
        private static DBManager instance;
        private static Workbook workbook;
        private DBManager() { }

        //singleton

        /// <summary>
        /// tạo đối tượng dbManager thông qua getInstance();
        /// ví dụ: var dbManager = DBManager.getInstance();
        /// </summary>
        /// <returns></returns>
        public static DBManager getInstance()
        {
            if (instance == null)
            {
                instance = new DBManager();

                var file = File.Open(Global.getBaseFolder() + DATA_PATH, FileMode.Open);
                workbook = new Workbook(file);
                file.Close();
            }

            return instance;
        }

        /// <summary>
        /// load tất cả các sheet
        /// </summary>
        /// <returns></returns>
        public List<Worksheet> getAllSheet()
        {
            List<Worksheet> sheets = new List<Worksheet>();

            if (workbook != null)
            {
                try
                {
                    int i = 0;
                    Worksheet sheet = null;

                    while ((sheet = workbook.Worksheets[i++]) != null)
                    {
                        sheets.Add(sheet);
                    }
                }
                catch { }

            }

            return sheets;
        }

        /// <summary>
        /// chỉ load tên và icon và các category
        /// </summary>
        /// <returns> danh sách các category chỉ bao gồm tên và icon</returns>
        public BindingList<Category> getAllCategoryName()
        {
            BindingList<Category> categoryName = new BindingList<Category>();

            var sheets = getAllSheet();
            foreach (var sheet in sheets)
            {
                categoryName.Add(
                    new Category {
                        Name = sheet.Name,
                        Icon = sheet.Cells[$"{Category.COL_ICON}{2}"].StringValue
                    }    
                );
            }

            return categoryName;
        }

        /// <summary>
        /// chuyển 1 row dữ liệu thành đối tượng cosmetic
        /// </summary>
        /// <param name="rowData"> là 1 row dữ liệu lấy ra từ file excel</param>
        /// <returns> 1 đối tượng cosmetic </returns>
        private Cosmetic getCosmeticFrom(List<string> rowData)
        {
            return new Cosmetic
            {
                ID = rowData[1],
                Name = rowData[2],
                Image_url = rowData[3],
                Price = uint.Parse(rowData[4]),
                Origin = rowData[5],
                Detail = rowData[6],
                row_in_db = int.Parse(rowData[7])
            };
        }


        /// <summary>
        /// load tất cả các cosmetic của 1 category dựa vào sheet
        /// lưu ý: hàm này bổ trợ cho getAllCosmeticBySheetName() bên dưới
        /// </summary>
        /// <param name="sheet"> 1 sheet của file excel </param>
        /// <returns> tất cả các cosmetic của category nào đó</returns>
        private BindingList<Cosmetic> getAllCosmeticOf(Worksheet sheet)
        {
            BindingList<Cosmetic> cosmetics = new BindingList<Cosmetic>();
            if (sheet != null)
            {
                char col = 'A';
                int row = 2;
                var cell = sheet.Cells[$"{col}{row}"];

                List<string> row_data = new List<string>();
                while (true)
                {
                    // check value
                    if (cell.Value != null)
                    {
                        // -1 -> product has been deleted
                        if (col == 'A' && cell.StringValue.Equals("-1"))
                        {
                            row++;
                        }
                        else
                        {
                            // product available
                            row_data.Add(cell.StringValue);
                            col++;
                            cell = sheet.Cells[$"{col}{row}"];
                        }

                        continue;
                    }

                    // end of row, add cosmetic to list
                    if (row_data.Count > 0)
                    {
                        // save location of data 
                        row_data.Add(row.ToString());
                        cosmetics.Add(getCosmeticFrom(row_data));

                        // reset row data
                        row_data.Clear();
                    }


                    // check the next row available
                    row++;
                    col = 'A';
                    cell = sheet.Cells[$"{col}{row}"];
                    if (cell.Value == null)
                    {
                        break;
                    }
                }
            }

            return cosmetics;
        }

        /// <summary>
        /// load tất cả cosmetic của category dựa vào tên của category
        /// </summary>
        /// <param name="sheetName">tên của category </param>
        /// <returns> tất cả các cosmetic của category với tên truyền vào </returns>
        public BindingList<Cosmetic> getAllCosmeticBySheetName(string sheetName)
        {
            if (workbook != null)
            {
                var sheet = workbook.Worksheets[sheetName];
                if (sheet != null)
                {
                    return getAllCosmeticOf(sheet);
                }
            }

            return null;
        }

        /// <summary>
        /// load tất cả category có trong database
        /// </summary>
        /// <returns> Trả ra tất cả category, bao gồm tên, icon và tất cả các cosmetic mà nó chứa </returns>
        public BindingList<Category> loadAllCategoryOfCosmetic()
        {
            BindingList<Category> categories = new BindingList<Category>();
            if (workbook != null)
            {
                var sheets = getAllSheet();

                foreach (var sheet in sheets)
                {
                    categories.Add(
                        new Category
                        {
                            Name = sheet.Name,
                            Icon = sheet.Cells[$"{Category.COL_ICON}{2}"].StringValue,
                            Cosmetics = getAllCosmeticOf(sheet)
                        }
                    );

                }
            }
            return categories;
        }


        /// <summary>
        /// thêm 1 category
        /// </summary>
        /// <param name="name"> tên của category</param>
        /// <param name="icon"> icon của category
        ///     lưu ý: icon chính là tên file icon
        ///     ví dụ: icon.png
        /// </param>
        /// <returns> 
        ///     true: thêm thành công
        ///     false: ngược lại
        ///     
        /// lưu ý: 
        ///         +> sau khi cho người ta chọn icon nhớ copy file icon đó vào thư mục image :v
        ///         +> nhớ làm tương tự với update :v
        /// </returns>
        public bool addNewCategory(string name, string icon)
        {
            if (workbook != null)
            {
                if (!workbook.Worksheets.Contains(workbook.Worksheets[name]))
                {
                    workbook.Worksheets.Add(name);

                    var sheet = workbook.Worksheets[name];
                    sheet.Cells[$"{Cosmetic.COL_STATUS}{1}"].Value = "Status";
                    sheet.Cells[$"{Cosmetic.COL_ID}{1}"].Value = "ID";
                    sheet.Cells[$"{Cosmetic.COL_NAME}{1}"].Value = "Name";
                    sheet.Cells[$"{Cosmetic.COL_IMAGE_URL}{1}"].Value = "Image";
                    sheet.Cells[$"{Cosmetic.COL_PRICE}{1}"].Value = "Price";
                    sheet.Cells[$"{Cosmetic.COL_ORIGIN}{1}"].Value = "Origin";
                    sheet.Cells[$"{Cosmetic.COL_DETAIL}{1}"].Value = "Detail";
                    sheet.Cells[$"{Category.COL_ICON}{1}"].Value = "Icon Category";
                    sheet.Cells[$"{Category.COL_ICON}{2}"].Value = icon;

                    saveChanged();
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// chỉ sửa tên category
        /// </summary>
        /// <param name="oldName"> tên ban đầu </param>
        /// <param name="newName"> tên muốn sửa </param>
        /// <returns>
        ///     true: sửa thành công
        ///     false: ngược lại
        /// </returns>
        public bool editNameCategory(string oldName, string newName)
        {
            if (workbook != null)
            {
                var sheet = workbook.Worksheets[oldName];
                if (workbook.Worksheets.Contains(sheet))
                {
                    sheet.Name = newName;

                    saveChanged();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// chỉ sửa icon category
        /// </summary>
        /// <param name="sheetName">tên category cần sửa</param>
        /// <param name="newIcon"> icon mới </param>
        /// <returns> true nếu thành công và ngược lại</returns>
        public bool editIconCategory(string sheetName, string newIcon)
        {
            if (workbook != null)
            {
                var sheet = workbook.Worksheets[sheetName];
                if (workbook.Worksheets.Contains(sheet))
                {
                    sheet.Cells[$"{Category.COL_ICON}{2}"].Value = newIcon;

                    saveChanged();
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// sửa cả tên và icon của category
        /// </summary>
        /// <param name="oldSheetName"> tên ban đầu </param>
        /// <param name="newSheetName"> tên mới </param>
        /// <param name="newIcon"> iocn mới </param>
        public void editCategory(string oldSheetName, string newSheetName, string newIcon)
        {
            if (!string.IsNullOrEmpty(newSheetName))
            {
                bool c = editNameCategory(oldSheetName, newSheetName);
                if (c)
                {
                    editIconCategory(newSheetName, newIcon);
                }
                else
                {
                    editIconCategory(oldSheetName, newIcon);
                }
            }
        }


        /// <summary>
        /// tìm ra dòng tiếp theo trong sheet có thể thêm dữ liệu
        /// </summary>
        /// <param name="name"> tên của category </param>
        /// <returns>
        ///     -1: nếu có lỗi xảy ra
        ///     any: thành công
        /// </returns>
        private int findNewRowOfSheet(string name)
        {
            if (workbook != null)
            {
                var sheet = workbook.Worksheets[name];
                if (sheet != null)
                {
                    int row = 2;
                    Cell cell = null;
                    while ((cell = sheet.Cells[$"A{row}"]).Value != null)
                    {
                        row++;
                    }

                    return row;
                }
            }

            return -1;
        }


        /// <summary>
        /// thêm 1 cosmetic vào category
        /// </summary>
        /// <param name="cosmetic"> cosmetic cần thêm </param>
        /// <param name="sheetName"> category của cosmetic </param>
        /// <returns>
        ///     trả về vị trí row của cosmetic trong database nếu thêm thành công
        ///     -1: nếu thêm bị ngu
        ///     
        /// lưu ý: +> sau khi thêm thành công 1 cosmetic, cần thêm thuộc tính row_in_db của comestic đó bằng chính giá trị trả về
        ///        +> giá trị row_in_db càu cosmetic nhằm để dễ chỉnh sửa cosmetic sau này :v
        ///        +> ví dụ:
        ///             var cosmetic = new Cosmetic{..........}
        ///             var categoryName = "Body"
        ///             int result = dbManager.addNewCosmeticOfSheet(cosmetic, categoryName)
        ///             
        ///             if( result != -1){
        ///                 cosmetic.row_in_db = result
        ///             }else{
        ///                 add bị ngu
        ///             }
        ///             
        ///        +> nếu có update icon, sau khi cho người ta chọn icon nhớ copy file icon đó vào thư mục image :v
        /// 
        /// </returns>
        public int addNewCosmeticOfSheet(Cosmetic cosmetic, string sheetName)
        {
            if (workbook != null)
            {
                var sheet = workbook.Worksheets[sheetName];
                if (sheet != null)
                {
                    int row = findNewRowOfSheet(sheetName);
                    if (row != -1)
                    {
                        sheet.Cells[$"{Cosmetic.COL_STATUS}{row}"].Value = "0";
                        sheet.Cells[$"{Cosmetic.COL_ID}{row}"].Value = row - 1;
                        sheet.Cells[$"{Cosmetic.COL_NAME}{row}"].Value = cosmetic.Name;
                        sheet.Cells[$"{Cosmetic.COL_IMAGE_URL}{row}"].Value = cosmetic.Image_url;
                        sheet.Cells[$"{Cosmetic.COL_PRICE}{row}"].Value = cosmetic.Price;
                        sheet.Cells[$"{Cosmetic.COL_ORIGIN}{row}"].Value = cosmetic.Origin;
                        sheet.Cells[$"{Cosmetic.COL_DETAIL}{row}"].Value = cosmetic.Detail;

                        saveChanged();
                        return row;
                    }
                }
            }

            return -1;
        }


        /// <summary>
        /// xóa 1 cosmetic
        /// </summary>
        /// <param name="row_db"> row_in_db của cosmetic </param>
        /// <param name="nameSheet"> tên category mà cosmetic thuộc về </param>
        /// <returns> true nếu thành công </returns>
        public bool deleteCosmeticInSheetWithRow(int row_db, string nameSheet)
        {
            if (workbook != null)
            {
                var sheet = workbook.Worksheets[nameSheet];
                if (sheet != null)
                {
                    sheet.Cells[$"{Cosmetic.COL_STATUS}{row_db}"].Value = "-1";

                    saveChanged();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// kiểm tra xem category có rỗng hay không
        /// </summary>
        /// <param name="sheet"> tên category cần check </param>
        /// <returns> true nếu rỗng </returns>
        private bool isEmptyCategory(Worksheet sheet)
        {
            if (workbook != null)
            {
                if (sheet != null)
                {
                    int row = 2;
                    Cell cell = null;
                    while ((cell = sheet.Cells[$"{Cosmetic.COL_STATUS}{row}"]).Value != null)
                    {
                        if (cell.StringValue.Equals("-1"))
                        {
                            row++;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// xóa 1 category, nếu rỗng mới dk xóa
        /// </summary>
        /// <param name="nameSheet"> tên category cần xóa </param>
        /// <returns> true nếu thành công </returns>
        public bool deleteCategoryWithName(string nameSheet)
        {
            if (workbook != null)
            {
                var sheet = workbook.Worksheets[nameSheet];
                if (sheet != null)
                {
                    if (isEmptyCategory(sheet))
                    {
                        workbook.Worksheets.RemoveAt(nameSheet);

                        saveChanged();
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// update 1 cosmetic
        /// </summary>
        /// <param name="cosmetic"> cosmetic </param>
        /// <param name="sheetName"> category của comestic
        ///     lưu ý: tại sao không lưu thuộc tính category Name vào trong comestic cho dễ truy cập :v
        ///            -> trả lời: nếu sửa category name thì phải duyệt hết các thằng con để sửa category name thì mất công :v
        /// 
        /// </param>
        /// <returns>
        /// true nếu thành công
        ///</returns>
        public bool editCosmeticOfSheet(Cosmetic cosmetic, string sheetName)
        {
            if (workbook != null)
            {
                var sheet = workbook.Worksheets[sheetName];
                if (sheet != null)
                {
                    int row = cosmetic.row_in_db;
                    if (row > 1)
                    {
                        sheet.Cells[$"{Cosmetic.COL_NAME}{row}"].Value = cosmetic.Name;
                        sheet.Cells[$"{Cosmetic.COL_IMAGE_URL}{row}"].Value = cosmetic.Image_url;
                        sheet.Cells[$"{Cosmetic.COL_PRICE}{row}"].Value = cosmetic.Price;
                        sheet.Cells[$"{Cosmetic.COL_ORIGIN}{row}"].Value = cosmetic.Origin;
                        sheet.Cells[$"{Cosmetic.COL_DETAIL}{row}"].Value = cosmetic.Detail;

                        saveChanged();
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// save tất cả thay đổi sau khi tương tác với file excel
        /// </summary>
        public void saveChanged()
        {
            workbook.Save(Global.getBaseFolder() + DATA_PATH);
        }
    }
}
