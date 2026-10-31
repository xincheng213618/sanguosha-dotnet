#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJuShouMouRuntime
{
    internal MouJuShouRuntime MouJuShou => _mouJuShouOperations.Get();

    int IMouJuShouFacts.Turn => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    long IMouJuShouFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouJuShouFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouJuShouFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMouJuShouFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    SkillProgram? IMouJuShouFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    IEnumerable<T> IMouJuShouFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    bool IMouJuShouFacts.Parent(ProgramSkillFrame f)
    {
        if (f.SkillId == MouJuShouRuntime.ZuoJun)
            return f.TriggerId is null && f.ActivationId == "draw-and-choose" && f.WindowContext is null && f.ActivatingSeat is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 1 && ((IMouJuShouFacts)this).History<ProgramSkillStartedEvent>().Count(x => x.FrameId == f.Id && x.OwnerSeat == f.OwnerSeat && x.SkillId == f.SkillId && x.ActivationId == f.ActivationId) == 1;
        return MouJsMovementParent(f);
    }

    bool IMouJuShouFacts.InHand(int seat, int id) => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(seat);
    IReadOnlyList<int> IMouJuShouFacts.Discardable(int seat) => !((IMouJuShouFacts)this).Alive(seat) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, seat)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, c, new(z, seat), OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, new(z, seat), OwnedCardMoveIntent.Discard))).Select(c => c.Id).ToArray());
    string IMouJuShouFacts.Label(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName;
    IReadOnlyList<MouJsPlan> IMouJuShouFacts.Plans(ProgramSkillFrame f) => MouJsPlans(f);
    IReadOnlyList<MouJsZuoJunGrant> IMouJuShouFacts.Grants(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MouJsZuoJunGrants ?? [];
    IReadOnlyList<MouJsFirstLoss> IMouJuShouFacts.Losses(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MouJsFirstLosses ?? [];
    IReadOnlyList<int> IMouJuShouFacts.Seats => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(p => p.Seat).ToArray());

    IReadOnlyList<(CardConversionSource Source, string Hash)> IMouJuShouFacts.MuwangSources(int seat) => !((IMouJuShouFacts)this).Alive(seat) ? [] : Array.AsReadOnly(_coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[seat]).Where(p => p.Id == MouJuShouRuntime.Muwang).Select(p => (new CardConversionSource(p.Id, "first-loss", seat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[seat], p.Id)), p.GameplayHash)).ToArray());
    bool IMouJuShouFacts.Eligible(int id)
    {
        var kind = _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind;
        return CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) == SkillProgramCardCategory.Basic || CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind);
    }

    bool IMouJuShouFacts.InDiscard(int id) => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DiscardPile;
    bool IMouJuShouFacts.Reclaimable(MouJsFirstLoss loss, CardMovementRecord discarded) => MouJsReclaimable(loss, discarded);
    bool IMouJuShouFacts.ReadyToReclaim(MouJsFirstLoss loss) => ((IMouJuShouFacts)this).InDiscard(loss.CardId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == loss.CardId)is { } last && MouJsReclaimable(loss, last);
    MouJsMuwangQueuedEvent? IMouJuShouFacts.MuwangOpportunity(ProgramSkillFrame f) => MouJsMuwangOpportunity(f);
    void IMouJuShouNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouJuShouNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IMouJuShouNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.MouJuShou?.Stage == MouJuShouStage.HealthChildren ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    SkillProgramStepOutcome IMouJuShouNative.LoseHp(ProgramSkillFrame f) => _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.MouJuShou!.Actor, 1);
    void IMouJuShouNative.Use(ProgramSkillFrame f) => IssueMouJsUse(f);
    void IMouJuShouNative.Discard(ProgramSkillFrame f)
    {
        if (!MouJuShou.Valid(f) || f.MouJuShou is not { Stage: MouJuShouStage.DiscardChildren or MouJuShouStage.PenaltyChildren } r || r.Discard.Any(id => !_characterJuShouMouOperations.CreateMouJuShouFacts().Discardable(r.Actor).Contains(id)))
            throw new InvalidOperationException("Mou Ju Shou requires actual payable bound leftovers or a selected penalty entity.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Actor, r.Discard, new("skill-program." + f.SkillId + ".discard"), (_, moves) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.MouJuShou!;
            if (!MouJuShou.Valid(parent) || moves.Count != r.Discard.Count || moves.Any(m => !r.Discard.Contains(m.CardId) || m.From.OwnerSeat != r.Actor || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Ju Shou lost his actual native logical-discard receipt.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouJuShou = paid with { After = moves.Count > 0 ? moves.Max(m => m.Sequence) : r.Before } });
            foreach (var m in moves)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJsMovedEvent(f.Id, m.CardId, m.From, m.To, m.Sequence));
        });
    }

    void IMouJuShouNative.Recover(ProgramSkillFrame f)
    {
        if (!MouJuShou.Valid(f) || f.MouJuShou is not { Stage: MouJuShouStage.RecoverChildren, Recover: true, CardId: > 0 } r || _characterJuShouMouOperations.CreateMouJuShouFacts().Losses(r.Actor).SingleOrDefault(l => l.Source == r.Source && l.Turn == r.Turn && l.CardId == r.CardId && l.FirstSequence == r.LossSequence)is not { } loss || !_characterJuShouMouOperations.CreateMouJuShouFacts().ReadyToReclaim(loss))
            throw new InvalidOperationException("Muwang requires the original tracked entity actually in Discard.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([r.CardId], CardLocation.Hand(r.Actor), new("skill-program.mou:muwang.return"), (_, moves) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            if (moves is not [var m] || m.CardId != r.CardId || m.From != CardLocation.DiscardPile || m.Sequence <= r.Before || m.To != CardLocation.Hand(r.Actor) && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Muwang cannot invent a returned entity.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouJuShou = parent.MouJuShou!with { After = m.Sequence } });
            if (m.To == CardLocation.Hand(r.Actor))
                MouJuShou.Recovered(f.Id, m);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJsMovedEvent(f.Id, m.CardId, m.From, m.To, m.Sequence));
        });
    }

    void IMouJuShouNative.SetGrants(int seat, IReadOnlyList<MouJsZuoJunGrant> grants) => _coreMatchParticipantsPlayersAccess._players[seat].MouJsZuoJunGrants = Array.AsReadOnly(grants.ToArray());
    void IMouJuShouNative.SetLosses(int seat, IReadOnlyList<MouJsFirstLoss> losses) => _coreMatchParticipantsPlayersAccess._players[seat].MouJsFirstLosses = Array.AsReadOnly(losses.ToArray());
    internal void ObserveMouJsMovement(CardMovementRecord movement) => MouJuShou.ObserveMovement(movement);
    internal bool IsMouJsHandLimitExempt(int actor, int card) => MouJsHasGrant(actor, card);
    // Registration only; the query still belongs to legacy GameEngine.MouJuShouServices.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 50)]
    internal void ProjectMouJuShouCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasMouJsEntityMark(cardId) ? "佐军" : null);
    }

    internal bool HasMouJsEntityMark(int card) => _coreMatchParticipantsPlayersAccess._players.Any(p => MouJsHasGrant(p.Seat, card));
    internal bool MouJsHasGrant(int actor, int card) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && _coreMatchParticipantsPlayersAccess._players[actor].MouJsZuoJunGrants?.Any(g => g.Recipient == actor && g.CardId == card && _cardZonesOperations._cardZones.GetLocation(card) == CardLocation.Hand(actor)) == true;
    internal bool MouJsUsePurpose(int actor, CardKind kind, bool response, bool dying = false)
    {
        if (!response || dying || kind == CardKind.Nullification)
            return true;
        if (_characterSimaYiMouOperations.IsMouSmPlayedActor(_coreMatchParticipantsPlayersAccess._players[actor]) || _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MouLuSuAnniversary: { Yin: true, Operation: SkillProgramEffectOp.ResolveMouLuSuMengmou } played } f && played.Other == actor && _characterLuSuMouOperations.Value.MouLuSuAnniversary.Valid(f))
            return false;
        return _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[actor], kind) || CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true } b && b.WeaponOwnerSeat == actor || _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.SourceSeat == actor);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 36900)]
    internal bool MouJsMaterialBan(ProgramCardMaterialQuery q) => MouJsUsePurpose(q.ActorSeat, q.Kind, q.IsResponse) && q.CardIds.Any(id => MouJsHasGrant(q.ActorSeat, id));
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 36900)]
    internal bool MouJsResponseBan(ProgramNativeResponseQuery q) => MouJsUsePurpose(q.Provider.Seat, q.Kind, true, q.DyingUse) && q.CardIds.Any(id => MouJsHasGrant(q.Provider.Seat, id));
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 36900)]
    internal bool MouJsViewAsBan(ProgramViewAsLegalityQuery q) => MouJsUsePurpose(q.Owner.Seat, q.Kind, q.ForResponse, q.DyingUse) && q.CardIds.Any(id => MouJsHasGrant(q.Owner.Seat, id));
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 36900)]
    internal bool MouJsActionBan(ProgramLegalActionQuery q) => q.Action.Kind != LegalActionKind.Recast && q.Action.CardId is { } id && MouJsHasGrant(q.Actor.Seat, id);
    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 36900)]
    internal void MouJsSubmittedUse(ProgramSubmittedUseQuery q)
    {
        if (q.CardIds.Any(id => MouJsHasGrant(q.ActorSeat, id)))
            throw new InvalidOperationException("A currently held Zuo Jun entity cannot be used.");
    }
}
