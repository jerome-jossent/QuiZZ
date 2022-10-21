using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;

public class Questions : MonoBehaviour
{
    public List<Question> questions = new List<Question>();
    public string path = @"D:\Projets\QuiZZ\Ressources\Quizz\quizz.xlsx";






    // Start is called before the first frame update
    void Start()
    {
        Excel xl = LoadExcel(path);
        ExcelTable table = xl.Tables[0];

        int NumberOfRows = table.NumberOfRows;
        //Debug.Log("lignes = " + NumberOfRows);

        for (int row = 1; row <= NumberOfRows; row++)
        {
            Question q = new Question(table, row);
            questions.Add(q);
        }

        Debug.Log(questions.Count);
    }



    public static Excel LoadExcel(string path)
    {
        FileInfo file = new FileInfo(path);
        ExcelPackage ep = new ExcelPackage(file);
        Excel xls = new Excel(ep.Workbook);
        return xls;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
