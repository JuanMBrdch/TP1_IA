using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionTree : ITreeNode
{
    Func<bool> question;
    ITreeNode trueNode;
    ITreeNode falseNode;


    public QuestionTree(Func<bool> question, ITreeNode trueNode, ITreeNode falseNode) //me aseguro de al crear el nodo pregunta
                                                                                      //que se le pase la pregunta
    {
        this.question = question; //me guardo la pregunta y m�s tarde
        this.trueNode = trueNode;
        this.falseNode = falseNode;
        // al ejecutar, hago esta pregunta

        //de esta manera el booleano puede ser true o false dependiendo de lo que pase
        //cuando se ejecuta la funci�n y NO cuando se crea
    }

    public void Execute()
    {
        if (question())
        {
            //ac� necesitamos la acci�n si es true
            trueNode.Execute();
        }
        else
        {
            //y si es false
            falseNode.Execute();
        }
    }
}
