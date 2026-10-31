#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiMouOlRuntime
{
    internal const string MouDengAiCollectReason = "skill-program.mou-ol:jigu.collect", MouDengAiExchangeReason = "skill-program.mou-ol:jigu.exchange", MouDengAiRemoveReason = "skill-program.mou-ol:jiewan.remove";
    internal bool HasMouDengAiOl => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.CollectMouDengAiJigu) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.UseMouDengAiJiewan);

    internal static CardConversionSource MouDengAiSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal PublicPersistentPileSource[] MouDengAiPiles(int owner) => _corePublicPileSourcesOperations.PublicPileSources(owner).Where(s => s.SkillId == MouDengAiOlComposition.Jigu).ToArray();
    internal Card[] MouDengAiGu(int owner) => MouDengAiPiles(owner).SelectMany(_corePublicPileSourcesOperations.PublicPileCards).ToArray();
    internal bool MouDengAiIsGu(int owner, int id) => MouDengAiPiles(owner).Any(p => p.Location == _cardZonesOperations._cardZones.GetLocation(id));
    SkillProgramStepOutcome IMouDengAiOlHost.Start(ProgramSkillFrame f, SkillProgramEffectOp op) => StartMouDengAiOl(f, op);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 33400)]
    internal void ObserveMouDengAiEntry(IGameEvent fact)
    {
        if (!HasMouDengAiOl || fact is not NativeTrueUseProcessingEntryEvent e)
            return;
        var actor = e.Kind == NativeTrueUseDiscardKind.Use && _coreCardUseLifecycleOperations.LifecycleCardUse(e.ParentFrameId)?.Action is { } a ? a.ActorSeat : e.ProviderSeat;
        var ownPlay = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == actor;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDengAiUseEntryEvent(e.ParentFrameId, e.ParentActionId, e.CardId, e.EntrySequence, actor, !ownPlay));
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 33400)]
    internal void CaptureMouDengAiUsedDiscard(CardMovementBatchContext batch)
    {
        if (!HasMouDengAiOl)
            return;
        foreach (var move in batch.Movements.Where(m => m.To == CardLocation.DiscardPile && m.From == CardLocation.Processing))
        {
            if (!_coreNativeUsedDiscardOriginsOperations.IsNativeTrueUseDiscardMovement(batch, move))
                continue;
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == move.CardId && m.Sequence < move.Sequence);
            var proof = _coreProgramEventHistoryOperations.ProgramEventHistory<MouDengAiUseEntryEvent>().SingleOrDefault(e => e.ParentId == batch.ParentFrameId && e.CardId == move.CardId && e.EntrySequence == entry?.Sequence);
            if (proof is { OutsideOwnPlay: true })
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDengAiUseDiscardEvent(batch.Id, move.CardId, move.Sequence, proof.ParentId, proof.ActorSeat));
        }
    }

    internal int[] MouDengAiDiscardIndexes(CardMovementBatchContext batch) => batch.Movements.Select((m, i) => (m, i)).Where(x => x.m.To == CardLocation.DiscardPile && x.m.From == CardLocation.Processing && _coreAdvancedLifecycleOperations.GetAdvancedCard(x.m.CardId).Suit != Suit.Heart && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDengAiUseDiscardEvent>().Any(e => e.BatchId == batch.Id && e.CardId == x.m.CardId && e.Sequence == x.m.Sequence && e.ParentId == batch.ParentFrameId)).Select(x => x.i).ToArray();
    [ProgramPipelineHook(ProgramPipelinePhase.SelectDiscardBatchIndexes, 33400)]
    internal int[]? SelectMouDengAiDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => features.HasOperation(SkillProgramEffectOp.CollectMouDengAiJigu) ? MouDengAiDiscardIndexes(batch) : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 33400, SkillProgramEffectOp.CollectMouDengAiJigu, SkillProgramEffectOp.ExchangeMouDengAiJigu, SkillProgramEffectOp.UseMouDengAiJiewan, SkillProgramEffectOp.GrowMouDengAiJiewan)]
    internal bool CanOfferMouDengAiOl(ProgramTriggerLegalityContext c)
    {
        var op = c.Trigger.Effects.Single().Op;
        if (op == SkillProgramEffectOp.CollectMouDengAiJigu)
            return c.Context.MovementBatch is { } b && MouDengAiGu(c.Owner.Seat).Length < c.Owner.MaxHp && MouDengAiDiscardIndexes(b).Any(i => _cardZonesOperations._cardZones.GetLocation(b.Movements[i].CardId) == CardLocation.DiscardPile);
        if (op == SkillProgramEffectOp.ExchangeMouDengAiJigu)
            return c.Context.SourceSeat is { } seat && _coreMatchParticipantsPlayersAccess._players[seat].MaxHp == c.Owner.MaxHp && _corePhysicalOwnedCardQueriesOperations.GetHand(c.Owner).Count > 0 && MouDengAiGu(c.Owner.Seat).Length > 0;
        if (op == SkillProgramEffectOp.GrowMouDengAiJiewan)
            return MouDengAiCanGrow(c.Owner.Seat);
        return _corePhysicalOwnedCardQueriesOperations.GetHand(c.Owner).Count > 0 && (c.Owner.MaxHp > 1 || MouDengAiGu(c.Owner.Seat).Length >= 2);
    }

    internal bool MouDengAiCanGrow(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && MouDengAiGu(owner).Length == _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Count && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner && p.MaxHp >= _coreMatchParticipantsPlayersAccess._players[owner].MaxHp);
    internal bool ValidMouDengAiOl(ProgramSkillFrame f) => f.MouDengAiOl is { } r && r.Source == MouDengAiSource(f) && r.GameplayHash == f.GameplayHash && f.InstructionIndex == 1 && f.ActivationId == f.TriggerId && f.WindowContext is { OwnerSeat: var owner, ParentFrameId: var window } && owner == f.OwnerSeat && window == r.WindowId && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f).Op == r.Operation && Enum.IsDefined(r.Stage) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDengAiStartedEvent>().Count(e => e == new MouDengAiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.WindowId)) == 1;
    internal SkillProgramStepOutcome StartMouDengAiOl(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouDengAiOl is not null || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || (op == SkillProgramEffectOp.CollectMouDengAiJigu ? f.SkillId != MouDengAiOlComposition.Jigu || w.Window != SkillProgramTriggerWindow.DiscardPileReceived || w.MovementBatch is null : op == SkillProgramEffectOp.ExchangeMouDengAiJigu ? f.SkillId != MouDengAiOlComposition.Jigu || w.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow : op == SkillProgramEffectOp.UseMouDengAiJiewan ? f.SkillId != MouDengAiOlComposition.Jiewan || w.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow : op != SkillProgramEffectOp.GrowMouDengAiJiewan || f.SkillId != MouDengAiOlComposition.Jiewan || w.Window != SkillProgramTriggerWindow.TurnEnding))
            throw new InvalidOperationException("Mou Deng Ai requires its original live native binding and window.");
        var stage = op == SkillProgramEffectOp.ExchangeMouDengAiJigu ? MouDengAiOlStage.ExchangeHand : op == SkillProgramEffectOp.UseMouDengAiJiewan ? MouDengAiOlStage.Cost : MouDengAiOlStage.Children;
        var r = new MouDengAiOlReceipt(MouDengAiSource(f), f.GameplayHash, op, stage)
        {
            WindowId = w.ParentFrameId,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDengAiStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId));
        if (op == SkillProgramEffectOp.CollectMouDengAiJigu)
        {
            var b = w.MovementBatch!;
            var count = MouDengAiGu(f.OwnerSeat).Length;
            var max = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp;
            var cards = MouDengAiDiscardIndexes(b).Select(i => b.Movements[i]).Where(m => _cardZonesOperations._cardZones.GetLocation(m.CardId) == CardLocation.DiscardPile).ToArray();
            if (count >= max || cards.Length == 0)
            {
                FinishMouDengAiOl(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            var pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, int.MaxValue);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { Paid = true }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCards(cards.Select(m => _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId)).ToArray(), CardLocation.DiscardPile, pile.Location, new(MouDengAiCollectReason), disclosure: CardMovementDisclosure.FaceUp);
            foreach (var m in cards)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDengAiGuCollectedEvent(f.Id, m.CardId, m.Sequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(x => x.CardId == m.CardId).Sequence, count, max));
            SealMouDengAiMovement(f.Id);
            ContinueMouDengAiOl(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.GrowMouDengAiJiewan)
        {
            if (!MouDengAiCanGrow(f.OwnerSeat))
            {
                FinishMouDengAiOl(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { Paid = true }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDengAiGrownEvent(f.Id, before, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp));
            SealMouDengAiMovement(f.Id);
            ContinueMouDengAiOl(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishMouDengAiOl(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ConfirmMouDengAiExchange(ProgramSkillFrame f)
    {
        var r = f.MouDengAiOl!;
        if (r.HandCards.Count == 0 || r.HandCards.Count != r.GuCards.Count || r.HandCards.Distinct().Count() != r.HandCards.Count || r.GuCards.Distinct().Count() != r.GuCards.Count || r.HandCards.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat)) || r.GuCards.Any(id => !MouDengAiIsGu(f.OwnerSeat, id)))
            throw new InvalidOperationException("Jigu must exchange equal exact current Hand and Gu entities.");
        var destinations = r.GuCards.Select(id => _cardZonesOperations._cardZones.GetLocation(id)).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { Paid = true, Stage = MouDengAiOlStage.Children, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCards(r.HandCards.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray(), CardLocation.Hand(f.OwnerSeat), CardLocation.Processing, new(MouDengAiExchangeReason), disclosure: CardMovementDisclosure.FaceUp);
        foreach (var group in r.GuCards.Select((id, i) => (id, location: destinations[i])).GroupBy(x => x.location))
            _corePhysicalMovementFacadesOperations.MoveCards(group.Select(x => _coreAdvancedLifecycleOperations.GetAdvancedCard(x.id)).ToArray(), group.Key, CardLocation.Hand(f.OwnerSeat), new(MouDengAiExchangeReason), disclosure: CardMovementDisclosure.FaceUp);
        for (var i = 0; i < r.HandCards.Count; i++)
        {
            _coreAdvancedWeaponsOperations.MoveProcessingCardUnlessDestroyed(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.HandCards[i]), destinations[i], new(MouDengAiExchangeReason));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDengAiExchangeEvent(f.Id, r.HandCards[i], r.GuCards[i], destinations[i]));
        }

        SealMouDengAiMovement(f.Id);
        ContinueMouDengAiOl(f.Id);
    }
}
