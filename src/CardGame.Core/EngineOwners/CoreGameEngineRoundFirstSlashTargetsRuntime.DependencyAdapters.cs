#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreRoundFirstSlashTargetsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRoundFirstSlashTargetsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreRoundFirstSlashTargetsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRoundFirstSlashTargetsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime))]
internal sealed class CoreRoundFirstSlashTargetsCoreSlashUseOrdinalsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRoundFirstSlashTargetsCoreSlashUseOrdinalsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind SlashUseKindAtPrefix(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent fact, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int end, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent? current) => this._targetFactory().SlashUseKindAtPrefix(fact, history, end, current);
    public int CountSlashUsePrefix(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int start, int end, int expectedActorSeat, bool elemental, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent? current) => this._targetFactory().CountSlashUsePrefix(history, start, end, expectedActorSeat, elemental, current);
}
