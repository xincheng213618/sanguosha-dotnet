#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchParticipantSetupEntries
{
    internal void ApplyGeneralSelection(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition general) => _resolve().ApplyGeneralSelection(player, general);
    internal void ApplyGodFactionSelection(global::CardGame.Core.CharacterState player, string factionId) => _resolve().ApplyGodFactionSelection(player, factionId);
    internal void AssignAutomaticGodFactions() => _resolve().AssignAutomaticGodFactions();
    internal void CompleteSetup() => _resolve().CompleteSetup();
    internal void DealInitialHands() => _resolve().DealInitialHands();
    internal void FinalizeSetup() => _resolve().FinalizeSetup();
    internal global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.GeneralDefinition General, string CandidateId)> GetAvailableGeneralOffers(global::CardGame.Core.CharacterState player, bool fullRoster = false) => _resolve().GetAvailableGeneralOffers(player, fullRoster);
    internal int GetStartingSeat() => _resolve().GetStartingSeat();
    internal global::CardGame.Core.Role GetTeamRole(string teamId) => _resolve().GetTeamRole(teamId);
    internal void RequestHumanGeneralSelection(global::CardGame.Core.CharacterState player, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GeneralDefinition> candidates, global::System.Collections.Generic.IReadOnlyDictionary<string, string> candidateIds, bool fullRoster = false) => _resolve().RequestHumanGeneralSelection(player, candidates, candidateIds, fullRoster);
    internal void RequestHumanGodFactionSelection(global::CardGame.Core.CharacterState player) => _resolve().RequestHumanGodFactionSelection(player);
    internal void RunOneSetupStep() => _resolve().RunOneSetupStep();
    internal void SetupDeck() => _resolve().SetupDeck();
    internal void SetupPlayers() => _resolve().SetupPlayers();
}
