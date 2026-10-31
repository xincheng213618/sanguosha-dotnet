#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChengPuMobileShiRuntime
{
    internal const string ShiCpDuzuoSkill = "mobile:shi-duzuo", ShiCpBihanSkill = "mobile:shi-bihan";
    internal const string ShiCpDuzuoGrantMoveReason = "skill-program.mobile:shi-duzuo.fire-slash-grant";
    internal const string ShiCpBihanDiscardMoveReason = "skill-program.mobile:shi-bihan.discard";
    internal static CardConversionSource ShiCpSource(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IMobileShiChengPuProgramHost.ResolveDuzuoGrant(ProgramSkillFrame frame) => BeginMobileShiDuzuoGrantProgram(frame);
    SkillProgramStepOutcome global::CardGame.Core.IMobileShiChengPuProgramHost.ResolveBihanGuard(ProgramSkillFrame frame) => BeginMobileShiBihanGuardProgram(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 67300)]
    internal bool CanOfferMobileShiChengPu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId == ShiCpDuzuoSkill)
            return trigger.Id == MobileShiChengPuComposition.DuzuoGrantTrigger && MobileShiChengPuFireSlash()is not null;
        if (candidate.SkillId != ShiCpBihanSkill || trigger.Id != MobileShiChengPuComposition.BihanGuardTrigger || context is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } victimSeat })
            return true;
        return new[]
        {
            candidate.OwnerSeat,
            victimSeat
        }.Distinct().Any(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count > _coreMatchParticipantsPlayersAccess._players[seat].Hp);
    }

    internal Card? MobileShiChengPuFireSlash() => _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(card => card.Kind == CardKind.FireSlash) ?? _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Reverse().FirstOrDefault(card => card.Kind == CardKind.FireSlash);
    internal SkillProgramStepOutcome BeginMobileShiDuzuoGrantProgram(ProgramSkillFrame frame)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (f.MobileShiChengPu is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id)
            throw new InvalidOperationException("Duzuo requires its original unpaid instruction.");
        if (f.TriggerId != MobileShiChengPuComposition.DuzuoGrantTrigger || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardsGained, MovementBatch: not null })
            throw new InvalidOperationException("Duzuo requires its exact excluded hand-gain window.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId) || MobileShiChengPuFireSlash()is null)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChengPu = new("duzuo", _coreActualTurnProgressTurnNumberAccess._turnNumber) });
        ContinueMobileShiDuzuo(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueMobileShiDuzuo(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiChengPu: { Operation: "duzuo" } r } f || f.Id != id)
            return;
        if (r.Stage == 9)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        if (r.TargetSeat < 0)
        {
            var candidates = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray();
            if (candidates.Length == 1)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChengPu = r with { TargetSeat = candidates[0] } });
                ContinueMobileShiDuzuo(id);
                return;
            }

            PublishShiCpDuzuoRecipient(f);
            return;
        }

        var recipient = r.TargetSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || MobileShiChengPuFireSlash()is not { } slash)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        var from = _cardZonesOperations._cardZones.GetLocation(slash.Id);
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChengPu = r with { Stage = 9, CardId = slash.Id, Before = before } });
        _corePhysicalMovementFacadesOperations.MoveCard(slash, from, CardLocation.Hand(recipient), new(ShiCpDuzuoGrantMoveReason));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiDuzuoGrantIssuedEvent(f.Id, ShiCpSource(f), f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, recipient, slash.Id, from == CardLocation.DiscardPile, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 督佐：{(_coreMatchParticipantsPlayersAccess._players[recipient].Name)} 从{((from == CardLocation.DiscardPile ? "弃牌堆" : "牌堆"))}获得一张火【杀】。", f.OwnerSeat);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal IReadOnlyList<PromptChoice> ShiCpDuzuoChoices(ProgramSkillFrame f) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => new PromptChoice(new($"shi-cheng-pu.{(f.Id)}.r{(p.Seat)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[p.Seat].Name)} 从牌堆或弃牌堆获得一张火【杀】", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "mobile-shi-duzuo", ["seat"] = p.Seat.ToString(CultureInfo.InvariantCulture) })).ToArray());
    internal void PublishShiCpDuzuoRecipient(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "督佐：选择一名角色获得一张火【杀】。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = ShiCpDuzuoChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mobile-shi-duzuo")]
    internal void AnswerMobileShiDuzuo(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiChengPu: { Operation: "duzuo", TargetSeat: -1 } } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], ShiCpDuzuoChoices(f).Where(current => current.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Duzuo choice lost its original frame, chooser or legal recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var seat = int.Parse(selected.Parameters["seat"], CultureInfo.InvariantCulture);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChengPu = f.MobileShiChengPu!with { TargetSeat = seat }, ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding("duzuo-recipient", seat.ToString(CultureInfo.InvariantCulture), f.OwnerSeat)).ToArray()) });
        ContinueMobileShiDuzuo(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal SkillProgramStepOutcome BeginMobileShiBihanGuardProgram(ProgramSkillFrame frame)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (f.MobileShiChengPu is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id)
            throw new InvalidOperationException("Bihan requires its original unpaid instruction.");
        if (f.TriggerId != MobileShiChengPuComposition.BihanGuardTrigger || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } victim })
            throw new InvalidOperationException("Bihan requires its exact slash-damage window.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChengPu = new("bihan", _coreActualTurnProgressTurnNumberAccess._turnNumber, DamageTargetSeat: victim) });
        ContinueMobileShiBihan(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal int[] MobileShiBihanDiscarderSeats(ProgramSkillFrame f) => new[]
    {
        f.OwnerSeat,
        f.MobileShiChengPu!.DamageTargetSeat
    }.Distinct().Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count > _coreMatchParticipantsPlayersAccess._players[seat].Hp).ToArray();
    internal void BindShiCpBihanDiscarder(ProgramSkillFrame f, int seat)
    {
        var r = f.MobileShiChengPu!;
        var discarder = _coreMatchParticipantsPlayersAccess._players[seat];
        var planned = Math.Max(0, _corePhysicalOwnedCardQueriesOperations.GetHand(discarder).Count - discarder.Hp);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChengPu = r with { Stage = 3, TargetSeat = seat }, ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding("bihan-discarder", seat.ToString(CultureInfo.InvariantCulture), f.OwnerSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiBihanGuardIssuedEvent(f.Id, ShiCpSource(f), f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, r.DamageTargetSeat, seat, planned));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 蔽扞：{(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 将手牌数弃置至当前体力值。", f.OwnerSeat);
    }

    internal IReadOnlyList<PromptChoice> ShiCpBihanDiscarderChoices(ProgramSkillFrame f) => Array.AsReadOnly(MobileShiBihanDiscarderSeats(f).Select(seat => new PromptChoice(new($"shi-cheng-pu.{(f.Id)}.d{(seat)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 将手牌数弃置至当前体力值（{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}：{(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count)} 张手牌，{(_coreMatchParticipantsPlayersAccess._players[seat].Hp)} 点体力）", [], [seat], new Dictionary<string, string> { ["program-action"] = "mobile-shi-bihan", ["branch"] = "discarder", ["seat"] = seat.ToString(CultureInfo.InvariantCulture) })).ToArray());
    internal void PublishShiCpBihanDiscarder(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "蔽扞：选择将手牌数弃置至当前体力值的角色。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = ShiCpBihanDiscarderChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal IReadOnlyList<PromptChoice> ShiCpBihanDiscardChoices(ProgramSkillFrame f) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.MobileShiChengPu!.TargetSeat, f.MobileShiChengPu.TargetSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard).Select(choice => choice with { Parameters = new Dictionary<string, string>(choice.Parameters) { ["program-action"] = "mobile-shi-bihan", ["branch"] = "discard" } }).ToArray());
    internal void PublishShiCpBihanDiscard(ProgramSkillFrame f)
    {
        var r = f.MobileShiChengPu!;
        var choices = ShiCpBihanDiscardChoices(f);
        if (choices.Count == 0)
        {
            AdvanceShiCpBihanReduction(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var discarder = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.TargetSeat, $"蔽扞：还需弃置 {(_corePhysicalOwnedCardQueriesOperations.GetHand(discarder).Count - discarder.Hp)} 张手牌（弃置至当前体力值）。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.TargetSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = discarder.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mobile-shi-bihan")]
    internal void AnswerMobileShiBihan(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiChengPu: { Operation: "bihan" } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == 0 ? f.OwnerSeat : r.TargetSeat) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], (r.Stage == 0 ? ShiCpBihanDiscarderChoices(f) : ShiCpBihanDiscardChoices(f)).Where(current => current.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Bihan choice lost its original frame, chooser or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters["branch"] == "discarder")
        {
            if (r.Stage != 0)
                throw new InvalidOperationException("Bihan discarder choice left its unpaid stage.");
            BindShiCpBihanDiscarder(f, int.Parse(selected.Parameters["seat"], CultureInfo.InvariantCulture));
        }
        else
        {
            if (r.Stage != 3)
                throw new InvalidOperationException("Bihan discard choice left its discard stage.");
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).ToArray();
            var index = int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture);
            if (index < 0 || index >= hand.Length)
                throw new InvalidOperationException("Bihan discard choice left its exact hand slot.");
            var card = hand[index].Id;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChengPu = r with { Stage = 9, CardId = card, Before = _coreCardMovementJournalMovementSequenceAccess._movementSequence, Discarded = r.Discarded + 1 }, PendingMovementContinuation = new(r.TargetSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.TargetSeat, [card], new(ShiCpBihanDiscardMoveReason));
        }

        ContinueMobileShiBihan(f.Id);
    }

    internal bool DrainShiCpBihan(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMobileShiBihan(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiChengPu: { Operation: "bihan" } r } f || f.Id != id)
            return;
        if (r.Stage == 0)
        {
            var discarders = MobileShiBihanDiscarderSeats(f);
            if (discarders.Length == 0)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return;
            }

            if (discarders.Length > 1)
            {
                PublishShiCpBihanDiscarder(f);
                return;
            }

            BindShiCpBihanDiscarder(f, discarders[0]);
            ContinueMobileShiBihan(id);
            return;
        }

        if (r.Stage == 3)
        {
            var discarder = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
            if (!discarder.IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(discarder).Count <= discarder.Hp)
            {
                AdvanceShiCpBihanReduction(f);
                return;
            }

            PublishShiCpBihanDiscard(f);
            return;
        }

        if (r.Stage == 9)
        {
            if (DrainShiCpBihan(f))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
            r = f.MobileShiChengPu!;
            var discarder = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
            if (discarder.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(discarder).Count > discarder.Hp)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChengPu = r with { Stage = 3 } });
                PublishShiCpBihanDiscard(f);
                return;
            }

            AdvanceShiCpBihanReduction(f);
            return;
        }

        throw new InvalidOperationException("Bihan has an unknown native continuation stage.");
    }

    internal void AdvanceShiCpBihanReduction(ProgramSkillFrame f)
    {
        var r = f.MobileShiChengPu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChengPu = r with { Stage = 6 } });
        r = f.MobileShiChengPu!;
        var reduced = false;
        if (f.WindowContext is { } context && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(window => window.Id == context.ParentFrameId)is { Prevented: false } window && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack && attack.SourceSeat == window.SourceSeat && attack.TargetSeat == window.TargetSeat)
        {
            _coreProgramDamageReductionOperations.ReduceProgramCurrentDamage(f, 1);
            reduced = true;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiBihanGuardCompletedEvent(f.Id, ShiCpSource(f), r.DamageTargetSeat, r.TargetSeat, r.Discarded, reduced));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 蔽扞：{(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 弃置 {(r.Discarded)} 张手牌" + (reduced ? "，此伤害-1。" : "，此伤害已不存在。"), f.OwnerSeat);
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 67300)]
    internal bool ResumeShiCpChengPu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiChengPu: { } r } f || f.Id != id)
            return false;
        if (r.Operation == "bihan")
            ContinueMobileShiBihan(id);
        else
            ContinueMobileShiDuzuo(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 67300)]
    internal bool ReturnShiCpChengPuMovement(ProgramSkillFrame f)
    {
        if (f.MobileShiChengPu is not { Operation: var operation, Stage: 9 })
            return false;
        if (operation == "bihan")
            ContinueMobileShiBihan(f.Id);
        else
            ContinueMobileShiDuzuo(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 67300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 67300)]
    internal bool ContinuePaidShiCpBihan(ProgramSkillFrame f) => f.MobileShiChengPu is { Operation: "bihan", Stage: > 0 };
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 67300)]
    internal bool ShiCpBihanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MobileShiChengPu is { Operation: "bihan", Stage: 9 } r && effect?.Op == SkillProgramEffectOp.ResolveMobileShiBihanGuard && p.CoverageResultBind is null && p.BeforeCount == 0 && p.SubjectSeat == r.TargetSeat;

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiDuzuoGrant)]
    internal PromptChoice ChooseShiCpDuzuo(PendingDecision prompt) => prompt.Choices.FirstOrDefault(choice => choice.Targets is [var seat] && seat == prompt.PlayerSeat) ?? prompt.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiBihanGuard)]
    internal PromptChoice ChooseShiCpBihan(PendingDecision prompt) => prompt.Choices[0];
}
