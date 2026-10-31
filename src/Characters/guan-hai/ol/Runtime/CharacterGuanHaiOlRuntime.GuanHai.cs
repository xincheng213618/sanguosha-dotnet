#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanHaiOlRuntime
{
    internal const string YouqueDiscardReason = "skill-program.guan-hai.youque.discard", YouqueDrawReason = "skill-program.guan-hai.youque.draw";
    internal static CardConversionSource GuanHaiSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_guan_hai_ol_GuanHai.GuanHaiSource(f);
    internal IEnumerable<PublicPersistentPileSource> XieweiPiles(int owner) => _corePublicPileSourcesOperations.PublicPileSources(owner).Where(s => s.SkillId == "ol:xiewei");
    SkillProgramStepOutcome global::CardGame.Core.IGuanHaiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginGuanHai(f, op);
    internal SkillProgramStepOutcome BeginGuanHai(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.GuanHai is not null || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op)
            throw new InvalidOperationException("Guan Hai requires one original owning instruction.");
        var r = new GuanHaiReceipt(GuanHaiSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, GuanHaiStage.ChooseBait);
        if (op == SkillProgramEffectOp.ResolveXieweiTwoHandBaitBasicUse)
        {
            if (f.TriggerId is null || f.WindowContext is not { } c || !IsIssuedXieweiCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c) || _resolutionStackOperations._resolutionStack.Single(w => w.Id == c.ParentFrameId)is not ProgramCardTriggerWindowFrame w || IssuedXieweiPayment(w.Action)is not { } p)
                throw new InvalidOperationException("Xiewei cannot commit an unpaid or different native basic use.");
            r = r with
            {
                Paid = true,
                Stage = GuanHaiStage.XieweiChildren,
                ParentId = c.ParentFrameId,
                PaymentId = p.Id,
                NativeUseId = p.UseId,
                ActionId = p.ActionId,
                Before = p.Before,
                After = p.After,
                Materials = p.Materials,
                Pile = p.Pile
            };
        }
        else if (op == SkillProgramEffectOp.ResolveYouqueMinimumBaitPindian)
        {
            if (f.TriggerId is not null || f.ActivationId != "minimum-bait-pindian" || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var peer] || !_coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(f.OwnerSeat, peer) || !_characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || !YouquePhaseActive(f.OwnerSeat) || !YouqueBaits(f.OwnerSeat).Any() || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
                throw new InvalidOperationException("Youque requires a real Play, original hand-bearing participants and actual bait.");
            r = r with
            {
                OpponentSeat = peer,
                Phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()
            };
        }
        else
        {
            if (f.TriggerId is null || f.WindowContext is not { } c || !IsIssuedGuanHaiYouqueCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c) || YouqueRewardUse(c)is not { YouqueSlash: { } origin } use)
                throw new InvalidOperationException("Youque rewards require this issued Slash's actual positive damage.");
            r = r with
            {
                Paid = true,
                Stage = GuanHaiStage.RewardDraw,
                ParentId = origin.ProgramFrameId,
                NativeUseId = use.Id,
                ActionId = use.Action!.ActionId,
                DamageFrameId = c.DamageFrameId!.Value,
                Phase = origin.Phase,
                Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GuanHai = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanHaiStartedEvent(f.Id, r.Source, f.GameplayHash, op, r.ActualTurn, r.TurnOwner, r.ParentId));
        if (op == SkillProgramEffectOp.ResolveXieweiTwoHandBaitBasicUse)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XieweiCommittedEvent(f.Id, r.PaymentId, r.NativeUseId, r.ActionId));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        }

        if (op == SkillProgramEffectOp.ResolveYouqueIssuedSlashDamageBenefit)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YouqueBenefitIssuedEvent(f.Id, r.ParentId, r.NativeUseId, r.DamageFrameId, f.OwnerSeat));
        ContinueGuanHai(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramAnswer("guan-hai")]
    internal void AnswerGuanHai(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { GuanHai: { Stage: GuanHaiStage.ChooseBait } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !YouqueBaitChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Youque requires the actual current public bait choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["branch"] == "cancel")
        {
            FinishGuanHai(f, false);
            return;
        }

        var selected = YouqueBaits(f.OwnerSeat).Single(b => b.Card.Id == choice.Cards.Single());
        var id = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        r = r with
        {
            Paid = true,
            Stage = GuanHaiStage.Pindian,
            Pile = selected.Pile,
            BaitCardId = selected.Card.Id,
            BaitRank = selected.Card.Rank,
            PindianId = id,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GuanHai = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YouquePindianIssuedEvent(f.Id, id, r.Source, r.GameplayHash, selected.Pile, r.BaitCardId, r.BaitRank, r.OpponentSeat, r.Phase!, r.Before));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePindianOperations.BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, skill.Name, skill.Description), f.OwnerSeat, r.OpponentSeat, r.BaitCardId, YouqueResultBind(f), SkillProgramCardSetVisibility.Public, new(f.Id, r.Source, selected.Pile, r.BaitCardId, r.BaitRank, r.Before));
    }

    internal bool IsGuanHaiPindianSource(PindianFrame p, bool revealed)
    {
        if (p.OwnedPileSource is not { } s || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == s.OriginProgramFrameId)is not ProgramSkillFrame { GuanHai: { Paid: true, Stage: GuanHaiStage.Pindian } r } parent || r.Op != SkillProgramEffectOp.ResolveYouqueMinimumBaitPindian || p.Id != r.PindianId || p.ParentFrameId != parent.Id || p.SourceSeat != parent.OwnerSeat || p.OpponentSeat != r.OpponentSeat || p.SourceCardId != r.BaitCardId || s.Origin != r.Source || s.Pile != r.Pile || s.CardId != r.BaitCardId || s.PrintedRank != r.BaitRank || s.SequenceBefore != r.Before || p.ProgramResultBind != YouqueResultBind(parent) || _coreProgramEventHistoryOperations.ProgramEventHistory<YouquePindianIssuedEvent>().Count(e => e == new YouquePindianIssuedEvent(parent.Id, p.Id, r.Source, r.GameplayHash, s.Pile, s.CardId, s.PrintedRank, r.OpponentSeat, r.Phase!, r.Before)) != 1)
            return false;
        if (!revealed)
            return _cardZonesOperations._cardZones.GetLocation(s.CardId) == s.Pile.Location && _coreAdvancedLifecycleOperations.GetAdvancedCard(s.CardId).Rank == s.PrintedRank && !YouqueBaits(parent.OwnerSeat).Any(b => b.Card.Rank < s.PrintedRank);
        return _corePindianOwnedPileSourcesOperations.HasPaidOwnedPilePindianSource(p) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > s.SequenceBefore && m.CardId == s.CardId && m.From == s.Pile.Location && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.PindianReveal) == 1;
    }

    internal bool DrainGuanHai(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueGuanHai(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { GuanHai: { } r } f || f.Id != id)
            return;
        AssertGuanHai(f);
        if (r.Stage == GuanHaiStage.ChooseBait)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishYouque(f);
            return;
        }

        if (r.Stage is GuanHaiStage.NativeSlash or GuanHaiStage.SlashIssuing || f.NativeDraw is not null)
            return;
        if (DrainGuanHai(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.GuanHai!;
        if (f.PendingMovementContinuation is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == GuanHaiStage.Pindian)
        {
            var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Where(e => e.FrameId == r.PindianId && e.SkillId == f.SkillId).ToArray();
            if (facts is not [var fact] || (fact.Result.SourceCardId != r.BaitCardId && !_characterPangJiOlOperations.IsDouyuHistoricalPairAssignment(r.PindianId, fact.Result, r.BaitCardId) && !_characterLuKangWumiaoOperations.IsWuLuKangHistoricalPairAssignment(r.PindianId, fact.Result, r.BaitCardId)) || fact.Result.SourceSeat != f.OwnerSeat || fact.Result.OpponentSeat != r.OpponentSeat || f.PindianResultBindings.SingleOrDefault(b => b.Name == YouqueResultBind(f))is not { } binding || binding.SourceRank != fact.Result.SourceRank || binding.OpponentRank != fact.Result.OpponentRank || binding.SourceWon != fact.Result.SourceWon)
                throw new InvalidOperationException("Youque requires its exact completed native Pindian result.");
            var result = fact.Result;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YouquePindianReturnedEvent(id, r.PindianId, result.SourceCardId, result.OpponentCardId, result.SourceRank, result.OpponentRank));
            if (result.SourceRank == result.OpponentRank)
            {
                FinishGuanHai(f, true);
                return;
            }

            var winner = result.SourceWon ? result.SourceSeat : result.OpponentSeat;
            var loser = result.SourceWon ? result.OpponentSeat : result.SourceSeat;
            var rank = loser == result.SourceSeat ? result.SourceRank : result.OpponentRank;
            var cards = _coreMatchParticipantsPlayersAccess._players[loser].IsAlive ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[loser]).Where(c => c.Rank > rank).ToArray() : [];
            r = r with
            {
                Result = result,
                Stage = GuanHaiStage.DiscardChildren,
                LoserSeat = loser,
                WinnerSeat = winner,
                Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                Materials = cards.Select(c => new GuanHaiMaterial(c.Id, c.Kind, CardLocation.Hand(loser))).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GuanHai = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            for (var i = 0; i < r.Materials.Count; i++)
            {
                var m = r.Materials[i];
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YouqueDiscardMaterialEvent(id, i, loser, m.CardId, m.Kind, m.From));
            }

            if (cards.Length > 0)
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, loser, cards.Select(c => c.Id).ToArray(), new(YouqueDiscardReason), (batch, records) =>
                {
                    foreach (var m in records)
                    {
                        var index = Array.FindIndex(cards, c => c.Id == m.CardId);
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YouqueDiscardPaidEvent(id, index, batch, m.CardId, m.Sequence));
                    }
                });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { GuanHai = f.GuanHai!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            ContinueGuanHai(id);
            return;
        }

        if (r.Stage == GuanHaiStage.DiscardChildren)
        {
            IssueYouqueSlash(f);
            return;
        }

        if (r.Stage == GuanHaiStage.RewardDraw)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishGuanHai(f, true);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GuanHai = r with { Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(YouqueDrawReason));
            return;
        }

        if (r.Stage == GuanHaiStage.RewardChildren)
        {
            if (r.Phase is { } phase && !YouqueBaits(f.OwnerSeat).Any(b => b.Card.Suit == Suit.Spade) && !_coreProgramEventHistoryOperations.ProgramEventHistory<YouquePhaseDisabledEvent>().Any(e => e.OwnerSeat == f.OwnerSeat && e.Phase == phase))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YouquePhaseDisabledEvent(f.Id, f.OwnerSeat, phase, r.Source));
            FinishGuanHai(f, true);
            return;
        }

        if (r.Stage is GuanHaiStage.XieweiChildren or GuanHaiStage.NativeChildren)
        {
            FinishGuanHai(f, true);
            return;
        }

        throw new InvalidOperationException("Guan Hai resumed outside an issued native stage.");
    }

    internal void FinishGuanHai(ProgramSkillFrame f, bool applied)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanHaiCompletedEvent(f.Id, applied));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GuanHai = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, applied);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27578)]
    internal bool ResumeGuanHai(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { GuanHai: not null } f || f.Id != id)
            return false;
        ContinueGuanHai(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27578)]
    internal bool ReturnGuanHaiMovement(ProgramSkillFrame f)
    {
        if (f.GuanHai is null)
            return false;
        ContinueGuanHai(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27578)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27578)]
    internal bool ContinuePaidGuanHai(ProgramSkillFrame f) => CanContinueIssuedGuanHai(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveYouqueMinimumBaitPindian)]
    internal PromptChoice ChooseGuanHai(PendingDecision p, ProgramSkillFrame f) => p.Choices.First(c => c.Parameters.GetValueOrDefault("branch") != "cancel");
}
