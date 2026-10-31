#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActualPhaseEndFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries> getCharacterZhongHuiAnniversaryGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSimaShiMouEntries> getCharacterSimaShiMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries> getCharacterHuCheErMouRuntime, global::System.Func<global::CardGame.Core.ModeRuleSet> getModeRuleSet, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuShiOlEntries> getCharacterLuShiOlRuntime) : IActualPhaseEndFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public bool HasLishiCapability => getCharacterZhongHuiAnniversaryGodRuntime().HasLishiCapability;
    public bool HasMouSimaShi => getCharacterSimaShiMouRuntime().HasMouSimaShi;
    public bool HasMouHuCheEr => getCharacterHuCheErMouRuntime().HasMouHuCheEr;
    public bool ModeTracksActualPhaseEnds => getModeRuleSet().RuntimeRequirements.TracksActualPhaseEnds;
    public bool HasOlLuShi => getCharacterLuShiOlRuntime().HasOlLuShi;
}
