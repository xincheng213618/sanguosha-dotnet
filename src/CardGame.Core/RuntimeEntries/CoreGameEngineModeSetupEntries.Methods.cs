#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineModeSetupEntries
{
    internal void ApplyModeSelectedGeneral(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition selected) => _resolve().ApplyModeSelectedGeneral(player, selected);
    internal void ApplyModeSetupPlan(global::CardGame.Core.ModeSetupPlan plan) => _resolve().ApplyModeSetupPlan(plan);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ModeSeatSetupFacts> CaptureModeSetupSeats() => _resolve().CaptureModeSetupSeats();
    internal global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.GeneralDefinition General, string CandidateId)> GetModeGeneralOffers(global::CardGame.Core.CharacterState player, bool fullRoster) => _resolve().GetModeGeneralOffers(player, fullRoster);
    internal global::CardGame.Core.IMatchModeSetupFeature ModeSetup { get => _resolve().ModeSetup; }
}
