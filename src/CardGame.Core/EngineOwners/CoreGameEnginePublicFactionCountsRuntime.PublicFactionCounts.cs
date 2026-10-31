#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePublicFactionCountsRuntime
{
    internal string? GetPublicEffectiveFactionId(CharacterState player) => _coreModeAllegianceOperations.ModeAllegiancePolicy.IsPublicFactionVisible(new(player.GeneralRevealed, player.FactionRevealed)) ? _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(player) : null;
    internal int GetPublicLivingFactionCount() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(GetPublicEffectiveFactionId).OfType<string>().Distinct(StringComparer.Ordinal).Count();
}
