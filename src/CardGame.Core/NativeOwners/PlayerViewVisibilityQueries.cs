#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct PlayerViewVisibilityQueries(Func<ModeRuleSet> getModeRuleSet, Func<RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, Func<bool> getBoolean, Func<RuntimeEntries.CharacterLuSuMobileShiEntries> getCharacterLuSuMobileShiRuntime, Func<RuntimeEntries.CoreProgramShanheSourceZhaoranEntries> getCoreProgramShanheSourceZhaoranRuntime, Func<RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, Func<RuntimeEntries.CoreGameEngineScenarioGodFactionsEntries> getCoreGameEngineScenarioGodFactionsRuntime, Func<RuntimeEntries.CoreGameEngineNativeFactionChangesEntries> getCoreGameEngineNativeFactionChangesRuntime, Func<RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries> getCoreGameEnginePrivateGeneralLibrariesRuntime, Func<MatchModeRuntime> getMatchModeRuntime) : IPlayerViewVisibilityQueries
{
    public ModeVisibilityRules Rules => getModeRuleSet().Visibility;
    public bool SupportsGodFactionSelection => getCoreNativeDamageHandlesRuntime().SupportsGodFactionSelection;
    public bool IsWushuangMode => getBoolean();

    public bool HasShiLsHandView(int viewerSeat, int ownerSeat) => getCharacterLuSuMobileShiRuntime().HasShiLsHandView(viewerSeat, ownerSeat);
    public bool HasShanheSourceZhaoranPublicHand(int ownerSeat) => getCoreProgramShanheSourceZhaoranRuntime().HasShanheSourceZhaoranPublicHand(ownerSeat);
    public string? GetEffectiveFactionId(CharacterState player) => getCoreMatchSetupDefinitionsRuntime().GetEffectiveFactionId(player);
    public string? GetScenarioGodFaction(CharacterState player) => getCoreGameEngineScenarioGodFactionsRuntime().GetScenarioGodFaction(player);
    public bool HasNativeEffectiveFactionDeclaration(CharacterState player) => getCoreGameEngineNativeFactionChangesRuntime().HasNativeEffectiveFactionDeclaration(player);
    public string? GetPrivateGeneralLibraryFaction(CharacterState player) => getCoreGameEnginePrivateGeneralLibrariesRuntime().GetPrivateGeneralLibraryFaction(player);
    public bool IsSkillVisible(int seat, string? skillId, int viewerSeat, bool revealAll) => getMatchModeRuntime().IsSkillVisible(seat, skillId, viewerSeat, revealAll);
}
