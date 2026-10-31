#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeHealthRecoveryServicesRuntime
{
    int INativeHealthRecoveryFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    ResolutionFrame INativeHealthRecoveryFacts.FrameAt(int index) => _resolutionStackOperations._resolutionStack[index];
    int INativeHealthRecoveryFacts.FrameIndex(long id) => _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
    int INativeHealthRecoveryFacts.FrameLastIndex(long id) => _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == id);
    IEnumerable<IGameEvent> INativeHealthRecoveryFacts.CompleteProgramEventHistory() => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory();
    int INativeHealthRecoveryFacts.ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    bool INativeHealthRecoveryProtocols.HulaoActivityFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreHulaoActivityProgramsOperations.HulaoActivityFrameRidesOn(child, parent);
    bool INativeHealthRecoveryProtocols.RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    void INativeHealthRecoveryProtocols.ValidateHulaoPeachUse(CardUseFrame use) => _coreHulaoActivityProgramsOperations.ValidateHulaoPeachUse(use);
    bool INativeHealthRecoveryProtocols.TypedRecoveryApplicationBoundsMatch(RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => _coreTypedRecoveryDamageCursorOperations.TypedRecoveryApplicationBoundsMatch(recovery, history);
    bool INativeHealthRecoveryProtocols.TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child) => _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child);
    bool INativeHealthRecoveryProtocols.DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window) => _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(root, window);
    bool INativeHealthRecoveryProtocols.ProgramRecoveryReplacementReceiptMatches(ProgramSkillFrame producer, RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(producer, recovery, history);
    bool INativeHealthRecoveryProtocols.TypedRecoveryHpReturnMatches(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => _coreTypedRecoveryDamageCursorOperations.TypedRecoveryHpReturnMatches(recovery, hp, history);
    bool INativeHealthRecoveryProtocols.ProgramRecoveryReceiptMatches(ProgramSkillFrame producer, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(producer, hp, history);
    SkillProgramEffect? INativeHealthRecoveryProtocols.TypedRecoveryPausedEffect(ProgramSkillFrame producer) => _coreTypedRecoveryDamageCursorOperations.TypedRecoveryPausedEffect(producer);
}
