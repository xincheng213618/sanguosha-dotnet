#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTurnWindowIssuedCandidates(Func<ProgramPipelineHooks> getProgramPipelineHooks, Func<RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries> getCoreGameEnginePhaseHandSeizureRuntime, EngineStates.CoreGameEnginePhaseNamePredictionState gameEnginePhaseNamePredictionState, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangShiPingOlRuntime>> getCharacterZhangShiPingOlRuntime, Func<RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries> getCoreGameEnginePhaseNamePredictionRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineZhuijiProgramsRuntime>> getCoreGameEngineZhuijiProgramsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuZhiWeiOlRuntime>> getCharacterLuZhiWeiOlRuntime, Func<RuntimeEntries.CharacterLvQianOlEntries> getCharacterLvQianOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWangKuangOlRuntime>> getCharacterWangKuangOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiuBaOlRuntime>> getCharacterLiuBaOlRuntime, Func<RuntimeEntries.CharacterCaoHongMouEntries> getCharacterCaoHongMouRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangXunAnniversaryRuntime>> getCharacterZhangXunAnniversaryRuntime) : IProgramTurnWindowIssuedCandidates
{
    public bool HasUnissuedTurnStartObligation(int ownerSeat) => getProgramPipelineHooks().HasUnissuedTurnStartObligation(ownerSeat);
    public IEnumerable<ProgramTriggerCandidate> PhaseHandDebt(CharacterState owner, SkillProgramTriggerWindow window, int occurrence) => getCoreGameEnginePhaseHandSeizureRuntime().IssuedPhaseHandDebtCandidates(owner, window, occurrence);
    public bool HasPhaseNamePredictions => gameEnginePhaseNamePredictionState.ReferenceToPhaseNamePredictions.Count != 0;

    public bool HasOutstandingHongjiPhase(CharacterState owner, SkillProgramTriggerWindow window) => getCharacterZhangShiPingOlRuntime().Value.HasOutstandingHongjiPhase(owner, window);
    public IReadOnlyList<ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => getCharacterZhangShiPingOlRuntime().Value.IncludeHongjiPhaseCandidates(owner, window, ordinary);
    public void AppendPhaseNamePredictionCandidates(List<ProgramTriggerCandidate> candidates) => getCoreGameEnginePhaseNamePredictionRuntime().AppendPhaseNamePredictionCandidates(candidates);
    public IEnumerable<ProgramTriggerCandidate> ZhuijiEnding(CharacterState owner) => getCoreGameEngineZhuijiProgramsRuntime().Value.IssuedZhuijiEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> QingzhongEnding(CharacterState owner) => getCharacterLuZhiWeiOlRuntime().Value.IssuedQingzhongEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> WeiluEnding(CharacterState owner) => getCharacterLvQianOlRuntime().IssuedWeiluEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> WangKuangRenxiaEnding(CharacterState owner) => getCharacterWangKuangOlRuntime().Value.IssuedWangKuangRenxiaEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> TongduPlayEnding(CharacterState owner) => getCharacterLiuBaOlRuntime().Value.IssuedTongduPlayEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> MouCaoHongPlayEnding(CharacterState owner) => getCharacterCaoHongMouRuntime().IssuedMouCaoHongPlayEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> AnniversaryZhangXunPlayEnding(CharacterState owner) => getCharacterZhangXunAnniversaryRuntime().Value.IssuedAnniversaryZhangXunPlayEndingCandidates(owner);
}
