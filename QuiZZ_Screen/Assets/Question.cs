using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    //N°	Nature	Thème 1	Thème 2	Thème 3	Question 1	Question 2	Fichier	Réponse 1	Réponse 2	dossier	début fichier	fin fichier	Indice 1	Indice 2	Indice 3	Niveau difficultés question 1	Niveau difficultés question 2
    public int index;
    public string nature;
    public string theme1;
    public string theme2;
    public string theme3;
    public string question1;
    public string question2;
    public string fichier;
    public string reponse1;
    public string reponse2;
    public string dossier;
    public string debutfichier;
    public string finfichier;
    public string indice1;
    public string indice2;
    public string indice3;
    public string niveaudifficultequestion1;
    public string niveaudifficultequestion2;

    public Question() { }
    public Question(ExcelTable table, int row)
    {
        int i = 1;
        nature = GetString(table, i++, row);
        theme1 = GetString(table, i++, row);
        theme2 = GetString(table, i++, row);
        theme3 = GetString(table, i++, row);
        question1 = GetString(table, i++, row);
        question2 = GetString(table, i++, row);
        fichier = GetString(table, i++, row);
        reponse1 = GetString(table, i++, row);
        reponse2 = GetString(table, i++, row);
        dossier = GetString(table, i++, row);
        debutfichier = GetString(table, i++, row);
        finfichier = GetString(table, i++, row);
        indice1 = GetString(table, i++, row);
        indice2 = GetString(table, i++, row);
        indice3 = GetString(table, i++, row);
        niveaudifficultequestion1 = GetString(table, i++, row);
        niveaudifficultequestion2 = GetString(table, i++, row);
    }

    string GetString(ExcelTable table, int col, int row)
    {
        return table.GetValue(row, col).ToString();
    }
    int GetInt(ExcelTable table, int col, int row)
    {
        try
        {
            string val = GetString(table, row, col);
            return int.Parse(val);
        }
        catch (System.Exception ex)
        {
            Debug.Log(col + " " + row + " " + ex.Message);
            return -666;
        }
    }

    public override string ToString()
    {
        return "[" + index + "] " +
               nature + " " +
               theme1 + " " +
               theme2 + " " +
               theme3 + " " +
               question1 + " " +
               question2 + " " +
               fichier + " " +
               reponse1 + " " +
               reponse2 + " " +
               dossier + " " +
               debutfichier + " " +
               finfichier + " " +
               indice1 + " " +
               indice2 + " " +
               indice3 + " " +
               niveaudifficultequestion1 + " " +
               niveaudifficultequestion2;
    }
}
