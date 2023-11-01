/****************************************************************************

Functions for interpreting c# code for blocks.

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
using UnityEngine;

namespace UBlockly
{
    [CodeInterpreter(BlockType = "logic_cells_occupied")]
    public class Cells_Occupied_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            string colour = block.GetFieldValue("TYPE");

            string msg = (colour == "RED" ? 2 : (colour == "GREEN" ? 3 : 4)).ToString();

            DataStruct returnData = new DataStruct(false);
            returnData.BooleanValue = MessageManager.Instance.SendBoolMessage(msg, MSG_TYPE.CELL_OCCUPIED);

            yield return null;

            ReturnData(returnData);
        }
    }

    [CodeInterpreter(BlockType = "logic_if")]
    public class Controls_If_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            int n = 0;
            bool satisfyIf = false;
            do
            {
                CmdEnumerator ctor = CheckInput.TryValueReturn(block, "IF" + n, "Missing_Value_Logic_Condition");
                yield return ctor;
                DataStruct condition = ctor.Data;
                if (!condition.IsUndefined && condition.IsBoolean && condition.BooleanValue)
                {
                    yield return CSharp.Interpreter.StatementRun(block, "DO" + n);
                    satisfyIf = true;
                    break;
                }
                ++n;
            } while (block.GetInput("IF" + n) != null);
            
            if (!satisfyIf && block.GetInput("ELSE") != null)
            {
                yield return CSharp.Interpreter.StatementRun(block, "ELSE");
            }
        }
    }

    [CodeInterpreter(BlockType = "logic_ifelse")]
    public class Controls_IfElse_Cmdtor : Controls_If_Cmdtor
    {
    }

    [CodeInterpreter(BlockType = "logic_compare")]
    public class Logic_Compare_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            string op = block.GetFieldValue("OP");

            CmdEnumerator ctor = CheckInput.TryValueReturn(block, "A", new DataStruct(0), "Missing_Value_Logic_Comparison");
            yield return ctor;
            DataStruct argument0 = ctor.Data;

            ctor = CheckInput.TryValueReturn(block, "B", new DataStruct(0), "Missing_Value_Logic_Comparison");
            yield return ctor;
            DataStruct argument1 = ctor.Data;

            TypeChecker.SameType(argument0, argument1);

            DataStruct returnData = new DataStruct(false);
            try
            {
                switch (op)
                {
                    case "EQ":
                        returnData.BooleanValue = argument0 == argument1;
                        break;

                    case "NEQ":
                        returnData.BooleanValue = argument0 != argument1;
                        break;

                    case "LT":
                        if (argument0.Type != Define.EDataType.Number)
                            throw new Exception("block logic_compare's \"<\" can't compare two strings and booleans");
                        returnData.BooleanValue = argument0.NumberValue < argument1.NumberValue;
                        break;

                    case "LTE":
                        if (argument0.Type != Define.EDataType.Number)
                            throw new Exception("block logic_compare's \"<=\" can't compare two strings and booleans");
                        returnData.BooleanValue = argument0.NumberValue <= argument1.NumberValue;
                        break;

                    case "GT":
                        if (argument0.Type != Define.EDataType.Number)
                            throw new Exception("block logic_compare's \">\" can't compare two strings and booleans");
                        returnData.BooleanValue = argument0.NumberValue > argument1.NumberValue;
                        break;

                    case "GTE":
                        if (argument0.Type != Define.EDataType.Number)
                            throw new Exception("block logic_compare's \">=\" can't compare two strings and booleans");
                        returnData.BooleanValue = argument0.NumberValue >= argument1.NumberValue;
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                MessageManager.Instance.SendMessage("Incompatible_Types_Comparison", MSG_TYPE.CODE_END);
            }
            ReturnData(returnData);
        }
    }

    [CodeInterpreter(BlockType = "logic_operation")]
    public class Logic_Operation_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            string op = block.GetFieldValue("OP");

            CmdEnumerator ctor = CheckInput.TryValueReturn(block, "A", new DataStruct(false), "Missing_Value_Logic_Comparison");
            yield return ctor;
            DataStruct argument0 = ctor.Data;

            ctor = CheckInput.TryValueReturn(block, "B", new DataStruct(false), "Missing_Value_Logic_Comparison");
            yield return ctor;
            DataStruct argument1 = ctor.Data;
            
            TypeChecker.CheckBool(argument0);
            TypeChecker.SameType(argument0, argument1);
            
            DataStruct returnData = new DataStruct(false);
            switch (op)
            {
                case "AND":
                    returnData.BooleanValue = argument0.BooleanValue && argument1.BooleanValue;
                    break;
                case "OR":
                    returnData.BooleanValue = argument0.BooleanValue || argument1.BooleanValue;
                    break;
            }
            ReturnData(returnData);
        }
    }

    [CodeInterpreter(BlockType = "logic_negate")]
    public class Logic_Negate_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            CmdEnumerator ctor = CheckInput.TryValueReturn(block, "BOOL", new DataStruct(false), "Missing_Value_Logic_Negation");
            yield return ctor;
            DataStruct argument = ctor.Data;
            
            TypeChecker.CheckBool(argument);
            
            ReturnData(new DataStruct(!argument.BooleanValue));
        }
    }

    [CodeInterpreter(BlockType = "logic_boolean")]
    public class Logic_Boolean_Cmdtor : ValueCmdtor
    {
        protected override DataStruct Execute(Block block)
        {
            string op = block.GetFieldValue("BOOL");
            switch (op)
            {
                case "TRUE": return new DataStruct(true);
                case "FALSE": return new DataStruct(false);
            }
            return new DataStruct(false);
        }
    }

    [CodeInterpreter(BlockType = "logic_null")]
    public class Logic_Null_Cmdtor : ValueCmdtor
    {
        protected override DataStruct Execute(Block block)
        {
            return new DataStruct(false);
        }
    }

    [CodeInterpreter(BlockType = "logic_ternary")]
    public class Logic_Ternary_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            CmdEnumerator ctor = CheckInput.TryValueReturn(block, "IF", new DataStruct(false));
            yield return ctor;
            DataStruct condition = ctor.Data;
            
            TypeChecker.CheckBool(condition);
            
            if (condition.BooleanValue)
            {
                yield return CSharp.Interpreter.StatementRun(block, "THEN");
            }
            else
            {
                yield return CSharp.Interpreter.StatementRun(block, "ELSE");
            }
        }
    }

    [CodeInterpreter(BlockType = "logic_toggle_boolean")]
    public class Logic_Toggle_Boolean_Cmdtor : ValueCmdtor
    {
        protected override DataStruct Execute(Block block)
        {
            string toggleString = block.GetFieldValue("CHECKBOX");
            bool toggleValue = toggleString.Equals("TRUE");
            return new DataStruct(toggleValue);
        }
    }
    public class TypeChecker
    {
        public static void CheckBool(DataStruct data)
        {
            try
            {
                if (data.Type != Define.EDataType.Boolean)
                    throw new Exception("La expresión no es booleana, es de tipo: " + data.Type);
            }
            catch (Exception e)
            {
                MessageManager.Instance.SendMessage("Not_Boolean_Expression", MSG_TYPE.CODE_END);
                Debug.LogError(e.Message);
            }
        }

        public static void CheckNumber(DataStruct data)
        {
            try
            {
                if (data.Type != Define.EDataType.Number)
                    throw new Exception("Not a number: " + data.Type);
            }
            catch (Exception e)
            {
                MessageManager.Instance.SendMessage("Not_Numeric_Expression", MSG_TYPE.CODE_END);
                Debug.LogError(e.Message);
            }
        }
        public static void SameType(DataStruct first, DataStruct second)
        {
            try
            {
                if (first.Type != second.Type)
                    throw new Exception("No se pueden comparar los tipos " + first.Type + " y " + second.Type);
            }
            catch (Exception e)
            {
                MessageManager.Instance.SendMessage("Incompatible_Types_Comparison", MSG_TYPE.CODE_END);
            }
        }
    }
}
