using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UBlockly.UGUI;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class StarsController : Listener
{
    public Image minimumStepsStar;
    public Image specialBlockStar;
    public Image noHangingCodeStar;
    public Text stepsOverflowText;
    public Color activatedColor;
    private Color deactivatedColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

    private string[] blocksUsedInLevel;


    public override void ReceiveMessage(string msg, MSG_TYPE type)
    {
        if(type == MSG_TYPE.SOLUTION)
        {
            blocksUsedInLevel = msg.Split(';');
        }
    }
    public override bool ReceiveBoolMessage(string msg, MSG_TYPE type)
    {
        if(type == MSG_TYPE.SOLUTION)
        {
            blocksUsedInLevel = msg.Split(';');
            return true;
        }
        return false;
    }
    public int GetStars()
    {
        int nStars = 3;
        if (specialBlockStar.color == deactivatedColor)
            nStars --;
        if (noHangingCodeStar.color == deactivatedColor)
            nStars --;
        if (minimumStepsStar.color == deactivatedColor)
            nStars --;

        return nStars;
    }

    public void GiveMinimumStepsStar(int stepsOverflow)
    {
        if (stepsOverflow > 0)
        {
            minimumStepsStar.color = deactivatedColor;
            stepsOverflowText.text = stepsOverflow.ToString() + "+";
        }
    }
    public void GiveHangingCodeStar(int topBlocks)
    {
        if (topBlocks > 1)
            noHangingCodeStar.color = deactivatedColor;
    }
    public void GiveSpecialStar(string specialBlock)
    {
        string s = Array.Find(blocksUsedInLevel,((string s) => {return s==specialBlock;}));
        if(s == null && specialBlock != "None" )
        {
            specialBlockStar.color = deactivatedColor;
        }
    }

    public void ReactivateMinimumStepsStar()
    {
        minimumStepsStar.color = activatedColor;
        stepsOverflowText.text = "";
    }

    public bool IsFirstRunStarActive()
    {
        return specialBlockStar.color != deactivatedColor;
    }

    public bool IsMinimumStepsStarActive()
    {
        return minimumStepsStar.color != deactivatedColor;
    }

    public bool IsNoHintsStarActive()
    {
        return noHangingCodeStar.color != deactivatedColor;
    }
}
