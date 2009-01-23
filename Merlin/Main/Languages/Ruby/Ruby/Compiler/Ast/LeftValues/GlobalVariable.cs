/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the  Microsoft Public License, please send an email to 
 * ironruby@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 *
 *
 * ***************************************************************************/

using System.Diagnostics;
using System.Dynamic;
using Microsoft.Scripting;
using MSA = System.Linq.Expressions;

namespace IronRuby.Compiler.Ast {
    public partial class GlobalVariable : Variable {

        public string/*!*/ FullName {
            get { return "$" + Name; }
        }

        public GlobalVariable(string/*!*/ name, SourceSpan location)
            : base(name, location) {
            Debug.Assert(name.ToString() == "$" || !name.ToString().StartsWith("$"));
        }

        internal override MSA.Expression/*!*/ TransformReadVariable(AstGenerator/*!*/ gen, bool tryRead) {
            return Methods.GetGlobalVariable.OpCall(gen.CurrentScopeVariable, TransformName(gen));
        }

        internal override MSA.Expression/*!*/ TransformWriteVariable(AstGenerator/*!*/ gen, MSA.Expression/*!*/ rightValue) {
            return Methods.SetGlobalVariable.OpCall(AstFactory.Box(rightValue), gen.CurrentScopeVariable, TransformName(gen));
        }

        internal override MSA.Expression TransformDefinedCondition(AstGenerator/*!*/ gen) {
            return Methods.IsDefinedGlobalVariable.OpCall(gen.CurrentScopeVariable, TransformName(gen));
        }

        internal override string/*!*/ GetNodeName(AstGenerator/*!*/ gen) {
            return "global-variable";
        }
    }
}