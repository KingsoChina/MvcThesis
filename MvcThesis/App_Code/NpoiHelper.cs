using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace MvcThesis
{
    public class NpoiHelper
    {
        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <returns>Excel工作表</returns>
        private static Stream ExportDataTableToExcel(DataTable sourceTable, string sheetName,params int[] ColumnWidth)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(sheetName);
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
            headerRow.HeightInPoints = 28;
            //设置表头字体样式
            HSSFCellStyle HeadStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFFont Headfont = (HSSFFont)workbook.CreateFont();
            HeadStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            HeadStyle.VerticalAlignment = VerticalAlignment.Center;
            HeadStyle.WrapText = true;
            Headfont.FontHeightInPoints = 12;
            Headfont.FontName = "宋体";
            HeadStyle.SetFont(Headfont);

            if(ColumnWidth.Length==0) ColumnWidth = new int[]{20,9,25,50,9,12};
            //设置列宽
            for (int i = 0; i < ColumnWidth.Length; i++)
            {
                sheet.SetColumnWidth(i, (ColumnWidth[i] + 2) * 256);
            }
            //sheet.SetColumnWidth(0, 20 * 256);
            //sheet.SetColumnWidth(1, 9  * 256);
            //sheet.SetColumnWidth(2, 25 * 256);
            //sheet.SetColumnWidth(3, 50 * 256);
            //sheet.SetColumnWidth(4, 9 * 256);
            //sheet.SetColumnWidth(5, 12 * 256);
            //sheet.SetColumnWidth(6, 9 * 256);
            //sheet.SetColumnWidth(7, 20 * 256);
            //sheet.SetColumnWidth(8, 10 * 256);
            // handling header.
            foreach (DataColumn column in sourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                headerRow.GetCell(column.Ordinal).CellStyle = HeadStyle;

            }

            // handling value.
            int rowIndex = 1;

            //设置列的字体样式
            HSSFCellStyle Style = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFFont font = (HSSFFont)workbook.CreateFont();
            Style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            Style.VerticalAlignment = VerticalAlignment.Center;
            font.FontHeightInPoints = 12;
            font.FontName = "宋体";
            Style.SetFont(font);

            foreach (DataRow row in sourceTable.Rows)
            {
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                dataRow.HeightInPoints = 17;

                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    dataRow.GetCell(column.Ordinal).CellStyle = Style;
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }
        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="fileName">指定Excel工作表名称</param>
        /// <returns>Excel工作表</returns>
        public static void ExportDataTableToExcel(DataTable sourceTable, string fileName, string sheetName, params int[] ColumnWidth)
        {
            MemoryStream ms = ExportDataTableToExcel(sourceTable, sheetName,ColumnWidth) as MemoryStream;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
            ms.Close();
            ms = null;
        }
    }
}