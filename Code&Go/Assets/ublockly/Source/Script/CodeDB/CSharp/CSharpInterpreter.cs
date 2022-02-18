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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UBlockly
{
    public class CSharpInterpreter : Interpreter
    {
        public override CodeName Name
        {
            get { return CodeName.CSharp; }
        }

        /// <summary>
        /// run code representing the specified value input.
        /// should return a DataStruct
        /// </summary>
        public CmdEnumerator ValueReturn(Block block, string name)
        {
            var targetBlock = block.GetInputTargetBlock(name);
            if (targetBlock == null)
            {
                Debug.Log(string.Format("Value input block of {0} is null", block.Type));
                return null;
            }
            if (targetBlock.OutputConnection == null)
            {
                Debug.Log(string.Format("Value input block of {0} must have an output connection", block.Type));
                return null;
            }
            return new CmdEnumerator(targetBlock);
        }

        /// <summary>
        /// run code representing the specified value input. WITH a default DataStruct
        /// </summary>
        public CmdEnumerator ValueReturn(Block block, string name, DataStruct defaultData)
        {
            CmdEnumerator etor = ValueReturn(block, name);
            etor.Cmdtor.DefaultData = defaultData;
            return etor;
        }

        /// <summary>
        /// Run code representing the statement.
        /// </summary>
        public CmdEnumerator StatementRun(Block block, string name)
        {
            var targetBlock = block.GetInputTargetBlock(name);
            if (targetBlock == null)
            {
                Debug.Log(string.Format("Statement input block of {0} is null", block.Type));
                return null;
            }
            if (targetBlock.PreviousConnection == null)
            {
                Debug.Log(string.Format("Statement input block of {0} must have a previous connection", block.Type));
                return null;
            }

            return new CmdEnumerator(targetBlock);
        }

        public Cmdtor GetBlockInterpreter(Block block)
        {
            // function definition doesn't need interpreter. 
            if (ProcedureDB.IsDefinition(block))
                return null;

            Cmdtor cmdtor;
            if (!mCmdMap.TryGetValue(block.Type, out cmdtor))
                Debug.Log(string.Format(
                    "<color='orange'>Language {0} does not know how to interprete code for block type {1}. If this block type doesn't need to be interpreted, please ignore this message.</color>",
                    Name, block.Type));
            return cmdtor;
        }
    }
    
    // Articoding: added to check that the cards have every input needed. 
    // Use it every time the input may not be there.
    public class CheckInput
    {
        public static CmdEnumerator TryValueReturn(Block block, string key)
        {
            CmdEnumerator ctor = null;
            try
            {
                ctor = CSharp.Interpreter.ValueReturn(block, key);
                DataStruct data = ctor.Data;
            }
            catch (Exception e)
            {
                Debug.LogError("Missing the value: " + key);
                MessageManager.Instance.SendMessage("Error", MSG_TYPE.CODE_END);
            }

            return ctor;
        }
        public static CmdEnumerator TryValueReturn(Block block, string key, DataStruct dataStruct)
        {
            CmdEnumerator ctor = null;
            try
            {
                ctor = CSharp.Interpreter.ValueReturn(block, key, dataStruct);
            }
            catch (Exception e)
            {
                Debug.LogError("Missing the value: " + key);
                MessageManager.Instance.SendMessage("Error", MSG_TYPE.CODE_END);
            }

            return ctor;
        }

        public static string TryGetFieldValue(Block block, string key)
        {
            string str = null;
            try
            {
                str = block.GetFieldValue(key);
                if (str == null) throw new Exception();
            }
            catch (Exception e)
            {
                Debug.LogError("Missing the value: " + key);
                MessageManager.Instance.SendMessage("Error", MSG_TYPE.CODE_END);
                str = "";
            }

            return str;
        }
    }
}
