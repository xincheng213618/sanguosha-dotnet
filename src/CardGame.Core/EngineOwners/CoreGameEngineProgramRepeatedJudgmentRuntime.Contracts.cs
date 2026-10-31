#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramRepeatedJudgmentCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreProgramRepeatedJudgmentCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramRepeatedJudgmentKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreProgramRepeatedJudgmentCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreProgramRepeatedJudgmentCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.SkillProgramStepOutcome StartProgramJudgment(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string reason, string resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, int sourceSeat);
}

internal interface ICoreProgramRepeatedJudgmentCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreProgramRepeatedJudgmentCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreProgramRepeatedJudgmentKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreProgramRepeatedJudgmentCharacterZhangQiYingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveZhenyiJudgmentSuit(global::CardGame.Core.JudgmentFrame frame, global::CardGame.Core.Suit fallback);
}

internal interface ICoreProgramRepeatedJudgmentCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreProgramRepeatedJudgmentCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
    void ReturnRuntimeProgramMovement(long frameId);
}

internal interface ICoreProgramRepeatedJudgmentResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramRepeatedJudgmentCoreExactRepeatedJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResumeProgramExactRepeatedJudgment(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramRepeatedJudgment state);
}

internal interface ICoreProgramRepeatedJudgmentCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreProgramRepeatedJudgmentKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}
