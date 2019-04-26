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

        public BindingList<Category> getAllCategoryName()
        {
            BindingList<Category> categoryName = new BindingList<Category>();

            var sheets = getAllSheet();
            foreach (var sheet in sheets)
            {
                categoryName.Add(
                    new Category
                    {
                        Name = sheet.Name,
                        Icon = sheet.Cells[$"{Category.COL_ICON}{2}"].StringValue
                    }
                );
            }

            return categoryName;
        }

        private Cosmetic getCosmeticFrom(List<string> rowData)
        {
            return new Cosmetic
            {
                ID = int.Parse(rowData[1]),
                Name = rowData[2],
                Image = rowData[3],
                Price = int.Parse(rowData[4]),
                Origin = rowData[5],
                Detail = rowData[6],
                //row_in_db = int.Parse(rowData[7])
            };
        }

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

        public bool addNewCategory(string name)
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

                    saveChanged();
                    return true;
                }
            }

            return false;
        }

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
                        sheet.Cells[$"{Cosmetic.COL_IMAGE_URL}{row}"].Value = cosmetic.Image;
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

        public bool deleteCosmeticWithInSheetWithRow(int row_db, string nameSheet)
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

        public bool editCosmeticOfSheet(Cosmetic cosmetic, string sheetName)
        {
            if (workbook != null)
            {
                var sheet = workbook.Worksheets[sheetName];
                if (sheet != null)
                {
                    int row = cosmetic.ID + 1;
                    if (row > 1)
                    {
                        sheet.Cells[$"{Cosmetic.COL_NAME}{row}"].Value = cosmetic.Name;
                        sheet.Cells[$"{Cosmetic.COL_IMAGE_URL}{row}"].Value = cosmetic.Image;
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

        public void saveChanged()
        {
            workbook.Save(Global.getBaseFolder() + DATA_PATH);

            workbook = null;
            var file = File.Open(Global.getBaseFolder() + DATA_PATH, FileMode.Open);
            workbook = new Workbook(file);
            file.Close();
        }
    }
}