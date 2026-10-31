#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseBoundaryFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, Func<RuntimeEntries.CoreGameEngineNeighborDiscardOpportunitiesEntries> getCoreGameEngineNeighborDiscardOpportunitiesRuntime, Func<RuntimeEntries.CoreWenheActivityProgramsEntries> getCoreWenheActivityProgramsRuntime, Func<RuntimeEntries.CharacterZhangJiaoAnniversaryGodEntries> getCharacterZhangJiaoAnniversaryGodRuntime, Func<RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime, Func<RuntimeEntries.CharacterLuZhiWeiOlEntries> getCharacterLuZhiWeiOlRuntime, Func<RuntimeEntries.CharacterLvQianOlEntries> getCharacterLvQianOlRuntime, Func<RuntimeEntries.CharacterGuoTuOlEntries> getCharacterGuoTuOlRuntime) : IProgramPhaseBoundaryFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;

    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
    public int PreviousLivingSeatFor(int owner) => getCoreGameEngineNeighborDiscardOpportunitiesRuntime().PreviousLivingSeatFor(owner);
    public bool HasWenheDeadOwnerEnding(int seat) => getCoreWenheActivityProgramsRuntime().HasWenheDeadOwnerEnding(seat);
    public bool HasAnniversaryTianjieDeadOwnerEnding(int seat) => getCharacterZhangJiaoAnniversaryGodRuntime().HasAnniversaryTianjieDeadOwnerEnding(seat);
    public bool IsWenheDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int seat) => getCoreWenheActivityProgramsRuntime().IsWenheDeadOwnerEndingCandidate(candidate, seat);
    public bool IsAnniversaryTianjieDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int seat) => getCharacterZhangJiaoAnniversaryGodRuntime().IsAnniversaryTianjieDeadOwnerEndingCandidate(candidate, seat);
    public bool UsesTreatedWounded => getCoreGameEngineTreatedWoundedRuntime().UsesTreatedWounded;

    public bool IsRuleWounded(CharacterState player) => getCoreGameEngineTreatedWoundedRuntime().IsRuleWounded(player);
    public QingzhongPlayPhaseKey? CaptureQingzhongStartingPhase(CharacterState owner) => getCharacterLuZhiWeiOlRuntime().CaptureQingzhongStartingPhase(owner);
    public WeiluPhaseKey? CaptureWeiluStartingPhase(CharacterState owner) => getCharacterLvQianOlRuntime().CaptureWeiluStartingPhase(owner);
    public ProgramSkillWindowContext QushiEndingContext(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => getCharacterGuoTuOlRuntime().CreateQushiEndingContext(frame, candidate, context);
}
