/****************************************************************************

Copyright 2021 sophieml1989@gmail.com

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


using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UBlockly
{
    public class CSharpRunner : Runner
    {
        private readonly Names mVariableNames;
        private readonly Datas mVariableDatas;
        private readonly List<CmdRunner> mCodeRunners;
        
        public CSharpRunner(Names variableNames, Datas variableDatas)
        {
            mVariableNames = variableNames;
            mVariableDatas = variableDatas;
            mCodeRunners = new List<CmdRunner>();
        }
        
        public override void Run(Workspace workspace)
        {
            mVariableNames.Reset();
            mVariableDatas.Reset();
            
            //start runner from the topmost blocks, exclude the procedure definition blocks
            List<Block> blocks = workspace.GetTopBlocks(true).FindAll(block => !ProcedureDB.IsDefinition(block));
            
#region articoding - start block
            Block startBlock = null;
            foreach (Block block in blocks)
            {
                if (block.Type == "start_start") //articoding: es un poco feo
                {
                    startBlock = block;
                    break;
                }
            }
#endregion
            if (blocks.Count == 0 || startBlock == null) //articoding - solo ejecuta si existe un bloque start
            {
                MessageManager.Instance.SendMessage("Missing_Start_Block", MSG_TYPE.CODE_END);  
                CSharp.Runner.FireUpdate(new RunnerUpdateState(RunnerUpdateState.Stop));              
                return;
            }

            // Enviamos la cuenta de los bloques "de comienzo" y de los totales que se estan utilizando
            MessageManager.Instance.SendMessage(blocks.Count.ToString(), MSG_TYPE.NUM_OF_TOP_BLOCKS);
            MessageManager.Instance.SendMessage(workspace.GetAllBlocks().Count.ToString(), MSG_TYPE.TOTAL_NUM_OF_BLOCKS);
            string blocksText = "";
            foreach(Block b in workspace.GetAllBlocks() )
                blocksText+=(b.Type + ";");
            MessageManager.Instance.SendMessage(blocksText, MSG_TYPE.SOLUTION);

            string s = "";
            foreach(Block b in blocks)
            {
                //s.Append(b.ID);
            }
            CurStatus = Status.Running;

            if (workspace.Options.Synchronous)
            {
                RunSync(startBlock); //articoding
            }
            else
            {
                RunAsync(startBlock); //articoding
            }
        }

        private void RunSync(Block topBlock) //articoding
        {
            //foreach (Block block in topBlocks)
            CmdRunner runner = CmdRunner.Create(topBlock.Type);
            mCodeRunners.Add(runner);

            runner.RunMode = RunMode;
            runner.SetFinishCallback(() =>
            {
                GameObject.Destroy(runner.gameObject);
                mCodeRunners.Remove(runner);
                if (mCodeRunners.Count == 0)
                {
                    CurStatus = Status.Stop;
                    CSharp.Runner.FireUpdate(new RunnerUpdateState(RunnerUpdateState.Stop));
                }
            });
            runner.StartRun(new CmdEnumerator(topBlock));
        }

        private void RunAsync(Block topBlock) //articoding
        {
            CmdRunner runner = CmdRunner.Create(topBlock.Type);
            mCodeRunners.Add(runner);

            runner.RunMode = RunMode;
            
            //int index = 0; articoding
            runner.SetFinishCallback(() =>
            {
                //articoding
                // index++;
                // if (index < topBlocks.Count)
                // {
                //     runner.StartRun(new CmdEnumerator(topBlocks[index]));
                // }
                // else
                GameObject.Destroy(runner.gameObject);
                mCodeRunners.Clear();
                CurStatus = Status.Stop;
                CSharp.Runner.FireUpdate(new RunnerUpdateState(RunnerUpdateState.Stop));
            });
            runner.StartRun(new CmdEnumerator(topBlock));
        }

        public override void Pause()
        {
            if (RunMode == Mode.Step || CurStatus != Status.Running)
                return;
            CurStatus = Status.Pause;
            
            foreach (CmdRunner runner in mCodeRunners)
            {
                if (runner.CurStatus == Runner.Status.Running)
                    runner.Pause();
            }
            CSharp.Runner.FireUpdate(new RunnerUpdateState(RunnerUpdateState.Pause));
        }

        public override void Resume()
        {
            if (RunMode == Mode.Step || CurStatus != Status.Pause)
                return;
            CurStatus = Status.Running;
            
            foreach (CmdRunner runner in mCodeRunners)
            {
                if (runner.CurStatus == Runner.Status.Pause)
                    runner.Resume();
            }
            CSharp.Runner.FireUpdate(new RunnerUpdateState(RunnerUpdateState.Resume));
        }

        public override void Stop()
        {
            if (CurStatus == Status.Stop)
                return;
            CurStatus = Status.Stop;
            
            foreach (CmdRunner runner in mCodeRunners)
            {
                runner.Stop();
                GameObject.Destroy(runner.gameObject);
            }
            mCodeRunners.Clear();
            
            CSharp.Runner.FireUpdate(new RunnerUpdateState(RunnerUpdateState.Stop));
        }

        public override void Error(string msg)
        {
            CurStatus = Status.Stop;
            
            foreach (CmdRunner runner in mCodeRunners)
            {
                runner.Stop();
            }
            CSharp.Runner.FireUpdate(new RunnerUpdateState(RunnerUpdateState.Error, msg));
        }

        public override void Step()
        {
            //fix bug: mCodeRunners can be modified in loop. If runner finishes running, it is removed from the list
            for (int i = mCodeRunners.Count - 1; i >= 0; i--)
            {
                mCodeRunners[i].Step();
            }
        }

        public List<string> GetCallStack()
        {
            if (RunMode != Mode.Step || mCodeRunners.Count == 0)
                return null;

            List<string> callstack = mCodeRunners[0].GetCallStack();
            for (int i = 1; i < mCodeRunners.Count; i++)
            {
                callstack.Add("");
                callstack.Concat(mCodeRunners[i].GetCallStack());
            }
            return callstack;
        }
    }
}