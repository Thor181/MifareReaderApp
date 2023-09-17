using MifareReaderApp.Stuff.Results;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff
{
    public class Excel
    {
        public List<string> Headers { get; set; }
        public List<List<string>> Values { get; set; }
        private string _sheetName;
        private string _fileName;

        public Excel(List<string> headers, List<List<string>> rowItems, string sheetName, string fileName)
        {
            Headers = headers;
            Values = rowItems;
            _sheetName = sheetName;
            _fileName = fileName;
        }

        public async Task<ExportResult> Export()
        {
            var result = new ExportResult() { Message = "Успешно экспортировано"};

            await Task.Run(() =>
            {
                try
                {
                    var workbook = new XSSFWorkbook();

                    var sheet = workbook.CreateSheet(_sheetName);

                    var headerRow = sheet.CreateRow(0);

                    for (int i = 0; i < Headers.Count; i++)
                    {
                        var value = Headers[i];
                        var cell = headerRow.CreateCell(i);
                        cell.SetCellValue(value);

                        sheet.SetColumnWidth(i, 700 * 5);

                        var cellStyle = workbook.CreateCellStyle();

                        cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Double;
                        cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Double;
                        cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Double;
                        cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Double;
                        cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                        cellStyle.FillPattern = FillPattern.SolidForeground;

                        cell.CellStyle = cellStyle;
                    }

                    for (int i = 0; i < Values.Count; i++)
                    {
                        var row = sheet.CreateRow(i + 1);
                        for (int j = 0; j < Values[i].Count; j++)
                        {
                            var value = Values[i][j];
                            var cell = row.CreateCell(j);
                            cell.SetCellValue(value);
                            var cellStyle = workbook.CreateCellStyle();

                            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

                            cell.CellStyle = cellStyle;
                        }

                    }

                    using (FileStream fileStream = new FileStream(_fileName, FileMode.Create))
                    {
                        workbook.Write(fileStream, false);
                    }
                }
                catch (Exception e)
                {
                    Logger.Instance.LogError("Во время экспорта в Excel возникла ошибка", e);
                    result.IsSuccess = false;
                    result.Message = $"Во время экспорта в Excel возникла ошибка\n{e.Message}";
                }
            });
            
            
            return result;
        }
    }
}
