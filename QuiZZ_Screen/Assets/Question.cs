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
        Question q = new Question();
        q.index = GetInt(table, row, i++);
        q.nature = GetString(table, row, i++);
        q.theme1 = GetString(table, row, i++);
        q.theme2 = GetString(table, row, i++);
        q.theme3 = GetString(table, row, i++);
        q.question1 = GetString(table, row, i++);
        q.question2 = GetString(table, row, i++);
        q.fichier = GetString(table, row, i++);
        q.reponse1 = GetString(table, row, i++);
        q.reponse2 = GetString(table, row, i++);
        q.dossier = GetString(table, row, i++);
        q.debutfichier = GetString(table, row, i++);
        q.finfichier = GetString(table, row, i++);
        q.indice1 = GetString(table, row, i++);
        q.indice2 = GetString(table, row, i++);
        q.indice3 = GetString(table, row, i++);
        q.niveaudifficultequestion1 = GetString(table, row, i++);
        q.niveaudifficultequestion2 = GetString(table, row, i++);
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

    //Question FromVals(int index, 
    //    string nature, 
    //    string theme1, string theme2, string theme3, 
    //    string question1, string question2, 
    //    string fichier, 
    //    string reponse1, string reponse2, 
    //    string dossier, 
    //    string debutfichier, string finfichier, 
    //    string indice1, string indice2, string indice3, 
    //    string niveaudifficultequestion1, string niveaudifficultequestion2)
    //{
    //    Question q = new Question();
    //    q.index = index;
    //    q.nature = nature;
    //    q.theme1 = theme1;
    //    q.theme2 = theme2;
    //    q.theme3 = theme3;
    //    q.question1 = question1;
    //    q.question2 = question2;
    //    q.fichier = fichier;
    //    q.reponse1 = reponse1;
    //    q.reponse2 = reponse2;
    //    q.dossier = dossier;
    //    q.debutfichier = debutfichier;
    //    q.finfichier = finfichier;
    //    q.indice1 = indice1;
    //    q.indice2 = indice2;
    //    q.indice3 = indice3;
    //    q.niveaudifficultequestion1 = niveaudifficultequestion1;
    //    q.niveaudifficultequestion2 = niveaudifficultequestion2;

    //    return q;
    //}
}
