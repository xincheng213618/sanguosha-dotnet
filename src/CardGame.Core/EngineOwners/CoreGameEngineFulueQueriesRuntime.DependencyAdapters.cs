#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime))]
internal sealed class CoreFulueQueriesCharacterPeiYuanShaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFulueQueriesCharacterPeiYuanShaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime> _targetFactory = targetFactory;
    public bool HasIssuedFulueRequest(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasIssuedFulueRequest(f);
    public bool ValidFulueCounterInvoice(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.FulueCounterInvoice r) => this._targetFactory().ValidFulueCounterInvoice(use, r);
}
