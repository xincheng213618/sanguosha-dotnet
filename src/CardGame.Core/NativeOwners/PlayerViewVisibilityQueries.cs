#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct PlayerViewVisibilityQueries(global::System.Func<global::CardGame.Core.ModeRuleSet> getModeRuleSet, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, global::System.Func<bool> getBoolean, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries> getCharacterLuSuMobileShiRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramShanheSourceZhaoranEntries> getCoreProgramShanheSourceZhaoranRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineScenarioGodFactionsEntries> getCoreGameEngineScenarioGodFactionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeFactionChangesEntries> getCoreGameEngineNativeFactionChangesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries> getCoreGameEnginePrivateGeneralLibrariesRuntime, global::System.Func<global::CardGame.Core.MatchModeRuntime> getMatchModeRuntime) : IPlayerViewVisibilityQueries
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
