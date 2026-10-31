#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuLiAnniversaryRuntime
{
    internal const string LiuLiDiscardReason = "skill-program.anniversary:fuli.discard", LiuLiDrawReason = "skill-program.anniversary:fuli.draw";
    internal CardConversionSource LiuLiSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryLiuLiHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginLiuLi(f, op);
    internal static int LiuLiNameLength(Card c) => (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind) ? "杀" : c.DisplayName).EnumerateRunes().Count();
    internal Card[][] LiuLiCategories(int seat) => new[]
    {
        SkillProgramCardCategory.Basic,
        SkillProgramCardCategory.Trick,
        SkillProgramCardCategory.Equipment
    }.Select(category => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Where(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind) == category).ToArray()).ToArray();
    internal bool LiuLiCanDiscard(int seat, IReadOnlyList<Card> cards) => cards.Count > 0 && cards.All(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, CardLocation.Hand(seat), OwnedCardMoveIntent.Discard));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 43300, SkillProgramEffectOp.ResolveAnniversaryLiuLiFuli)]
    internal bool CanStartLiuLiFuli(CharacterState owner) => LiuLiCategories(owner.Seat).Any(cards => LiuLiCanDiscard(owner.Seat, cards));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 43300, SkillProgramEffectOp.ResolveAnniversaryLiuLiDehua)]
    internal bool CanStartLiuLiDehua(ProgramTriggerLegalityContext g) => g.SkillId == AnniversaryLiuLiComposition.Dehua && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuLiStartedEvent>().Any(e => e.Source.OwnerSeat == g.OwnerSeat && e.Source.SkillId == g.SkillId && e.Source.BindingId == g.Candidate.BindingId && e.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber);
    internal SkillProgramStepOutcome BeginLiuLi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var fuli = op == SkillProgramEffectOp.ResolveAnniversaryLiuLiFuli;
        if (f.AnniversaryLiuLi is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || (fuli ? f.SkillId != AnniversaryLiuLiComposition.Fuli || f.TriggerId is not null || f.ActivationId != "reveal-discard-draw" || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play : f.SkillId != AnniversaryLiuLiComposition.Dehua || f.TriggerId != "round-damage-name" || f.ActivationId != f.TriggerId || f.WindowContext?.Window != SkillProgramTriggerWindow.RoundStarting))
            throw new InvalidOperationException("Liu Li requires his original unpaid zero-input activation or exact round candidate.");
        var r = new AnniversaryLiuLiReceipt(LiuLiSource(f), f.GameplayHash, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, fuli ? AnniversaryLiuLiStage.Revealing : AnniversaryLiuLiStage.UseChoice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuLi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuLiStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.Round));
        if (fuli)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray())));
            if (!_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(f.Id, "anniversary-liu-li-fuli"))
                CompleteLiuLiReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        }
        else
        {
            EnsureLiuLiDamagePool(f.OwnerSeat);
            PublishLiuLi(f);
        }

        return SkillProgramStepOutcome.AwaitChild;
    }

    [PublicHandRevealReturn("anniversary-liu-li-fuli")]
    internal void CompleteLiuLiReveal(ProgramSkillFrame f)
    {
        if (!ValidLiuLi(f) || f.AnniversaryLiuLi!.Stage != AnniversaryLiuLiStage.Revealing)
            throw new InvalidOperationException("Fuli lost its original complete public Hand reveal.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuLi = f.AnniversaryLiuLi with { Stage = AnniversaryLiuLiStage.Category } });
        PublishLiuLi(f);
    }

    internal IReadOnlyList<PromptChoice> LiuLiChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryLiuLi!;
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string key, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => new(new($"liu-li.{(f.Id)}.{(r.Stage)}.{(key)}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = "anniversary-liu-li", ["option"] = key });
        if (r.Stage == AnniversaryLiuLiStage.Category)
        {
            var groups = LiuLiCategories(f.OwnerSeat);
            var labels = new[]
            {
                "基本牌",
                "锦囊牌",
                "装备牌"
            };
            for (var i = 0; i < groups.Length; i++)
                if (LiuLiCanDiscard(f.OwnerSeat, groups[i]))
                    choices.Add(Choice("category-" + i, "弃置所有" + labels[i], groups[i].Select(c => c.Id).ToArray(), []));
        }
        else if (r.Stage == AnniversaryLiuLiStage.RangeTarget)
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                choices.Add(Choice("range-" + p.Seat, $"令{(p.Name)}的攻击范围" + (r.DamageDiscarded ? "减至0" : "-1"), [], [p.Seat]));
        else if (r.Stage == AnniversaryLiuLiStage.UseChoice)
            foreach (var(plan, i)in LiuLiPlans(f.OwnerSeat).Select((p, i) => (p, i)))
                choices.Add(Choice("use-" + i, "视为使用【" + CardCatalog.Get(plan.Kind).DisplayName + "】" + string.Join("、", plan.Targets.Select(t => _coreMatchParticipantsPlayersAccess._players[t].Name)), [], plan.Targets));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishLiuLi(ProgramSkillFrame f)
    {
        var choices = LiuLiChoices(f);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || choices.Count == 0)
        {
            FinishLiuLi(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, f.SkillId == AnniversaryLiuLiComposition.Fuli ? "抚黎：选择弃牌类别或受影响角色。" : "德化：选择尚可使用的伤害牌及其原生目标。", f.OwnerSeat);
    }

    [ProgramAnswer("anniversary-liu-li")]
    internal void AnswerLiuLi(PromptChoice c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryLiuLi: { } r } f || !ValidLiuLi(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !LiuLiChoices(f).Any(choice => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [c])))
            throw new InvalidOperationException("Liu Li rejects a stale category, native use plan or range recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var option = c.Parameters["option"];
        if (r.Stage == AnniversaryLiuLiStage.Category)
        {
            var category = int.Parse(option[9..], CultureInfo.InvariantCulture);
            var cards = LiuLiCategories(f.OwnerSeat)[category];
            if (!LiuLiCanDiscard(f.OwnerSeat, cards) || !cards.Select(card => card.Id).SequenceEqual(c.Cards))
                throw new InvalidOperationException("Fuli must pay the complete live category of discardable Hand cards.");
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuLi = r with { Stage = AnniversaryLiuLiStage.CostChildren, Category = category, Cards = cards.Select(card => card.Id).ToArray(), NameSum = cards.Sum(LiuLiNameLength), DamageDiscarded = cards.Any(card => global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(card.Kind)), Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuLiDiscardPaidEvent(f.Id, category, f.AnniversaryLiuLi.Cards.Count, f.AnniversaryLiuLi.NameSum, f.AnniversaryLiuLi.DamageDiscarded, before, before));
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, f.AnniversaryLiuLi.Cards, new(LiuLiDiscardReason), (_, _) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { AnniversaryLiuLi = current.AnniversaryLiuLi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.AnniversaryLiuLi!;
            ContinueLiuLi(f.Id);
            return;
        }

        if (r.Stage == AnniversaryLiuLiStage.RangeTarget)
        {
            var target = c.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuLiRangeIssuedEvent(f.Id, r.Source, r.Hash, target, _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().Count(e => e.ActorSeat == f.OwnerSeat), r.DamageDiscarded));
            FinishLiuLi(f);
            return;
        }

        var plan = LiuLiPlans(f.OwnerSeat)[int.Parse(option[4..], CultureInfo.InvariantCulture)];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuLi = r with { Stage = AnniversaryLiuLiStage.IssuingUse, Plan = plan } });
        IssueLiuLiUse(f);
    }

    internal bool DrainLiuLi(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.Program);
    [ProgramContinuation(ProgramContinuationKind.Resume, 43300)]
    internal bool ContinueLiuLi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryLiuLi: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || r.Stage == AnniversaryLiuLiStage.Completed)
            return false;
        if (!ValidLiuLi(f))
            throw new InvalidOperationException("Liu Li lost his original paid native continuation.");
        if (r.Stage is AnniversaryLiuLiStage.Category or AnniversaryLiuLiStage.RangeTarget or AnniversaryLiuLiStage.UseChoice)
        {
            PublishLiuLi(f);
            return true;
        }

        if (r.Stage == AnniversaryLiuLiStage.NativeUse && f.VirtualBasicDraft is null && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(r.Plan!.Kind))
        {
            CompleteLiuLiNative(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return true;
        }

        if (r.Stage is not (AnniversaryLiuLiStage.CostChildren or AnniversaryLiuLiStage.DrawChildren or AnniversaryLiuLiStage.SkillChildren))
            return true;
        if (DrainLiuLi(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.AnniversaryLiuLi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == AnniversaryLiuLiStage.SkillChildren)
        {
            FinishLiuLi(f);
            return true;
        }

        if (r.Stage == AnniversaryLiuLiStage.CostChildren)
        {
            var maximum = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count).DefaultIfEmpty(0).Max();
            var count = Math.Min(r.NameSum, maximum);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuLiDrawIssuedEvent(f.Id, r.NameSum, maximum, count));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuLi = r = r with { Stage = count == 0 ? AnniversaryLiuLiStage.RangeTarget : AnniversaryLiuLiStage.Drawing, DrawCount = count } });
            if (count > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
            {
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new(LiuLiDrawReason));
                return true;
            }
        }
        else
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuLi = r with { Stage = AnniversaryLiuLiStage.RangeTarget } });
        PublishLiuLi(f);
        return true;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryLiuLiFuli)]
    internal void ReturnLiuLiDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryLiuLi!;
        if (!ValidLiuLi(f) || r.Stage != AnniversaryLiuLiStage.Drawing || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != r.DrawCount || f.NativeDraw is not null)
            throw new InvalidOperationException("Fuli requires its once-issued native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryLiuLi = r with { Stage = AnniversaryLiuLiStage.DrawChildren, Draw = draw, Before = draw.SequenceBefore, After = draw.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuLiDrawReturnedEvent(f.Id, draw.RequestedCount, draw.ActualDrawIds.Count, draw.SequenceBefore, draw.SequenceAfter));
        ContinueLiuLi(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 43300)]
    internal bool ReturnLiuLiMovement(ProgramSkillFrame f) => f.AnniversaryLiuLi is not null && ContinueLiuLi(f.Id);
    internal void FinishLiuLi(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuLi = f.AnniversaryLiuLi!with { Stage = AnniversaryLiuLiStage.Completed }, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryLiuLiFuli, SkillProgramEffectOp.ResolveAnniversaryLiuLiDehua)]
    internal PromptChoice LiuLiAi(PendingDecision p) => p.Choices[0];
}
