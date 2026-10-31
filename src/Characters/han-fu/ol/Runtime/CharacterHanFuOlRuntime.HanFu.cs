#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanFuOlRuntime
{
    internal const string ShuziGiftReason = "skill-program.han-fu.shuzi.gift", ShuziReturnReason = "skill-program.han-fu.shuzi.return", ShuziBoardReason = "skill-program.han-fu.shuzi.board", KuangshouDrawReason = "skill-program.han-fu.kuangshou.draw", KuangshouDiscardReason = "skill-program.han-fu.kuangshou.discard";
    internal static CardConversionSource HanFuSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_han_fu_ol_HanFu.HanFuSource(f);
    internal HanFuMaterial HanFuMaterialFor(int id)
    {
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        return new(id, card.Kind, global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(from is { Zone: CardZoneKind.Hand, OwnerSeat: { } owner } ? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[owner], card) : from.Zone == CardZoneKind.Judgment ? _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(card) : card.Kind), from, card.IsGeneralWeapon);
    }

    internal IReadOnlyList<HanFuMaterial> HanFuDiscardCards(ProgramSkillFrame f) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Select(c => HanFuMaterialFor(c.Cards.Single())).Where(m => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId), m.From, OwnedCardMoveIntent.Discard)).ToArray());
    SkillProgramStepOutcome global::CardGame.Core.IHanFuHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHanFu(f, op);
    internal SkillProgramStepOutcome BeginHanFu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var shuzi = op == SkillProgramEffectOp.ResolveShuziExchange;
        if (f.HanFu is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("HanFu requires a new exact first instruction.");
        int target;
        long parent = 0;
        long? damage = null;
        var amount = 0;
        HanFuMaterial[]? gifts = null;
        if (shuzi)
        {
            var a = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation;
            if (a is null || f.TriggerId is not null || f.WindowContext is not null || f.SelectedTargetSeats is not [var recipient] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, a.UsageGroup)) != 1 || f.SelectedCardIds.Count != 2 || f.SelectedCardIds.Distinct().Count() != 2)
                throw new InvalidOperationException("Shuzi lost its actual once-per-Play two-card activation and living recipient.");
            HanFuComposition.Activation(f.SkillId, a);
            target = recipient;
            gifts = f.SelectedCardIds.Select(HanFuMaterialFor).ToArray();
            if (gifts.Any(m => m.From.OwnerSeat != f.OwnerSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId), m.From, OwnedCardMoveIntent.Transfer)))
                throw new InvalidOperationException("Shuzi requires two transferable actual owned hand or equipment entities.");
        }
        else
        {
            if (f.TriggerId is null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } context || context.TargetSeat != f.OwnerSeat)
                throw new InvalidOperationException("Kuangshou requires its actual positive whole-damage observer.");
            target = f.OwnerSeat;
            parent = context.ParentFrameId;
            damage = context.DamageFrameId;
            amount = context.Amount;
        }

        var r = new HanFuReceipt(HanFuSource(f), f.GameplayHash, op, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, target, parent, damage, amount, shuzi ? HanFuStage.GiftChildren : HanFuStage.DrawChildren, Gifts: gifts, Movements: []);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HanFuStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.PhaseInstance, target, parent, damage, amount));
        if (shuzi)
            PayHanFuCards(f, gifts!, CardLocation.Hand(target), ShuziGiftReason, HanFuStage.GiftChildren);
        else
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 3, true, new(KuangshouDrawReason)).Count;
            RecordHanFuMovement(f.Id, HanFuStage.DrawChildren, KuangshouDrawReason, before, count);
        }

        ContinueHanFu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void RecordHanFuMovement(long id, HanFuStage stage, string reason, long before, int actual)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.HanFu!;
        var m = new HanFuMovement(r.Movements!.Count, stage, reason, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, actual);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HanFu = r with { Movements = r.Movements.Append(m).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HanFuMovementIssuedEvent(id, m));
    }

    internal void PayHanFuCards(ProgramSkillFrame f, IReadOnlyList<HanFuMaterial> cards, CardLocation destination, string reason, HanFuStage stage)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = f.HanFu!with { Stage = stage }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var ids = cards.Where(m => m.From != destination).Select(m => m.CardId).ToArray();
        if (ids.Length > 0)
        {
            if (stage == HanFuStage.DiscardChildren)
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(reason));
            else
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, new(reason), (stage is HanFuStage.GiftChildren or HanFuStage.ReturnChildren ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, stage == HanFuStage.ReturnChildren ? f.HanFu!.TargetSeat : f.OwnerSeat, destination.OwnerSeat!.Value, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null) : null));
        }

        for (var i = 0; i < cards.Count; i++)
        {
            var m = cards[i];
            var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(move => move.Sequence > before && move.CardId == m.CardId && move.Reason.Value == reason);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HanFuMaterialPaidEvent(f.Id, stage, i, m, move?.To ?? destination, move?.Sequence));
        }

        RecordHanFuMovement(f.Id, stage, reason, before, ids.Length);
    }

    internal IReadOnlyList<PromptChoice> HanFuChoices(ProgramSkillFrame f)
    {
        var r = f.HanFu!;
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null) => new(new($"hanfu.{(f.Id)}.{(r.Stage)}.{(branch)}.{(string.Join('.', cards ?? []))}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "han-fu", ["branch"] = branch });
        if (r.Stage == HanFuStage.ReturnChoice)
            return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Select(c => Choice("return", "交给韩馥【" + c.DisplayName + "】", [c.Id])).ToArray());
        if (r.Stage == HanFuStage.Benefit)
        {
            var result = new List<PromptChoice>
            {
                Choice("skip", "不执行额外效果")
            };
            if (_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
                result.Add(Choice("damage", "对其造成1点伤害"));
            if (ShuziBoardCards(r.TargetSeat).Count > 0)
                result.Add(Choice("board", "将其他角色场上的一张牌移入其对应区域"));
            return Array.AsReadOnly(result.ToArray());
        }

        if (r.Stage == HanFuStage.BoardChoice)
            return Array.AsReadOnly(ShuziBoardCards(r.TargetSeat).Select(m => Choice("move", "移动" + _coreMatchParticipantsPlayersAccess._players[m.From.OwnerSeat!.Value].Name + "的【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName + "】", [m.CardId])).Prepend(Choice("skip", "不移动")).ToArray());
        if (r.Stage == HanFuStage.DiscardChoice)
            return Array.AsReadOnly(r.DiscardCandidates!.Where(m => !r.SelectedDiscard!.Contains(m.CardId)).Select(m => Choice("discard", $"弃置【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName)}】（{(r.SelectedDiscard!.Count + 1)}/{(r.RequiredDiscard)}）", [m.CardId])).ToArray());
        return[];
    }

    internal void PublishHanFuChoice(ProgramSkillFrame f)
    {
        var r = f.HanFu!;
        var choices = HanFuChoices(f);
        if (choices.Count == 0)
        {
            FinishHanFu(f);
            return;
        }

        var chooser = r.Stage == HanFuStage.ReturnChoice ? r.TargetSeat : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == HanFuStage.ReturnChoice ? "束辎：交给韩馥一张自己的手牌。" : r.Stage == HanFuStage.DiscardChoice ? $"恇守：弃置{(r.RequiredDiscard)}张牌。" : "束辎：选择额外效果。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("han-fu")]
    internal void AnswerHanFu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HanFu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == HanFuStage.ReturnChoice ? r.TargetSeat : f.OwnerSeat) || !HanFuChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("HanFu requires the original chooser's exact published private physical-card or benefit choice.");
        AssertHanFu(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "skip")
        {
            FinishHanFu(f);
            return;
        }

        if (branch == "return")
        {
            var material = HanFuMaterialFor(choice.Cards.Single());
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = r with { Returned = material } });
            PayHanFuCards(f, [material], CardLocation.Hand(f.OwnerSeat), ShuziReturnReason, HanFuStage.ReturnChildren);
            ContinueHanFu(f.Id);
            return;
        }

        if (branch == "board")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = r with { Stage = HanFuStage.BoardChoice } });
            PublishHanFuChoice(f);
            return;
        }

        if (branch == "move")
        {
            PayShuziBoard(f, HanFuMaterialFor(choice.Cards.Single()));
            ContinueHanFu(f.Id);
            return;
        }

        if (branch == "damage")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = r with { Stage = HanFuStage.DamageIssued } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShuziDamageIssuedEvent(f.Id, f.OwnerSeat, r.TargetSeat, 1));
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.TargetSeat, 1);
            return;
        }

        if (branch != "discard")
            throw new InvalidOperationException("Unknown HanFu choice.");
        var selected = r.SelectedDiscard!.Append(choice.Cards.Single()).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = r with { SelectedDiscard = selected } });
        if (selected.Length < r.RequiredDiscard)
        {
            PublishHanFuChoice(f);
            return;
        }

        PayHanFuCards(f, selected.Select(id => r.DiscardCandidates!.Single(m => m.CardId == id)).ToArray(), CardLocation.DiscardPile, KuangshouDiscardReason, HanFuStage.DiscardChildren);
        ContinueHanFu(f.Id);
    }

    internal bool DrainHanFuChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueHanFu(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HanFu: { } r } f && f.Id == id)
        {
            AssertHanFu(f);
            if (r.Stage == HanFuStage.DamageIssued)
            {
                if (f.AttackAttempt is not null)
                    return;
                FinishHanFu(f);
                return;
            }

            if (r.Stage is HanFuStage.ReturnChoice or HanFuStage.Benefit or HanFuStage.BoardChoice or HanFuStage.DiscardChoice)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishHanFuChoice(f);
                return;
            }

            if (DrainHanFuChildren(id))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Op == SkillProgramEffectOp.ResolveShuziExchange && !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            {
                FinishHanFu(f);
                return;
            }

            if (r.Stage == HanFuStage.GiftChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = f.HanFu!with { Stage = HanFuStage.ReturnChoice } });
                PublishHanFuChoice(f);
                return;
            }

            if (r.Stage == HanFuStage.ReturnChildren && r.Returned is { } returned && r.Gifts!.Any(m => m.Name == returned.Name))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = f.HanFu!with { Stage = HanFuStage.Benefit } });
                PublishHanFuChoice(f);
                return;
            }

            if (r.Stage == HanFuStage.DrawChildren)
            {
                FreezeKuangshouDiscard(f);
                return;
            }

            FinishHanFu(f);
            return;
        }
    }

    internal void FinishHanFu(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HanFuCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27020)]
    internal bool ResumeHanFu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HanFu: not null } f || f.Id != id)
            return false;
        ContinueHanFu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27020)]
    internal bool ReturnHanFuMovement(ProgramSkillFrame f)
    {
        if (f.HanFu is null)
            return false;
        ContinueHanFu(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShuziExchange, SkillProgramEffectOp.ResolveKuangshouDamageDrawDiscard)]
    internal PromptChoice AiHanFu(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderByDescending(c => c.Parameters.GetValueOrDefault("branch") == "damage").ThenBy(c => c.Parameters.GetValueOrDefault("branch") == "skip").ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
