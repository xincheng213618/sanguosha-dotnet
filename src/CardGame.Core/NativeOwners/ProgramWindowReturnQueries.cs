#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramWindowReturnQueries(EngineStates.CoreActualTurnProgressState actualTurnProgressState, Func<RuntimeEntries.CoreDamageCursorRuntimeEntries> getCoreDamageCursorRuntimeRuntime, Func<RuntimeEntries.CharacterYanPuOlEntries> getCharacterYanPuOlRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineWushuangDyingExitEntries> getCoreGameEngineWushuangDyingExitRuntime, Func<RuntimeEntries.CharacterGuanYuHappyGodEntries> getCharacterGuanYuHappyGodRuntime, Func<RuntimeEntries.CoreGameEngineShanheYongzhanEntries> getCoreGameEngineShanheYongzhanRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CharacterBeiMiHuOlEntries> getCharacterBeiMiHuOlRuntime, Func<RuntimeEntries.CharacterRuanHuiOlEntries> getCharacterRuanHuiOlRuntime, Func<RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries> getCoreGameEnginePhaseHandSeizureRuntime) : IProgramWindowReturnQueries
{
    public TurnPhase Phase => actualTurnProgressState.ReferenceToPhase;
    public DamageTriggerWindowFrame? ActiveDamageTrigger => getCoreDamageCursorRuntimeRuntime().ActiveDamageTrigger;

    public bool IsCurrentYanPuDyingExitEdge() => getCharacterYanPuOlRuntime().IsYanPuDyingExitEdge(getFrameStore().Count - 1);
    public bool IsWushuangDyingExitReturn(ProgramSkillFrame frame) => getCoreGameEngineWushuangDyingExitRuntime().IsWushuangDyingExitReturn(frame);
    public bool IsHappyGuanYuDyingExitReturn(ProgramSkillFrame frame) => getCharacterGuanYuHappyGodRuntime().IsHappyGuanYuDyingExitReturn(frame);
    public bool IsShanheYongzhanDyingExitReturn(ProgramSkillFrame frame) => getCoreGameEngineShanheYongzhanRuntime().IsShanheYongzhanDyingExitReturn(frame);
    public bool MountObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate) => RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame, candidate);
    public SkillProgramTrigger GetProgramTrigger(ProgramSkillFrame frame) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTrigger(frame);
    public bool ValidBeiMiHuPreludeLifecycleWindow(ProgramLifecycleTriggerWindowFrame frame) => getCharacterBeiMiHuOlRuntime().ValidBeiMiHuPreludeLifecycleWindow(frame);
    public bool IsMingchaNormalDrawReplacement(ProgramSkillFrame frame) => getCharacterRuanHuiOlRuntime().IsMingchaNormalDrawReplacement(frame);
    public bool IsPhaseHandDebtResolver(ProgramTriggerCandidate candidate) => getCoreGameEnginePhaseHandSeizureRuntime().IsPhaseHandDebtResolver(candidate);
}
