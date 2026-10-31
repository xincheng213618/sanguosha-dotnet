#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTurnWindowIssuedCandidates(Func<ProgramPipelineHooks> getProgramPipelineHooks, Func<RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries> getCoreGameEnginePhaseHandSeizureRuntime, EngineStates.CoreGameEnginePhaseNamePredictionState gameEnginePhaseNamePredictionState, Func<RuntimeEntries.CharacterZhangShiPingOlEntries> getCharacterZhangShiPingOlRuntime, Func<RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries> getCoreGameEnginePhaseNamePredictionRuntime, Func<RuntimeEntries.CoreGameEngineZhuijiProgramsEntries> getCoreGameEngineZhuijiProgramsRuntime, Func<RuntimeEntries.CharacterLuZhiWeiOlEntries> getCharacterLuZhiWeiOlRuntime, Func<RuntimeEntries.CharacterLvQianOlEntries> getCharacterLvQianOlRuntime, Func<RuntimeEntries.CharacterWangKuangOlEntries> getCharacterWangKuangOlRuntime, Func<RuntimeEntries.CharacterLiuBaOlEntries> getCharacterLiuBaOlRuntime, Func<RuntimeEntries.CharacterCaoHongMouEntries> getCharacterCaoHongMouRuntime, Func<RuntimeEntries.CharacterZhangXunAnniversaryEntries> getCharacterZhangXunAnniversaryRuntime) : IProgramTurnWindowIssuedCandidates
{
    public bool HasUnissuedTurnStartObligation(int ownerSeat) => getProgramPipelineHooks().HasUnissuedTurnStartObligation(ownerSeat);
    public IEnumerable<ProgramTriggerCandidate> PhaseHandDebt(CharacterState owner, SkillProgramTriggerWindow window, int occurrence) => getCoreGameEnginePhaseHandSeizureRuntime().IssuedPhaseHandDebtCandidates(owner, window, occurrence);
    public bool HasPhaseNamePredictions => gameEnginePhaseNamePredictionState.ReferenceToPhaseNamePredictions.Count != 0;

    public bool HasOutstandingHongjiPhase(CharacterState owner, SkillProgramTriggerWindow window) => getCharacterZhangShiPingOlRuntime().HasOutstandingHongjiPhase(owner, window);
    public IReadOnlyList<ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => getCharacterZhangShiPingOlRuntime().IncludeHongjiPhaseCandidates(owner, window, ordinary);
    public void AppendPhaseNamePredictionCandidates(List<ProgramTriggerCandidate> candidates) => getCoreGameEnginePhaseNamePredictionRuntime().AppendPhaseNamePredictionCandidates(candidates);
    public IEnumerable<ProgramTriggerCandidate> ZhuijiEnding(CharacterState owner) => getCoreGameEngineZhuijiProgramsRuntime().IssuedZhuijiEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> QingzhongEnding(CharacterState owner) => getCharacterLuZhiWeiOlRuntime().IssuedQingzhongEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> WeiluEnding(CharacterState owner) => getCharacterLvQianOlRuntime().IssuedWeiluEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> WangKuangRenxiaEnding(CharacterState owner) => getCharacterWangKuangOlRuntime().IssuedWangKuangRenxiaEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> TongduPlayEnding(CharacterState owner) => getCharacterLiuBaOlRuntime().IssuedTongduPlayEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> MouCaoHongPlayEnding(CharacterState owner) => getCharacterCaoHongMouRuntime().IssuedMouCaoHongPlayEndingCandidates(owner);
    public IEnumerable<ProgramTriggerCandidate> AnniversaryZhangXunPlayEnding(CharacterState owner) => getCharacterZhangXunAnniversaryRuntime().IssuedAnniversaryZhangXunPlayEndingCandidates(owner);
}
