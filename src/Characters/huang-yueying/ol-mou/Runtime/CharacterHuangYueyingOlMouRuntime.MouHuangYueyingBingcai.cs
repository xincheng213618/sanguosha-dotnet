#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingOlMouRuntime
{
    internal const string MouLixianSkill = "mou-ol:lixian", MouBingcaiSkill = "mou-ol:bingcai";
    internal static CardConversionSource MouHuangYueyingSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_huang_yueying_ol_mou_MouHuangYueyingBingcai.MouHuangYueyingSource(f);
    internal static bool IsMouHuangYueyingDamageCardKind(CardKind kind) => RuntimeSkills.RuntimeRules_huang_yueying_ol_mou_MouHuangYueyingBingcai.IsMouHuangYueyingDamageCardKind(kind);
    internal CardKind MouHuangYueyingOwnedKind(CharacterState owner, Card card) => _coreActionQueriesOperations.GetProgramCardIdentityMatches(owner, card).FirstOrDefault()?.Identity.OutputKind ?? card.Kind;
    SkillProgramStepOutcome IMouHuangYueyingHost.ResolveMouHuangYueyingLixian(ProgramSkillFrame frame) => BeginMouLixian(frame);
    SkillProgramStepOutcome IMouHuangYueyingHost.ResolveMouHuangYueyingBingcai(ProgramSkillFrame frame) => BeginMouBingcai(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28961)]
    internal bool CanOfferMouBingcai(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMouHuangYueyingBingcai) || candidate.SkillId == MouBingcaiSkill && candidate.BindingId == "first-basic-recast" && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && TryGetMouBingcaiFirstBasic(context, out var use, out _) && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).ProgramInstances.Where(i => i.SkillId == MouBingcaiSkill).OrderBy(i => i.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault()?.SkillInstanceId == candidate.SkillInstanceId && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouBingcaiStartedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.UseId == use.Id) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])).Any();
    internal SkillProgramStepOutcome BeginMouBingcai(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouBingcai is not null)
        {
            ContinueMouBingcai(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != MouBingcaiSkill || f.TriggerId != "first-basic-recast" || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || f.WindowContext is not { } context || !TryGetMouBingcaiFirstBasic(context, out var use, out var window))
            throw new InvalidOperationException("Bingcai requires the original global first Basic true Use and its frozen committed window.");
        var r = new MouBingcaiReceipt(MouHuangYueyingSource(f), f.GameplayHash, window.Id, use.Id, window.Action.ActionId, use.CardKind)
        {
            Stage = MouBingcaiStage.MaterialChoice,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouBingcai = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouBingcaiStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.UseId, r.ActionId, r.BasicKind));
        PublishMouBingcai(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouBingcaiChoices(ProgramSkillFrame f)
    {
        var r = f.MouBingcai!;
        var choices = new List<PromptChoice>();
        PromptChoice C(string branch, string label, int? card = null, CardKind? kind = null) => new(new($"mou-bingcai.{(f.Id)}.{(branch)}.{(card?.ToString(CultureInfo.InvariantCulture) ?? kind?.ToString() ?? "none")}"), label, card is { } id ? [id] : [], [], new Dictionary<string, string> { ["program-action"] = "mou-bingcai", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["output-kind"] = kind?.ToString() ?? "" });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return[];
        if (r.Stage == MouBingcaiStage.MaterialChoice && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Select(c => C("material", $"重铸【{(c.DisplayName)}】", c.Id)));
            choices.Add(C("cancel", "取消并才"));
        }
        else if (r.Stage == MouBingcaiStage.AddNameChoice && IsPaidMouBingcai(f))
            choices.AddRange(CharacterHuangYueyingOlMouDefinitions.MouLixianAdditionalNames.Except(MouLixianAddedNames(f.OwnerSeat)).Select(kind => C("add-name", $"为理贤添加【{(CardCatalog.Get(kind).DisplayName)}】", kind: kind)));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouBingcai(ProgramSkillFrame f)
    {
        var choices = MouBingcaiChoices(f);
        if (choices.Count == 0)
        {
            FinishMouBingcai(f, false);
            return;
        }

        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, f.MouBingcai!.Stage == MouBingcaiStage.MaterialChoice ? "并才：重铸一张手牌或装备牌" : "并才：为理贤添加一个尚未添加的牌名", f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-bingcai")]
    internal void AnswerMouBingcai(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouBingcai: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], MouBingcaiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Bingcai cannot answer a foreign, stale or unpublished material/name choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["branch"] == "cancel")
        {
            FinishMouBingcai(f, false);
            return;
        }

        if (choice.Parameters["branch"] == "add-name")
        {
            var name = Enum.Parse<CardKind>(choice.Parameters["output-kind"]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLixianNameAddedEvent(f.Id, f.OwnerSeat, name, r.Source, r.GameplayHash));
            FinishMouBingcai(f, true);
            return;
        }

        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Single(c => c.Id == choice.Cards.Single());
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var effectiveKind = MouHuangYueyingOwnedKind(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card);
        f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, f.OwnerSeat, [card.Id]);
        if (f.NativeRecastInvocation is not { } invoice)
            throw new InvalidOperationException("Bingcai requires its real native recast invoice before payment.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouBingcai = r with { Stage = MouBingcaiStage.CostChildren, Paid = true, MaterialId = card.Id, PrintedKind = card.Kind, EffectiveKind = effectiveKind, EffectiveSuit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), EffectiveRank = _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), From = from, RecastInvocationId = invoice.InvocationId, MatchesDamage = IsMouHuangYueyingDamageCardKind(effectiveKind) == IsMouHuangYueyingDamageCardKind(r.BasicKind), IsTrick = CardUseCategoryCatalog.Get(effectiveKind)is CardUseCategories.InstantTrick or CardUseCategories.DelayedTrick, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.DiscardPile, CardMoveReasons.RecastDiscard, (batch, moves) =>
        {
            var m = moves.Single();
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = current.MouBingcai!;
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MouBingcai = paid with { BatchId = batch, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, MovementSequence = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouBingcaiRecastPaidEvent(f.Id, paid.RecastInvocationId, batch, f.OwnerSeat, card.Id, card.Kind, effectiveKind, from, m.Sequence, paid.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, paid.MatchesDamage, paid.IsTrick));
        }, disclosure: CardMovementDisclosure.FaceUp);
        ContinueMouBingcai(f.Id);
    }

    internal bool DrainMouHuangYueying(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueMouBingcai(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouBingcai: { } r } f || f.Id != id || f.NativeDraw is not null)
            return;
        if (!r.Paid)
        {
            PublishMouBingcai(f);
            return;
        }

        if (!IsPaidMouBingcai(f))
            throw new InvalidOperationException("Bingcai lost its original once-paid recast source, material or native invoice.");
        if (r.Stage == MouBingcaiStage.Drawing)
            throw new InvalidOperationException("Bingcai Draw1 must return through its typed native producer.");
        if (DrainMouHuangYueying(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouBingcai!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == MouBingcaiStage.CostChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouBingcai = r with { Stage = MouBingcaiStage.Drawing, DrawBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DrawAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, CardMoveReasons.RecastDraw);
            return;
        }

        if (r.Stage == MouBingcaiStage.DrawChildren)
        {
            if (r.MatchesDamage)
                IssueMouBingcaiExtraEffect(f);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.MouBingcai!;
            if (r.IsTrick && CharacterHuangYueyingOlMouDefinitions.MouLixianAdditionalNames.Except(MouLixianAddedNames(f.OwnerSeat)).Any() && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouBingcai = r with { Stage = MouBingcaiStage.AddNameChoice } });
                PublishMouBingcai(f);
                return;
            }

            FinishMouBingcai(f, false);
            return;
        }

        if (r.Stage == MouBingcaiStage.AddNameChoice)
        {
            PublishMouBingcai(f);
            return;
        }

        throw new InvalidOperationException("Bingcai resumed outside its paid native stage.");
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouHuangYueyingBingcai)]
    internal void ReturnMouBingcaiDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.MouBingcai!;
        if (r.Stage != MouBingcaiStage.Drawing || f.NativeDraw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1 || outcome.SequenceBefore < r.DrawBefore || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Bingcai changed its real once-issued Draw1 or typed return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouBingcai = r with { Stage = MouBingcaiStage.DrawChildren, DrawAttemptFrameId = outcome.AttemptFrameId ?? 0, DrawBefore = outcome.SequenceBefore, DrawAfter = outcome.SequenceAfter, ActualDrawIds = outcome.ActualDrawIds, ReplacementGainedIds = outcome.ReplacementGainedIds }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(f.OwnerSeat, r.MaterialId!.Value, r.EffectiveKind!.Value, outcome.ActualDrawIds.Count));
        f = _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(f, outcome.ActualDrawIds.Count);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouBingcaiDrawReturnedEvent(f.Id, outcome.AttemptFrameId ?? 0, outcome.SequenceBefore, outcome.SequenceAfter, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count));
        ContinueMouBingcai(f.Id);
    }

    internal void FinishMouBingcai(ProgramSkillFrame f, bool added)
    {
        var r = f.MouBingcai!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouBingcaiCompletedEvent(f.Id, r.UseId, r.Paid, r.Paid && r.MatchesDamage, added));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouBingcai = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, r.Paid);
    }
}
