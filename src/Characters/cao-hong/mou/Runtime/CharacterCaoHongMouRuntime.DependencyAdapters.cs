#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterCaoHongMouCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterCaoHongMouCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouCaoHongRuntime))]
internal sealed class CharacterCaoHongMouMouCaoHongAdapter(global::System.Func<global::CardGame.Core.MouCaoHongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterCaoHongMouMouCaoHongOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouCaoHongRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouCaoHongRuntime Get() => this._targetFactory();
}
