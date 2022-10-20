using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buzzer : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image image;
    public Color colorON;
    Color colorOFF;
    public string nom;
    internal int number;

    void Start()
    {
        image.color = colorON;
        colorOFF = new Color(colorON.r, colorON.g, colorON.b, 0.5f);
    }

    void Update()
    {
        
    }

    internal void _SetNameAndColor(string msg)
    {
        nom = msg;
        char cara = ' ';
        string[] couleur_numero = msg.Split(cara);
        switch (couleur_numero[0].ToLower())
        {
            case "jaune": colorON = Color.yellow; break;
            case "vert": colorON = Color.green; break;
            case "rouge": colorON = Color.red; break;
            case "bleu": colorON = Color.blue; break;
            default: break;
        }
        number = int.Parse(couleur_numero[1]);
    }

    internal void _Action(string value)
    {
        switch (value)
        {
            case "ON":
                image.color = colorON;
                break;
            case "OFF":
                image.color = colorOFF;
                break;
            default:
                break;
        }
    }
}
