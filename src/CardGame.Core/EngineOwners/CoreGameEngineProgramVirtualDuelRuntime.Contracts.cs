#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramVirtualDuelCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICoreProgramVirtualDuelCharacterLuZhiMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMouLzDirectDesignation(long producerId);
}

internal interface ICoreProgramVirtualDuelCoreCurrentTurnUseKindsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCurrentTurnUseKinds(long frameId, int actorSeat, global::CardGame.Core.CardActionContext? action, global::CardGame.Core.CardKind effectiveKind, bool nullificationUse, int? programInstructionIndex = default);
}

internal interface ICoreProgramVirtualDuelCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreProgramVirtualDuelCharacterYangYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsJuanxiaDuelOrigin(global::CardGame.Core.ProgramSkillFrame frame, out int targetSeat);
}

internal interface ICoreProgramVirtualDuelProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreProgramVirtualDuelCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreProgramVirtualDuelCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreProgramVirtualDuelCoreWholeActualTurnUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksWholeActualTurnUseOrdinal { get; }
}

internal interface ICoreProgramVirtualDuelCoreNativeDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, bool keepCurrentRequest = false);
}

internal interface ICoreProgramVirtualDuelKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    global::CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreProgramVirtualDuelCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }
}

internal interface ICoreProgramVirtualDuelContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramVirtualDuelResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramVirtualDuelCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramVirtualDuelCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreProgramVirtualDuelCoreCurrentRoundActualUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CurrentUseKindDeduplicationHistory();
}
