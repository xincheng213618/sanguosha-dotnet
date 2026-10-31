#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangZengouRuntime
{
    internal const string WushuangZengou = "wushuang:skill-3287";
    internal static bool UsesActualDodgeZengou(string skillId) => skillId is WushuangZengou or "ol:zengou";
    SkillProgramStepOutcome global::CardGame.Core.IWushuangZengouHost.Resolve(ProgramSkillFrame f) => BeginWushuangZengou(f);
    internal ProgramCardTriggerWindowFrame? WushuangZengouWindow(ProgramSkillWindowContext? context)
    {
        var w = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context?.ParentFrameId);
        if (w is not { ResponseCompletion: { CostsDrained: true, OriginalContinuation: ProgramCardContinuation.Dodge or ProgramCardContinuation.GroupResponse } completion } || w.Action is not { EffectiveKind: CardKind.Dodge, Type: CardActionType.Response } action || action.ActorSeat != action.ProviderSeat || action.RequesterSeat is not null || action.ResponderSeat != action.ActorSeat || w.AttackOwnerFrameId is not { } parent || completion.AttackOwnerFrameId != parent || completion.ActionId != action.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(parent)is not { CardAttack: { EffectiveCardKind: { } kind } attack, Action: { } original } || !(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && completion.OriginalContinuation == ProgramCardContinuation.Dodge || kind == CardKind.ArrowBarrage && completion.OriginalContinuation == ProgramCardContinuation.GroupResponse) || attack.TargetSeat != action.ActorSeat || action.ParentActionId != original.ActionId || action.OpponentSeat != attack.SourceSeat || !_coreResponseCompletionOperations.ValidResponseCompletionWindow(w))
            return null;
        return w;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 61220)]
    internal bool CanTriggerWushuangZengou(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!UsesActualDodgeZengou(c.SkillId))
            return true;
        return WushuangZengouWindow(context)is { } w && w.Action.ActorSeat != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[w.Action.ActorSeat].IsAlive && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(c.OwnerSeat, w.Action.ActorSeat) && _coreCardUseLifecycleOperations.LifecycleCardUse(w.AttackOwnerFrameId!.Value)?.WushuangZengouInvalidation?.Payment.ActionId != w.Action.ActionId;
    }

    internal Card[] WushuangZengouCosts(ProgramSkillFrame f) => WushuangZengouCosts(f.OwnerSeat, f.SkillId, f.SkillInstanceId);
    internal Card[] WushuangZengouCosts(int ownerSeat, string skillId, string skillInstanceId) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(ownerSeat)).Where(c => CardCatalog.Get(_cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Hand(ownerSeat) ? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[ownerSeat], c) : c.Kind).CategoryName != "基本牌" && !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(ownerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, skillInstanceId, c)).ToArray();
    internal IReadOnlyList<PromptChoice> WushuangZengouChoices(ProgramSkillFrame f)
    {
        PromptChoice Choice(string key, string label, int[] cards) => new(new($"wushuang-zengou.{(f.Id)}.{(key)}"), label, cards, [], new Dictionary<string, string> { ["program-action"] = "wushuang-zengou", ["cost"] = key });
        var choices = WushuangZengouCosts(f).Select(c => Choice(c.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), $"弃置【{(c.DisplayName)}】", [c.Id])).ToList();
        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp > 0)
            choices.Add(Choice("hp", "失去1点体力", []));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal SkillProgramStepOutcome BeginWushuangZengou(ProgramSkillFrame f)
    {
        if (f.WushuangZengou is not null)
            throw new InvalidOperationException("Zengou cannot repeat a paid instruction.");
        var w = WushuangZengouWindow(f.WindowContext);
        if (w is null)
            return SkillProgramStepOutcome.Continue;
        var attack = _coreCardUseLifecycleOperations.LifecycleCardUse(w.AttackOwnerFrameId!.Value)!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangZengou = new(f.InstructionIndex, w.Id, attack.Id, w.Action.ActionId, w.Action.ActorSeat, attack.CardAttack!.SuccessfulDodgeResponses) { Cards = w.Action.PhysicalCards.Where(c => w.ResponseCompletion!.NativeCosts.Any(m => m.CardId == c.CardId && m.To == CardLocation.DiscardPile)).Select(c => c.CardId).ToArray() } });
        PublishWushuangZengou(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishWushuangZengou(ProgramSkillFrame f)
    {
        var choices = WushuangZengouChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
        {
            CompleteWushuangZengou(f);
            return;
        }

        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "谮构：支付代价令此张闪无效并获得之。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("wushuang-zengou")]
    internal void AnswerWushuangZengou(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Zengou lost its program parent.");
        if (f.WushuangZengou is not { Stage: "choosing" } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], WushuangZengouChoices(f).Where(c => c.Id == selected.Id).ToArray()) || WushuangZengouWindow(f.WindowContext)?.Action.ActionId != r.ActionId)
            throw new InvalidOperationException("Zengou lost its exact unpaid Dodge use or published choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(f.OwnerSeat, r.DodgerSeat))
        {
            CompleteWushuangZengou(f);
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var cardId = selected.Cards.Count == 1 ? selected.Cards[0] : (int? )null;
        var from = cardId is { } id ? _cardZonesOperations._cardZones.GetLocation(id) : (CardLocation? )null;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangZengou = r with { Stage = "paid", CostCardId = cardId, CostFrom = from, Before = before, After = before } });
        if (cardId is { } paid)
        {
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(f.OwnerSeat, f.OwnerSeat, [_coreAdvancedLifecycleOperations.GetAdvancedCard(paid)], from!.Value, new("skill-program.wushuang-zengou.cost"));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangZengou = f.WushuangZengou!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            PublishWushuangZengouPayment(f);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        // The receipt is already paid before genuine HP/Dying publishes; a resumed child never pays twice.
        PublishWushuangZengouPayment(f);
        var outcome = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, 1);
        if (outcome == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal WushuangZengouPaidEvent WushuangZengouPayment(ProgramSkillFrame f)
    {
        var r = f.WushuangZengou!;
        return new(f.Id, r.ResponseWindowId, r.AttackFrameId, r.ActionId, f.OwnerSeat, r.DodgerSeat, r.DodgeCursor, r.CostCardId, r.CostFrom, r.Before, r.After);
    }

    internal void PublishWushuangZengouPayment(ProgramSkillFrame f) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(WushuangZengouPayment(f));
    [ProgramContinuation(ProgramContinuationKind.Resume, 61220)]
    internal bool ResumeWushuangZengou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WushuangZengou: { } r } f || f.Id != id)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return true;
        if (r.Stage == "choosing")
        {
            PublishWushuangZengou(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.WushuangZengou!;
        if (r.Stage == "paid")
        {
            var native = _coreCardUseLifecycleOperations.LifecycleCardUse(r.AttackFrameId) ?? throw new InvalidOperationException("Paid Zengou lost its actual Slash parent.");
            var proof = new WushuangZengouInvalidatedEvent(WushuangZengouPayment(f), _coreWushuangSkillsBOperations.BSource(f), f.GameplayHash);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(proof);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(native.Id, current => current with { WushuangZengouInvalidation = proof });
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangZengou = r with { Stage = "gained" } });
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                foreach (var cardId in r.Cards.Where(cardId => _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.DiscardPile))
                {
                    _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new("skill-program.wushuang-zengou.gain"));
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangZengouGainedCardEvent(f.Id, r.ActionId, f.OwnerSeat, cardId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
                }

            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        CompleteWushuangZengou(f);
        return true;
    }

    internal void CompleteWushuangZengou(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WushuangZengou = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool ContinueWushuangZengouDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        if (continuation is not (ProgramCardContinuation.Dodge or ProgramCardContinuation.GroupResponse) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.WushuangZengouInvalidation is not { } proof || proof.Payment.ActionId != action.ActionId)
            return false;
        if (proof.Payment.AttackFrameId != attack.ResolutionId || proof.Payment.DodgerSeat != attack.TargetSeat || action.ActorSeat != attack.TargetSeat || action.EffectiveKind != CardKind.Dodge || action.ParentActionId != _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action?.ActionId || proof.Payment.DodgeCursor != attack.SuccessfulDodgeResponses || _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangZengouPaidEvent>().Count(e => e == proof.Payment) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangZengouInvalidatedEvent>().Count(e => e == proof) != 1)
            throw new InvalidOperationException("An invalidated Dodge changed its exact native response cursor.");
        _characterHuanShuJunOlOperations.ClearHuanpeiDodgeResponse(attack);
        _coreRoundOnceCardlessNativeResponsesOperations.ClearRoundOnceCardlessDodgeResponse(attack);
        _coreTieredRoundZeroResponsesOperations.ClearTieredRoundDodgeResponse(attack);
        _coreChainedStateBasicReturnsOperations.ClearChainedStateBasicDodgeResponse(attack);
        _coreDrawFundedDistinctBasicResponsesOperations.ClearDrawFundedDistinctBasicDodgeResponse(attack);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 61220)]
    internal void AssertWushuangZengou(ProgramSkillFrame f)
    {
        if (f.WushuangZengou is not { } r)
            return;
        var effect = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        var w = WushuangZengouWindow(f.WindowContext);
        if (!UsesActualDodgeZengou(f.SkillId) || effect.Op != SkillProgramEffectOp.ResolveWushuangZengou || r.InstructionIndex != f.InstructionIndex || w is null || w.Id != r.ResponseWindowId || w.Action.ActionId != r.ActionId || w.AttackOwnerFrameId != r.AttackFrameId || w.Action.ActorSeat != r.DodgerSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(r.AttackFrameId)?.CardAttack?.SuccessfulDodgeResponses != r.DodgeCursor || r.Cards.Distinct().Count() != r.Cards.Count || r.Cards.Any(card => !w.Action.PhysicalCards.Any(c => c.CardId == card)) || r.Stage is not ("choosing" or "paid" or "gained") || r.Before < 0 || r.After < r.Before)
            throw new InvalidOperationException("Zengou lost its owning once-paid response return.");
        if (r.Stage != "choosing" && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangZengouPaidEvent>().Count(e => e == WushuangZengouPayment(f)) != 1)
            throw new InvalidOperationException("Zengou must retain exactly one cost invoice.");
        if (r.Stage != "choosing" && r.CostCardId is null && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == 1) != 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<IdentityGeneralAppearedEvent>().Any(e => e.Receipt.ParentFrameId == f.Id && e.Receipt.OwnerSeat == f.OwnerSeat && e.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && e.Receipt.RequestedDeduction == 1))
            throw new InvalidOperationException("Zengou lost its exact native HP-loss cost or identity replacement.");
        if (r.Stage != "choosing" && r.CostCardId is { } cardId && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == cardId && m.Sequence > r.Before && m.Sequence <= r.After && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m) == r.CostFrom && m.To == CardLocation.DiscardPile && m.Reason.Value.StartsWith("skill-program.wushuang-zengou.cost", StringComparison.Ordinal)) != 1)
            throw new InvalidOperationException("Zengou lost its exact physical non-basic discard cost.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveWushuangZengou)]
    internal PromptChoice ChooseWushuangZengou(PendingDecision prompt, ProgramSkillFrame frame) => prompt.Choices.OrderBy(choice => choice.Cards.Count == 1 ? 8d : _characterQingheGongzhuOlOperations.ActualDodgeZengouHpCostForAi(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])).First();
}
