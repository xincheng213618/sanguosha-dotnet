#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawInvoiceFacts(global::System.Func<global::CardGame.Core.ModeRuleSet> getModeRuleSet, global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPangJiOlEntries> getCharacterPangJiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries> getCharacterMiHengLeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries> getCharacterZhouFeiLeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTaoQianOlEntries> getCharacterTaoQianOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries> getCharacterZhaoZhongOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHellKingProgramsEntries> getCoreHellKingProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries> getCharacterLiuZhangOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries> getCharacterFeiYiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTangTangOlEntries> getCharacterTangTangOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYuanJiOlEntries> getCharacterYuanJiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangCanOlEntries> getCharacterWangCanOlRuntime) : INativeDrawInvoiceFacts
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
    public bool HasHellZhiwangHolder => getCoreHellKingProgramsRuntime().HasHellZhiwangHolder;
    public bool HasFengweiCapability => getCharacterLiuZhangOlRuntime().HasFengweiCapability;
    public bool HasFeiYiHezhong => getCharacterFeiYiOlRuntime().HasFeiYiHezhong;
    public bool HasTangTangZhiyi => getCharacterTangTangOlRuntime().HasTangTangZhiyi;
    public bool HasYuanJiCapability => getCharacterYuanJiOlRuntime().HasYuanJiCapability;
    public bool HasSanwenCapability => getCharacterWangCanOlRuntime().HasSanwenCapability;
}
