#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBaOlRuntime
{
    internal bool ValidLiuBaCoin(LiuBaCoinMintedEvent e) => e.Source.SkillId == "ol:zhubi" && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash && _coreTableSeatQueriesOperations.IsValidPlayerSeat(e.RecipientSeat) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiuBaStartedEvent>().Count(s => s.FrameId == e.FrameId && s.Source == e.Source && s.GameplayHash == e.GameplayHash && s.Op == SkillProgramEffectOp.RecastZhubiAndMarkActualDraw) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiuBaEntityMovedEvent>().Count(p => p.FrameId == e.FrameId && p.Step == 1 && p.From.OwnerSeat == e.RecipientSeat && p.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == p.Sequence && m.CardId == p.CardId && m.From == p.From && m.To == p.To && m.Reason == CardMoveReasons.RecastDiscard)) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.GainSequence && m.CardId == e.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(e.RecipientSeat) && m.Reason == CardMoveReasons.RecastDraw) == 1;
    internal LiuBaCoinMintedEvent? LiuBaCoin(int id) => HasLiuBaCapability ? _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiuBaCoinMintedEvent>().FirstOrDefault(e => e.CardId == id && ValidLiuBaCoin(e)) : null;
    // Registration only; the query still belongs to legacy GameEngine.Zhubi.Ledger.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 30)]
    internal void ProjectLiuBaCoinCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasLiuBaCoinEntity(cardId) ? "币" : null);
    }

    internal bool HasLiuBaCoinEntity(int cardId) => LiuBaCoin(cardId)is not null;
    internal void AppendLiuBaCoinEndingItems(List<TurnEndingBoundaryItem> items)
    {
        if (!HasLiuBaCapability || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat].IsAlive)
            return;
        var coin = LiuBaCoins(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat).Select(e => LiuBaCoin(e.CardId)).FirstOrDefault(e => e is not null);
        if (coin is null)
            return;
        var t = _contentRegistryOperations._contentRegistry.GetSkill("ol:zhubi").Program!.Triggers.Single(t => t.Id == "issued-holder-ending");
        var c = new ProgramTriggerCandidate(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, "ol:zhubi", t.Id, coin.Source.SkillInstanceId, coin.GameplayHash, t.Priority, items.Count);
        items.Add(new(TurnEndingBoundaryItemKind.Program, t.Priority, $"liu-ba.coin:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)}:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)}:{(coin.CardId)}", c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
    }

    [ProgramIssuedTrigger(510)]
    internal bool IsIssuedLiuBaCoinEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!HasLiuBaCapability || c.SkillId != "ol:zhubi" || c.BindingId != "issued-holder-ending" || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || c.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || context.Window != SkillProgramTriggerWindow.TurnEnding || context.OwnerSeat != c.OwnerSeat || context.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || context.TargetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || context.OccurrenceIndex != c.OccurrenceIndex || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate != c)
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiuBaCoinMintedEvent>().Any(e => ValidLiuBaCoin(e) && e.Source.SkillInstanceId == c.SkillInstanceId && e.GameplayHash == c.GameplayHash && parent.Items[parent.ItemIndex].StableIdentity == $"liu-ba.coin:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)}:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)}:{(e.CardId)}") && (LiuBaCoins(c.OwnerSeat).Length > 0 || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.LiuBa is { Op: SkillProgramEffectOp.ExchangeZhubiCoinsAtEnding, Paid: true } && f.WindowContext == context && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c)));
    }
}
