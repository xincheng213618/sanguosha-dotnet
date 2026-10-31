#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduCardsRuntime
{
    internal static bool IsGuanduCard(CardKind kind) => kind == CardKind.Intoxication || IsGuanduTrick(kind);
    internal static bool IsGuanduTrick(CardKind kind) => global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsDefinitions.GuanduOrdinaryTrickKinds.Contains(kind);
    internal static LegalActionKind GuanduCardAction(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_GuanduCards.GuanduCardAction(kind);
    internal bool GuanduIntoxicationTargetSpent(int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduIntoxicationCommittedEvent>().Any(e => e.ActualTurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && e.TargetSeat == target);
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 220)]
    internal int GuanduIntoxicationDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _coreActivityBattleRulesOperations.GuanduRules?.IntoxicationDamageBonus(attack.TargetSeat) ?? 0;
    internal bool CanBeGuanduAdditionalTarget(CardUseFrame use, CharacterState target) => target.IsAlive && (use.CardKind switch
    {
        CardKind.StoreGrain => true,
        CardKind.Reinforcements => target.Seat != use.SourceSeat && _coreTreatedWoundedOperations.IsRuleWounded(target),
        CardKind.Intoxication => !_coreGuanduActivitiesOperations.GuanduAllies(use.SourceSeat, target.Seat) && !GuanduIntoxicationTargetSpent(target.Seat),
        _ => false
    });
    internal IReadOnlyList<int> GuanduEligibleTargets(CharacterState actor, CardKind kind, Suit? suit, bool? color, bool excludeOwner = false) => Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(actor.Seat, offset)]).Where(target => target.IsAlive && (!excludeOwner || actor.Seat != target.Seat) && (kind switch
    {
        CardKind.Intoxication => !_coreGuanduActivitiesOperations.GuanduAllies(actor.Seat, target.Seat) && !GuanduIntoxicationTargetSpent(target.Seat),
        CardKind.Reinforcements => target.Seat != actor.Seat && _coreTreatedWoundedOperations.IsRuleWounded(target),
        CardKind.StoreGrain or CardKind.UnitedEffort => true,
        _ => false
    }) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, suit) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(actor.Seat, target.Seat, kind) && (kind != CardKind.UnitedEffort || !_coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ExcludeGlobalTarget, kind))).Select(target => target.Seat).ToArray();
    internal IEnumerable<IReadOnlyList<int>> GuanduTargetChoices(CharacterState actor, CardKind kind, Suit? suit, bool? color, bool excludeOwner = false)
    {
        var seats = GuanduEligibleTargets(actor, kind, suit, color, excludeOwner);
        if (kind == CardKind.UnitedEffort)
        {
            foreach (var allies in new[]
            {
                true,
                false
            }

            )
            {
                var team = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, seats.Where(seat => _coreGuanduActivitiesOperations.GuanduAllies(actor.Seat, seat) == allies).ToArray());
                if (team.Count > 0)
                    yield return team;
            }

            yield break;
        }

        var cap = kind == CardKind.StoreGrain ? 3 : kind == CardKind.Reinforcements ? 2 : 1;
        for (var mask = 1; mask < (1 << seats.Count); mask++)
        {
            var selected = seats.Where((_, i) => (mask & (1 << i)) != 0).ToArray();
            if (selected.Length <= cap)
                yield return Array.AsReadOnly(selected);
        }
    }

    internal IReadOnlyList<int>? GuanduExpectedNativeTargets(CharacterState actor, Card card, LegalAction action) => IsGuanduCard(action.PlayedCardKind ?? card.Kind) ? action.TargetSeats : null;
    internal void AddGuanduCardActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> cards)
    {
        foreach (var card in cards.Where(card => IsGuanduCard(card.Kind)))
        {
            AddGuanduPhysicalOrConvertedActions(actions, actor, card, card.Kind, null);
            if (card.Kind == CardKind.UnitedEffort)
                actions.Add(new(LegalActionKind.Recast, card.Id, null, "重铸【勠力同心】：弃置此牌，摸一张牌"));
        }
    }

    internal void AddGuanduPhysicalOrConvertedActions(List<LegalAction> actions, CharacterState actor, Card card, CardKind kind, CardConversionSource? conversion)
    {
        if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return;
        foreach (var targets in GuanduTargetChoices(actor, kind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))))
            actions.Add(new(GuanduCardAction(kind), card.Id, targets.Count == 1 ? targets[0] : null, _coreCardConversionsOperations.DescribeConversion(conversion, $"使用【{(CardCatalog.Get(kind).DisplayName)}】：{(string.Join('、', targets.Select(t => _coreMatchParticipantsPlayersAccess._players[t].Name)))}"), conversion is null ? null : kind, TargetSeats: targets) { ConversionSource = conversion });
    }

    internal void ResolveGuanduCard(CharacterState actor, Card card, LegalAction action)
    {
        var kind = action.PlayedCardKind ?? card.Kind;
        if (!IsGuanduCard(kind) || action.TargetSeats.Count == 0)
            throw new InvalidOperationException("A Guandu card lost its published card name or real targets.");
        var targets = Array.AsReadOnly(action.TargetSeats.ToArray());
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, targets, kind, conversionSource: action.ConversionSource, physicalCardIds: card.Id == 0 ? [] : null);
        if (card.Id != 0)
            _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        if (kind == CardKind.Intoxication)
        {
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
            foreach (var target in targets)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduIntoxicationCommittedEvent(id, use.Action!.ActionId, _coreActualTurnProgressTurnNumberAccess._turnNumber, actor.Seat, target));
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(card.Id, SimpleCardUseEffect.GuanduIntoxication));
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, targets, GuanduCardAction(kind), playedCardKind: kind);
    }

    internal bool ContinueGuanduIntoxicationUse(long id, ProgramSimpleCardContinuation continuation)
    {
        if (continuation.Effect != SimpleCardUseEffect.GuanduIntoxication)
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id) ?? throw new InvalidOperationException("Intoxication lost its native basic use.");
        if (use.CardKind != CardKind.Intoxication || use.Action is not { Type: CardActionType.Use } action || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count || use.DyingResponse is not null)
            throw new InvalidOperationException("Intoxication cannot become a rescue, ordinary Wine, or unrelated basic effect.");
        var target = use.TargetSeats[use.TargetIndex];
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<GuanduIntoxicationCommittedEvent>().Any(e => e.UseFrameId == id && e.TargetSeat == target))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduIntoxicationCommittedEvent(id, action.ActionId, _coreActualTurnProgressTurnNumberAccess._turnNumber, use.SourceSeat, target));
        if (!_coreGuanduActivitiesOperations.GuanduAllies(use.SourceSeat, target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(id, target))
        {
            var occurrence = _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduIntoxicationAppliedEvent>().Count(e => e.UseFrameId == id && e.TargetSeat == target) + 1;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduIntoxicationAppliedEvent(id, action.ActionId, _coreActualTurnProgressTurnNumberAccess._turnNumber, use.SourceSeat, target, occurrence));
        }

        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        _coreNativeCardUseCompletionOperations.FinishCardUse(id, _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, requireProcessing: true), use.CardKind);
        return true;
    }

    internal void ResolveGuanduTrickEffect(NullificationWindowFrame pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != pending.ParentFrameId || !IsGuanduTrick(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)) || pending.EffectCardKind != global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) || pending.ActionKind != GuanduCardAction(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)) || use.GuanduCardEffect is not null || use.Action is not { Type: CardActionType.Use } action || use.SourceSeat != pending.SourceSeat || use.TargetIndex < 0 || use.TargetIndex >= global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending)is not { } target || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != target)
            throw new InvalidOperationException("A Guandu trick lost its exact resolved native target and effect occurrence.");
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use));
            return;
        }

        var allied = _coreGuanduActivitiesOperations.GuanduAllies(use.SourceSeat, target);
        var amount = global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.StoreGrain || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.UnitedEffort && allied && _coreMatchParticipantsPlayersAccess._players[target].IsChained ? 1 : 0;
        var occurrence = _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduCardEffectStartedEvent>().Count(e => e.UseFrameId == use.Id && e.TargetIndex == use.TargetIndex) + 1;
        var receipt = new GuanduCardEffectReceipt(action.ActionId, pending.Id, global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), target, use.TargetIndex, occurrence, global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.Reinforcements ? GuanduCardEffectStage.RecoveryChildren : global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.UnitedEffort && !allied ? GuanduCardEffectStage.ChainChildren : GuanduCardEffectStage.Drawing)
        {
            AlliedBranch = allied,
            RequestedCount = amount,
            AwaitingProgramFrameId = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()is { } parent && global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime.IsAwaitingProgramMovement(parent) ? parent.Id : null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { Step = ResolutionFrameStep.ResolvingEffect, GuanduCardEffect = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduCardEffectStartedEvent(use.Id, action.ActionId, pending.Id, global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), target, use.TargetIndex, occurrence, allied, amount));
        if (global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.Reinforcements)
            ApplyGuanduRecovery(use.Id);
        else if (global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.UnitedEffort && !allied)
        {
            _coreMatchParticipantsPlayersAccess._players[target].IsChained = _coreEquipmentSlotGroupProgramsOperations.ResolveEnteringChain(_coreMatchParticipantsPlayersAccess._players[target], true);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IronChainStateChangedEvent(use.Id, use.SourceSeat, target, _coreMatchParticipantsPlayersAccess._players[target].IsChained));
        }

        ContinueGuanduCardEffect(use.Id);
    }

    internal void ApplyGuanduRecovery(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var r = use.GuanduCardEffect!;
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        var amount = target.IsAlive && _coreTreatedWoundedOperations.IsRuleWounded(target) ? _coreCardRecoveryModifiersOperations.ApplyIssuedCardRecoveryModifiers(id, use.SourceSeat, target.Seat, 1) : 0;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { GuanduCardEffect = current.GuanduCardEffect!with { RecoveryAmount = amount } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduCardRecoveryIssuedEvent(id, r.NullificationFrameId, r.Occurrence, use.SourceSeat, target.Seat, amount));
        if (amount <= 0 || _coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(id, use.SourceSeat, target.Seat, amount, new(RecoveryAttemptProducer.CardUse, CardId: use.CardId, CardKind: CardKind.Reinforcements, Policies: [])))
            return;
        var recoveryId = _coreNativeRecoveryFramesOperations.BeginRecovery(id, use.SourceSeat, target.Seat, amount);
        try
        {
            var actual = Math.Min(amount, target.MaxHp - target.Hp);
            target.Hp += actual;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(use.SourceSeat, target.Seat, actual, target.Hp));
        }
        finally
        {
            _coreRuntimeOperations.PopResolutionFrame(recoveryId, ResolutionFrameKind.Recovery);
        }
    }

    [NativeFrameRoute(typeof(CardUseFrame), 40, typeof(NativeFrameRouteConditions), "GuanduCardEffect")]
    internal void ContinueGuanduCardEffect(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { GuanduCardEffect: { } r } use && use.Id == id)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
                return;
            if (r.Stage != GuanduCardEffectStage.Drawing)
            {
                if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.GuanduCardEffect) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.GuanduCardEffect) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.GuanduCardEffect) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                    return;
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { GuanduCardEffect = null, NativeDraw = null });
                _coreNativeCardUseCompletionOperations.FinishCardUse(id, _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, requireProcessing: true), global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use));
                return;
            }

            if (use.NativeDraw is null)
                _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { NativeDraw = new NativeDrawProducerReceipt(0, r.RequestedCount, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw) { Recipients = [r.TargetSeat], ReturnKind = NativeDrawAttemptReturnKind.GuanduCardEffect } });
            var paid = use.NativeDraw!;
            if (paid.ActiveAttemptId is not null && !paid.AttemptReturned)
                throw new InvalidOperationException("A Guandu draw resumed before its actual request returned.");
            if (!paid.AttemptReturned && _coreNativeDrawAttemptsOperations.TryBeginNativeDrawAttempt(use))
                return;
            var before = _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;
            var ids = paid.Replacement is null && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], r.RequestedCount, true, CardMoveReasons.Draw) : Array.AsReadOnly(Array.Empty<int>());
            var outcome = new NativeDrawOutcome(r.TargetSeat, r.RequestedCount, paid.ActiveAttemptId, before, _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence, paid.Replacement)
            {
                ActualDrawIds = ids
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { NativeDraw = null, GuanduCardEffect = r with { Stage = GuanduCardEffectStage.DrawingChildren, DrawOutcome = outcome } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduCardDrawAppliedEvent(id, r.NullificationFrameId, r.Occurrence, r.TargetSeat, r.RequestedCount, ids.Count, before, outcome.SequenceAfter));
        }
    }

    internal bool IsGuanduCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => use.GuanduCardEffect is { Stage: GuanduCardEffectStage.DrawingChildren, DrawOutcome: { } result } r && batch.ParentFrameId == use.Id && batch.AwaitingProgramFrameId == r.AwaitingProgramFrameId && batch.Movements.Count > 0 && batch.Movements.All(m => m.Sequence > result.SequenceBefore && m.Sequence <= result.SequenceAfter && (result.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(result.RecipientSeat) && m.Reason == CardMoveReasons.Draw || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle));
    internal bool IsGuanduCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => frame.ResumeGuanduCardEffectFrameId == use.Id && frame.Id == frame.Batch.Id && frame.ResumeProgramFrameId is null && !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(frame with { ResumeGuanduCardEffectFrameId = null }) && IsGuanduCardEffectMovement(use, frame.Batch);
    internal bool IsGuanduCardEffectHpReturn(CardUseFrame use, HpChangedTriggerWindowFrame frame) => use.GuanduCardEffect is { Stage: GuanduCardEffectStage.RecoveryChildren } r && frame.ResumeFrameId == use.Id && frame.Continuation == PostEventContinuation.GuanduCardEffect && frame.Change.ParentFrameId == use.Id && frame.Change.SourceSeat == use.SourceSeat && frame.Change.TargetSeat == r.TargetSeat && frame.CardId is null && frame.CardKind is null;
    internal bool IsGuanduCardRecoveryAttempt(CardUseFrame use, RecoveryAttempt attempt) => use.GuanduCardEffect is { Stage: GuanduCardEffectStage.RecoveryChildren, RecoveryAmount: > 0 } r && attempt.SourceSeat == use.SourceSeat && attempt.TargetSeat == r.TargetSeat && attempt.Amount == r.RecoveryAmount && attempt.Completion.Producer == RecoveryAttemptProducer.CardUse && attempt.Completion.CardId == use.CardId && attempt.Completion.CardKind == global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) && attempt.Completion.Policies is { Count: 0 } && _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduCardRecoveryIssuedEvent>().Count(e => e.UseFrameId == use.Id && e.NullificationFrameId == r.NullificationFrameId && e.Occurrence == r.Occurrence && e.SourceSeat == use.SourceSeat && e.TargetSeat == r.TargetSeat && e.Amount == attempt.Amount) == 1;
    internal bool IsGuanduCardEffectRecoveryReturn(CardUseFrame use, RecoveryReplacementFrame frame) => frame.ParentFrameId == use.Id && frame.Return == new RecoveryReplacementReturn(PostEventContinuation.GuanduCardEffect, use.Id) && IsGuanduCardRecoveryAttempt(use, frame.Attempt);
}
