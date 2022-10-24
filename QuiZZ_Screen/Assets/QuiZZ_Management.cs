using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuiZZ_Management : MonoBehaviour
{
    public string path = @"D:\Projets\QuiZZ\Ressources\Quizz\quizz.xlsx";
    public List<Question> questions;

    public TMPro.TMP_Text txt_question1;

    // Start is called before the first frame update
    void Start()
    {
        questions = Questions._SetQuestions(path);
        _NewQuestion_Random();
    }

    public void _NewQuestion_Random()
    {
        int i = Random.Range(0, questions.Count);
        Question q = questions[i];
        Debug.Log(q);
        txt_question1.text = q.ToString();
    }



    // Update is called once per frame
    void Update()
    {

    }

    //chargement du fichier data Quizz
    // data (lecture d'un fichier excel)
    // m�dia


    //nouvelle question
    //  affichage question � l'�cran + m�dia (son, image, vid�o) ?
    //  affichage question au Master, (Player ?)

    //�dition des joueurs
    //  �cran Nouveau joueur, choix couleur, choix image, choix son, choix pseudo
    //



    //affichage des joueurs
    //  infos : [nom], pseudo, son, couleur, points, actif/inactif/a buzz�/a le lead
    //  retour d'�tat du bouton (push/unpush) des buzzers
    //  retour d'�tat de la lumi�re (allum�/�teint/duree fade-in/fade-out des buzzers)

    //Retour Master
    //validation/invalidation



}
