#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhongYaoSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhongYaoMobileEntries> getCharacterZhongYaoMobileRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries> getCoreGameEngineCardActionsRuntime) : IMobileZhongYaoSelectionQueries
{
    public IReadOnlyList<int> EligibleTargets(int ownerSeat) => getCharacterZhongYaoMobileRuntime().MobileZhongYaoZuodingTargets(ownerSeat, getFrameStore().LastOrDefault() switch
    {
        ProgramSkillFrame frame => frame.WindowContext,
        ProgramCardTriggerWindowFrame window when window.CandidateIndex < window.Candidates.Count => getCoreGameEngineCardActionsRuntime().CreateCardActionProgramContext2(window, window.Candidates[window.CandidateIndex]),
        _ => null
    });
}
