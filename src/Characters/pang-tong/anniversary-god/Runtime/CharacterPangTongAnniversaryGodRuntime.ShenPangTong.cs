#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangTongAnniversaryGodRuntime
{
    internal const string ShenPangTongRemovalReason = "skill-program.anniversary:kunyu.remove-fire-card";
    internal static CardConversionSource ShenPtSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId ?? f.ActivationId, f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IShenPangTongHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShenPt(f, op);
    internal CharacterState? ShenPtLuanOwner => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase is not (TurnPhase.NotStarted or TurnPhase.Finished) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat) && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat], ShenPangTongComposition.Luansuo) ? _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat] : null;

    internal bool IsShenPangTongHandDiscardForbidden(int actor, Card card, CardLocation from, OwnedCardMoveIntent intent) => intent == OwnedCardMoveIntent.Discard && from.Zone == CardZoneKind.Hand && from.OwnerSeat is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _cardZonesOperations._cardZones.GetLocation(card.Id) == from && ShenPtLuanOwner is not null;
    internal global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? ShenPangTongLuansuoIdentity(CharacterState holder, Card card)
    {
        if (ShenPtLuanOwner is not { } owner || _cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Hand(holder.Seat))
            return null;
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(holder, card);
        if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && m.To == CardLocation.DiscardPile && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Suit == suit))
            return null;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(ShenPangTongComposition.Luansuo).Program!;
        var identity = program.CardIdentities.Single();
        return new(program, identity, new(ShenPangTongComposition.Luansuo, identity.Id, owner.Seat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, ShenPangTongComposition.Luansuo)));
    }

    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 39200)]
    internal void AddShenPtChainActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> cards, int? selectedPhysicalCardId = null)
    {
        var targets = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => p.Seat).ToArray();
        foreach (var card in cards.Where(c => selectedPhysicalCardId is null || c.Id == selectedPhysicalCardId))
        {
            if (ShenPangTongLuansuoIdentity(actor, card)is not { } identity)
                continue;
            // A mandatory card identity has Iron Chain's native recast option;
            // recasting remains a non-use and never changes Fengliao's state.
            actions.Add(new(LegalActionKind.Recast, card.Id, null, "重铸【铁索连环】，摸一张牌", PlayedCardKind: CardKind.IronChain) { ConversionSource = identity.Source });
            if (!_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card))
                _coreProgramIronChainTargetsOperations.AddProgramIronChainUseActions(actions, actor, card, targets, CardKind.IronChain, identity.Source);
        }
    }

    internal ProgramCardTriggerWindowFrame? ShenPtUse(ProgramSkillWindowContext w) => _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId);
    internal static int[] ShenPtTargets(CardActionContext a) => (a.EffectiveKind == CardKind.BorrowedSword ? a.TargetSeats.Where((_, i) => i % 2 == 0) : a.EffectiveDesignatedTargetSeats).Distinct().ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 39200, SkillProgramEffectOp.ResolveShenPangTongFengliao, SkillProgramEffectOp.ResolveShenPangTongKunyu)]
    internal bool ShenPtEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (t.Effects[0].Op == SkillProgramEffectOp.ResolveShenPangTongFengliao)
            return ShenPtUse(w)is { } use && use.Action.Type == CardActionType.Use && use.Action.ActorSeat == c.OwnerSeat && ShenPtTargets(use.Action)is [var target] && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShenPangTongStartedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId && e.Operation == SkillProgramEffectOp.ResolveShenPangTongFengliao && e.ActionId == use.Action.ActionId);
        return _resolutionStackOperations._resolutionStack.OfType<ProgramDeathTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } window && window.OwnerSeat == c.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().SingleOrDefault(p => p.Id == window.DeathFrameId)is { ShenPangTongOutcomeDeferred: true, ShenPangTongRevival: null } death && death.VictimSeat == c.OwnerSeat && !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive;
    }

    internal SkillProgramStepOutcome BeginShenPt(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShenPangTong is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Shen Pang Tong requires his original unpaid native instruction.");
        var use = op == SkillProgramEffectOp.ResolveShenPangTongFengliao ? ShenPtUse(f.WindowContext!) : null;
        var death = op == SkillProgramEffectOp.ResolveShenPangTongKunyu ? _coreDeathProgramsOperations.GetCurrentDeathFrame(_resolutionStackOperations._resolutionStack.OfType<ProgramDeathTriggerWindowFrame>().Single(p => p.Id == f.WindowContext!.ParentFrameId).DeathFrameId) : null;
        var polarity = use is not null ? _coreProgramConversionPolarityOperations.CommitProgramConversionPolarity(f) : SkillPolarity.Yang;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var target = use is not null ? ShenPtTargets(use.Action).Single() : f.OwnerSeat;
        var r = new ShenPangTongReceipt(ShenPtSource(f), f.GameplayHash, op, f.WindowContext!.ParentFrameId, use is null ? ShenPangTongStage.RemovalChildren : polarity == SkillPolarity.Yang ? ShenPangTongStage.DrawAttempt : ShenPangTongStage.DamageChildren)
        {
            ActionId = use?.Action.ActionId ?? 0,
            TargetSeat = target,
            Polarity = polarity,
            DeathId = death?.Id ?? 0,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenPangTong = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenPangTongStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, r.ActionId, target, polarity, r.DeathId));
        if (use is not null)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            {
                FinishShenPt(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            if (polarity == SkillPolarity.Yang)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, target, 1, new("skill-program.anniversary:fengliao.draw"));
            else
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, 1, nature: DamageNature.Fire);
        }
        else
        {
            var fire = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).FirstOrDefault(c => c.Kind is CardKind.FireSlash or CardKind.FireAttack);
            if (fire is null)
            {
                FinishShenPt(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenPangTong = r with { RemovedCardId = fire.Id }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([fire.Id], CardLocation.OutsideGame, new(ShenPangTongRemovalReason), (batch, moves) =>
            {
                var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                if (moves is not [var m] || m.CardId != fire.Id || m.From != CardLocation.DrawPile || m.To != CardLocation.OutsideGame)
                    throw new InvalidOperationException("Kunyu requires an actual fire damage card removed from the real draw pile.");
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { ShenPangTong = parent.ShenPangTong!with { After = m.Sequence, BatchId = batch } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenPangTongRemovalPaidEvent(f.Id, r.DeathId, batch, r.Before, m.Sequence));
            }, disclosure: CardMovementDisclosure.FaceDown);
            ContinueShenPt(f.Id);
        }

        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainShenPt(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    internal void ContinueShenPt(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenPangTong: { } r } f || f.Id != id)
            return;
        RequireShenPt(f);
        if (DrainShenPt(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.ShenPangTong!;
        if (r.Stage == ShenPangTongStage.DrawAttempt)
            return;
        if (r.Stage == ShenPangTongStage.RemovalChildren)
        {
            var death = _coreDeathProgramsOperations.GetCurrentDeathFrame(r.DeathId);
            if (!death.ShenPangTongOutcomeDeferred || death.VictimSeat != f.OwnerSeat || death.ShenPangTongRevival is not null || r.RemovedCardId <= 0 || r.After <= r.Before)
                throw new InvalidOperationException("Kunyu lost its original paid death-return grant.");
            _coreDeathProgramsOperations.UpdateDeathFrame(death.Id, d => d with { ShenPangTongRevival = new(f.Id, r.Source, r.GameplayHash, r.ParentId, r.RemovedCardId, r.Before, r.After, r.BatchId) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenPangTongRevivalGrantedEvent(f.Id, death.Id, f.OwnerSeat));
        }

        FinishShenPt(f);
    }

    internal void FinishShenPt(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenPangTong = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShenPangTongFengliao)]
    internal void ReturnShenPtDraw(ProgramSkillFrame supplied, NativeDrawOutcome o)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        RequireShenPt(f);
        var r = f.ShenPangTong!;
        if (r.Stage != ShenPangTongStage.DrawAttempt || r.DrawReturned || f.NativeDraw is not null || o.RequestedCount != 1 || o.RecipientSeat != r.TargetSeat)
            throw new InvalidOperationException("Fengliao lost its exact typed target draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenPangTong = r with { Stage = ShenPangTongStage.DrawChildren, DrawReturned = true, ActualDraw = o.ActualDrawIds.Count, Before = o.SequenceBefore, After = o.SequenceAfter } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenPangTongDrawReturnedEvent(f.Id, r.TargetSeat, o.ActualDrawIds.Count, o.AttemptFrameId, o.SequenceBefore, o.SequenceAfter));
        ContinueShenPt(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 39200)]
    internal bool ResumeShenPt(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenPangTong: not null } f || f.Id != id)
            return false;
        ContinueShenPt(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 39200)]
    internal bool ReturnShenPtMovement(ProgramSkillFrame f) => f.ShenPangTong is not null && ResumeShenPt(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 39200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 39200)]
    internal bool KeepPaidShenPt(ProgramSkillFrame f) => f.ShenPangTong is not null;
    internal bool TryBeginShenPangTongKunyuDeath(DeathFrame supplied)
    {
        var death = _coreDeathProgramsOperations.GetCurrentDeathFrame(supplied.Id);
        if (!_coreMatchParticipantsPlayersAccess._players[death.VictimSeat].HasShenPangTongKunyu || death.ShenPangTongOutcomeDeferred || death.OwnerDiedProgramsResolved)
            return false;
        _coreDeathProgramsOperations.UpdateDeathFrame(death.Id, d => d with { ShenPangTongOutcomeDeferred = true });
        if (_coreDeathProgramsOperations.TryBeginOwnerDiedProgramWindow(_coreDeathProgramsOperations.GetCurrentDeathFrame(death.Id), _coreMatchParticipantsPlayersAccess._players[death.VictimSeat]))
            return true;
        _coreDeathProgramsOperations.UpdateDeathFrame(death.Id, d => d with { ShenPangTongOutcomeDeferred = false });
        return false;
    }

    internal void CompleteShenPangTongKunyuDeath(DeathFrame supplied)
    {
        var death = _coreDeathProgramsOperations.GetCurrentDeathFrame(supplied.Id);
        if (!death.ShenPangTongOutcomeDeferred || !death.OwnerDiedProgramsResolved || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != death.Id)
            throw new InvalidOperationException("Kunyu must return through its original retired owner-death window.");
        _coreDeathProgramsOperations.UpdateDeathFrame(death.Id, d => d with { ShenPangTongOutcomeDeferred = false });
        if (death.ShenPangTongRevival is not { } r)
            return;
        RequireShenPtRevival(death, r);
        var p = _coreMatchParticipantsPlayersAccess._players[death.VictimSeat];
        var faceDown = p.IsFaceDown;
        p.IsAlive = true;
        p.MaxHp = 1;
        p.Hp = 1;
        p.IsFaceDown = false;
        p.AlcoholDamageBonus = 0;
        p.UsedPlayPhaseAlcoholThisTurn = false;
        if (p.IsChained)
        {
            p.IsChained = false;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IronChainStateChangedEvent(death.Id, p.Seat, p.Seat, false));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenPangTongRevivedEvent(death.Id, r.ProgramFrameId, p.Seat, p.Hp));
        if (faceDown)
            _coreCharacterStateProgramsOperations.RecordCharacterStateChange(p.Seat, SkillProgramTriggerWindow.CharacterTurnedFaceUp);
        _coreCharacterTurnedOverProgramsOperations.RecordCharacterTurnedOver(p.Seat, faceDown);
    }
}
