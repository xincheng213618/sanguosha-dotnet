#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GuanYuTargetSelectionQuery(EngineStates.CoreActualRoundProgressState actualRoundProgressState, Func<FrameStore> getFrameStore, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime) : IGuanYuTargetSelectionQuery
{
    public int RoundNumber => actualRoundProgressState.ReferenceToRoundNumber;
    public long? CurrentProgramFrameId => (getFrameStore().LastOrDefault() as ProgramSkillFrame)?.Id;
    public IReadOnlyList<ProgramTargetParticipantFacts> Participants => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => new ProgramTargetParticipantFacts(player.Seat, player.IsAlive, player.HasHpValue, player.Hp, player.MaxHp)).ToArray());
    public IReadOnlyList<DemonRoundFocusEvent> FocusHistory => Array.AsReadOnly(getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory().OfType<DemonRoundFocusEvent>().ToArray());
    public IReadOnlyList<DemonSupportGrantedEvent> SupportHistory => Array.AsReadOnly(getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory().OfType<DemonSupportGrantedEvent>().ToArray());
}
