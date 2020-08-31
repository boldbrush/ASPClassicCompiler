using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Scripting.Runtime;

using System.Dynamic;
#if USE35
using Microsoft.Scripting.Ast;
#else
#endif
using System.Linq.Expressions;

namespace Dlrsoft.VBScript.Runtime
{
    // This class represents Sympl modules or globals scopes.  We derive from
    // DynamicObject for an easy IDynamicMetaObjectProvider implementation, and
    // just hold onto the DLR internal scope object.  Languages typically have
    // their own scope so that 1) it can flow around an a dynamic object and 2)
    // to dope in any language-specific behaviors, such as case-INsensitivity.
    //
    public sealed class VBScriptDlrScope : DynamicObject
    {
        private readonly Microsoft.Scripting.Hosting.ScriptScope _scope;
        // private readonly Scope _scope;

        // public VBScriptDlrScope(Scope scope)
        public VBScriptDlrScope(Microsoft.Scripting.Hosting.ScriptScope scope)
        {
            _scope = scope;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            
            // (new Microsoft.Scripting.ScopeStorage).GetScopeVariableIgnoreCase()
            // Microsoft.Scripting.Runtime.
            // Microsoft.Scripting.Hosting.ScriptScope
            // return _scope.TryGetVariable(SymbolTable.StringToCaseInsensitiveId(binder.Name),
            return _scope.TryGetVariable(binder.Name.ToLower(), out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // DynamicObjectHelpers.SetMember(_scope, binder.Name, value); // can try this for SymbolTable replacement
            // _scope.SetVariable(SymbolTable.StringToId(binder.Name), value);
            _scope.SetVariable(binder.Name.ToLower(), value);
            return true;
        }
    }
}
