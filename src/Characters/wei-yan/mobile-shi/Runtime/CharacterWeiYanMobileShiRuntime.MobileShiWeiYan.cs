#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiYanMobileShiRuntime
{
    internal const string ShiWyZhuangshi = "mobile:shi-zhuangshi", ShiWyYinzhan = "mobile:shi-yinzhan", ShiWyZhongao = "mobile:shi-zhongao", ShiWyKuanggu = "mobile:shi-kuanggu", ShiWyKuanggu2 = "mobile:shi-kuanggu-upgraded", ShiWyKunfen = "mobile:shi-kunfen";
    internal const string ShiWyDiscard = "skill.mobile-shi-wei-yan.discard", ShiWyObtain = "skill.mobile-shi-wei-yan.obtain";
    internal bool ShiWyMissionOver(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiWeiYanMissionEvent>().Any(e => e.OwnerSeat == seat);
    internal int ShiWyPhaseUses(int seat, int turn, int phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiWeiYanUseEvent>().Count(e => e.OwnerSeat == seat && e.TurnNumber == turn && e.PhaseInstanceId == phase);
    internal MobileShiWeiYanPhaseGrantEvent? ShiWyPhaseGrant(int seat) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat ? _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiWeiYanPhaseGrantEvent>().LastOrDefault(e => e.OwnerSeat == seat && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) : null;
    SkillProgramStepOutcome global::CardGame.Core.IMobileShiWeiYanHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginMobileShiWeiYan(frame, operation);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 47100)]
    internal bool CanOfferMobileShiWeiYan(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId == ShiWyZhuangshi)
            return gate.Owner.IsAlive && gate.Owner.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat;
        if (gate.SkillId == ShiWyZhongao)
            return gate.Trigger.Id == "initialize" || !ShiWyMissionOver(gate.OwnerSeat) && (gate.Trigger.Id == "failure" || gate.Context.SourceSeat == gate.OwnerSeat);
        if (gate.SkillId is ShiWyKuanggu or ShiWyKuanggu2)
            return gate.Context.SourceSeat == gate.OwnerSeat && gate.Owner.IsAlive;
        if (gate.SkillId != ShiWyYinzhan)
            return true;
        return gate.Context.CardUse is { } use && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiYinzhanQualifiedEvent>().Any(e => e.UseId == use.ParentCardUseFrameId && e.OwnerSeat == gate.OwnerSeat && e.HandQualified && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiYinzhanAppliedEvent>().Any(a => a.QualificationId == e.Id) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiYinzhanClaimedEvent>().Any(a => a.QualificationId == e.Id));
    }

    internal SkillProgramStepOutcome BeginMobileShiWeiYan(ProgramSkillFrame input, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.MobileShiWeiYan is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id)
            throw new InvalidOperationException("Shi Wei Yan requires a fresh instruction on its original native trigger.");
        var branch = operation switch
        {
            SkillProgramEffectOp.ResolveMobileShiZhuangshi => "zhuangshi",
            SkillProgramEffectOp.ResolveMobileShiYinzhan => "yinzhan",
            SkillProgramEffectOp.ResolveMobileShiZhongao => "zhongao",
            _ => "kuanggu"
        };
        if (branch == "zhuangshi" && f.SkillId != ShiWyZhuangshi || branch == "yinzhan" && f.SkillId != ShiWyYinzhan || branch == "zhongao" && f.SkillId != ShiWyZhongao || branch == "kuanggu" && f.SkillId is not (ShiWyKuanggu or ShiWyKuanggu2))
            throw new InvalidOperationException("Shi Wei Yan instruction has an incompatible owning skill.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = new(branch, 0, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) });
        if (branch == "zhongao")
        {
            CommitShiWyMission(f, f.TriggerId == "success");
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = f.MobileShiWeiYan!with { Stage = 8 } });
            if (f.TriggerId == "success" && ShiWyPhaseGrant(f.OwnerSeat)is { } grant)
            {
                var used = ShiWyPhaseUses(f.OwnerSeat, grant.TurnNumber, grant.PhaseInstanceId);
                if (used < grant.DiscardedCards)
                    _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw);
                if (used < grant.LostHp)
                {
                    if (_coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
                        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
                    else
                        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw);
                }
            }

            ContinueMobileShiWeiYan(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (branch == "yinzhan")
        {
            ContinueMobileShiWeiYan(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishMobileShiWeiYan(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void CommitShiWyMission(ProgramSkillFrame source, bool success)
    {
        if (ShiWyMissionOver(source.OwnerSeat))
            return;
        var instance = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat]).ProgramInstances.SingleOrDefault(i => i.SkillId == ShiWyZhongao);
        if (instance is null)
            return;
        _coreProgramSkillChangesOperations.SetProgramBooleanState2(source, ShiWyZhongao, instance.SkillInstanceId, success ? "succeeded" : "failed", true);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiWeiYanMissionEvent(source.Id, source.OwnerSeat, success));
        _coreProgramSkillChangesOperations.ReplaceProgramSkills(source, success ? [ShiWyKuanggu] : [ShiWyZhuangshi], success ? [ShiWyKuanggu2] : [ShiWyKunfen], ProgramSkillSourceRemoval.Enabled, acquireFirst: true);
    }

    internal int[] ShiWyDiscardable(int seat, bool handOnly) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(handOnly ? [] : _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).Select(card => card.Id).ToArray();
    internal IReadOnlyList<PromptChoice> MobileShiWeiYanChoices(ProgramSkillFrame f)
    {
        var r = f.MobileShiWeiYan!;
        var choices = new List<PromptChoice>();
        void Add(string id, string description, int[]? cards = null, int[]? targets = null, string? branch = null, bool? selected = null)
        {
            var parameters = new Dictionary<string, string>
            {
                ["program-action"] = "mobile-shi-wei-yan",
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = branch ?? id
            };
            if (selected is { } value)
                parameters["selected"] = value ? "true" : "false";
            choices.Add(new(new($"shi-wei-yan.{(f.Id)}.{(id)}"), description, cards ?? [], targets ?? [], parameters));
        }

        if (r.Operation == "zhuangshi")
        {
            foreach (var id in ShiWyDiscardable(f.OwnerSeat, true))
                Add("card-" + id, (r.Cards.Contains(id) ? "取消弃置" : "选择弃置") + _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName, [id], selected: r.Cards.Contains(id));
            for (var n = 0; n <= Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp); n++)
                Add("hp-" + n, $"失去 {(n)} 点体力" + (r.HpLoss == n ? "（已选）" : ""), selected: r.HpLoss == n);
            if (r.Cards.Count + r.HpLoss > 0)
                Add("pay", $"确认：弃置 {(r.Cards.Count)} 张手牌，失去 {(r.HpLoss)} 点体力");
            Add("decline", "不执行壮誓" + (ShiWyMissionOver(f.OwnerSeat) ? "" : "（忠傲使命失败）"));
        }
        else if (r.Operation == "kuanggu")
        {
            if (r.Stage == 4)
            {
                foreach (var id in ShiWyDiscardable(f.OwnerSeat, false))
                    Add("kuanggu-cost-" + id, "背水：弃置" + _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName, [id], branch: "kuanggu-cost");
                return Array.AsReadOnly(choices.ToArray());
            }

            if (_coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
                Add("recover", "回复1点体力");
            Add("draw", "摸1张牌");
            if (f.SkillId == ShiWyKuanggu2)
                Add("backwater", "背水：回复1点体力并摸1张牌，然后弃置一张牌，本阶段可用杀次数+1");
            Add("cancel", "取消狂骨");
        }
        else if (r.Operation == "yinzhan")
            foreach (var old in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.TargetSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, destination: SkillProgramCardDestination.DiscardPile))
                choices.Add(old with { Parameters = new Dictionary<string, string>(old.Parameters) { ["program-action"] = "mobile-shi-wei-yan", ["branch"] = "discard", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) } });
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMobileShiWeiYan(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, f.OwnerSeat, f.MobileShiWeiYan!.Operation == "zhuangshi" ? $"壮誓已选：弃置 {(f.MobileShiWeiYan.Cards.Count)} 张手牌，失去 {(f.MobileShiWeiYan.HpLoss)} 点体力。" : $"【{(skill.Name)}】请选择。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = MobileShiWeiYanChoices(f)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mobile-shi-wei-yan")]
    internal void AnswerMobileShiWeiYan(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiWeiYan: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !MobileShiWeiYanChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Shi Wei Yan lost its exact current private payment or choice.");
        var branch = selected.Parameters["branch"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Operation == "zhuangshi")
        {
            if (branch.StartsWith("card-", StringComparison.Ordinal))
            {
                var id = selected.Cards.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = r with { Cards = r.Cards.Contains(id) ? r.Cards.Where(c => c != id).ToArray() : r.Cards.Append(id).ToArray() } });
                PublishMobileShiWeiYan(f);
                return;
            }

            if (branch.StartsWith("hp-", StringComparison.Ordinal))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = r with { HpLoss = int.Parse(branch.AsSpan(3), CultureInfo.InvariantCulture) } });
                PublishMobileShiWeiYan(f);
                return;
            }

            if (branch == "decline")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = r with { Stage = 8 } });
                CommitShiWyMission(f, false);
                ContinueMobileShiWeiYan(f.Id);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = r with { Stage = 1 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (r.Cards.Count > 0)
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Cards, new(ShiWyDiscard));
            ContinueMobileShiWeiYan(f.Id);
            return;
        }

        if (r.Operation == "kuanggu")
        {
            if (branch == "cancel")
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return;
            }

            if (branch == "kuanggu-cost")
            {
                var paid = selected.Cards.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = r with { Stage = 5, CardId = paid }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [paid], new(ShiWyDiscard));
                ContinueMobileShiWeiYan(f.Id);
                return;
            }

            var backwater = branch == "backwater";
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = r with { Stage = backwater ? 4 : 8 } });
            if (branch == "recover" || backwater)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
            if (branch == "draw" || backwater)
            {
                var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw);
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = f.MobileShiWeiYan!with { KuangguDraw = new(f.OwnerSeat, 1, null, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null) { ActualDrawIds = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > before && move.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(f.OwnerSeat) && move.Reason == CardMoveReasons.Draw).Select(move => move.CardId).ToArray() } } });
            }

            ContinueMobileShiWeiYan(f.Id);
            return;
        }

        var idToDiscard = selected.Cards.Count == 1 ? selected.Cards[0] : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat])[int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture)].Id;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = r with { Stage = 6, CardId = idToDiscard }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [idToDiscard], new(ShiWyDiscard));
        ContinueMobileShiWeiYan(f.Id);
    }

    internal bool DrainMobileShiWeiYan(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal bool MobileShiKuangguThresholdDrawMatches(ProgramSkillFrame producer, NativeDrawInvocationProof proof) => producer.SkillId is ShiWyKuanggu or ShiWyKuanggu2 && producer.TriggerId is not null && producer.MobileShiWeiYan is { Operation: "kuanggu", Stage: 4 or 8, KuangguDraw: { } paid } && _coreProgramLifecycleOperations.GetProgramTrigger(producer).Effects is [{ Op: SkillProgramEffectOp.ResolveMobileShiKuanggu }] && producer.InstructionIndex == 1 && paid.RecipientSeat == producer.OwnerSeat && proof.OwnerSeat == producer.OwnerSeat && paid.RequestedCount == 1 && proof.RequestedCount == 1 && proof.Reason == CardMoveReasons.Draw && paid.AttemptFrameId is null && paid.Replacement is null && proof.SequenceBefore == paid.SequenceBefore && proof.SequenceAfter == paid.SequenceAfter && proof.Materials.Select(material => material.CardId).SequenceEqual(paid.ActualDrawIds);
    internal void ContinueMobileShiWeiYan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiWeiYan: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage > 0 && DrainMobileShiWeiYan(f))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        r = f.MobileShiWeiYan!;
        if (r.Operation == "zhuangshi" && r.Stage is 1 or 2 or 3)
        {
            if (r.Stage == 1 && r.HpLoss > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = r with { Stage = 3 } });
                var outcome = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, r.HpLoss);
                if (outcome == SkillProgramStepOutcome.AwaitChild)
                    return;
                ContinueMobileShiWeiYan(id);
                return;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiWeiYanPhaseGrantEvent(f.Id, f.OwnerSeat, r.TurnNumber, r.PhaseInstanceId, r.Cards.Count, r.HpLoss) { Source = new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) });
            _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), true);
            return;
        }

        if (r.Operation == "kuanggu" && r.Stage == 4)
        {
            if (ShiWyDiscardable(f.OwnerSeat, false).Length > 0)
            {
                PublishMobileShiWeiYan(f);
                return;
            }

            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        if (r.Operation == "kuanggu" && r.Stage == 5)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiKuangguSlashGrantedEvent(f.Id, f.OwnerSeat, r.TurnNumber, r.PhaseInstanceId));
        if (r.Operation == "yinzhan")
        {
            if (r.Stage == 6 && r.Chengshi)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = r with { Stage = 7 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
                if (r.CardId is { } discarded && _cardZonesOperations._cardZones.GetLocation(discarded) == CardLocation.DiscardPile)
                    _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([discarded], CardLocation.Hand(f.OwnerSeat), new(ShiWyObtain));
                ContinueMobileShiWeiYan(id);
                return;
            }

            var useId = f.WindowContext!.CardUse!.ParentCardUseFrameId;
            var next = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiYinzhanQualifiedEvent>().FirstOrDefault(e => e.UseId == useId && e.OwnerSeat == f.OwnerSeat && e.HandQualified && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiYinzhanAppliedEvent>().Any(a => a.QualificationId == e.Id) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiYinzhanClaimedEvent>().Any(a => a.QualificationId == e.Id));
            if (next is not null)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiYinzhanClaimedEvent(next.Id, f.Id));
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiWeiYan = r with { Stage = 0, TargetSeat = next.TargetSeat, CardId = null, ClaimId = next.Id, Chengshi = next.HpQualified } });
                if (_coreMatchParticipantsPlayersAccess._players[next.TargetSeat].IsAlive && MobileShiWeiYanChoices(f).Count > 0)
                {
                    PublishMobileShiWeiYan(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShiWeiYan = f.MobileShiWeiYan!with { Stage = 6 } });
                ContinueMobileShiWeiYan(id);
                return;
            }
        }

        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 47100)]
    internal bool ResumeMobileShiWeiYan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiWeiYan: not null } f || f.Id != id)
            return false;
        ContinueMobileShiWeiYan(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 47100)]
    internal bool ReturnMobileShiWeiYan(ProgramSkillFrame f)
    {
        if (f.MobileShiWeiYan is null)
            return false;
        ContinueMobileShiWeiYan(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 47100)]
    internal bool ContinuePaidMobileShiWeiYan(ProgramSkillFrame f) => f.MobileShiWeiYan is { Stage: > 0 };

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiZhuangshi, SkillProgramEffectOp.ResolveMobileShiYinzhan, SkillProgramEffectOp.ResolveMobileShiKuanggu)]
    internal PromptChoice SelectMobileShiWeiYanAi(PendingDecision decision)
    {
        var choices = decision.Choices;
        return choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pay") ?? choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "hp-1" && _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat].Hp > 1) ?? choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "recover") ?? choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "draw") ?? choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard") ?? choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "decline") ?? choices[0];
    }
}
