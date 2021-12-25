/****************************************************************************

Copyright 2016 sophieml1989@gmail.com

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

****************************************************************************/

using AssetPackage; //articoding
using System.Xml; //articoding
using UnityEngine;
using UnityEngine.UI;

namespace UBlockly.UGUI
{
    public class VariableNameDialog : BaseDialog
    {
        [SerializeField] private Text m_InputLabel;
        [SerializeField] private InputField m_Input;
        private PanelControl inventoryControl; //articoding

        private bool mIsRename = false;
        
        private string mOldVarName;
        public void Rename(string varName)
        {
            mOldVarName = varName;
            mIsRename = true;
            m_InputLabel.text = I18n.Get(MsgDefine.RENAME_VARIABLE);
        }

        protected override void OnInit()
        {
            inventoryControl = GameObject.Find("OpenArea").GetComponent<PanelControl>(); //articoding
            m_InputLabel.text = I18n.Get(MsgDefine.NEW_VARIABLE);
            inventoryControl.DisableDissapear(true); //articoding

            AddCloseEvent(() =>
            {
                inventoryControl.DisableDissapear(false); //articoding
                if (mIsRename)
                {
                    BlocklyUI.WorkspaceView.Workspace.RenameVariable(mOldVarName, m_Input.text);

                    TrackerAsset.Instance.setVar("new_variable_name", m_Input.text); //articoding
                    TrackerAsset.Instance.setVar("old_variable_name", mOldVarName);
                    TrackerAsset.Instance.setVar("block_type", "variable");
                    TrackerAsset.Instance.setVar("action", "rename");
                    TrackerAsset.Instance.setVar("level", GameManager.Instance.GetCurrentLevelName().ToLower());
                    TrackerAsset.Instance.GameObject.Interacted("rename_variable");
                }
                else
                {
                    VariableModel model = BlocklyUI.WorkspaceView.Workspace.CreateVariable(m_Input.text);

                    if (model == null) return; //articoding

                    TrackerAsset.Instance.setVar("variable_name", model.Name); //articoding
                    TrackerAsset.Instance.setVar("block_type", "variable");
                    TrackerAsset.Instance.setVar("action", "declare");
                    TrackerAsset.Instance.setVar("level", GameManager.Instance.GetCurrentLevelName().ToLower());
                    TrackerAsset.Instance.GameObject.Interacted("new_variable");
                }

            });
        }

        private void OnDestroy()
        {
            inventoryControl.DisableDissapear(false);
        }
    }
}
