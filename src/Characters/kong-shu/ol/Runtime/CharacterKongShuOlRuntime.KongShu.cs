#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKongShuOlRuntime
{
    internal const string LeiluanDrawReason = "skill-program.kong-shu.leiluan.draw", LeiluanGainReason = "skill-program.kong-shu.leiluan.gain", FuchaoDrawReason = "skill-program.kong-shu.fuchao.draw", FuchaoDiscardReason = "skill-program.kong-shu.fuchao.discard";
    internal static CardConversionSource KongShuSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_kong_shu_ol_KongShu.KongShuSource(f);
    SkillProgramStepOutcome global::CardGame.Core.IKongShuHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginKongShu(f, op);
    internal SkillProgramStepOutcome BeginKongShu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.KongShu is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !KongShuComposition.Handles(op))
            throw new InvalidOperationException("Kong Shu requires the original new native trigger instruction.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        var stage = op == SkillProgramEffectOp.ResolveLeiluanLastHandUseBenefit ? KongShuStage.BenefitDraw : op == SkillProgramEffectOp.ResolveFuchaoCurrentTurnChain ? KongShuStage.ChainChildren : KongShuStage.RoundDraw;
        var r = new KongShuReceipt(KongShuSource(f), f.GameplayHash, op, context.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, stage)
        {
            Paid = true,
            Payer = f.OwnerSeat
        };
        if (op == SkillProgramEffectOp.ResolveLeiluanLastHandUseBenefit)
        {
            if (!IsIssuedLeiluanCandidate(candidate, context) || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == context.ParentFrameId)is not { } window || IssuedLeiluanPayment(window.Action)is not { } payment)
                throw new InvalidOperationException("Leiluan lost its exact already-paid last-Hand native Use.");
            r = r with
            {
                PaymentId = payment.Id,
                NativeUseId = payment.UseId,
                ActionId = payment.ActionId
            };
        }
        else if (op == SkillProgramEffectOp.ResolveFuchaoRoundEndingDrawAndDiscard)
        {
            if (!_coreRoundEndingOperations.IsExactRoundEndingCandidate(candidate, context))
                throw new InvalidOperationException("Fuchao requires the real RoundEnding current candidate.");
            r = r with
            {
                DrawRecipients = new[]
                {
                    f.OwnerSeat
                }.Concat(KongShuLivingChained()).ToArray()
            };
        }
        else if (context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.CardUse is not { } completed || completed.ActorSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(completed.EffectiveKind, [SkillProgramCardCategory.Basic]))
            throw new InvalidOperationException("Fuchao lost the final actual basic-use actor.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KongShuStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.ActualTurn, r.TurnOwner, r.Round));
        if (op == SkillProgramEffectOp.ResolveLeiluanLastHandUseBenefit)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeiluanBenefitIssuedEvent(f.Id, r.PaymentId, r.NativeUseId, r.ActionId, f.OwnerSeat));
        if (op == SkillProgramEffectOp.ResolveFuchaoRoundEndingDrawAndDiscard)
            for (var i = 0; i < r.DrawRecipients.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KongShuDrawRecipientFrozenEvent(f.Id, i, r.DrawRecipients[i]));
        if (op == SkillProgramEffectOp.ResolveFuchaoCurrentTurnChain)
        {
            if (_coreMatchParticipantsPlayersAccess._players[r.TurnOwner].IsAlive)
                _coreProgramLifecycleOperations.SetProgramChainedState(f, true, r.TurnOwner);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id)
                return SkillProgramStepOutcome.AwaitChild;
        }

        ContinueKongShu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<int> KongShuLivingChained() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.IsChained).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray();
    internal IReadOnlyList<PromptChoice> KongShuChoices(ProgramSkillFrame f)
    {
        var r = f.KongShu!;
        PromptChoice C(string branch, string label, int? id = null) => new(new($"kong-shu.{(f.Id)}.{(r.Cursor)}.{(r.Stage)}.{(branch)}.{(id ?? -1)}"), label, id is { } card ? [card] : [], [], new Dictionary<string, string> { ["program-action"] = "kong-shu", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == KongShuStage.BenefitChooseTrick)
            return Array.AsReadOnly(LeiluanTrickCandidates(f).Select(c => C("gain", $"获得【{(c.DisplayName)}】", c.Id)).ToArray());
        if (r.Stage != KongShuStage.RoundDiscard || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Payer) || !_coreMatchParticipantsPlayersAccess._players[r.Payer].IsAlive)
            return[];
        var legal = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, r.Payer, r.Payer, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard);
        var result = legal.Where(c => !r.Selected.Contains(c.Cards.Single()) && r.Selected.Count < r.DiscardCount).Select(c => C("pick", c.Label, c.Cards.Single())).ToList();
        result.AddRange(r.Selected.Select(id => C("unpick", $"撤回【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", id)));
        if (r.Selected.Count == r.DiscardCount)
            result.Add(C("discard", "弃置所选牌"));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishKongShu(ProgramSkillFrame f)
    {
        var choices = KongShuChoices(f);
        if (choices.Count == 0)
        {
            if (f.KongShu!.Stage == KongShuStage.BenefitChooseTrick)
                FinishKongShu(f);
            else
                ContinueKongShu(f.Id);
            return;
        }

        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.KongShu!.Stage == KongShuStage.RoundDiscard ? f.KongShu.Payer : f.OwnerSeat, choices, f.KongShu.Stage == KongShuStage.BenefitChooseTrick ? "累卵：获得一张本回合进入弃牌堆的普通锦囊牌。" : $"覆巢：弃置{(f.KongShu.DiscardCount)}张牌。", f.KongShu.Payer);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("kong-shu")]
    internal void AnswerKongShu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { KongShu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == KongShuStage.RoundDiscard ? r.Payer : f.OwnerSeat) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], KongShuChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Kong Shu lost her exact native private chooser or material selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch is "pick" or "unpick")
        {
            var id = choice.Cards.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = r with { Selected = branch == "pick" ? r.Selected.Append(id).ToArray() : r.Selected.Where(i => i != id).ToArray() } });
            PublishKongShu(f);
            return;
        }

        if (branch == "gain")
            IssueKongShuMovement(f, [choice.Cards.Single()], false);
        else if (branch == "discard")
            IssueKongShuMovement(f, r.Selected, true);
        else
            throw new InvalidOperationException("Kong Shu cannot invent an unoffered branch.");
        ContinueKongShu(f.Id);
    }

    internal void IssueKongShuDraw(ProgramSkillFrame f, int payer, int count, KongShuStage stage)
    {
        var r = f.KongShu!with
        {
            Stage = stage,
            Payer = payer,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            ActualDrawCount = 0,
            Materials = [],
            Grain = [],
            Selected = []
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = r, PendingMovementContinuation = new(payer, 0, null) });
        var reason = r.Op == SkillProgramEffectOp.ResolveLeiluanLastHandUseBenefit ? LeiluanDrawReason : FuchaoDrawReason;
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, payer, count, null, null, SkillProgramCardSetVisibility.Private, new(reason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= after && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(payer) && m.Reason.Value == reason);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { KongShu = f.KongShu!with { After = after, ActualDrawCount = actual } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KongShuDrawIssuedEvent(f.Id, r.Cursor, payer, count, actual, r.Before, after));
    }

    internal void IssueKongShuMovement(ProgramSkillFrame f, IReadOnlyList<int> ids, bool discard)
    {
        var r = f.KongShu!;
        var materials = ids.Select(id => new KongShuEntity(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, _cardZonesOperations._cardZones.GetLocation(id))).ToArray();
        var grain = materials.Where(c => c.Kind == CardKind.WoodenOx && c.From.Zone == CardZoneKind.Equipment).SelectMany(c => _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[c.From.OwnerSeat!.Value])).Select(c => c.Id).ToArray();
        r = r with
        {
            Stage = discard ? KongShuStage.RoundDiscardChildren : KongShuStage.BenefitGainChildren,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Materials = materials,
            Grain = grain
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = r, PendingMovementContinuation = new(r.Payer, 0, null) });
        for (var i = 0; i < materials.Length; i++)
        {
            var c = materials[i];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KongShuMaterialFrozenEvent(f.Id, r.Cursor, i, c.CardId, c.Kind, c.From));
        }

        for (var i = 0; i < grain.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KongShuGrainFrozenEvent(f.Id, r.Cursor, i, grain[i]));
        void Paid(long batch, IReadOnlyList<CardMovementRecord> records)
        {
            foreach (var c in materials)
            {
                var m = records.Single(m => m.CardId == c.CardId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KongShuMaterialMovedEvent(f.Id, r.Cursor, batch, m.CardId, m.From, m.To, m.Sequence));
            }
        }

        if (discard && ids.Count > 0)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Payer, ids, new(FuchaoDiscardReason), Paid);
        else if (!discard)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(LeiluanGainReason), Paid);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { KongShu = f.KongShu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KongShuMovementIssuedEvent(f.Id, r.Cursor, r.Payer, ids.Count, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal bool DrainKongShu(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueKongShu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { KongShu: { } r } f || f.Id != id)
            return;
        if (r.Stage is KongShuStage.BenefitDrawChildren or KongShuStage.BenefitGainChildren or KongShuStage.ChainChildren or KongShuStage.RoundDrawChildren or KongShuStage.RoundDiscardChildren)
        {
            if (DrainKongShu(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.KongShu!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == KongShuStage.ChainChildren || r.Stage == KongShuStage.BenefitGainChildren)
            {
                FinishKongShu(f);
                return;
            }

            if (r.Stage == KongShuStage.BenefitDrawChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishKongShu(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = r with { Stage = KongShuStage.BenefitChooseTrick } });
                PublishKongShu(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = r with { Stage = r.Stage == KongShuStage.RoundDrawChildren ? KongShuStage.RoundDraw : KongShuStage.RoundDiscard, Cursor = r.Cursor + 1, Selected = [], Materials = [], Grain = [] } });
            r = f.KongShu!;
        }

        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            FinishKongShu(f);
            return;
        }

        if (r.Stage == KongShuStage.BenefitDraw)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishKongShu(f);
                return;
            }

            IssueKongShuDraw(f, f.OwnerSeat, 2, KongShuStage.BenefitDrawChildren);
            ContinueKongShu(id);
            return;
        }

        if (r.Stage == KongShuStage.BenefitChooseTrick)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                FinishKongShu(f);
            else
                PublishKongShu(f);
            return;
        }

        if (r.Stage == KongShuStage.RoundDraw)
        {
            while (r.Cursor < r.DrawRecipients.Count && !_coreMatchParticipantsPlayersAccess._players[r.DrawRecipients[r.Cursor]].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = r with { Cursor = r.Cursor + 1 } });
                r = f.KongShu!;
            }

            if (r.Cursor < r.DrawRecipients.Count)
            {
                IssueKongShuDraw(f, r.DrawRecipients[r.Cursor], 1, KongShuStage.RoundDrawChildren);
                ContinueKongShu(id);
                return;
            }

            var recipients = KongShuLivingChained();
            r = r with
            {
                Stage = KongShuStage.RoundDiscard,
                Cursor = 0,
                DiscardRecipients = recipients,
                DiscardCount = Math.Max(1, recipients.Count),
                Selected = []
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = r });
            for (var i = 0; i < recipients.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KongShuDiscardRecipientFrozenEvent(f.Id, i, recipients[i], r.DiscardCount));
        }

        if (r.Stage == KongShuStage.RoundDiscard)
        {
            while (r.Cursor < r.DiscardRecipients.Count && !_coreMatchParticipantsPlayersAccess._players[r.DiscardRecipients[r.Cursor]].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = r with { Cursor = r.Cursor + 1 } });
                r = f.KongShu!;
            }

            if (r.Cursor >= r.DiscardRecipients.Count)
            {
                FinishKongShu(f);
                return;
            }

            var payer = r.DiscardRecipients[r.Cursor];
            var available = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, payer, payer, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Count;
            var count = Math.Min(_coreProgramEventHistoryOperations.ProgramEventHistory<KongShuDiscardRecipientFrozenEvent>().First(e => e.FrameId == id).X, available);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = r with { Payer = payer, DiscardCount = count } });
            if (count == 0)
            {
                IssueKongShuMovement(f, [], true);
                ContinueKongShu(id);
            }
            else if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishKongShu(f);
            return;
        }

        throw new InvalidOperationException("Kong Shu returned to an unissued native stage.");
    }

    internal void FinishKongShu(ProgramSkillFrame f)
    {
        var r = f.KongShu!;
        if (r.Op == SkillProgramEffectOp.ResolveLeiluanLastHandUseBenefit)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeiluanDisabledEvent(f.Id, f.OwnerSeat, f.SkillId, "leiluan"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KongShuCompletedEvent(f.Id, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KongShu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, r.Paid);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27550)]
    internal bool ResumeKongShu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { KongShu: not null } f || f.Id != id)
            return false;
        ContinueKongShu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27550)]
    internal bool ReturnKongShuMovement(ProgramSkillFrame f)
    {
        if (f.KongShu is null)
            return false;
        ContinueKongShu(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27550)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27550)]
    internal bool ContinuePaidKongShu(ProgramSkillFrame f)
    {
        if (f.KongShu is { Paid: true })
        {
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.TriggerId is not null && f.WindowContext is { Window: SkillProgramTriggerWindow.CardUseCommitted } c && IsIssuedLeiluanCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveLeiluanLastHandUseBenefit, SkillProgramEffectOp.ResolveFuchaoCurrentTurnChain, SkillProgramEffectOp.ResolveFuchaoRoundEndingDrawAndDiscard)]
    internal PromptChoice KongShuAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard") ?? p.Choices.First(c => c.Parameters.GetValueOrDefault("branch") != "unpick");
}
