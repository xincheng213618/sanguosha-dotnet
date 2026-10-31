#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterSharedGameEngineOlMoRumoCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterSharedGameEngineOlMoRumoCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterSharedGameEngineOlMoRumoCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidMoKuangmoEntry(global::CardGame.Core.ProgramSkillFrame f);
    bool IsPaidMoKuangmoOrigin(long frameId, global::CardGame.Core.CardConversionSource source, string hash);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? AppendMoLuBuKuangMarker(int owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? ordinary);
}

internal interface ICharacterSharedGameEngineOlMoRumoCharacterSharedGameEngineOlMoRumoProofsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.OlMoRumoEnteredEvent? OlMoRumoEntry(int ownerSeat);
    bool OlMoRumoCandidateSource(global::CardGame.Core.ProgramTriggerCandidate candidate);
    void AssertOlMoRumo(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICharacterSharedGameEngineOlMoRumoProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICharacterSharedGameEngineOlMoRumoCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterSharedGameEngineOlMoRumoCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterSharedGameEngineOlMoRumoCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterSharedGameEngineOlMoRumoCoreRoundEndingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactRoundEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICharacterSharedGameEngineOlMoRumoKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICharacterSharedGameEngineOlMoRumoCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterSharedGameEngineOlMoRumoCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterSharedGameEngineOlMoRumoCharacterDiaoChanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidMoDcQingshiEntry(global::CardGame.Core.ProgramSkillFrame f);
    bool IsPaidMoDcQingshiOrigin(long id, global::CardGame.Core.CardConversionSource source, string hash);
}

internal interface ICharacterSharedGameEngineOlMoRumoCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterSharedGameEngineOlMoRumoCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICharacterSharedGameEngineOlMoRumoCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterSharedGameEngineOlMoRumoCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidQiangangRumoEntry(global::CardGame.Core.ProgramSkillFrame f);
    bool IsPaidQiangangRumoOrigin(long frameId, global::CardGame.Core.CardConversionSource source, string gameplayHash);
}

internal interface ICharacterSharedGameEngineOlMoRumoCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICharacterSharedGameEngineOlMoRumoKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICharacterSharedGameEngineOlMoRumoResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterSharedGameEngineOlMoRumoContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterSharedGameEngineOlMoRumoCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary);
}
