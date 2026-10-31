#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawInvoiceFacts(Func<ModeRuleSet> getModeRuleSet, EngineStates.CoreMatchSetupState matchSetupState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<RuntimeEntries.CharacterPangJiOlEntries> getCharacterPangJiOlRuntime, Func<RuntimeEntries.CharacterMiHengLeEntries> getCharacterMiHengLeRuntime, Func<RuntimeEntries.CharacterZhouFeiLeEntries> getCharacterZhouFeiLeRuntime, Func<RuntimeEntries.CharacterTaoQianOlEntries> getCharacterTaoQianOlRuntime, Func<RuntimeEntries.CharacterZhaoZhongOlEntries> getCharacterZhaoZhongOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreHellKingProgramsRuntime>> getCoreHellKingProgramsRuntime, Func<RuntimeEntries.CharacterLiuZhangOlEntries> getCharacterLiuZhangOlRuntime, Func<RuntimeEntries.CharacterFeiYiOlEntries> getCharacterFeiYiOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterTangTangOlRuntime>> getCharacterTangTangOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYuanJiOlRuntime>> getCharacterYuanJiOlRuntime, Func<RuntimeEntries.CharacterWangCanOlEntries> getCharacterWangCanOlRuntime) : INativeDrawInvoiceFacts
{
    public bool ModeTracksInvocations => getModeRuleSet().RuntimeRequirements.TracksNativeDrawInvocations;
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;

    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool HasPangJiCapability => getCharacterPangJiOlRuntime().HasPangJiCapability;
    public bool HasLeMiHengJiguCapability => getCharacterMiHengLeRuntime().HasLeMiHengJiguCapability;
    public bool HasLeZhouFeiLingkongCapability => getCharacterZhouFeiLeRuntime().HasLeZhouFeiLingkongCapability;
    public bool HasTaoQianCapability => getCharacterTaoQianOlRuntime().HasTaoQianCapability;
    public bool HasZhaoZhongCapability => getCharacterZhaoZhongOlRuntime().HasZhaoZhongCapability;
    public bool HasHellZhiwangHolder => getCoreHellKingProgramsRuntime().Value.HasHellZhiwangHolder;
    public bool HasFengweiCapability => getCharacterLiuZhangOlRuntime().HasFengweiCapability;
    public bool HasFeiYiHezhong => getCharacterFeiYiOlRuntime().HasFeiYiHezhong;
    public bool HasTangTangZhiyi => getCharacterTangTangOlRuntime().Value.HasTangTangZhiyi;
    public bool HasYuanJiCapability => getCharacterYuanJiOlRuntime().Value.HasYuanJiCapability;
    public bool HasSanwenCapability => getCharacterWangCanOlRuntime().HasSanwenCapability;
}
