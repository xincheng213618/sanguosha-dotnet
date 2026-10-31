#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreProgramTieredRoundConversionCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime))]
internal sealed class CoreProgramTieredRoundConversionCharacterWoLongFengChuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCharacterWoLongFengChuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> _targetFactory = targetFactory;
    public bool TryGetEquipmentSlotNamedConversionAvailability(global::CardGame.Core.CharacterState actor, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool responseUse, bool dyingUse, out bool available) => this._targetFactory().TryGetEquipmentSlotNamedConversionAvailability(actor, instance, rule, responseUse, dyingUse, out available);
    public bool TryConsumePaidEquipmentSlotNamedConversion(global::CardGame.Core.CardConversionSource source) => this._targetFactory().TryConsumePaidEquipmentSlotNamedConversion(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramTieredRoundConversionCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreProgramTieredRoundConversionCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get => this._targetFactory().ActiveFactionDefense; set => this._targetFactory().ActiveFactionDefense = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime))]
internal sealed class CoreProgramTieredRoundConversionCoreCurrentTurnUseKindsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCoreCurrentTurnUseKindsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> _targetFactory = targetFactory;
    public bool IsActualTurnLegacyVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsActualTurnLegacyVirtualUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreProgramTieredRoundConversionCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreProgramTieredRoundConversionCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreProgramTieredRoundConversionCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().IsProgramResponseCardUse(responder, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreProgramTieredRoundConversionCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed class CoreProgramTieredRoundConversionCoreTieredRoundZeroResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCoreTieredRoundZeroResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _targetFactory = targetFactory;
    public bool IsIssuedTieredRoundZeroResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action) => this._targetFactory().IsIssuedTieredRoundZeroResponse(owner, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreProgramTieredRoundConversionCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTieredRoundConversionCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}
