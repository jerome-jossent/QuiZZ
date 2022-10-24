using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;

public static class Questions
{
    public static List<Question> _SetQuestions(string path)
    {
        List<Question> questions = new List<Question>();

        Excel xl = _LoadExcel(path);
        ExcelTable table = xl.Tables[0];

        int NumberOfRows = table.NumberOfRows;

        for (int row = 2; row <= NumberOfRows; row++)
        {
            Question q = new Question(table, row);
            q.index = questions.Count;
            questions.Add(q);
        }
        return questions;
    }

    public static Excel _LoadExcel(string path)
    {
        FileInfo file = new FileInfo(path);
        ExcelPackage ep = new ExcelPackage(file);
        Excel xls = new Excel(ep.Workbook);
        return xls;
    }
}