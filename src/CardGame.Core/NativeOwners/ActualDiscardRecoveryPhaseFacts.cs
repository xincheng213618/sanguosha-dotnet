#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActualDiscardRecoveryPhaseFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<ModeRuleSet> getModeRuleSet, Func<RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries> getCharacterZhongHuiAnniversaryGodRuntime) : IActualDiscardRecoveryPhaseFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public bool ModeTracksActualDiscardRecoveryPhases => getModeRuleSet().RuntimeRequirements.TracksActualDiscardRecoveryPhases;
    public bool HasLishiCapability => getCharacterZhongHuiAnniversaryGodRuntime().HasLishiCapability;
}
