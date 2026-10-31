#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuShiOlRuntime
{
    internal bool HasOlLuShi => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveOlLuShiZhuyan);

    internal static CardConversionSource OlLuShiSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 42700)]
    internal void CaptureOlLuShiOpening(GameStartedEvent fact)
    {
        if (!HasOlLuShi)
            return;
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            CaptureOlLuShiSnapshot(player, 0, true);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 42701)]
    internal void CaptureOlLuShiEnding(ActualPhaseEndedEvent fact)
    {
        if (HasOlLuShi && fact.Phase.Kind == ActualDiscardRecoveryPhaseKind.Ending)
            CaptureOlLuShiSnapshot(_coreMatchParticipantsPlayersAccess._players[fact.Phase.ActorSeat], fact.BoundaryFrameId, false);
    }

    internal void CaptureOlLuShiSnapshot(CharacterState player, long boundary, bool initial)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<OlLuShiSnapshotEvent>().Any(e => e.Seat == player.Seat && e.Initial == initial && e.BoundaryId == boundary))
            return;
        var dual = player.HasHpValue ? player.HappyQiaoHealth : null;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlLuShiSnapshotEvent(boundary, _coreActualTurnProgressTurnNumberAccess._turnNumber, player.Seat, initial, dual is null && player.HasHpValue ? player.Hp : null, dual, _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count));
    }

    internal OlLuShiSnapshotEvent? OlLuShiLastSnapshot(int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<OlLuShiSnapshotEvent>().LastOrDefault(e => e.Seat == target);
    internal bool OlLuShiUnused(int owner, int target, string item) => !_coreProgramEventHistoryOperations.ProgramEventHistory<OlLuShiAdjustmentIssuedEvent>().Any(e => e.Source.OwnerSeat == owner && e.TargetSeat == target && e.Item == item);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 42700, SkillProgramEffectOp.ResolveOlLuShiZhuyan)]
    internal bool CanOfferOlLuShi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => c.SkillId == "ol:zhuyan" && w.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && OlLuShiLastSnapshot(p.Seat)is { } snapshot && (OlLuShiUnused(c.OwnerSeat, p.Seat, "hand") || (snapshot.Hp is not null || snapshot.Dual is not null) && p.HasHpValue && OlLuShiUnused(c.OwnerSeat, p.Seat, "hp")));
    SkillProgramStepOutcome global::CardGame.Core.IOlLuShiHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginOlLuShi(f, op);
    internal SkillProgramStepOutcome BeginOlLuShi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.OlLuShi is not null)
        {
            ContinueOlLuShi(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.ResolveOlLuShiZhuyan ? f.SkillId != "ol:zhuyan" || f.InstructionIndex != 1 || f.WindowContext?.Window != SkillProgramTriggerWindow.DiscardPhaseEnded : f.SkillId != "ol:leijie" || f.InstructionIndex != 3 || f.ActivationId != "judge-target" || f.SelectedTargetSeats.Count != 1)
            throw new InvalidOperationException("Lu Shi lost her original adjustment or finalized judgment instruction.");
        var r = new OlLuShiReceipt(OlLuShiSource(f), f.GameplayHash, op, f.InstructionIndex, f.WindowContext?.ParentFrameId ?? 0, op == SkillProgramEffectOp.ResolveOlLuShiZhuyan ? OlLuShiStage.Choice : OlLuShiStage.Ready);
        if (op == SkillProgramEffectOp.ResolveOlLuShiLeijie)
        {
            var outcome = f.JudgmentOutcomeBindings?.SingleOrDefault(o => o.Name == "leijie-result") ?? throw new InvalidOperationException("Leijie requires its original finalized judgment result.");
            if (!_coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(f, outcome) || outcome.TargetSeat != f.SelectedTargetSeats[0])
                throw new InvalidOperationException("Leijie cannot replace its judged participant or final card value.");
            r = r with
            {
                TargetSeat = outcome.TargetSeat,
                Item = outcome is { Suit: Suit.Spade, Rank: >= 2 and <= 9 } ? "slashes" : "draw",
                Amount = 2
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlLuShi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlLuShiStartedEvent(f.Id, r.Source, r.Hash, r.InstructionIndex, r.Parent, op));
        if (r.Stage == OlLuShiStage.Choice)
            PublishOlLuShi(f);
        else
            ContinueOlLuShi(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal Card[] OlLuShiDiscardCandidates(ProgramSkillFrame f) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OlLuShi!.TargetSeat]).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OlLuShi.TargetSeat, c, CardLocation.Hand(f.OlLuShi.TargetSeat), OwnedCardMoveIntent.Discard)).ToArray();
    internal IReadOnlyList<PromptChoice> OlLuShiChoices(ProgramSkillFrame f)
    {
        var r = f.OlLuShi!;
        Dictionary<string, string> Args(string item) => new()
        {
            ["program-action"] = "ol-lushi",
            ["item"] = item
        };
        if (r.Stage == OlLuShiStage.Pool)
            return Enum.GetValues<HappyQiaoPool>().Select(pool => new PromptChoice(new($"ol-lushi.{(f.Id)}.{(pool)}"), pool == HappyQiaoPool.DaQiao ? "大乔" : "小乔", [], [r.TargetSeat], new Dictionary<string, string>(Args("pool")) { ["pool"] = pool.ToString() })).ToArray();
        if (r.Stage == OlLuShiStage.Discard)
            return OlLuShiDiscardCandidates(f).Where(c => !r.Cards.Contains(c.Id)).Select(c => new PromptChoice(new($"ol-lushi.{(f.Id)}.discard.{(c.Id)}"), $"弃置【{(c.DisplayName)}】（{(r.Cards.Count + 1)}/{(r.RequiredCount)}）", [c.Id], [], Args("discard"))).ToArray();
        if (r.Stage != OlLuShiStage.Choice)
            return[];
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).SelectMany(p => new[] { "hp", "hand" }.Where(item => OlLuShiUnused(f.OwnerSeat, p.Seat, item) && OlLuShiLastSnapshot(p.Seat)is { } snapshot && (item == "hand" || p.HasHpValue && (snapshot.Hp is not null || snapshot.Dual is not null))).Select(item => new PromptChoice(new($"ol-lushi.{(f.Id)}.{(p.Seat)}.{(item)}"), $"{(p.Name)}：调整{((item == "hp" ? "体力" : "手牌数"))}", [], [p.Seat], Args(item)))).ToArray();
    }

    internal void PublishOlLuShi(ProgramSkillFrame f)
    {
        var r = f.OlLuShi!;
        var chooser = r.Stage is OlLuShiStage.Pool or OlLuShiStage.Discard ? r.TargetSeat : f.OwnerSeat;
        var choices = OlLuShiChoices(f);
        if (choices.Count == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, "驻颜：选择此次真实调整。", choices.SelectMany(c => c.Cards).ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage == OlLuShiStage.Discard,
            Choices = Array.AsReadOnly(choices.ToArray()),
            TargetSeat = r.TargetSeat < 0 ? null : r.TargetSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("ol-lushi")]
    internal void AnswerOlLuShi(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OlLuShi: { } r } f || !OlLuShiChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Lu Shi rejects a stale participant, spent lifetime option, pool or private discard material.");
        RequireOlLuShi(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == OlLuShiStage.Choice)
        {
            var target = choice.Targets.Single();
            var item = choice.Parameters["item"];
            var snapshot = OlLuShiLastSnapshot(target)!;
            r = r with
            {
                TargetSeat = target,
                Item = item,
                Snapshot = snapshot,
                Stage = item == "hp" && _characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(target) ? OlLuShiStage.Pool : OlLuShiStage.Ready
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlLuShi = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlLuShiAdjustmentIssuedEvent(f.Id, r.Source, target, item, snapshot));
            if (r.Stage == OlLuShiStage.Pool)
            {
                PublishOlLuShi(f);
                return;
            }
        }
        else if (r.Stage == OlLuShiStage.Pool)
        {
            var pool = Enum.Parse<HappyQiaoPool>(choice.Parameters["pool"]);
            var before = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].HappyQiaoHealth!;
            var value = Math.Min(before.MaximumHp, r.Snapshot!.Dual?.Hp(pool) ?? r.Snapshot.Hp!.Value);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlLuShi = r = r with { Pool = pool, TargetValue = value, Stage = OlLuShiStage.Ready } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlLuShiPoolAcceptedEvent(f.Id, r.TargetSeat, pool, value, before));
            BindOlLuShiHealthIntent(f, before, pool, value);
        }
        else
        {
            var cards = r.Cards.Append(choice.Cards.Single()).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlLuShi = r with { Cards = cards } });
            if (cards.Length < r.RequiredCount)
            {
                PublishOlLuShi(f);
                return;
            }

            if (cards.Length != r.RequiredCount || cards.Any(id => !OlLuShiDiscardCandidates(f).Any(c => c.Id == id)))
                throw new InvalidOperationException("Zhuyan's selected hand discard is no longer completely payable.");
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlLuShi = f.OlLuShi!with { Stage = OlLuShiStage.Children, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.TargetSeat, cards, new("skill.ol.zhuyan.discard"));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlLuShi = f.OlLuShi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlLuShiMovementEvent(f.Id, r.TargetSeat, false, cards.Length, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }

        ContinueOlLuShi(f.Id);
    }

    internal void BindOlLuShiHealthIntent(ProgramSkillFrame f, HappyQiaoDualHealth before, HappyQiaoPool pool, int targetHp)
    {
        var r = f.OlLuShi!;
        var recover = targetHp >= before.Hp(pool);
        var amount = recover ? targetHp : checked(before.Hp(pool) - targetHp);
        var kind = recover ? HpChangeKind.Recovery : HpChangeKind.Loss;
        var returnKind = recover ? HappyQiaoHealthReturnKind.RecoverTo : HappyQiaoHealthReturnKind.Program;
        var key = recover ? $"recover-to:{(f.Id)}:{(f.InstructionIndex)}:{(r.TargetSeat)}" : $"program:{(f.Id)}:{(f.InstructionIndex)}:{(r.TargetSeat)}:{(kind)}";
        var request = new HappyQiaoHealthReceipt(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, f.Id, f.Kind, r.TargetSeat, f.OwnerSeat, kind, amount, returnKind, key, f.InstructionIndex, before, Pool: pool);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyQiaoHealth = request, HappyQiaoRecoverTo = recover ? new(f.Id, f.InstructionIndex, f.GameplayHash, f.OwnerSeat, r.TargetSeat, targetHp, request.Id, before) : null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoHealthRequestedEvent(request.Id, f.Id, r.TargetSeat, kind, amount, returnKind, key));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoHealthPoolSelectedEvent(request.Id, f.Id, r.TargetSeat, pool, before.Hp(pool), before.MaximumHp));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 42700)]
    internal bool ContinueOlLuShi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OlLuShi: not null } root || root.Id != id)
            return false;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { OlLuShi: { } r } f && f.Id == id)
        {
            RequireOlLuShi(f);
            if (f.ReexecuteParticipantInstruction)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ReexecuteParticipantInstruction = false });
            if (r.Stage is OlLuShiStage.Choice or OlLuShiStage.Pool or OlLuShiStage.Discard)
            {
                PublishOlLuShi(f);
                return true;
            }

            if (r.Stage is OlLuShiStage.Drawing or OlLuShiStage.NativeUse)
                return true;
            if (r.Stage == OlLuShiStage.Children)
            {
                if (DrainOlLuShi(f))
                    return true;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
                if (r.Item == "slashes")
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { OlLuShi = r with { Stage = OlLuShiStage.Ready } });
                    continue;
                }

                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            if (r.Stage == OlLuShiStage.Loss)
            {
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(id, f.SkillId, r.TargetSeat, r.Amount);
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id || _coreProgramLifecycleOperations.GetActiveProgramFrame(id).OlLuShi!.Stage == OlLuShiStage.Loss)
                    return true;
                continue;
            }

            if (r.Item == "slashes")
            {
                if (r.SlashCount >= 2 || !CanOlLuShiLeijieSlash(f, r.TargetSeat))
                {
                    _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                    return true;
                }

                IssueOlLuShiLeijieSlash(f);
                return true;
            }

            if (r.Item == "hp")
            {
                var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
                var current = r.Pool is { } pool ? target.HappyQiaoHealth!.Hp(pool) : target.Hp;
                var desired = r.Pool is not null ? r.TargetValue : Math.Min(target.MaxHp, r.Snapshot!.Hp ?? throw new InvalidOperationException("A formerly dual-health target cannot silently become scalar."));
                if (desired < current)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { OlLuShi = r with { Stage = OlLuShiStage.Loss, Amount = checked(current - desired) } });
                    continue;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlLuShi = r with { Stage = OlLuShiStage.Children } });
                if (r.Pool is not null)
                    _ = _characterDaQiaoXiaoQiaoHappyGodOperations.TryHandleHappyQiaoRecoverTo(id, f.OwnerSeat, r.TargetSeat, desired);
                else if (desired > current)
                    _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(id, f.OwnerSeat, r.TargetSeat, desired - current, null, null);
                continue;
            }

            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Count;
            var desiredHand = r.Item == "draw" ? hand + 2 : r.Snapshot!.HandCount;
            var count = r.Item == "draw" ? 2 : Math.Max(0, Math.Min(5, desiredHand) - hand);
            if (count > 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlLuShi = r with { Stage = OlLuShiStage.Drawing, Amount = count } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.TargetSeat, count, new("skill.ol.lushi.draw"));
                return true;
            }

            var discard = r.Item == "hand" ? Math.Min(Math.Max(0, hand - desiredHand), OlLuShiDiscardCandidates(f).Length) : 0;
            if (discard == 0)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlLuShi = r with { Stage = OlLuShiStage.Discard, RequiredCount = discard } });
            PublishOlLuShi(f);
            return true;
        }

        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 42702)]
    internal void CaptureOlLuShiLoss(ProgramSkillHpLostEvent fact)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { OlLuShi: { Stage: OlLuShiStage.Loss } r } f && fact.FrameId == f.Id && fact.SkillId == f.SkillId && fact.TargetSeat == r.TargetSeat && fact.Amount == r.Amount)
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { OlLuShi = r with { Stage = OlLuShiStage.Children } });
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 42703)]
    internal void CaptureOlLuShiPreventedLoss(IdentityHpDeductionPreventedEvent fact)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is IdentityAppearanceFrame appearance && appearance.Id == fact.AppearanceFrameId && fact.Entry == IdentityAppearanceEntry.ProgramHpLoss && _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == appearance.Receipt.ParentFrameId)is ProgramSkillFrame { OlLuShi: { Stage: OlLuShiStage.Loss, Item: "hp" } r } f && fact.RequestedDeduction == r.Amount && appearance.Receipt.OwnerSeat == r.TargetSeat && appearance.Receipt.OriginalInstructionIndex == f.InstructionIndex)
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { OlLuShi = r with { Stage = OlLuShiStage.Children } });
    }

    internal bool DrainOlLuShi(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.Program);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveOlLuShiZhuyan)]
    internal void ReturnOlLuShiZhuyanDraw(ProgramSkillFrame f, NativeDrawOutcome o) => ReturnOlLuShiDraw(f, o);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveOlLuShiLeijie)]
    internal void ReturnOlLuShiLeijieDraw(ProgramSkillFrame f, NativeDrawOutcome o) => ReturnOlLuShiDraw(f, o);
    internal void ReturnOlLuShiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.OlLuShi is not { Stage: OlLuShiStage.Drawing, Draw: null } r || outcome.RecipientSeat != r.TargetSeat || outcome.RequestedCount != r.Amount || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Lu Shi draw lost its original recipient, requested count or single native return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { OlLuShi = r with { Stage = OlLuShiStage.Children, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlLuShiMovementEvent(f.Id, r.TargetSeat, true, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueOlLuShi(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 42700)]
    internal bool ReturnOlLuShiMovement(ProgramSkillFrame f) => f.OlLuShi is not null && ContinueOlLuShi(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 42700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 42700)]
    internal bool RetainOlLuShiIssued(ProgramSkillFrame f) => f.OlLuShi is { Item: not null };
    [ProgramAiChoice(SkillProgramEffectOp.ResolveOlLuShiZhuyan)]
    internal PromptChoice ChooseOlLuShiZhuyan(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveOlLuShiLeijie)]
    internal PromptChoice ChooseOlLuShiLeijie(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
}
