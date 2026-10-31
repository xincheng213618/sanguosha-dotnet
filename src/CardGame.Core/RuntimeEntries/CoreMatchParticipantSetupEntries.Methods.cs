#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchParticipantSetupEntries
{
    internal void ApplyGeneralSelection(CharacterState player, GeneralDefinition general) => _resolve().ApplyGeneralSelection(player, general);
    internal void ApplyGodFactionSelection(CharacterState player, string factionId) => _resolve().ApplyGodFactionSelection(player, factionId);
    internal void AssignAutomaticGodFactions() => _resolve().AssignAutomaticGodFactions();
    internal void CompleteSetup() => _resolve().CompleteSetup();
    internal void DealInitialHands() => _resolve().DealInitialHands();
    internal void FinalizeSetup() => _resolve().FinalizeSetup();
    internal IReadOnlyList<(GeneralDefinition General, string CandidateId)> GetAvailableGeneralOffers(CharacterState player, bool fullRoster = false) => _resolve().GetAvailableGeneralOffers(player, fullRoster);
    internal int GetStartingSeat() => _resolve().GetStartingSeat();
    internal Role GetTeamRole(string teamId) => _resolve().GetTeamRole(teamId);
    internal void RequestHumanGeneralSelection(CharacterState player, IReadOnlyList<GeneralDefinition> candidates, IReadOnlyDictionary<string, string> candidateIds, bool fullRoster = false) => _resolve().RequestHumanGeneralSelection(player, candidates, candidateIds, fullRoster);
    internal void RequestHumanGodFactionSelection(CharacterState player) => _resolve().RequestHumanGodFactionSelection(player);
    internal void RunOneSetupStep() => _resolve().RunOneSetupStep();
    internal void SetupDeck() => _resolve().SetupDeck();
    internal void SetupPlayers() => _resolve().SetupPlayers();
}
