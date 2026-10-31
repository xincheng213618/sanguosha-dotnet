#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBaoXinOlRuntime
{
    internal const string BaoXinStore = "skill-program.bao-xin.mutao.store", BaoXinGift = "skill-program.bao-xin.mutao.gift", BaoXinCleanup = "skill-program.bao-xin.mutao.cleanup", BaoXinObtain = "skill-program.bao-xin.yimou.obtain", BaoXinYimouGift = "skill-program.bao-xin.yimou.gift", BaoXinDraw = "skill-program.bao-xin.yimou.draw";
    SkillProgramStepOutcome IBaoXinHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginBaoXin(f, op);
    internal SkillProgramStepOutcome BeginBaoXin(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var holder = op == SkillProgramEffectOp.ResolveBaoXinMutao ? f.SelectedTargetSeats.Single() : f.WindowContext!.TargetSeat!.Value;
        var r = new BaoXinReceipt(BaoXinSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, holder, op == SkillProgramEffectOp.ResolveBaoXinMutao ? BaoXinStage.StoreChildren : BaoXinStage.Mode)
        {
            Cursor = holder,
            Recipient = holder
        };
        if (op == SkillProgramEffectOp.ResolveBaoXinMutao)
        {
            var materials = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).Where(c => CoreNativeCardClassificationsRuntime.IsSlashCard(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[holder], c))).Select(c => new BaoXinEntity(c.Id, c.Kind, _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[holder], c), CardLocation.Hand(holder))).ToArray();
            r = r with
            {
                Paid = true,
                Original = materials,
                Pile = materials.Length == 0 ? null : new(holder, "ol:mutao", "bao-xin:" + f.Id, 0, "bao-xin.mutao." + f.Id)
            };
        }
        else
        {
            var context = f.WindowContext ?? throw new InvalidOperationException("Yimou requires its native damage context.");
            var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
            r = r with
            {
                Qualification = BaoXinQualification(c, context) ?? throw new InvalidOperationException("Yimou lost its exact damage occurrence or frozen directional distance.")
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.TurnOwner, holder));
        if (op == SkillProgramEffectOp.ResolveBaoXinMutao)
        {
            foreach (var(c, index)in r.Original.Select((c, index) => (c, index)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinOriginalEntityEvent(f.Id, index, c.CardId, c.Kind, c.EffectiveKind, c.From));
            if (r.Pile is null)
            {
                FinishBaoXin(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            _cardZonesOperations._cardZones.EnsurePublicPersistentPile(r.Pile.Location);
            _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Add((r.Pile.OwnerSeat, r.Pile.SkillId, r.Pile.SkillInstanceId), r.Pile);
            MoveBaoXin(f, r.Original, BaoXinStage.StoreChildren, r.Pile.Location, BaoXinStore);
            ContinueBaoXin(f.Id);
        }
        else
            PublishBaoXin(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void PrepareBaoXinMove(ref ProgramSkillFrame f, IReadOnlyList<BaoXinEntity> entities, BaoXinStage stage)
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
        var r = f.BaoXin!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r with { Stage = stage, Step = r.Step + 1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = 0, Moving = entities }, PendingMovementContinuation = new(r.Recipient, 0, null) });
    }

    internal void CompleteBaoXinMove(long id, int count)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.BaoXin!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinMovementPaidEvent(id, r.Step, r.Stage, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count, r.Recipient));
    }

    internal void MoveBaoXin(ProgramSkillFrame f, IReadOnlyList<BaoXinEntity> entities, BaoXinStage stage, CardLocation destination, string reason)
    {
        PrepareBaoXinMove(ref f, entities, stage);
        var step = f.BaoXin!.Step;
        if (entities.Count > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(entities.Select(c => c.CardId).ToArray(), destination, new(reason), (stage is BaoXinStage.GiftChildren or BaoXinStage.YimouGiftChildren ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.BaoXin!.Holder, destination.OwnerSeat ?? f.OwnerSeat, entities.Select(c => c.CardId).ToArray(), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batch, records) =>
            {
                foreach (var m in records)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinEntityMovedEvent(f.Id, step, batch, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
                if (stage == BaoXinStage.GiftChildren && records is [var moved])
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    var r = current.BaoXin!;
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { BaoXin = r with { Consumed = r.Consumed.Append(moved.CardId).ToArray(), Cursor = r.Recipient, LastRecipient = r.Recipient } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinMutaoGiftEvent(f.Id, r.GiftOrdinal, moved.CardId, r.Recipient, moved.Sequence));
                }
            }) : (batch, records) =>
            {
                foreach (var m in records)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinEntityMovedEvent(f.Id, step, batch, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
                if (stage == BaoXinStage.GiftChildren && records is [var moved])
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    var r = current.BaoXin!;
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { BaoXin = r with { Consumed = r.Consumed.Append(moved.CardId).ToArray(), Cursor = r.Recipient, LastRecipient = r.Recipient } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinMutaoGiftEvent(f.Id, r.GiftOrdinal, moved.CardId, r.Recipient, moved.Sequence));
                }
            }));
        CompleteBaoXinMove(f.Id, entities.Count);
    }

    internal void RecordBaoXinRandom(ProgramSkillFrame f, int ordinal, int recipient)
    {
        var r = f.BaoXin!;
        for (var i = 0; i < r.Eligible.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinRandomCandidateEvent(f.Id, ordinal, i, r.Eligible[i]));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinRandomIssuedEvent(f.Id, ordinal, r.SelectedCard!.Value, recipient, r.RandomIndex, r.Eligible.Count, r.RandomBefore, r.RandomAfter));
    }

    internal bool DrainBaoXin(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueBaoXin(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BaoXin: { } r } f || f.Id != id)
            return;
        if (r.Stage is BaoXinStage.Mode or BaoXinStage.GiftCard or BaoXinStage.GiftTarget)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishBaoXin(f);
            return;
        }

        if (f.AttackAttempt is not null)
            return;
        if (DrainBaoXin(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.BaoXin!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Op == SkillProgramEffectOp.ResolveBaoXinYimou)
        {
            if (r.Stage == BaoXinStage.YimouGiftChildren && _coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r with { Recipient = r.Holder } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinYimouDrawIssuedEvent(id, r.Holder));
                PrepareBaoXinMove(ref f, [], BaoXinStage.DrawChildren);
                _coreProgramLifecycleOperations.DrawProgramCards(id, r.Holder, 1, null, null, SkillProgramCardSetVisibility.Private, new(BaoXinDraw));
                CompleteBaoXinMove(id, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > f.BaoXin!.Before && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Holder) && m.Reason.Value == BaoXinDraw));
                ContinueBaoXin(id);
                return;
            }

            FinishBaoXin(f);
            return;
        }

        if (r.Stage is BaoXinStage.CleanupChildren or BaoXinStage.DamageChildren)
        {
            FinishBaoXin(f);
            return;
        }

        var remaining = r.Original.Where(c => !r.Consumed.Contains(c.CardId)).ToArray();
        foreach (var missing in remaining.Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) != r.Pile!.Location))
        {
            var departed = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == missing.CardId && m.From == r.Pile!.Location) ?? throw new InvalidOperationException("Mutao cannot retire an original entity without its real departure.");
            r = r with
            {
                Consumed = r.Consumed.Append(missing.CardId).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinMutaoRetiredEvent(id, missing.CardId, departed.Sequence));
        }

        var eligible = r.Original.Where(c => !r.Consumed.Contains(c.CardId) && _cardZonesOperations._cardZones.GetLocation(c.CardId) == r.Pile!.Location).OrderBy(c => c.CardId).ToArray();
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && eligible.Length > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive))
        {
            var recipient = _coreTableSeatQueriesOperations.FindNextAliveSeat(r.Cursor);
            var before = _randomOperations._random.State;
            var index = _randomOperations._random.Next(eligible.Length);
            var card = eligible[index];
            r = r with
            {
                Recipient = recipient,
                SelectedCard = card.CardId,
                Eligible = eligible.Select(c => c.CardId).ToArray(),
                RandomBefore = before,
                RandomAfter = _randomOperations._random.State,
                RandomIndex = index,
                GiftOrdinal = r.GiftOrdinal + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r });
            RecordBaoXinRandom(f, r.GiftOrdinal, recipient);
            MoveBaoXin(f, [card with { From = r.Pile!.Location }], BaoXinStage.GiftChildren, CardLocation.Hand(recipient), BaoXinGift);
            ContinueBaoXin(id);
            return;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && r.LastRecipient is { } last && _coreMatchParticipantsPlayersAccess._players[last].IsAlive)
        {
            var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[last]).Count(c => CoreNativeCardClassificationsRuntime.IsSlashCard(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[last], c)));
            var damage = Math.Min(2, count);
            if (damage > 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r with { Stage = BaoXinStage.DamageChildren, DamageIssued = true, Damage = damage } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinMutaoDamageIssuedEvent(id, r.Holder, last, count, damage));
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, last, damage, nature: DamageNature.Normal, explicitSourceSeat: r.Holder);
                return;
            }
        }

        FinishBaoXin(f);
    }

    internal void FinishBaoXin(ProgramSkillFrame f)
    {
        var r = f.BaoXin!;
        if (r.Pile is { } pile)
        {
            var residue = _corePublicPileSourcesOperations.PublicPileCards(pile).ToArray();
            if (residue.Length > 0 && r.Stage != BaoXinStage.CleanupChildren)
            {
                var own = r.Original.Select(c => c.CardId).ToHashSet();
                MoveBaoXin(f, residue.Select(c => new BaoXinEntity(c.Id, c.Kind, c.Kind, pile.Location)).ToArray(), BaoXinStage.CleanupChildren, CardLocation.DiscardPile, BaoXinCleanup);
                ContinueBaoXin(f.Id);
                return;
            }

            _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Remove((pile.OwnerSeat, pile.SkillId, pile.SkillInstanceId));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinCompletedEvent(f.Id, r.Op));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27450)]
    internal bool ResumeBaoXin(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BaoXin: not null } f || f.Id != id)
            return false;
        ContinueBaoXin(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27450)]
    internal bool ReturnBaoXinMovement(ProgramSkillFrame f)
    {
        if (f.BaoXin is null)
            return false;
        ContinueBaoXin(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27450)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27450)]
    internal bool ContinueIssuedBaoXin(ProgramSkillFrame f) => f.BaoXin is { Paid: true } && ValidBaoXin(f);
}
