using UnityEngine;
using System;

[Serializable]
public class figureProperties : MonoBehaviour {
    public RewardType type;
    public Figures2D id;
    public bool reflectionSymmetry;
    public bool rotationalSymmetry;
    public bool regular;
    public bool rectangle;
    public bool trapezium;
    public bool isosceles;
    public bool rightAngle;
    public bool rhombus;
    public bool obtuseAngle;
    public bool acuteAngle;
    public NumOfEdges numOfEdges;
    public Color solutionColor;
    private bool bomb = false;


    public bool hasBomb()
    {
        return bomb;
    }

    public void setBomb(bool b)
    {
        bomb = b;
    }
}
