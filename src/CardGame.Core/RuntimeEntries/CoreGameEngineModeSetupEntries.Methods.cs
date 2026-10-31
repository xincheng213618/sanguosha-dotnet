#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineModeSetupEntries
{
    internal void ApplyModeSelectedGeneral(CharacterState player, GeneralDefinition selected) => _resolve().ApplyModeSelectedGeneral(player, selected);
    internal void ApplyModeSetupPlan(ModeSetupPlan plan) => _resolve().ApplyModeSetupPlan(plan);
    internal IReadOnlyList<ModeSeatSetupFacts> CaptureModeSetupSeats() => _resolve().CaptureModeSetupSeats();
    internal IReadOnlyList<(GeneralDefinition General, string CandidateId)> GetModeGeneralOffers(CharacterState player, bool fullRoster) => _resolve().GetModeGeneralOffers(player, fullRoster);
    internal IMatchModeSetupFeature ModeSetup { get => _resolve().ModeSetup; }
}
