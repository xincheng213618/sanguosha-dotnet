#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouLidanRuntime))]
internal sealed class CharacterChengYuMouOlMouLidanAdapter(global::System.Func<global::CardGame.Core.MouLidanRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouOlMouLidanOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouLidanRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouLidanRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouDanchiRuntime))]
internal sealed class CharacterChengYuMouOlMouDanchiAdapter(global::System.Func<global::CardGame.Core.MouDanchiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouOlMouDanchiOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouDanchiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouDanchiRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterChengYuMouOlCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouOlCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CharacterChengYuMouOlCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouOlCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
