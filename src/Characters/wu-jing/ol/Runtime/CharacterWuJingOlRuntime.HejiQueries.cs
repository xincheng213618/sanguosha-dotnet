#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuJingOlRuntime
{
    internal bool WuJingHejiIncoming(CardUseFrame use) => use.Action is { Type: CardActionType.Use } a && use.TargetSeats is [var target] && a.EffectiveDesignatedTargetSeats.SequenceEqual([target]) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && (use.CardKind == CardKind.Duel || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && a.EffectiveIsRed == true);
}
