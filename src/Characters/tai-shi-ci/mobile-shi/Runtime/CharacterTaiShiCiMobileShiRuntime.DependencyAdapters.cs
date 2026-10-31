#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ShiTaiShiCiRuntime))]
internal sealed class CharacterTaiShiCiMobileShiShiTaiShiCiAdapter(global::System.Func<global::CardGame.Core.ShiTaiShiCiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterTaiShiCiMobileShiShiTaiShiCiOperations
{
    private readonly global::System.Func<global::CardGame.Core.ShiTaiShiCiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ShiTaiShiCiRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterTaiShiCiMobileShiCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterTaiShiCiMobileShiCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}
