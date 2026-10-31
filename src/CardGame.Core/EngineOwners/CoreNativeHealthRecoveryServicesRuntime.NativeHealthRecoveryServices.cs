#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeHealthRecoveryServicesRuntime
{
    int global::CardGame.Core.INativeHealthRecoveryFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    ResolutionFrame global::CardGame.Core.INativeHealthRecoveryFacts.FrameAt(int index) => _resolutionStackOperations._resolutionStack[index];
    int global::CardGame.Core.INativeHealthRecoveryFacts.FrameIndex(long id) => _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
    int global::CardGame.Core.INativeHealthRecoveryFacts.FrameLastIndex(long id) => _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == id);
    IEnumerable<IGameEvent> global::CardGame.Core.INativeHealthRecoveryFacts.CompleteProgramEventHistory() => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory();
    int global::CardGame.Core.INativeHealthRecoveryFacts.ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    bool global::CardGame.Core.INativeHealthRecoveryProtocols.HulaoActivityFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreHulaoActivityProgramsOperations.HulaoActivityFrameRidesOn(child, parent);
    bool global::CardGame.Core.INativeHealthRecoveryProtocols.RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    void global::CardGame.Core.INativeHealthRecoveryProtocols.ValidateHulaoPeachUse(CardUseFrame use) => _coreHulaoActivityProgramsOperations.ValidateHulaoPeachUse(use);
    bool global::CardGame.Core.INativeHealthRecoveryProtocols.TypedRecoveryApplicationBoundsMatch(RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => _coreTypedRecoveryDamageCursorOperations.TypedRecoveryApplicationBoundsMatch(recovery, history);
    bool global::CardGame.Core.INativeHealthRecoveryProtocols.TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child) => _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child);
    bool global::CardGame.Core.INativeHealthRecoveryProtocols.DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window) => _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(root, window);
    bool global::CardGame.Core.INativeHealthRecoveryProtocols.ProgramRecoveryReplacementReceiptMatches(ProgramSkillFrame producer, RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(producer, recovery, history);
    bool global::CardGame.Core.INativeHealthRecoveryProtocols.TypedRecoveryHpReturnMatches(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => _coreTypedRecoveryDamageCursorOperations.TypedRecoveryHpReturnMatches(recovery, hp, history);
    bool global::CardGame.Core.INativeHealthRecoveryProtocols.ProgramRecoveryReceiptMatches(ProgramSkillFrame producer, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(producer, hp, history);
    SkillProgramEffect? global::CardGame.Core.INativeHealthRecoveryProtocols.TypedRecoveryPausedEffect(ProgramSkillFrame producer) => _coreTypedRecoveryDamageCursorOperations.TypedRecoveryPausedEffect(producer);
}
