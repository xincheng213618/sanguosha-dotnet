#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramWindowReturnQueries(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries> getCoreDamageCursorRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries> getCharacterYanPuOlRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangDyingExitEntries> getCoreGameEngineWushuangDyingExitRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuanYuHappyGodEntries> getCharacterGuanYuHappyGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheYongzhanEntries> getCoreGameEngineShanheYongzhanRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries> getCharacterBeiMiHuOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterRuanHuiOlEntries> getCharacterRuanHuiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries> getCoreGameEnginePhaseHandSeizureRuntime) : IProgramWindowReturnQueries
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
