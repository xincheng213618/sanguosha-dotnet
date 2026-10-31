#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanHuanOlRuntime
{
    internal static int DeruNameBit(CardKind kind) => RuntimeSkills.RuntimeRules_yuan_huan_ol_Deru_Queries.DeruNameBit(kind);
    internal int DeruActualNames(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Aggregate(0, (mask, card) => mask | DeruNameBit(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[seat], card)));
    internal void IssueDeruTake(ProgramSkillFrame f)
    {
        var r = f.YuanHuan!;
        var candidates = r.GuessMask != r.ActualMask && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Where(c => DeruNameBit(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], c)) != 0).Select(c => c.Id).ToArray() : [];
        var index = candidates.Length == 0 ? -1 : _randomOperations._random.Next(candidates.Length);
        var card = index < 0 ? null : _coreAdvancedLifecycleOperations.GetAdvancedCard(candidates[index]);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanHuan = r with { Stage = YuanHuanStage.TakeChildren, BasicCandidates = candidates, RandomIndex = index, TakenKind = card is null ? null : _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], card), TakenPrintedKind = card?.Kind }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (card is not null)
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(r.TargetSeat), CardLocation.Hand(f.OwnerSeat), new(DeruTakeReason));
        RecordYuanHuanStep(f.Id, DeruTakeReason, before, card is null ? 0 : 1, card: card?.Id, from: card is null ? null : CardLocation.Hand(r.TargetSeat), to: card is null ? null : CardLocation.Hand(f.OwnerSeat));
    }
}
