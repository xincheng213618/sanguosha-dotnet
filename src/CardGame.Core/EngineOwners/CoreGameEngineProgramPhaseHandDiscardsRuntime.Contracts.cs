#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramPhaseHandDiscardsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreProgramPhaseHandDiscardsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreProgramPhaseHandDiscardsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreProgramPhaseHandDiscardsCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddThought(global::CardGame.Core.AiThoughtRecord thought);
}

internal interface ICoreProgramPhaseHandDiscardsCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreProgramPhaseHandDiscardsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreProgramPhaseHandDiscardsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreProgramPhaseHandDiscardsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreProgramPhaseHandDiscardsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramPhaseHandDiscardsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramPhaseHandDiscardsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreProgramPhaseHandDiscardsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}
