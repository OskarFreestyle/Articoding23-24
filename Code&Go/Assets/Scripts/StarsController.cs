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
    public Image firstRunStar;
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
        if (firstRunStar.color == deactivatedColor)
            nStars --;
        if (noHangingCodeStar.color == deactivatedColor)
            nStars --;
        if (minimumStepsStar.color == deactivatedColor)
            nStars --;

        return nStars;
    }

    public void GiveSpecialStar(string specialBlock)
    {
        // GameObject block = GameObject.Find("Block_start_start");
        // while (block != null && block.name != specialBlock)
        // {
        //     block = Array.Find(block.GetComponentsInChildren<ConnectionView>(), (view =>
        //     {
        //         return view.gameObject.name == "Connection_next";
        //     })).gameObject.GetComponentInChildren<Transform>().gameObject;
            
        // }
        
        
        string s = Array.Find(blocksUsedInLevel,((string s) => {return s==specialBlock;}));
        Debug.LogError(s);
        if(s == null && specialBlock != "None" )
        {
            firstRunStar.color = deactivatedColor;
        }
    }
    
    public void DeactivateFirstRunStar()
    {
        firstRunStar.color = deactivatedColor;
    }

    public void DeactivateNoHangingCodeStar()
    {
        noHangingCodeStar.color = deactivatedColor;
    }

    public void DeactivateMinimumStepsStar(int stepsOverflow)
    {
        minimumStepsStar.color = deactivatedColor;
        stepsOverflowText.text = stepsOverflow.ToString() + "+";
    }

    public void ReactivateMinimumStepsStar()
    {
        minimumStepsStar.color = activatedColor;
        stepsOverflowText.text = "";
    }

    public bool IsFirstRunStarActive()
    {
        return firstRunStar.color != deactivatedColor;
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
