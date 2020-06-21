using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using NPOI.XSSF.UserModel;

namespace Common
{
    public class ExcelTool
    {
        public static MemoryStream RenderToExcel(DataSet ds)
        {
            var ms = new MemoryStream();
            if (ds == null || ds.Tables.Count <= 0)
            {
                return ms;
            }
            using (ds)
            {
                IWorkbook workbook = new HSSFWorkbook();

                for (var i = 0; i < ds.Tables.Count; i++)
                {
                    var dt = ds.Tables[i];

                    var sheet = workbook.CreateSheet(ds.Tables[i].TableName);
                    var headerRow = sheet.CreateRow(0);
                    headerRow.HeightInPoints = 25;
                    sheet.CreateFreezePane(0, 1);

                    var headercellFont = sheet.Workbook.CreateFont();
                    headercellFont.Boldweight = (short)FontBoldWeight.Bold;
                    headercellFont.FontHeight = 180;
                    headercellFont.Color = HSSFColor.White.Index;

                    var headercellStyle = sheet.Workbook.CreateCellStyle();
                    headercellStyle.FillPattern = FillPattern.SolidForeground;
                    headercellStyle.FillForegroundColor = HSSFColor.LightOrange.Index;
                    headercellStyle.VerticalAlignment = VerticalAlignment.Center;
                    headercellStyle.Alignment = HorizontalAlignment.Center;
                    headercellStyle.WrapText = true;
                    headercellStyle.SetFont(headercellFont);
                    foreach (DataColumn column in dt.Columns)
                    {
                        var cell = headerRow.CreateCell(column.Ordinal);
                        cell.SetCellValue(column.Caption);
                        cell.CellStyle = headercellStyle;
                    }
                    var rowIndex = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        var dataRow = sheet.CreateRow(rowIndex);
                        foreach (DataColumn column in dt.Columns)
                        {
                            dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                        }
                        rowIndex++;
                    }
                    foreach (DataColumn column in dt.Columns)
                    {
                        sheet.AutoSizeColumn(column.Ordinal);
                    }
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }



        public static MemoryStream RenderToExcel(DataTable dt)
        {
            var ms = new MemoryStream();
            if (dt == null)
            {
                return ms;
            }
            using (dt)
            {
                IWorkbook workbook = new HSSFWorkbook();

                var sheet = workbook.CreateSheet(dt.TableName);
                var headerRow = sheet.CreateRow(0);
                headerRow.HeightInPoints = 25;
                sheet.CreateFreezePane(0, 1);

                var headercellFont = sheet.Workbook.CreateFont();
                headercellFont.Boldweight = (short)FontBoldWeight.Bold;
                headercellFont.FontHeight = 180;
                headercellFont.Color = HSSFColor.Black.Index;

                var headercellStyle = sheet.Workbook.CreateCellStyle();
                headercellStyle.FillPattern = FillPattern.SolidForeground;
                headercellStyle.FillForegroundColor = HSSFColor.PaleBlue.Index;
                headercellStyle.VerticalAlignment = VerticalAlignment.Center;
                headercellStyle.Alignment = HorizontalAlignment.Center;
                headercellStyle.WrapText = true;
                headercellStyle.SetFont(headercellFont);
                foreach (DataColumn column in dt.Columns)
                {
                    var cell = headerRow.CreateCell(column.Ordinal);
                    cell.SetCellValue(column.Caption);
                    cell.CellStyle = headercellStyle;
                }
                var rowIndex = 1;
                foreach (DataRow row in dt.Rows)
                {
                    var dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dt.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
                foreach (DataColumn column in dt.Columns)
                {
                    sheet.AutoSizeColumn(column.Ordinal);
                }

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }


        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string sheetName, Stream fs, bool isFirstRowColumn, string extName = ".xls")
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {

                if (extName == ".xlsx") // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn("col" + i);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
    }
}
