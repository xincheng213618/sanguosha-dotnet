#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActualPhaseEndFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries> getCharacterZhongHuiAnniversaryGodRuntime, Func<RuntimeEntries.CharacterSimaShiMouEntries> getCharacterSimaShiMouRuntime, Func<RuntimeEntries.CharacterHuCheErMouEntries> getCharacterHuCheErMouRuntime, Func<ModeRuleSet> getModeRuleSet, Func<RuntimeEntries.CharacterLuShiOlEntries> getCharacterLuShiOlRuntime) : IActualPhaseEndFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public bool HasLishiCapability => getCharacterZhongHuiAnniversaryGodRuntime().HasLishiCapability;
    public bool HasMouSimaShi => getCharacterSimaShiMouRuntime().HasMouSimaShi;
    public bool HasMouHuCheEr => getCharacterHuCheErMouRuntime().HasMouHuCheEr;
    public bool ModeTracksActualPhaseEnds => getModeRuleSet().RuntimeRequirements.TracksActualPhaseEnds;
    public bool HasOlLuShi => getCharacterLuShiOlRuntime().HasOlLuShi;
}
