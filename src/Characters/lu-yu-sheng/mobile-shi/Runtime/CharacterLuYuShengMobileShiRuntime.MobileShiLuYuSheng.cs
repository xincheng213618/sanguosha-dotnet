#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuYuShengMobileShiRuntime
{
    internal const string ShiLysRunweiSkill = "mobile:shi-runwei", ShiLysShuanghuaiSkill = "mobile:shi-shuanghuai";
    internal static CardConversionSource ShiLysSource(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    internal global::CardGame.Core.GameEngine.MobileShiLuYuShengTrack MobileShiLuYuShengTrackAt(int seat)
    {
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        var all = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiRunweiGrantIssuedEvent>().Where(e => e.Source.OwnerSeat == seat && e.ActualTurn == _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber).ToArray();
        var grant = all.FirstOrDefault(e => e.Phase == phase && e.GainedCount > 0);
        var marked = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiRunweiCardGrantedEvent>().Where(e => e.OwnerSeat == seat && e.TargetSeat == seat && e.Phase == phase).Select(e => e.CardId).Distinct().ToArray();
        var refunded = phase is not null && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiRunweiDebtRefundedEvent>().Any(e => e.Source.OwnerSeat == seat && e.Phase == phase);
        var progress = grant is null ? 0 : _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > grant.SequenceAfter && m.TurnNumber == _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber && m.From.OwnerSeat == seat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment);
        return new(phase, all.Where(e => e.GainedCount > 0).Select(e => e.TargetSeat).Distinct().ToArray(), marked, grant?.GainedCount ?? 0, progress, refunded);
    }

    internal global::CardGame.Core.GameEngine.MobileShiLuYuShengTrack? PeekMobileShiLuYuSheng(int seat) => MobileShiLuYuShengTrackAt(seat);
    internal MobileShiRunweiGrantIssuedEvent? ShiLysRunweiGrant(long id) => _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiRunweiGrantIssuedEvent>().SingleOrDefault(e => e.FrameId == id);
    internal IReadOnlyList<ActualPhaseEndItem> CollectShiLysPhaseEndCandidates(ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiRunweiGrantIssuedEvent>().Where(e => e.Phase == phase && e.TargetSeat == e.Source.OwnerSeat && e.GainedCount > 0 && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive).GroupBy(e => (e.Source.OwnerSeat, e.Source.SkillInstanceId)).Select(g => g.First()).Where(e => !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiRunweiPhaseEndConsumedEvent>().Any(c => c.GrantId == e.FrameId && c.Phase == phase)).Select(e => new ActualPhaseEndItem(new(e.Source.OwnerSeat, ShiLysRunweiSkill, MobileShiLuYuShengComposition.RunweiPhaseEndTrigger, e.Source.SkillInstanceId, e.GameplayHash, 0), e.FrameId)).ToArray();
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 67400)]
    internal bool IsIssuedShiLysPhaseEnd(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == ShiLysRunweiSkill && c.BindingId == MobileShiLuYuShengComposition.RunweiPhaseEndTrigger && _coreActualPhaseEndsOperations.IsExactActualPhaseEndCandidate(c, context) && context.ActualPhaseEnd is { } due && ShiLysRunweiGrant(due.EntitlementId)is { } grant && grant.Source.OwnerSeat == c.OwnerSeat && grant.Source.SkillInstanceId == c.SkillInstanceId && grant.GameplayHash == c.GameplayHash && grant.Phase == due.Phase && grant.TargetSeat == c.OwnerSeat && grant.GainedCount > 0;
    SkillProgramStepOutcome global::CardGame.Core.IMobileShiLuYuShengProgramHost.ResolveRunweiGrant(ProgramSkillFrame frame) => BeginMobileShiRunweiGrant(frame);
    SkillProgramStepOutcome global::CardGame.Core.IMobileShiLuYuShengProgramHost.ResolveRunweiLossDebt(ProgramSkillFrame frame) => ResolveMobileShiRunweiLossDebt(frame);
    SkillProgramStepOutcome global::CardGame.Core.IMobileShiLuYuShengProgramHost.ResolveRunweiPhaseEnd(ProgramSkillFrame frame) => ResolveMobileShiRunweiPhaseEnd(frame);
    SkillProgramStepOutcome global::CardGame.Core.IMobileShiLuYuShengProgramHost.ResolveShuanghuaiGuard(ProgramSkillFrame frame) => BeginMobileShiShuanghuaiGuard(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 67400)]
    internal bool CanOfferMobileShiLuYuSheng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId == ShiLysRunweiSkill)
        {
            var track = PeekMobileShiLuYuSheng(candidate.OwnerSeat);
            if (trigger.Id == MobileShiLuYuShengComposition.RunweiLossDebtTrigger)
                return _coreActualTurnProgressPhaseTurnNumberAccess._phase == TurnPhase.Play && track is { DebtRequired: > 0, Refunded: false } && context.MovementBatch?.DiscardRecoveryPhase == track.Phase;
            if (trigger.Id == MobileShiLuYuShengComposition.RunweiPhaseEndTrigger)
                return IsIssuedShiLysPhaseEnd(candidate, context);
            return true;
        }

        if (candidate.SkillId != ShiLysShuanghuaiSkill || trigger.Id != MobileShiLuYuShengComposition.ShuanghuaiGuardTrigger || context is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } victimSeat })
            return true;
        return victimSeat != candidate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[victimSeat].IsAlive && _coreNativeRuleQueriesOperations.GetCombatDistance(candidate.OwnerSeat, victimSeat) == 1;
    }

    internal int[] MobileShiRunweiRecipientSeats(ProgramSkillFrame f)
    {
        var track = PeekMobileShiLuYuSheng(f.OwnerSeat);
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (track is null || !track.Refunded || !track.GainedSeats.Contains(p.Seat))).Select(p => p.Seat).ToArray();
    }

    internal void RefundMobileShiRunweiPhaseUse(int ownerSeat)
    {
        var activation = _contentRegistryOperations._contentRegistry.GetSkill(ShiLysRunweiSkill).Program!.Activations.Single(item => item.Id == MobileShiLuYuShengComposition.RunweiActivation);
        var key = (ownerSeat, ShiLysRunweiSkill, activation.UsageGroup);
        _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses[key] = Math.Max(0, _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault(key) - 1);
    }

    internal SkillProgramStepOutcome BeginMobileShiRunweiGrant(ProgramSkillFrame frame)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (f.MobileShiLuYuSheng is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id)
            throw new InvalidOperationException("Runwei requires its original unpaid instruction.");
        if (f.SkillId != ShiLysRunweiSkill || f.ActivationId != MobileShiLuYuShengComposition.RunweiActivation || f.TriggerId != null)
            throw new InvalidOperationException("Runwei requires its exact reveal-grant activation.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var pileCount = _cardZonesOperations._cardZones.Count(CardLocation.DrawPile);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        if (pileCount == 0 || MobileShiRunweiRecipientSeats(f).Length == 0)
        {
            // Nothing can be revealed or nobody is targetable: refund the spent
            // activation exactly like the friend-event maximum-cost refund.
            RefundMobileShiRunweiPhaseUse(f.OwnerSeat);
            return SkillProgramStepOutcome.Continue;
        }

        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        if (phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } || phase.ActorSeat != f.OwnerSeat)
            throw new InvalidOperationException("Runwei needs its original actual Play phase.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = new("runwei", _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber) { Source = ShiLysSource(f), GameplayHash = f.GameplayHash, Phase = phase } });
        ContinueMobileShiRunwei(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static bool ShiLysIsRed(Card card) => card.Suit is Suit.Heart or Suit.Diamond;
    internal void ContinueMobileShiRunwei(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiLuYuSheng: { Operation: "runwei" } r } f || f.Id != id)
            return;
        if (r.Stage == 0)
        {
            var pileCount = _cardZonesOperations._cardZones.Count(CardLocation.DrawPile);
            var counts = Enumerable.Range(1, Math.Min(5, pileCount)).ToArray();
            if (counts.Length == 0)
            {
                FinishShiLys(f);
                return;
            }

            if (counts.Length == 1)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { Stage = 1, RevealedCount = counts[0] } });
                ContinueMobileShiRunwei(id);
                return;
            }

            PublishShiLysRunweiCount(f, counts);
            return;
        }

        if (r.Stage == 1)
        {
            _coreProgramLifecycleOperations.RevealProgramTopCards(f.Id, f.OwnerSeat, r.RevealedCount, null, MobileShiLuYuShengComposition.RunweiRevealBind, SkillProgramCardSetVisibility.Public);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.MobileShiLuYuSheng!;
            var recipients = MobileShiRunweiRecipientSeats(f);
            if (recipients.Length == 0)
            {
                DisposeShiLysRunweiLeftovers(f, []);
                return;
            }

            if (recipients.Length == 1)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { Stage = 2, TargetSeat = recipients[0] } });
                ContinueMobileShiRunwei(id);
                return;
            }

            PublishShiLysRunweiRecipient(f, recipients);
            return;
        }

        if (r.Stage == 2)
        {
            var cards = ShiLysRunweiRevealedCards(f);
            var hasRed = cards.Any(pair => ShiLysIsRed(pair.Card));
            var hasBlack = cards.Any(pair => !ShiLysIsRed(pair.Card));
            if (hasRed == hasBlack)
            {
                PublishShiLysRunweiColor(f, hasRed, hasBlack);
                return;
            }

            ApplyMobileShiRunweiGrant(f, hasRed);
            return;
        }

        if (r.Stage == 9)
        {
            if (DrainShiLysLuYuSheng(f))
                return;
            FinishShiLys(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return;
        }

        throw new InvalidOperationException("Runwei has an unknown native continuation stage.");
    }

    internal (Card Card, CardLocation Location)[] ShiLysRunweiRevealedCards(ProgramSkillFrame f)
    {
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(f, MobileShiLuYuShengComposition.RunweiRevealBind);
        var pairs = new (Card, CardLocation)[set.CardIds.Count];
        for (var i = 0; i < set.CardIds.Count; i++)
        {
            var location = set.SourceLocations[i];
            if (_cardZonesOperations._cardZones.GetLocation(set.CardIds[i]) != location)
                throw new InvalidOperationException("Runwei revealed cards left their frozen processing positions.");
            pairs[i] = (_cardZonesOperations._cardZones.CardsAt(location).Single(card => card.Id == set.CardIds[i]), location);
        }

        return pairs;
    }

    internal void DisposeShiLysRunweiLeftovers(ProgramSkillFrame f, IReadOnlyList<Card> granted)
    {
        var cards = ShiLysRunweiRevealedCards(f);
        var kept = granted.Select(card => card.Id).ToHashSet();
        var leftovers = cards.Where(pair => !kept.Contains(pair.Card.Id)).ToArray();
        var r = f.MobileShiLuYuSheng!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { Stage = 9 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        StampShiLysBefore(f.Id);
        if (leftovers.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(leftovers.Select(p => p.Card.Id).ToArray(), CardLocation.DiscardPile, new(MobileShiLuYuShengComposition.RunweiLeftoverMoveReason));
        SealShiLys(f.Id);
        ContinueMobileShiRunwei(f.Id);
    }

    internal void ApplyMobileShiRunweiGrant(ProgramSkillFrame f, bool red)
    {
        var r = f.MobileShiLuYuSheng!;
        var cards = ShiLysRunweiRevealedCards(f);
        var matched = cards.Where(pair => ShiLysIsRed(pair.Card) == red).Select(p => p.Card.Id).ToArray();
        var leftovers = cards.Where(pair => ShiLysIsRed(pair.Card) != red).Select(p => p.Card.Id).ToArray();
        if (r.Phase is null || !MobileShiRunweiRecipientSeats(f).Contains(r.TargetSeat) || matched.Length == 0)
            throw new InvalidOperationException("Runwei lost its original phase, legal recipient or chosen real color group.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { Stage = 9, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(matched, CardLocation.Hand(r.TargetSeat), new(MobileShiLuYuShengComposition.RunweiGrantMoveReason), (batch, moves) =>
        {
            var gained = moves.Where(m => m.To == CardLocation.Hand(r.TargetSeat)).ToArray();
            foreach (var m in gained)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiRunweiCardGrantedEvent(f.Id, f.OwnerSeat, r.TargetSeat, m.CardId, r.Phase));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiRunweiGrantIssuedEvent(f.Id, ShiLysSource(f), f.GameplayHash, _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber, r.TargetSeat, cards.Length, gained.Length, red) { Phase = r.Phase, SequenceAfter = moves.Max(m => m.Sequence) });
        });
        if (leftovers.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(leftovers, CardLocation.DiscardPile, new(MobileShiLuYuShengComposition.RunweiLeftoverMoveReason));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 润微：展示 {(cards.Length)} 张，令 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 获得其中{((red ? "红色" : "黑色"))}牌。", f.OwnerSeat, r.TargetSeat);
        SealShiLys(f.Id);
        ContinueMobileShiRunwei(f.Id);
    }

    internal IReadOnlyList<PromptChoice> ShiLysRunweiCountChoices(ProgramSkillFrame f, int[] counts) => Array.AsReadOnly(counts.Select(count => new PromptChoice(new($"shi-lu-yu-sheng.{(f.Id)}.n{(count)}"), $"展示牌堆顶 {(count)} 张牌", [], [], new Dictionary<string, string> { ["program-action"] = "mobile-shi-runwei", ["branch"] = "count", ["count"] = count.ToString(CultureInfo.InvariantCulture) })).ToArray());
    internal void PublishShiLysRunweiCount(ProgramSkillFrame f, int[] counts)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "润微：选择展示牌堆顶的牌数。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = ShiLysRunweiCountChoices(f, counts),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal IReadOnlyList<PromptChoice> ShiLysRunweiRecipientChoices(ProgramSkillFrame f, int[] seats) => Array.AsReadOnly(seats.Select(seat => new PromptChoice(new($"shi-lu-yu-sheng.{(f.Id)}.r{(seat)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 获得展示牌中一种颜色的所有牌", [], [seat], new Dictionary<string, string> { ["program-action"] = "mobile-shi-runwei", ["branch"] = "target", ["seat"] = seat.ToString(CultureInfo.InvariantCulture) })).ToArray());
    internal void PublishShiLysRunweiRecipient(ProgramSkillFrame f, int[] seats)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "润微：选择一名角色获得展示牌中一种颜色的所有牌。", [], seats, f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = ShiLysRunweiRecipientChoices(f, seats),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal IReadOnlyList<PromptChoice> ShiLysRunweiColorChoices(ProgramSkillFrame f, bool hasRed, bool hasBlack)
    {
        var cards = ShiLysRunweiRevealedCards(f);
        List<PromptChoice> choices = [];
        if (hasRed)
            choices.Add(new(new($"shi-lu-yu-sheng.{(f.Id)}.red"), $"令其获得红色牌（{(cards.Count(pair => ShiLysIsRed(pair.Card)))} 张）", [], [], new Dictionary<string, string> { ["program-action"] = "mobile-shi-runwei", ["branch"] = "color", ["color"] = "red" }));
        if (hasBlack)
            choices.Add(new(new($"shi-lu-yu-sheng.{(f.Id)}.black"), $"令其获得黑色牌（{(cards.Count(pair => !ShiLysIsRed(pair.Card)))} 张）", [], [], new Dictionary<string, string> { ["program-action"] = "mobile-shi-runwei", ["branch"] = "color", ["color"] = "black" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishShiLysRunweiColor(ProgramSkillFrame f, bool hasRed, bool hasBlack)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "润微：选择令其获得哪种颜色的牌。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = ShiLysRunweiColorChoices(f, hasRed, hasBlack),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mobile-shi-runwei")]
    internal void AnswerMobileShiRunwei(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiLuYuSheng: { Operation: "runwei" } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat)
            throw new InvalidOperationException("Runwei choice lost its original frame or chooser.");
        var branch = selected.Parameters["branch"];
        IReadOnlyList<PromptChoice> current = branch switch
        {
            "count" => ShiLysRunweiCountChoices(f, Enumerable.Range(1, Math.Min(5, _cardZonesOperations._cardZones.Count(CardLocation.DrawPile))).ToArray()),
            "target" => ShiLysRunweiRecipientChoices(f, MobileShiRunweiRecipientSeats(f)),
            "color" => ShiLysRunweiColorChoices(f, ShiLysRunweiRevealedCards(f).Any(pair => ShiLysIsRed(pair.Card)), ShiLysRunweiRevealedCards(f).Any(pair => !ShiLysIsRed(pair.Card))),
            _ => throw new InvalidOperationException("Runwei choice lost its known native branch.")};
        if (r.Stage != (branch == "count" ? 0 : branch == "target" ? 1 : 2) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], current.Where(choice => choice.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Runwei choice lost its unpaid stage or legal candidates.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (branch)
        {
            case "count":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { Stage = 1, RevealedCount = int.Parse(selected.Parameters["count"], CultureInfo.InvariantCulture) } });
                break;
            case "target":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { Stage = 2, TargetSeat = int.Parse(selected.Parameters["seat"], CultureInfo.InvariantCulture) }, ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding("runwei-recipient", selected.Parameters["seat"], f.OwnerSeat)).ToArray()) });
                break;
            default:
                var red = selected.Parameters["color"] == "red";
                ApplyMobileShiRunweiGrant(f, red);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                return;
        }

        ContinueMobileShiRunwei(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal SkillProgramStepOutcome ResolveMobileShiRunweiLossDebt(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var track = MobileShiLuYuShengTrackAt(f.OwnerSeat);
        if (f.SkillId != ShiLysRunweiSkill || f.TriggerId != MobileShiLuYuShengComposition.RunweiLossDebtTrigger || f.WindowContext?.MovementBatch is not { } batch)
            throw new InvalidOperationException("Runwei refund needs the exact native owner-loss batch.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreActualTurnProgressPhaseTurnNumberAccess._phase != TurnPhase.Play || batch.DiscardRecoveryPhase != track.Phase || track.Phase?.ActorSeat != f.OwnerSeat || track.Refunded || track.DebtRequired <= 0 || track.DebtProgress < track.DebtRequired)
            return SkillProgramStepOutcome.Continue;
        RefundMobileShiRunweiPhaseUse(f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiRunweiDebtRefundedEvent(f.Id, ShiLysSource(f), f.GameplayHash, _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber, track.DebtRequired, track.DebtProgress) { Phase = track.Phase });
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 润微：本阶段失去牌数已达到 X={(track.DebtRequired)}，本阶段唯一一次重新发动机会已解锁。", f.OwnerSeat);
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome ResolveMobileShiRunweiPhaseEnd(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0);
        if (f.WindowContext is not { } context || !IsIssuedShiLysPhaseEnd(candidate, context) || context.ActualPhaseEnd is not { } due || ShiLysRunweiGrant(due.EntitlementId)is not { Phase: { } phase } grant)
            throw new InvalidOperationException("Runwei cleanup requires its original issued actual-phase grant.");
        var marked = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiRunweiCardGrantedEvent>().Where(e => e.OwnerSeat == f.OwnerSeat && e.TargetSeat == f.OwnerSeat && e.Phase == phase && ShiLysRunweiGrant(e.FrameId)?.Source.SkillInstanceId == f.SkillInstanceId).Select(e => e.CardId).Distinct().ToArray();
        var allowed = _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableOwnedHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => c.Id).ToHashSet();
        var hands = marked.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat) && allowed.Contains(id)).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = new("runwei-end", _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber, Stage: 9, Discarded: hands.Length) { Source = grant.Source, GameplayHash = grant.GameplayHash, Phase = phase, GrantId = grant.FrameId, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiRunweiPhaseEndConsumedEvent(f.Id, grant.FrameId, phase));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiRunweiPhaseEndDiscardedEvent(f.Id, grant.Source, grant.GameplayHash, _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber, hands.Length, marked.Length - hands.Length) { Phase = phase });
        if (hands.Length > 0)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, hands, new(MobileShiLuYuShengComposition.RunweiPhaseEndMoveReason));
        SealShiLys(f.Id);
        ResumeShiLysLuYuSheng(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void StampShiLysBefore(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShiLuYuSheng = f.MobileShiLuYuSheng!with { Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal void SealShiLys(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShiLuYuSheng = f.MobileShiLuYuSheng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool DrainShiLysLuYuSheng(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal SkillProgramStepOutcome BeginMobileShiShuanghuaiGuard(ProgramSkillFrame frame)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (f.MobileShiLuYuSheng is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id)
            throw new InvalidOperationException("Shuanghuai requires its original unpaid instruction.");
        if (f.SkillId != ShiLysShuanghuaiSkill || f.TriggerId != MobileShiLuYuShengComposition.ShuanghuaiGuardTrigger || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } victim })
            throw new InvalidOperationException("Shuanghuai requires its exact before-damage window.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = new("shuanghuai", _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber, TargetSeat: victim) { Source = ShiLysSource(f), GameplayHash = f.GameplayHash } });
        ContinueMobileShiShuanghuai(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal BeforeDamageProgramWindowFrame? ShiLysGuardParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } victim, Amount: > 0 } c || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not BeforeDamageProgramWindowFrame w || w.TargetSeat != victim || w.Amount != c.Amount || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex].Candidate is not { } candidate || candidate.OwnerSeat != f.OwnerSeat || candidate.SkillId != f.SkillId || candidate.BindingId != f.TriggerId || candidate.SkillInstanceId != f.SkillInstanceId || candidate.GameplayHash != f.GameplayHash)
            return null;
        return w;
    }

    internal bool ShiLysShuanghuaiDamagePending(ProgramSkillFrame f) => ShiLysGuardParent(f)is { Prevented: false } w && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attempt && attempt.TargetSeat == f.MobileShiLuYuSheng!.TargetSeat && attempt.ResolutionId == (w.ContinuationAttackResolutionId ?? w.ParentFrameId);
    internal bool ShiLysShuanghuaiPeachAvailable() => _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Any(card => card.Kind == CardKind.Peach);
    internal IReadOnlyList<PromptChoice> ShiLysShuanghuaiChoices(ProgramSkillFrame f)
    {
        var r = f.MobileShiLuYuSheng!;
        List<PromptChoice> choices = [];
        if (ShiLysShuanghuaiDamagePending(f))
            choices.Add(new(new($"shi-lu-yu-sheng.{(f.Id)}.prevent"), "防止此伤害", [], [], new Dictionary<string, string> { ["program-action"] = "mobile-shi-shuanghuai", ["option"] = "prevent" }));
        if (_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && ShiLysShuanghuaiPeachAvailable())
            choices.Add(new(new($"shi-lu-yu-sheng.{(f.Id)}.peach"), $"令 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 从弃牌堆中获得一张【桃】", [], [], new Dictionary<string, string> { ["program-action"] = "mobile-shi-shuanghuai", ["option"] = "peach" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishShiLysShuanghuaiChoice(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var r = f.MobileShiLuYuSheng!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"霜怀：选择对 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 受到的伤害的效果。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = ShiLysShuanghuaiChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mobile-shi-shuanghuai")]
    internal void AnswerMobileShiShuanghuai(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiLuYuSheng: { Operation: "shuanghuai", Stage: 0 } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], ShiLysShuanghuaiChoices(f).Where(choice => choice.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Shuanghuai choice lost its original frame, chooser or legal option.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        BindShiLysShuanghuaiOption(f, selected.Parameters["option"]);
        ContinueMobileShiShuanghuai(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void BindShiLysShuanghuaiOption(ProgramSkillFrame f, string option)
    {
        var r = f.MobileShiLuYuSheng!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { Stage = 9, CardId = option == "peach" ? -1 : 0, SameAsLast = SameShiLysShuanghuaiTarget(f) }, ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding("shuanghuai-option", option, f.OwnerSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiShuanghuaiGuardIssuedEvent(f.Id, ShiLysSource(f), f.GameplayHash, _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber, r.TargetSeat, option, SameShiLysShuanghuaiTarget(f)));
    }

    internal bool SameShiLysShuanghuaiTarget(ProgramSkillFrame f) => _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiShuanghuaiGuardIssuedEvent>().LastOrDefault(e => e.Source.OwnerSeat == f.OwnerSeat && e.FrameId != f.Id)?.VictimSeat == f.MobileShiLuYuSheng!.TargetSeat;
    internal void ContinueMobileShiShuanghuai(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiLuYuSheng: { Operation: "shuanghuai" } r } f || f.Id != id)
            return;
        if (r.Stage == 0)
        {
            var choices = ShiLysShuanghuaiChoices(f);
            if (choices.Count == 0)
            {
                FinishShiLys(f);
                return;
            }

            if (choices.Count == 1)
            {
                BindShiLysShuanghuaiOption(f, choices[0].Parameters["option"]);
                ContinueMobileShiShuanghuai(id);
                return;
            }

            PublishShiLysShuanghuaiChoice(f);
            return;
        }

        if (r.Stage == 9)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r = r with { Stage = 10, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (r.CardId == 0)
            {
                if (ShiLysShuanghuaiDamagePending(f))
                    _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
            }
            else if (ShiLysShuanghuaiPeachAvailable() && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            {
                var peach = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Reverse().First(c => c.Kind == CardKind.Peach);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { CardId = peach.Id } });
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([peach.Id], CardLocation.Hand(r.TargetSeat), new(MobileShiLuYuShengComposition.ShuanghuaiPeachMoveReason));
            }

            SealShiLys(id);
            ContinueMobileShiShuanghuai(id);
            return;
        }

        if (r.Stage is 10 or 11 or 13)
        {
            if (DrainShiLysLuYuSheng(f))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
            r = f.MobileShiLuYuSheng!;
            if (r.Stage == 11)
            {
                FinishShiLys(f);
                return;
            }

            if (r.SameAsLast)
            {
                if (r.DrawCursor >= 2 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                {
                    FinishShiLys(f);
                    return;
                }

                var recipient = r.DrawCursor == 0 ? f.OwnerSeat : r.TargetSeat;
                if (!_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShiLuYuSheng = r with { Stage = 13, DrawCursor = r.DrawCursor + 1 } });
                    ContinueMobileShiShuanghuai(id);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { Stage = 12, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, recipient, 1, new(MobileShiLuYuShengComposition.ShuanghuaiDrawMoveReason));
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishShiLys(f);
                return;
            }

            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IdentityConcealed)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { Stage = 11, HpAppearance = true } });
                _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(id, f.SkillId, f.OwnerSeat, 1);
                return;
            }

            var result = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(id, f.SkillId, f.OwnerSeat, 1, (before, after, lost) =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { MobileShiLuYuSheng = active.MobileShiLuYuSheng!with { Stage = 11, HpPaid = true } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiShuanghuaiHpPaidEvent(id, f.OwnerSeat, before, after));
            });
            if (result == SkillProgramStepOutcome.AwaitChild)
                return;
            ContinueMobileShiShuanghuai(id);
            return;
        }

        if (r.Stage == 12)
            return;
        throw new InvalidOperationException("Shuanghuai has an unknown native continuation stage.");
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiShuanghuaiGuard)]
    internal void ReturnShiLysShuanghuaiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.MobileShiLuYuSheng is not { Operation: "shuanghuai", Stage: 12, SameAsLast: true } r || outcome.RequestedCount != 1 || outcome.RecipientSeat != (r.DrawCursor == 0 ? f.OwnerSeat : r.TargetSeat) || r.DrawCursor is < 0 or > 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiShuanghuaiDrawReturnedEvent>().Any(e => e.FrameId == f.Id && e.Cursor == r.DrawCursor))
            throw new InvalidOperationException("Shuanghuai draw lost its exact once-issued real recipient or native return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiLuYuSheng = r with { Stage = 13, DrawCursor = r.DrawCursor + 1, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiShuanghuaiDrawReturnedEvent(f.Id, r.DrawCursor, outcome.RecipientSeat, outcome.ActualDrawIds.Count));
        ContinueMobileShiShuanghuai(f.Id);
    }

    internal void FinishShiLys(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 67400)]
    internal bool ResumeShiLysLuYuSheng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiLuYuSheng: { } r } f || f.Id != id)
            return false;
        if (r.Operation == "shuanghuai")
            ContinueMobileShiShuanghuai(id);
        else if (r.Operation == "runwei")
            ContinueMobileShiRunwei(id);
        else
        {
            if (DrainShiLysLuYuSheng(f))
                return true;
            FinishShiLys(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        }

        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 67400)]
    internal bool ReturnShiLysLuYuShengMovement(ProgramSkillFrame f)
    {
        if (f.MobileShiLuYuSheng is not { Operation: var operation })
            return false;
        if (operation == "shuanghuai")
            ContinueMobileShiShuanghuai(f.Id);
        else if (operation == "runwei")
            ContinueMobileShiRunwei(f.Id);
        else
        {
            if (DrainShiLysLuYuSheng(f))
                return true;
            FinishShiLys(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        }

        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 67400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 67400)]
    internal bool ContinuePaidShiLysLuYuSheng(ProgramSkillFrame f) => f.MobileShiLuYuSheng is { Stage: > 0 } || f.InstructionIndex == 0 && f.TriggerId == MobileShiLuYuShengComposition.RunweiPhaseEndTrigger && f.WindowContext is { } c && IsIssuedShiLysPhaseEnd(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), c);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 67400)]
    internal void AssertShiLysLuYuSheng(ProgramSkillFrame f)
    {
        if (f.MobileShiLuYuSheng is not { } r)
            return;
        if (r.Source is null || r.Source.OwnerSeat != f.OwnerSeat || r.Source.SkillId != f.SkillId || r.Source.SkillInstanceId != f.SkillInstanceId || r.GameplayHash != f.GameplayHash || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualTurn != _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber || r.Operation is not ("runwei" or "runwei-end" or "shuanghuai"))
            throw new InvalidOperationException("Shi Lu Yusheng lost her original typed trigger or paid stage receipt.");
        if (r.Operation != "runwei-end" && r.Source != ShiLysSource(f))
            throw new InvalidOperationException("Shi Lu Yusheng changed its exact original binding source.");
        if (r.Operation == "runwei" && (r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } || r.Phase.ActorSeat != f.OwnerSeat || f.SkillId != ShiLysRunweiSkill || f.ActivationId != MobileShiLuYuShengComposition.RunweiActivation || r.Stage is not (0 or 1 or 2 or 9) || r.RevealedCount is> 5 || r.Stage > 0 && r.RevealedCount < 1 || r.Stage is 2 or 9 && !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat)))
            throw new InvalidOperationException("Runwei lost its reveal-grant activation receipt.");
        if (r.Operation == "runwei-end" && (f.SkillId != ShiLysRunweiSkill || f.TriggerId != MobileShiLuYuShengComposition.RunweiPhaseEndTrigger || r.Stage != 9 || r.TargetSeat != -1 || r.Discarded < 0))
            throw new InvalidOperationException("Runwei phase end lost its locked trigger receipt.");
        if (r.Operation == "runwei-end" && (r.Phase is null || f.WindowContext?.ActualPhaseEnd?.EntitlementId != r.GrantId || ShiLysRunweiGrant(r.GrantId)is not { } grant || grant.Phase != r.Phase || grant.Source != r.Source))
            throw new InvalidOperationException("Runwei cleanup lost its exact original granted phase obligation.");
        if (r.Operation == "shuanghuai" && (f.SkillId != ShiLysShuanghuaiSkill || f.TriggerId != MobileShiLuYuShengComposition.ShuanghuaiGuardTrigger || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || ShiLysGuardParent(f)is null || f.WindowContext?.TargetSeat != r.TargetSeat || r.Stage is not (0 or 9 or 10 or 11 or 12 or 13)))
            throw new InvalidOperationException("Shuanghuai lost its before-damage guard receipt.");
        if (r.Operation == "shuanghuai" && r.Stage > 0 && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiShuanghuaiGuardIssuedEvent>().Any(issued => issued.FrameId == f.Id && issued.Source == ShiLysSource(f) && issued.GameplayHash == f.GameplayHash && issued.ActualTurn == r.ActualTurn && issued.VictimSeat == r.TargetSeat))
            throw new InvalidOperationException("Shuanghuai lost its issued guard obligation.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiRunweiGrant)]
    internal PromptChoice ChooseShiLysRunwei(PendingDecision prompt) => prompt.Choices.OrderByDescending(choice => choice.Parameters.TryGetValue("count", out var count) && int.TryParse(count, out var parsed) ? parsed : 0).FirstOrDefault() ?? prompt.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiShuanghuaiGuard)]
    internal PromptChoice ChooseShiLysShuanghuai(PendingDecision prompt) => prompt.Choices[0];
    internal bool IsMobileShiLuYuShengChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding) => frame.MobileShiLuYuSheng is { } r && binding.ChooserSeat == frame.OwnerSeat && (binding.Name == "runwei-recipient" && r.Operation == "runwei" && r.Stage >= 2 && r.TargetSeat >= 0 && binding.OptionId == r.TargetSeat.ToString(CultureInfo.InvariantCulture) || binding.Name == "shuanghuai-option" && r.Operation == "shuanghuai" && r.Stage > 0 && binding.OptionId is ("prevent" or "peach"));
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiLuYuShengPublicStates(CharacterState owner, string skillId)
    {
        if (skillId is not (ShiLysRunweiSkill or ShiLysShuanghuaiSkill))
            return[];
        var result = new List<ProgramPublicRuleStateSnapshot>();
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId))
        {
            if (skillId == ShiLysRunweiSkill)
            {
                var state = MobileShiLuYuShengTrackAt(owner.Seat);
                result.Add(new(instance.SkillInstanceId, "runwei-required", ProgramPublicRuleStateKind.GameActivationProgress, state.DebtRequired));
                result.Add(new(instance.SkillInstanceId, "runwei-progress", ProgramPublicRuleStateKind.GameActivationProgress, Math.Min(state.DebtRequired, state.DebtProgress)));
                result.Add(new(instance.SkillInstanceId, "runwei-refunded", ProgramPublicRuleStateKind.GameActivationProgress, state.Refunded ? 1 : 0));
                result.Add(new(instance.SkillInstanceId, "runwei-own-hand", ProgramPublicRuleStateKind.GameActivationProgress, state.MarkedCards.Count(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(owner.Seat))));
            }
            else
            {
                var last = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiShuanghuaiGuardIssuedEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner.Seat);
                result.Add(new(instance.SkillInstanceId, "shuanghuai-last-target", ProgramPublicRuleStateKind.GameActivationProgress, (last?.VictimSeat ?? -1) + 1));
                result.Add(new(instance.SkillInstanceId, "shuanghuai-turn-used", ProgramPublicRuleStateKind.GameActivationProgress, _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, skillId, MobileShiLuYuShengComposition.ShuanghuaiGuardTrigger + "@" + instance.SkillInstanceId, SkillUsageScope.Turn)));
            }
        }

        return result.ToArray();
    }
}
