#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeHealthRecoveryProtocols(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreHulaoActivityProgramsRuntime>> getCoreHulaoActivityProgramsRuntime, Func<RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries> getCoreGameEngineRecoveryReplacementDamageRidesRuntime, Func<RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries> getCoreGameEngineTypedRecoveryDamageCursorRuntime, Func<RuntimeEntries.CoreDamageObserverCursorQueriesEntries> getCoreDamageObserverCursorQueriesRuntime, Func<RuntimeEntries.CoreGameEngineProgramRecoveryReceiptsEntries> getCoreGameEngineProgramRecoveryReceiptsRuntime) : INativeHealthRecoveryProtocols
{
    public bool HulaoActivityFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => getCoreHulaoActivityProgramsRuntime().Value.HulaoActivityFrameRidesOn(child, parent);
    public bool RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => getCoreGameEngineRecoveryReplacementDamageRidesRuntime().RecoveryReplacementFrameRidesOn(child, parent);
    public void ValidateHulaoPeachUse(CardUseFrame use) => getCoreHulaoActivityProgramsRuntime().Value.ValidateHulaoPeachUse(use);
    public bool TypedRecoveryApplicationBoundsMatch(RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => getCoreGameEngineTypedRecoveryDamageCursorRuntime().TypedRecoveryApplicationBoundsMatch(recovery, history);
    public bool TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child) => getCoreDamageObserverCursorQueriesRuntime().TypedDamagePostEventEdge(parent, child);
    public bool DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window) => getCoreDamageObserverCursorQueriesRuntime().DamageProgramMatchesWindow(root, window);
    public bool ProgramRecoveryReplacementReceiptMatches(ProgramSkillFrame producer, RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => getCoreGameEngineProgramRecoveryReceiptsRuntime().ProgramRecoveryReplacementReceiptMatches(producer, recovery, history);
    public bool TypedRecoveryHpReturnMatches(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => getCoreGameEngineTypedRecoveryDamageCursorRuntime().TypedRecoveryHpReturnMatches(recovery, hp, history);
    public bool ProgramRecoveryReceiptMatches(ProgramSkillFrame producer, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => getCoreGameEngineProgramRecoveryReceiptsRuntime().ProgramRecoveryReceiptMatches(producer, hp, history);
    public SkillProgramEffect? TypedRecoveryPausedEffect(ProgramSkillFrame producer) => getCoreGameEngineTypedRecoveryDamageCursorRuntime().TypedRecoveryPausedEffect(producer);
}
