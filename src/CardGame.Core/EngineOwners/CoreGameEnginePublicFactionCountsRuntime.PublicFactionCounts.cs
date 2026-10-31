#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePublicFactionCountsRuntime
{
    internal string? GetPublicEffectiveFactionId(CharacterState player) => _coreModeAllegianceOperations.ModeAllegiancePolicy.IsPublicFactionVisible(new(player.GeneralRevealed, player.FactionRevealed)) ? _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(player) : null;
    internal int GetPublicLivingFactionCount() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(GetPublicEffectiveFactionId).OfType<string>().Distinct(StringComparer.Ordinal).Count();
}
