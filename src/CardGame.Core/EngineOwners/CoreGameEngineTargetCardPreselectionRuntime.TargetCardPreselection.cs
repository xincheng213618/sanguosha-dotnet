#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTargetCardPreselectionRuntime
{
    internal long TargetHandMovementSequence(int seat) => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(move => move.From != move.To && (move.From == CardLocation.Hand(seat) || move.To == CardLocation.Hand(seat)))?.Sequence ?? 0;
}
