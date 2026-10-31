#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramDiscardOriginsRuntime
{
    internal CardLocation? GetProgramDiscardSource(CardMovementRecord movement)
    {
        if (_characterZhangFenAnniversaryOperations.IsDagongCheDestroyedMaterial(movement) && IsProgramDiscardOriginReason(movement.Reason))
            return movement.From;
        if (_coreWushuangJinzhiOperations.WushuangJinzhiDiscardSource(movement)is { } jinzhi)
            return jinzhi;
        if (movement.CardKind == CardKind.ThunderChariot && movement.To == CardLocation.OutsideGame && movement.From.Zone == CardZoneKind.Equipment && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(movement) && IsProgramDiscardOriginReason(movement.Reason))
            return movement.From;
        if (movement.To != CardLocation.DiscardPile && !(movement.To == CardLocation.OutsideGame && _coreShanheStartingResourcesOperations.IsShanheStartingResourceDestroyedMaterial(movement)))
            return null;
        if (IsOwnedDiscardSource(movement.From))
            return IsProgramDiscardOriginReason(movement.Reason) ? movement.From : null;
        if (movement.From != CardLocation.Processing)
            return null;
        var previous = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(item => item.CardId == movement.CardId && item.Sequence < movement.Sequence);
        if (previous is null || previous.To != CardLocation.Processing || !IsOwnedDiscardSource(previous.From) || !IsProgramDiscardOriginReason(previous.Reason))
            return null;
        var begin = previous.Reason.Value;
        return movement.Reason.Value == begin || movement.Reason.Value == begin + "-finished" || movement.Reason.Value == begin + "-discard" ? previous.From : null;
    }

    internal static bool IsOwnedDiscardSource(CardLocation location) => RuntimeSkills.RuntimeRules_Core_ProgramDiscardOrigins.IsOwnedDiscardSource(location);
    internal CardMovementSourceCount GetProgramDiscardOriginSourceCount(CardLocation source, IReadOnlyList<CardMovementRecord> completions)
    {
        if (!IsOwnedDiscardSource(source) || completions.Count == 0 || completions.Any(move => GetProgramDiscardSource(move) != source))
            throw new InvalidOperationException("Discard-origin counts require a frozen owned discard batch.");
        var firstEntry = completions.Min(move => move.From == CardLocation.Processing ? _coreCardMovementJournalCardMovementsAccess._cardMovements.Last(item => item.CardId == move.CardId && item.Sequence < move.Sequence).Sequence : move.Sequence);
        var lastCompletion = completions.Max(move => move.Sequence);
        int CountAt(int sequence) => _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(move => move.Sequence <= sequence).Sum(move => (move.To == source ? 1 : 0) - (move.From == source ? 1 : 0));
        // Owned zones begin empty. Replaying their recorded movements reconstructs
        // exact frozen counts even if other effects have changed the live hand.
        var before = CountAt(firstEntry - 1);
        var after = CountAt(lastCompletion);
        if (before < 0 || after < 0)
            throw new InvalidOperationException("Discard-origin counts lost their movement ledger.");
        return new(source, before, after);
    }

    internal static bool IsProgramDiscardOriginReason(CardMoveReason reason) => RuntimeSkills.RuntimeRules_Core_ProgramDiscardOrigins.IsProgramDiscardOriginReason(reason);
}
