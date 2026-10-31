#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterXiahouJieAnniversaryKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICharacterXiahouJieAnniversaryCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SameNameHandObserverSuffix(int index);
}

internal interface ICharacterXiahouJieAnniversaryCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterXiahouJieAnniversaryCorePrepPoisonOutcomesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome ApplyProgramHpLoss(long frameId, string skillId, int targetSeat, int amount, global::System.Action<int, int, int>? afterPayment = default);
}

internal interface ICharacterXiahouJieAnniversaryCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICharacterXiahouJieAnniversaryCoreProgramNativeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterXiahouJieAnniversaryCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterXiahouJieAnniversaryCharacterXiahouJieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AnniversaryXiahouJieFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICharacterXiahouJieAnniversaryCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinalizeEndTurn(global::CardGame.Core.CharacterState previous);
}

internal interface ICharacterXiahouJieAnniversaryContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterXiahouJieAnniversaryResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterXiahouJieAnniversaryCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterXiahouJieAnniversaryCoreProgramContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramChildWindows(long frameId, global::CardGame.Core.PostEventContinuation continuation);
}

internal interface ICharacterXiahouJieAnniversaryCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
    void ChangeProgramMaximumHp(global::CardGame.Core.ProgramSkillFrame frame, int amount, string? sourceSkillId = default);
}

internal interface ICharacterXiahouJieAnniversaryCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearActualDiscardRecoveryPhase();
}

internal interface ICharacterXiahouJieAnniversaryCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterXiahouJieAnniversaryCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterXiahouJieAnniversaryCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterXiahouJieAnniversaryCoreNativePlayerDeathOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginPlayerDeath(long parentFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer, global::CardGame.Core.GameEngine.IDamageAttempt? attack, global::CardGame.Core.DyingFrame? dying, long? causingProgramSkillFrameId = default, long? causingCardDamageReplacementId = default);
}

internal interface ICharacterXiahouJieAnniversaryCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActualAfterTurnEndedParent(global::CardGame.Core.DeferredTurnEndFrame parent);
    global::CardGame.Core.ProgramTriggerCandidate AfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent);
}

internal interface ICharacterXiahouJieAnniversaryCoreHealthQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? UnambiguousHp(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterXiahouJieAnniversaryCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}
