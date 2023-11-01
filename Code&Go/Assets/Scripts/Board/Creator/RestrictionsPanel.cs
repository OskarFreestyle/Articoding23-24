using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RestrictionsPanel : MonoBehaviour
{

    private void Awake() {
        ActivateActiveBlocks();
    }

    private void ActivateActiveBlocks() {
        categoryBlockNames = new string[8][];
        categoryBlockNames[0] = startBlocks;
        categoryBlockNames[1] = mathBlocks;
        categoryBlockNames[2] = variableBlocks;
        categoryBlockNames[3] = textBlocks;
        categoryBlockNames[4] = logicBlocks;
        categoryBlockNames[5] = controlBlocks;
        categoryBlockNames[6] = movementBlocks;
        categoryBlockNames[7] = procedureBlocks;

        activeBlocks = new ActiveBlocks();
        activeBlocks.categories = new CategoryData[8];
        blockTogglesLists = new List<List<BlockToggle>>();
        for (int i = 0; i < 8; i++)
        {
            blockTogglesLists.Add(new List<BlockToggle>());

            activeBlocks.categories[i] = new CategoryData();
            activeBlocks.categories[i].categoryName = categoryNames[i];
            activeBlocks.categories[i].blocksInfo = new CategoryBlocksInfo();
            activeBlocks.categories[i].blocksInfo.activate = true;
            activeBlocks.categories[i].blocksInfo.activeBlocks = new BlockInfo[categoryBlocksNum[i]];
            for (int j = 0; j < categoryBlocksNum[i]; j++)
            {
                activeBlocks.categories[i].blocksInfo.activeBlocks[j] = new BlockInfo();
                activeBlocks.categories[i].blocksInfo.activeBlocks[j].blockName = categoryBlockNames[i][j];
                activeBlocks.categories[i].blocksInfo.activeBlocks[j].maxUses = 100;
            }
        }
    }

    public void SetCategoryAllow(string category) {

        int catIndex = GetCategoryData(category);
        bool isActive = activeBlocks.categories[catIndex].categoryName != "";
        if (!isActive) activeBlocks.categories[catIndex].categoryName = categoryNames[catIndex];
        else activeBlocks.categories[catIndex].categoryName = "";

        for (int i = 0; i < categoryBlockNames[catIndex].Length; i++)
        {
            string realName = blockTogglesLists[catIndex][i].SetActive(!isActive);
            SetBlockAllow(category, realName, !isActive);
        }
    }

    public void SetBlockAllow(string category, string block)
    {
        int catIndex = GetCategoryData(category);
        int blockIndex = GetBlockInfo(catIndex, block);
        bool isActive = activeBlocks.categories[catIndex].blocksInfo.activeBlocks[blockIndex].blockName != "";
        if (!isActive) activeBlocks.categories[catIndex].blocksInfo.activeBlocks[blockIndex].blockName = categoryBlockNames[catIndex][blockIndex];
        else activeBlocks.categories[catIndex].blocksInfo.activeBlocks[blockIndex].blockName = "";
    }

    public void SetBlockAllow(string category, string block, bool toActive)
    {
        int catIndex = GetCategoryData(category);
        int blockIndex = GetBlockInfo(catIndex, block);
        if (toActive) activeBlocks.categories[catIndex].blocksInfo.activeBlocks[blockIndex].blockName = categoryBlockNames[catIndex][blockIndex];
        else activeBlocks.categories[catIndex].blocksInfo.activeBlocks[blockIndex].blockName = "";
    }

    private int GetCategoryData(string name)
    {
        string realname = RestrictionsNamesDefs.categoryNames[name];
        for(int i = 0; i < categoryNames.Length; i++)
        {
            if (categoryNames[i] == realname) return i;
        }
        return -1;
    }

    private int GetBlockInfo(int category, string name)
    {
        string realname = RestrictionsNamesDefs.blocksNames[name];
        for (int i = 0; i < categoryBlockNames[category].Length; i++)
        {
            if (categoryBlockNames[category][i] == realname) return i;
        }
        return -1;
    }

    public void SaveActiveBlocks()
    {
        string filepath = Application.dataPath + "/Boards/movida.json";
        FileStream file = new FileStream(filepath, FileMode.Create);
        file.Close();
        StreamWriter writer = new StreamWriter(filepath);
        writer.Write(activeBlocks.ToJson());
        writer.Close();
    }

    public ActiveBlocks GetActiveBlocks()
    {
        return activeBlocks;
    }

    public void AddBlockToggle(string category, BlockToggle toggle)
    {
        int cat = GetCategoryData(category);
        blockTogglesLists[cat].Add(toggle);

        togglesActivated++;
        if(togglesActivated >= totalToggles)
        {
            foreach (var panel in blocksPanels) panel.gameObject.SetActive(false);
        }
    }

    public void SetBlockUses(int uses, string category, string block)
    {
        int catIndex = GetCategoryData(category);
        int blockIndex = GetBlockInfo(catIndex, block);
        activeBlocks.categories[catIndex].blocksInfo.activeBlocks[blockIndex].maxUses = uses;
    }

    private ActiveBlocks activeBlocks;
    private List<List<BlockToggle>> blockTogglesLists;
    public List<GameObject> blocksPanels;

    private int togglesActivated = 0;
    private int totalToggles = 19;

    // Category and blocks name arrays

    string[] categoryNames = { "start", "math", "variable", "text", "logic", "control", "movement", "procedure" };
    int[] categoryBlocksNum = { 1, 2, 2, 1, 7, 2, 4, 2 };
    string[] startBlocks = { "start_start" };
    string[] mathBlocks = { "math_number", "math_arithmetic" };
    string[] variableBlocks = { "variables_get", "variables_set" };
    string[] textBlocks = { "text" };
    string[] logicBlocks = { "logic_negate", "logic_cells_occupied", "logic_compare", "logic_operation", "logic_boolean", "logic_if", "logic_ifelse" };
    string[] controlBlocks = { "controls_repeat", "controls_whileUntil" };
    string[] movementBlocks = { "movement_move", "movement_rotate", "movement_activate_door", "movement_laser_change_intensity" };
    string[] procedureBlocks = { "procedures_defnoreturn", "procedures_callnoreturn" };
    string[][] categoryBlockNames;
}
