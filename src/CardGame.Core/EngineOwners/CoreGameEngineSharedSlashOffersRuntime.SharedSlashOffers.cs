#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSharedSlashOffersRuntime
{
    internal const string SharedSlashGiftReason = "program.shared-slash.gift";
    internal const string SharedSlashOwnerDrawReason = "program.shared-slash.owner-draw";
    internal const string SharedSlashRecipientDrawReason = "program.shared-slash.recipient-draw";
    internal IEnumerable<IGameEvent> SharedSlashHistory() => _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents);
    internal bool IsSharedSlashGiftSelection(ProgramSkillFrame f, string bind)
    {
        if (f.TriggerId is not null)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        return f.InstructionIndex == 1 && plan.Instructions.Count == 2 && plan.Instructions[0].Op == SkillProgramEffectOp.SelectOwnedCards && plan.Instructions[0].ResultBind == bind && plan.Instructions[1].Op == SkillProgramEffectOp.GiveBoundCardThenOfferVirtualSlashOrSharedDraw && plan.Instructions[1].SourceBind == bind;
    }

    internal bool MatchesProgramOwnedSelectionKind(ProgramSkillFrame f, string bind, CharacterState owner, Card card, CardLocation from, IReadOnlyList<CardKind> kinds) => (!IsSharedSlashGiftSelection(f, bind) || !card.IsGeneralWeapon) && (kinds.Count == 0 || ProgramCardKindMatch.Contains(kinds, card.Kind) || IsSharedSlashGiftSelection(f, bind) && from == CardLocation.Hand(owner.Seat) && card.Kind == CardKind.Alcohol && kinds.Contains(CardKind.Slash) && _coreAlcoholIdentityPoliciesOperations.HasAlcoholKingIdentityPolicy(owner));
    internal bool SharedSlashGiftKindStillValid(ProgramSkillFrame f, Card card, CardLocation from)
    {
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        var selection = plan.Instructions[0];
        return selection.CardKinds.Count == 0 || ProgramCardKindMatch.Contains(selection.CardKinds, card.Kind) || from == CardLocation.Hand(f.OwnerSeat) && card.Kind == CardKind.Alcohol && selection.CardKinds.Contains(CardKind.Slash) && _coreAlcoholIdentityPoliciesOperations.HasAlcoholKingIdentityPolicy(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
    }

    internal CardConversionSource SharedSlashSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SkillProgramStepOutcome BeginSharedSlashGift(long frameId, int actorSeat, string bind)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var cards = _coreProgramLifecycleOperations.GetProgramCardSet(f, bind);
        if (f.SharedSlashOffer is not null)
            throw new InvalidOperationException("A shared Slash gift cannot repay its cost.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) || actorSeat == f.OwnerSeat || f.SelectedTargetSeats is not [var selected] || selected != actorSeat || !_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || cards.CardIds.Count != 1 || cards.SourceLocations.Count != 1 || cards.SourceLocations[0].OwnerSeat != f.OwnerSeat || cards.SourceLocations[0].Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _cardZonesOperations._cardZones.GetLocation(cards.CardIds[0]) != cards.SourceLocations[0])
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "赠牌来源、受赠者或技能实例已失效，未支付。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(cards.SourceLocations[0]).Single(c => c.Id == cards.CardIds[0]);
        if (card.IsGeneralWeapon || !SharedSlashGiftKindStillValid(f, card, cards.SourceLocations[0]))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "赠牌的实际身份已失效，未支付。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var receipt = new SharedSlashOfferReceipt(f.InstructionIndex, bind, SharedSlashSource(f), actorSeat, new(card.Id, card.Kind, cards.SourceLocations[0], _coreActionDiscardColorsOperations.CapturePhysicalCardColor(f.OwnerSeat, card)));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SharedSlashOffer = receipt });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(actorSeat), new(SharedSlashGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, actorSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, movements) =>
        {
            var moved = movements.Single();
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
            _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, current with { SharedSlashOffer = current.SharedSlashOffer!with { GiftMovementSequence = moved.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SharedSlashGiftCommittedEvent(frameId, receipt.Source, actorSeat, moved.Sequence));
        }));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frameId);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool SharedSlashUnchosenSourceValid(ProgramSkillFrame f) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.SharedSlashOffer!.ActorSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId);
    internal int[] SharedSlashLegalTargets(int actorSeat) => _coreProgramVirtualSlashOffersOperations.GetProgramVirtualSlashOfferTargets(actorSeat).Where(seat => !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.Slash, Suit.None)).ToArray();
    [ProgramContinuation(ProgramContinuationKind.Resume, 580)]
    internal bool ResumeSharedSlashOffer(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || f.SharedSlashOffer is not { } r)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return true;
        if (r.Stage == SharedSlashOfferStage.GiftChildren)
        {
            if (!SharedSlashUnchosenSourceValid(f))
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, false);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SharedSlashOffer = r with { Stage = SharedSlashOfferStage.ChoosingOption } });
            PublishSharedSlashOffer(f);
            return true;
        }

        if (r.Stage is SharedSlashOfferStage.ChoosingOption or SharedSlashOfferStage.ChoosingTarget)
        {
            if (!SharedSlashUnchosenSourceValid(f))
            {
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreSkillProgramsOperations.FinishProgramSkill(f, false);
                return true;
            }

            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishSharedSlashOffer(f);
            return true;
        }

        if (r.Stage == SharedSlashOfferStage.SlashIssued)
            throw new InvalidOperationException("A shared Slash producer returned without its exact typed completion.");
        if (r.Stage == SharedSlashOfferStage.OwnerDrawIssued)
        {
            IssueSharedSlashDraw(f, recipient: true);
            return true;
        }

        if (r.Stage == SharedSlashOfferStage.RecipientDrawIssued)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SharedSlashOffer = r with { Stage = SharedSlashOfferStage.Complete } });
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return true;
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    internal IReadOnlyList<PromptChoice> SharedSlashChoices(ProgramSkillFrame f)
    {
        var r = f.SharedSlashOffer!;
        PromptChoice Choice(string option, string label, IReadOnlyList<int> targets) => new(new($"shared-slash.{(f.Id)}.{(option)}"), label, [], targets, new Dictionary<string, string> { ["program-action"] = "shared-slash-offer", ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["offer-option"] = option });
        if (r.Stage == SharedSlashOfferStage.ChoosingTarget)
            return Array.AsReadOnly(SharedSlashLegalTargets(r.ActorSeat).Select(seat => Choice($"target-{(seat)}", $"令 {(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 使用【杀】", [seat])).ToArray());
        var choices = new List<PromptChoice>();
        if (SharedSlashLegalTargets(r.ActorSeat).Length > 0)
            choices.Add(Choice("slash", "视为使用一张【杀】；若造成伤害，双方各摸一张牌", []));
        choices.Add(Choice("draw", "你与明策拥有者各摸一张牌", []));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishSharedSlashOffer(ProgramSkillFrame f)
    {
        var r = f.SharedSlashOffer!;
        var target = r.Stage == SharedSlashOfferStage.ChoosingTarget;
        var chooser = target ? f.OwnerSeat : r.ActorSeat;
        var choices = SharedSlashChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, target ? "请选择该角色使用【杀】的目标。" : "请选择使用【杀】或双方摸牌。", [], target ? SharedSlashLegalTargets(r.ActorSeat) : [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shared-slash-offer")]
    internal void ResolveSharedSlashOfferChoice(PromptChoice choice)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack.Last().Id);
        var r = f.SharedSlashOffer ?? throw new InvalidOperationException("A shared Slash answer lost its paid gift.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == SharedSlashOfferStage.ChoosingTarget ? f.OwnerSeat : r.ActorSeat) || !SharedSlashChoices(f).Any(c => c.Id == choice.Id && c.Targets.SequenceEqual(choice.Targets) && c.Cards.SequenceEqual(choice.Cards)))
            throw new InvalidOperationException("A shared Slash choice changed its frozen chooser or target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!SharedSlashUnchosenSourceValid(f))
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        var option = choice.Parameters.GetValueOrDefault("offer-option");
        if (r.Stage == SharedSlashOfferStage.ChoosingOption && option == "draw")
        {
            IssueSharedSlashDraw(f, false);
            return;
        }

        if (r.Stage == SharedSlashOfferStage.ChoosingOption && option == "slash")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SharedSlashOffer = r with { Stage = SharedSlashOfferStage.ChoosingTarget } });
            PublishSharedSlashOffer(f);
            return;
        }

        if (r.Stage != SharedSlashOfferStage.ChoosingTarget || choice.Targets is not [var seat] || !SharedSlashLegalTargets(r.ActorSeat).Contains(seat) || option != $"target-{(seat)}")
            throw new InvalidOperationException("The offered shared Slash target is no longer legal.");
        BeginSharedSlashUse(f, seat);
    }

    internal void BeginSharedSlashUse(ProgramSkillFrame f, int targetSeat)
    {
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("A shared Slash cannot overwrite an attack.");
        var r = f.SharedSlashOffer!;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat];
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, CardKind.Slash, [targetSeat], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        var returned = new SharedSlashBenefitReturn(f.Id, f.InstructionIndex, r.Source, actor.Seat, targetSeat, id, action.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SharedSlashOffer = r with { Stage = SharedSlashOfferStage.SlashIssued, SlashReturn = returned } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, actor.Seat, 0, CardKind.Slash, [targetSeat], PhysicalCardIds: []) { Action = action, SharedSlashBenefit = returned });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SharedSlashUseIssuedEvent(returned));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.Slash, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, [targetSeat]));
        _characterLuMengMouOperations.RecordYingboCardUse(id, actor.Seat, CardKind.Slash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Slash);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, targetSeat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, targetSeat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 90)]
    internal bool ReturnRegisteredCompleteSharedSlashBenefit(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.SharedSlashBenefit is null)
            return false;
        CompleteSharedSlashBenefit(completion);
        return true;
    }

    internal void CompleteSharedSlashBenefit(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        var returned = completion.SharedSlashBenefit ?? throw new InvalidOperationException("A shared Slash lost its typed return.");
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(returned.ProgramFrameId);
        var r = f.SharedSlashOffer!;
        if (completion.ProgramFrameId != f.Id || completion.ResolutionId != returned.CardUseFrameId || r.Stage != SharedSlashOfferStage.SlashIssued || r.SlashReturn != returned || returned.Source != SharedSlashSource(f) || returned.InstructionIndex != f.InstructionIndex || !SharedSlashHistory().OfType<CardUseFinishedEvent>().Any(e => e.ResolutionId == returned.CardUseFrameId) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(u => u.Id == returned.CardUseFrameId))
            throw new InvalidOperationException("The shared Slash completed outside its exact owning use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SharedSlashOffer = r with { CausedDamage = completion.SharedSlashCausedDamage } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SharedSlashUseReturnedEvent(returned, completion.SharedSlashCausedDamage));
        if (completion.SharedSlashCausedDamage)
            IssueSharedSlashDraw(f, false);
        else
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SharedSlashOffer = f.SharedSlashOffer!with { Stage = SharedSlashOfferStage.Complete } });
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        }
    }

    internal void IssueSharedSlashDraw(ProgramSkillFrame f, bool recipient)
    {
        var r = f.SharedSlashOffer!;
        var seat = recipient ? r.ActorSeat : f.OwnerSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SharedSlashOffer = r with { Stage = recipient ? SharedSlashOfferStage.RecipientDrawIssued : SharedSlashOfferStage.OwnerDrawIssued } });
        var drawn = _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], 1, true, new(recipient ? SharedSlashRecipientDrawReason : SharedSlashOwnerDrawReason)) : Array.Empty<int>();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SharedSlashDrawIssuedEvent(f.Id, seat, drawn.Count));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.GiveBoundCardThenOfferVirtualSlashOrSharedDraw)]
    internal PromptChoice SelectAiSharedSlashOffer(PendingDecision decision, ProgramSkillFrame f)
    {
        var actor = f.SharedSlashOffer!.ActorSeat;
        var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var damage = new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false);
        if (f.SharedSlashOffer.Stage == SharedSlashOfferStage.ChoosingTarget)
            return decision.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ScoreProgramTarget(snapshot, c.Targets.Single(), damage)).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        var canDamage = SharedSlashLegalTargets(actor).Any(seat => _coreAiBrainsAiBrainsAccess._aiBrains[actor].ScoreProgramTarget(snapshot, seat, damage) > 0);
        return decision.Choices.First(c => c.Parameters.GetValueOrDefault("offer-option") == (canDamage ? "slash" : "draw"));
    }

    SkillProgramStepOutcome global::CardGame.Core.ISharedSlashOfferProgramHost.GiveBoundCardThenOffer(ProgramSkillFrame frame, int actorSeat, string sourceBind) => BeginSharedSlashGift(frame.Id, actorSeat, sourceBind);
}
