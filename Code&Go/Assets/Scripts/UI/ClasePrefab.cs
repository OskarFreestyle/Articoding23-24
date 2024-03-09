using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClasePrefab : MonoBehaviour
{
    ServerClasses.Clase thisClase;
    ServerClasses.LevelPage levelPage;

    public Text claseName;

    bool alreadyCreated = false;
    bool levelsCreated = false;

    public void SetupClase(ServerClasses.Clase clase)
    {
        thisClase = clase;
        claseName.text = clase.name;
        SetCreated();
    }

    public ServerClasses.LevelPage GetLevelPage()
    {
        return levelPage;
    }

    public void SetLevelPage(ServerClasses.LevelPage lPage)
    {
        levelPage = lPage;
    }

    public int GetClaseIndex()
    {
        return thisClase.id;
    }

    public string GetClaseName()
    {
        return thisClase.name;
    }

    public ServerClasses.Level GetLevelInfo(int index)
    {
        return levelPage.content[index].level;
    }

    public void SetCreated() { alreadyCreated = true; }
    public bool GetCreated() { return alreadyCreated; }
    public void SetLevelsCreated() { levelsCreated = true; }
    public bool GetLevelsCreated() { return levelsCreated; }
}
