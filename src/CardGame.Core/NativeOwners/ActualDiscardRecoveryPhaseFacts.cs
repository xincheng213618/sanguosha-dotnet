#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActualDiscardRecoveryPhaseFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.ModeRuleSet> getModeRuleSet, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries> getCharacterZhongHuiAnniversaryGodRuntime) : IActualDiscardRecoveryPhaseFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public bool ModeTracksActualDiscardRecoveryPhases => getModeRuleSet().RuntimeRequirements.TracksActualDiscardRecoveryPhases;
    public bool HasLishiCapability => getCharacterZhongHuiAnniversaryGodRuntime().HasLishiCapability;
}
