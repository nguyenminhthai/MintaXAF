#region Copyright (c) 2000-2015 Developer Express Inc.
/*
{*******************************************************************}
{                                                                   }
{       Developer Express .NET Component Library                    }
{       eXpressApp Framework                                        }
{                                                                   }
{       Copyright (c) 2000-2015 Developer Express Inc.              }
{       ALL RIGHTS RESERVED                                         }
{                                                                   }
{   The entire contents of this file is protected by U.S. and       }
{   International Copyright Laws. Unauthorized reproduction,        }
{   reverse-engineering, and distribution of all or any portion of  }
{   the code contained in this file is strictly prohibited and may  }
{   result in severe civil and criminal penalties and will be       }
{   prosecuted to the maximum extent possible under the law.        }
{                                                                   }
{   RESTRICTIONS                                                    }
{                                                                   }
{   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           }
{   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          }
{   SECRETS OF DEVELOPER EXPRESS INC. THE REGISTERED DEVELOPER IS   }
{   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    }
{   CONTROLS AS PART OF AN EXECUTABLE PROGRAM ONLY.                 }
{                                                                   }
{   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      }
{   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        }
{   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       }
{   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  }
{   AND PERMISSION FROM DEVELOPER EXPRESS INC.                      }
{                                                                   }
{   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       }
{   ADDITIONAL RESTRICTIONS.                                        }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2000-2015 Developer Express Inc.

using System;
using System.Linq;
using DevExpress.Data.Filtering;
namespace Xpand.ExpressApp.NH.DataLayer
{
    public static class CriteriaToHqlConverterHelper
    {
        public static Boolean ConvertCustomFunctionToValue(FunctionOperator functionOperator, out Object value)
        {
            Boolean result = false;
            value = null;
            if ((functionOperator.OperatorType == FunctionOperatorType.Custom)
                    || (functionOperator.OperatorType == FunctionOperatorType.CustomNonDeterministic))
            {
                String customFunctionName = string.Empty;
                if (functionOperator.Operands[0] is OperandValue)
                {
                    OperandValue operandValue = (OperandValue)functionOperator.Operands[0];
                    if (operandValue.Value is String)
                    {
                        customFunctionName = (String)operandValue.Value;
                    }
                }
                if (!String.IsNullOrWhiteSpace(customFunctionName))
                {
                    ICustomFunctionOperator customFunctionOperator = CriteriaOperator.GetCustomFunction(customFunctionName);
                    if (customFunctionOperator != null)
                    {
                        value = customFunctionOperator.Evaluate(functionOperator.Operands);
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}
