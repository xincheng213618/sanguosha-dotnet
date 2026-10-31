#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNextActualUseTargetAdjustmentsRuntime
{
    internal const string NextActualUseAdjustmentBinding = "next-actual-use-target-adjustment";
    void global::CardGame.Core.INextActualUseTargetAdjustmentProgramHost.GrantNextActualUseTargetAdjustment(ProgramSkillFrame frame, SkillProgramEffect effect) => GrantNextActualUseTargetAdjustment(frame, effect);
    internal void GrantNextActualUseTargetAdjustment(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var program = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!;
        var paid = active.PindianResultBindings.SingleOrDefault(b => b.Name == effect.Condition.SourceBind);
        var paused = ProgramInstructionResolver.Default.Resolve(active, program).GetPausedInstruction(active.InstructionIndex).Effect;
        if (active.TriggerId is not null || paused.Op != SkillProgramEffectOp.GrantNextActualUseTargetAdjustment || paused.StateId != effect.StateId || paused.Condition.SourceBind != effect.Condition.SourceBind || paid is not { SourceWon: true } || paid.SourceSeat != frame.OwnerSeat || active.SelectedTargetSeats.Count != 1 || paid.OpponentSeat != active.SelectedTargetSeats[0] || _coreProgramPersistentStateOperations.GetProgramBooleanState(active, effect.StateId!))
            throw new InvalidOperationException("Next actual-use adjustment lost its paid winning contest and enabled source.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NextActualUseTargetAdjustmentGrantedEvent(active.Id, active.InstructionIndex - 1, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, new(active.SkillId, NextActualUseAdjustmentBinding, active.OwnerSeat, active.SkillInstanceId), active.GameplayHash, effect.StateId!));
    }

    internal NextActualUseTargetAdjustmentGrantedEvent? CurrentNextActualUseAdjustment(int ownerSeat)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || !_coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).ProgramInstances.Any(instance => instance.Program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.GrantNextActualUseTargetAdjustment))))
            return null;
        var turnFacts = _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or TurnEndedEvent).ToArray();
        var grant = turnFacts.OfType<NextActualUseTargetAdjustmentGrantedEvent>().LastOrDefault(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && e.Source.OwnerSeat == ownerSeat);
        if (grant is null || turnFacts.OfType<NextActualUseTargetAdjustmentConsumedEvent>().Any(e => e.GrantProgramFrameId == grant.ProgramFrameId) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[ownerSeat], grant.Source.SkillId, grant.Source.SkillInstanceId) || _contentRegistryOperations._contentRegistry.GetSkill(grant.Source.SkillId).Program?.GameplayHash != grant.GameplayHash || _coreProgramPersistentStateOperations.GetProgramBooleanState2(ownerSeat, grant.Source.SkillId, grant.Source.SkillInstanceId, grant.DisabledStateId))
            return null;
        return _characterJianYongBoundaryMobileOperations.IsBoundaryJianYongAdjustmentCurrent(grant) ? grant : null;
    }

    internal bool HasNextActualUseAdjustment(CharacterState owner) => CurrentNextActualUseAdjustment(owner.Seat)is not null;
    internal bool IsNextActualUseAdjustmentAction(CharacterState actor, LegalAction action) => action.ProgramActivationId == NextActualUseAdjustmentBinding && CurrentNextActualUseAdjustment(actor.Seat)is { } grant && action.ProgramSkillId == grant.Source.SkillId;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 220)]
    internal void ObserveNextActualUseAdjustment(IGameEvent payload)
    {
        long frameId;
        int actor;
        long? actionId;
        CardKind kind;
        bool nullification;
        if (payload is CardUseDeclaredEvent declared && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == declared.ResolutionId)is { } use && use.SourceSeat == declared.SourceSeat && use.CardId == declared.CardId && use.CardKind == declared.CardKind && (use.Action is { Type: CardActionType.Use } || use.Action is null && use.CardId == 0 && use.PhysicalCardIds is { Count: 0 } && _coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use)))
        {
            MarkNextActualUseProgramChild(use);
            frameId = use.Id;
            actor = use.SourceSeat;
            actionId = use.Action?.ActionId;
            kind = use.CardKind;
            nullification = false;
        }
        else if (payload is CardActionAcceptedEvent { Action: { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification } action } && action.ActorSeat == action.ProviderSeat && action.ResponderSeat == action.ActorSeat && action.RequesterSeat is null && _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault()is { } window && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == window.ParentFrameId)is { } parent && action.ParentActionId == parent.Action?.ActionId && action.OpponentSeat == window.SourceSeat)
        {
            frameId = window.Id;
            actor = action.ActorSeat;
            actionId = action.ActionId;
            kind = action.EffectiveKind;
            nullification = true;
        }
        else
            return;
        if (CurrentNextActualUseAdjustment(actor)is not { } grant)
            return;
        if (!_characterJianYongBoundaryMobileOperations.BoundaryJianYongAdjustmentConsumes(grant, kind))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NextActualUseTargetAdjustmentConsumedEvent(grant.ProgramFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, grant.Source, frameId, actionId, kind, nullification));
    }

    internal bool IsNextActualUseCounterspellForbidden(int seat, CardKind kind, CardActionType type) => type == CardActionType.Response && kind == CardKind.Nullification && _turnCardUseEffectsOperations._turnCardUseEffects.ActionProhibitions.Any(p => p.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && p.Source.OwnerSeat == seat && p.ActionTypes.Contains(CardActionType.Use) && ProgramCardKindMatch.Contains(p.CardKinds, kind) && _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program?.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.GrantNextActualUseTargetAdjustment)) == true);
    [ProgramCardAction(ProgramCardActionPhase.AfterProgram, 20)]
    internal void AddNextActualUseAdjustmentActions(List<LegalAction> actions, CharacterState actor)
    {
        if (CurrentNextActualUseAdjustment(actor.Seat)is not { } grant)
            return;
        for (var i = 0; i < actions.Count; i++)
            if (actions[i] is { Kind: LegalActionKind.UseEquipmentEffect, EquipmentKind: CardKind.ZhangbaSerpentSpear, MinTargetCount: 1, MaxTargetCount: 1 } spear)
                actions[i] = spear with
                {
                    MaxTargetCount = 2
                };
        var keys = actions.Select(a => (a.CardId, a.Kind, a.PlayedCardKind, a.ConversionSource, a.TargetCardId, Targets: string.Join(",", a.TargetSeats))).ToHashSet();
        foreach (var action in actions.ToArray().Where(a => a.CardId is not null && a.ProgramActivationId is null && a.Kind != LegalActionKind.Recast))
        {
            var(kind, suit) = TargetAdjustmentAppearance(actor, action);
            var normal = action.Kind is LegalActionKind.DrawTwo or LegalActionKind.Peach or LegalActionKind.Alcohol ? new[]
            {
                actor.Seat
            }

            : _coreDirectedCardPolicyQueriesOperations.GetDeclaredCardTargets(actor, action.Kind, action.TargetSeats).ToArray();
            if (normal.Length == 0)
                continue;
            if (kind == CardKind.BorrowedSword)
            {
                normal = action.TargetSeats.ToArray();
                foreach (var holder in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != actor.Seat && !normal.Where((_, i) => i % 2 == 0).Contains(p.Seat) && _coreNativeBorrowedSwordOperations.GetWeapon(p)is not null && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, p.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, suit, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(suit)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, suit)))
                    foreach (var victim in _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(holder, p)))
                        Add([..normal, holder.Seat, victim.Seat]);
                if (normal.Length >= 4)
                    for (var i = 0; i < normal.Length; i += 2)
                        Add(normal.Where((_, j) => j != i && j != i + 1).ToArray());
                continue;
            }

            if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind) || kind is CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning)
                continue;
            var possible = kind is CardKind.BarbarianAssault or CardKind.ArrowBarrage or CardKind.PeachGarden or CardKind.FiveGrains ? normal : _coreMatchParticipantsPlayersAccess._players.Where(p => CanBeExtraTargetAdjustmentTarget(actor, p, action, kind, suit) && !(p.Seat == actor.Seat && action.ConversionSource is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)?.ExcludeOwnerEffects == true)).Select(p => p.Seat);
            foreach (var seat in possible.Except(normal))
                Add([..normal, seat]);
            if (normal.Length > 1)
                foreach (var seat in normal)
                    Add(normal.Where(s => s != seat).ToArray());
            void Add(IReadOnlyList<int> targets)
            {
                if (targets.Count == 0 || !keys.Add((action.CardId, action.Kind, action.PlayedCardKind, action.ConversionSource, action.TargetCardId, string.Join(",", targets))))
                    return;
                actions.Add(action with { TargetSeat = targets[0], TargetSeats = Array.AsReadOnly(targets.ToArray()), ProgramSkillId = grant.Source.SkillId, ProgramActivationId = NextActualUseAdjustmentBinding, Description = action.Description + "（巧说目标调整）" });
            }
        }
    }

    internal (CardKind Kind, Suit Suit) TargetAdjustmentAppearance(CharacterState actor, LegalAction action)
    {
        if (action.CardId == 0)
            return (action.PlayedCardKind!.Value, Suit.None);
        var physical = _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, action.CardId)!;
        return (action.PlayedCardKind ?? physical.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, physical, action.ConversionSource)));
    }

    internal bool CanBeExtraTargetAdjustmentTarget(CharacterState actor, CharacterState target, LegalAction action, CardKind kind, Suit suit)
    {
        if (action.CardId != 0)
            return _coreNextCardTargetAdjustmentsOperations.CanBeExtraNextCardTarget(actor, target, action, kind, suit);
        if (!target.IsAlive || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, suit, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(suit)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, suit))
            return false;
        return action.Kind switch
        {
            LegalActionKind.Slash => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, target, kind, Suit.None, effectiveRank: 0, physicalCardIds: [], ignoreDistance: true),
            LegalActionKind.Peach => _coreTreatedWoundedOperations.IsRuleWounded(target),
            LegalActionKind.Alcohol or LegalActionKind.DrawTwo or LegalActionKind.IronChain => true,
            LegalActionKind.WumiaoFlood => true,
            LegalActionKind.Duel or LegalActionKind.QizhengXiangsheng => target.Seat != actor.Seat,
            LegalActionKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            LegalActionKind.UnexpectedAssault => target.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            LegalActionKind.Dismantlement or LegalActionKind.Snatch => target.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(target).Count > 0,
            _ => false
        };
    }

    internal static SkillProgramEffect? NextActualUseSelectedProducer(ProgramExecutionPlan plan) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_NextActualUseTargetAdjustments.NextActualUseSelectedProducer(plan);
    internal bool CanUseNextActualUseSelectedSlash(CharacterState owner, CharacterState target, ProgramMultiCardViewAsSelection selection) => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(owner, target, selection.OutputKind, _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(owner, selection), _coreGuanHaiBridgesOperations.ProgramMultiCardColor(owner, selection), physicalCardIds: selection.Cards.Select(c => c.Id).ToArray()) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner.Seat, target.Seat, selection.OutputKind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, selection.OutputKind, _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(owner, selection), _coreGuanHaiBridgesOperations.ProgramMultiCardColor(owner, selection)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, target.Seat, _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(owner, selection));
    internal ProgramNextActualUseAdjustment? FreezeNextActualUseProgramSelection(CharacterState owner, ProgramExecutionPlan plan, IReadOnlyList<int> targets)
    {
        if (targets.Count <= plan.Activation!.MaxTargets)
            return null;
        if (_coreHpLossSlashMaterialsOperations.HasHpLossSlashTargets(owner) && global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime.HpLossSlashProgramProducer(plan)is not null)
            return null;
        var grant = CurrentNextActualUseAdjustment(owner.Seat) ?? throw new InvalidOperationException("The selected new adjustment expired before its producer started.");
        var effect = NextActualUseSelectedProducer(plan) ?? throw new InvalidOperationException("The producer cannot adjust targets.");
        return effect.OutputKind switch
        {
            CardKind.ArrowBarrage => new(grant, ProgramNextActualUseAdjustmentKind.RemoveGlobalTarget, CardKind.ArrowBarrage, owner.Seat, targets.Single()),
            CardKind.Peach => new(grant, ProgramNextActualUseAdjustmentKind.AddRecoveryTarget, CardKind.Peach, owner.Seat, targets.Single()),
            _ => new(grant, ProgramNextActualUseAdjustmentKind.AddSlashTarget, effect.OutputKind!.Value, targets[0], targets[1])};
    }

    internal bool HasExactNextActualUseProgramSelection(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        if (frame.NextActualUseAdjustment is not { } receipt || NextActualUseSelectedProducer(plan)is not { } producer || producer.OutputKind != receipt.OutputKind || receipt.Grant.Source.OwnerSeat != frame.OwnerSeat || receipt.Grant.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || receipt.Grant.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NextActualUseTargetAdjustmentGrantedEvent>().Any(e => e == receipt.Grant))
            return false;
        return receipt.Kind switch
        {
            ProgramNextActualUseAdjustmentKind.AddSlashTarget => frame.SelectedTargetSeats.SequenceEqual(receipt.Stage == ProgramNextActualUseAdjustmentStage.Canceled ? [receipt.OriginalTargetSeat] : new[] { receipt.OriginalTargetSeat, receipt.ChangedTargetSeat }),
            ProgramNextActualUseAdjustmentKind.RemoveGlobalTarget or ProgramNextActualUseAdjustmentKind.AddRecoveryTarget => frame.SelectedTargetSeats.SequenceEqual(receipt.Stage == ProgramNextActualUseAdjustmentStage.Canceled ? [] : new[] { receipt.ChangedTargetSeat }),
            _ => false
        };
    }

    internal bool ApplyNextActualUseGlobalRemoval(ProgramSkillFrame frame, IReadOnlyList<int> normal, out IReadOnlyList<int> targets)
    {
        targets = normal;
        if (frame.NextActualUseAdjustment is not { Kind: ProgramNextActualUseAdjustmentKind.RemoveGlobalTarget } receipt)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (!HasExactNextActualUseProgramSelection(frame, plan))
            throw new InvalidOperationException("Global removal lost its exact owning selection.");
        if (CurrentNextActualUseAdjustment(frame.OwnerSeat)?.ProgramFrameId == receipt.Grant.ProgramFrameId && normal.Contains(receipt.ChangedTargetSeat))
            targets = Array.AsReadOnly(normal.Where(t => t != receipt.ChangedTargetSeat).ToArray());
        else
            _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { SelectedTargetSeats = [], NextActualUseAdjustment = receipt with { Stage = ProgramNextActualUseAdjustmentStage.Canceled } });
        return true;
    }

    internal SkillProgramStepOutcome BeginNextActualUseSelectedSlash(ProgramSkillFrame frame, ProgramMultiCardViewAsSelection selection)
    {
        var receipt = frame.NextActualUseAdjustment ?? throw new InvalidOperationException("Adjusted Slash requires its owning producer receipt.");
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (receipt.Kind != ProgramNextActualUseAdjustmentKind.AddSlashTarget || !HasExactNextActualUseProgramSelection(frame, plan))
            throw new InvalidOperationException("Adjusted material Slash lost its exact current producer.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (CurrentNextActualUseAdjustment(owner.Seat)?.ProgramFrameId != receipt.Grant.ProgramFrameId || !CanUseNextActualUseSelectedSlash(owner, _coreMatchParticipantsPlayersAccess._players[receipt.ChangedTargetSeat], selection))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = [receipt.OriginalTargetSeat], NextActualUseAdjustment = receipt with { Stage = ProgramNextActualUseAdjustmentStage.Canceled } });
            if (!CanUseNextActualUseSelectedSlash(owner, _coreMatchParticipantsPlayersAccess._players[receipt.OriginalTargetSeat], selection))
                return SkillProgramStepOutcome.Continue;
            _coreNativeSlashUseOperations.ResolveSlashCore(owner, _coreMatchParticipantsPlayersAccess._players[receipt.OriginalTargetSeat], selection.Cards[0], selection.OutputKind, owner.Seat, physicalCards: selection.Cards, conversionSource: selection.Source);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (!CanUseNextActualUseSelectedSlash(owner, _coreMatchParticipantsPlayersAccess._players[receipt.OriginalTargetSeat], selection))
            throw new InvalidOperationException("The original Slash target became illegal before payment.");
        var seats = Array.AsReadOnly(new[] { receipt.OriginalTargetSeat, receipt.ChangedTargetSeat });
        BeginNextActualUseMaterialSlash(owner, selection.Cards, selection.OutputKind, seats, selection.Source, receipt.Grant, frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void BeginNextActualUseMaterialSlash(CharacterState owner, IReadOnlyList<Card> cards, CardKind kind, IReadOnlyList<int> seats, CardConversionSource? source, NextActualUseTargetAdjustmentGrantedEvent grant, ProgramSkillFrame? frame)
    {
        var armor = _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(owner);
        var id = _coreNativeCardUseOperations.BeginCardUse(cards[0], owner.Seat, seats, kind, ignoresArmor: armor, physicalCardIds: cards.Select(c => c.Id).ToArray(), conversionSource: source, isTrueZhangbaSlash: frame is null);
        var typedReturn = new ProgramAdjustedSlashReturn(frame?.Id, grant.ProgramFrameId, id, frame?.GameplayHash, frame?.SkillInstanceId, frame is null, _coreTrustedMovementDiagnosticsOperations.CardMovements.Count == 0 ? 1 : _coreTrustedMovementDiagnosticsOperations.CardMovements[^1].Sequence + 1);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { AdjustedSlashReturn = typedReturn, TargetsAdjusted = true });
        if (frame?.NextActualUseAdjustment is { } receipt)
            _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { NextActualUseAdjustment = receipt with { Stage = ProgramNextActualUseAdjustmentStage.Applied, CardUseFrameId = id } });
        _coreNativeMaterialSlashOperations.CompleteNativeMaterialSlash(id, owner, cards, kind, seats, source, armor);
    }

    internal CommandError? ValidateNextActualUseZhangba(CharacterState owner, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (targets.Count <= 1)
            return null;
        if (_coreHpLossSlashMaterialsOperations.HasHpLossSlashTargets(owner) && _coreHpLossSlashMaterialsOperations.ValidateHpLossSlashZhangba(owner, ids, targets)is null)
            return null;
        if (_coreNativeMaterialSlashOperations.TryGetNativeMaterialSlashExpansion(owner, ids, targets, out _, out _))
            return null;
        var cards = _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(owner).FirstOrDefault(pair => pair.Select(c => c.Id).Order().SequenceEqual(ids.Order()));
        if (targets.Count != 2 || CurrentNextActualUseAdjustment(owner.Seat)is null || !_coreNativeTargetLegalityOperations.CanUseZhangbaSerpentSpear(owner) || cards is null || !_characterSunQuanOlMagicOperations.IsMoQuanyuCompleteZhangbaQuotaLegal(owner, targets, cards) || targets.Distinct().Count() != targets.Count || targets.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t) || !_coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(owner, _coreMatchParticipantsPlayersAccess._players[t], CardKind.Slash, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, cards), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, cards), _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(owner, cards), ids) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner.Seat, t, CardKind.Slash) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[t], CardKind.Slash, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, cards), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, cards)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, t, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, cards))))
            return new(CommandErrorCode.InvalidTarget, "The adjusted Zhangba Slash lost its exact two materials and legal targets.");
        return null;
    }

    internal void ResolveNextActualUseZhangba(CharacterState owner, IReadOnlyList<int> targets, IReadOnlyList<Card> cards)
    {
        if (ValidateNextActualUseZhangba(owner, cards.Select(c => c.Id).ToArray(), targets)is { } error)
            throw new InvalidOperationException(error.Message);
        if (_coreHpLossSlashMaterialsOperations.HasHpLossSlashTargets(owner) && _coreHpLossSlashMaterialsOperations.ValidateHpLossSlashZhangba(owner, cards.Select(c => c.Id).ToArray(), targets)is null)
        {
            _coreHpLossSlashMaterialsOperations.ResolveHpLossSlashZhangba(owner, targets, cards);
            return;
        }

        if (targets.Count == 2 && CurrentNextActualUseAdjustment(owner.Seat)is { } grant)
        {
            BeginNextActualUseMaterialSlash(owner, cards, CardKind.Slash, targets, null, grant, null);
            return;
        }

        if (_coreNativeMaterialSlashOperations.TryGetNativeMaterialSlashExpansion(owner, cards.Select(c => c.Id).ToArray(), targets, out _, out _))
        {
            _coreNativeMaterialSlashOperations.BeginNativeMaterialSlashExpansion(owner, cards, targets);
            return;
        }

        BeginNextActualUseMaterialSlash(owner, cards, CardKind.Slash, targets, null, CurrentNextActualUseAdjustment(owner.Seat) ?? throw new InvalidOperationException("Adjusted spear source expired before payment."), null);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 260)]
    internal void AssertNextActualUseProgramSelection(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        if (frame.NextActualUseAdjustment is not { } receipt)
            return;
        if (!HasExactNextActualUseProgramSelection(frame, plan) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.OriginalTargetSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.ChangedTargetSeat) || receipt.OriginalTargetSeat == receipt.ChangedTargetSeat || receipt.Stage == ProgramNextActualUseAdjustmentStage.Applied && (receipt.CardUseFrameId is null || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Any(e => e.ResolutionId == receipt.CardUseFrameId && e.SourceSeat == frame.OwnerSeat && e.CardKind == receipt.OutputKind)))
            throw new InvalidOperationException("Next actual-use selection lost its exact issuance, producer or actual child.");
    }

    internal void PrepareNextActualUseSelectedRecovery(ProgramSkillFrame frame, ProgramMultiCardViewAsSelection selection)
    {
        if (frame.NextActualUseAdjustment is not { Kind: ProgramNextActualUseAdjustmentKind.AddRecoveryTarget } receipt)
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (!HasExactNextActualUseProgramSelection(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!)))
            throw new InvalidOperationException("Recovery adjustment lost its exact material producer.");
        var target = _coreMatchParticipantsPlayersAccess._players[receipt.ChangedTargetSeat];
        if (CurrentNextActualUseAdjustment(owner.Seat)?.ProgramFrameId != receipt.Grant.ProgramFrameId || !target.IsAlive || !_coreTreatedWoundedOperations.IsRuleWounded(target) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, CardKind.Peach, _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(owner, selection), _coreGuanHaiBridgesOperations.ProgramMultiCardColor(owner, selection)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, target.Seat, _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(owner, selection)))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = [], NextActualUseAdjustment = receipt with { Stage = ProgramNextActualUseAdjustmentStage.Canceled } });
            return;
        }

        _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = Array.AsReadOnly(new[] { owner.Seat, target.Seat });
    }

    internal void MarkNextActualUseProgramChild(CardUseFrame use)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || parent.NextActualUseAdjustment is not { Stage: ProgramNextActualUseAdjustmentStage.Selected } receipt || parent.OwnerSeat != use.SourceSeat || receipt.OutputKind != use.CardKind)
            return;
        var plan = ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!);
        if (!HasExactNextActualUseProgramSelection(parent, plan) || plan.GetPausedInstruction(parent.InstructionIndex).Effect.Op != SkillProgramEffectOp.UseSelectedCardsAs || use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != parent.OwnerSeat || action.ProviderSeat != parent.OwnerSeat || use.PhysicalCardIds is null || !use.PhysicalCardIds.Order().SequenceEqual(parent.SelectedCardIds.Order()) || !action.PhysicalCards.Select(c => c.CardId).SequenceEqual(use.PhysicalCardIds) || action.ConversionChain is not [var conversion] || conversion.SkillId != parent.SkillId || conversion.SkillInstanceId != parent.SkillInstanceId || conversion.OwnerSeat != parent.OwnerSeat || conversion.BindingId != plan.GetPausedInstruction(parent.InstructionIndex).Effect.SourceBind)
            throw new InvalidOperationException("The material-use child lost its exact adjusted producer and frozen actual costs.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { NextActualUseAdjustment = receipt with { Stage = ProgramNextActualUseAdjustmentStage.Applied, CardUseFrameId = use.Id } });
    }

    internal IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> NextActualUseOrdinaryTrickOptions(CharacterState actor, IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> ordinary, Suit? suit, bool? color, Suit? shieldSuit, bool excludeOwner)
    {
        ordinary = _characterZhangFeiAnniversaryGodOperations.ShenZfOrdinaryTrickOptions(actor, ordinary, suit, color, shieldSuit, excludeOwner);
        if (CurrentNextActualUseAdjustment(actor.Seat)is null)
            return ordinary;
        var options = ordinary.ToList();
        var keys = ordinary.Select(o => (o.EffectiveCardKind, o.TargetCardId, Targets: string.Join(",", o.TargetSeats))).ToHashSet();
        foreach (var option in ordinary)
        {
            var kind = option.EffectiveCardKind;
            var normal = option.TargetSeats.Count == 0 && kind == CardKind.DrawTwo ? new[]
            {
                actor.Seat
            }

            : option.TargetSeats.ToArray();
            if (normal.Length == 0)
                continue;
            if (kind == CardKind.BorrowedSword)
            {
                foreach (var holder in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != actor.Seat && !normal.Where((_, i) => i % 2 == 0).Contains(p.Seat) && _coreNativeBorrowedSwordOperations.GetWeapon(p)is not null && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, p.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, shieldSuit)))
                    foreach (var victim in _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(holder, p)))
                        Add([..normal, holder.Seat, victim.Seat]);
                if (normal.Length >= 4)
                    for (var i = 0; i < normal.Length; i += 2)
                        Add(normal.Where((_, j) => j != i && j != i + 1).ToArray());
                continue;
            }

            if (kind is not (CardKind.BarbarianAssault or CardKind.ArrowBarrage or CardKind.PeachGarden or CardKind.FiveGrains))
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (!excludeOwner || p.Seat != actor.Seat) && !normal.Contains(p.Seat) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, p.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, shieldSuit) && (kind switch
                {
                    CardKind.DrawTwo or CardKind.IronChain => true,
                    CardKind.Duel => p.Seat != actor.Seat,
                    CardKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0,
                    CardKind.Snatch or CardKind.Dismantlement => p.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(p).Count > 0,
                    _ => false
                })))
                    Add([..normal, target.Seat]);
            if (normal.Length > 1)
                foreach (var target in normal)
                    Add(normal.Where(t => t != target).ToArray());
            void Add(IReadOnlyList<int> targets)
            {
                if (targets.Count == 0 || !keys.Add((kind, option.TargetCardId, string.Join(",", targets))))
                    return;
                options.Add(option with { Id = new(option.Id.Value + ".next-actual.targets-" + string.Join('-', targets)), TargetSeats = Array.AsReadOnly(targets.ToArray()), NextActualUseAdjusted = true, Description = option.Description + "（巧说目标调整）" });
            }
        }

        return Array.AsReadOnly(options.ToArray());
    }

    internal bool IsNextActualUseTrickForbidden(int seat, CardKind kind) => _turnCardUseEffectsOperations._turnCardUseEffects.ActionProhibitions.Any(p => p.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && p.Source.OwnerSeat == seat && p.ActionTypes.Contains(CardActionType.Use) && ProgramCardKindMatch.Contains(p.CardKinds, kind) && _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program?.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.GrantNextActualUseTargetAdjustment)) == true);
    internal long? NextActualUseAdjustedSlashParent(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).AdjustedSlashReturn is not { } typed)
            return null;
        if (typed.IsZhangba && typed.ParentProgramFrameId is null && typed.ParentGameplayHash is null && typed.ParentSkillInstanceId is null)
            return null;
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == typed.ParentProgramFrameId);
        if (typed.CardUseFrameId != attack.ResolutionId || parent is null || parent.GameplayHash != typed.ParentGameplayHash || parent.SkillInstanceId != typed.ParentSkillInstanceId || parent.OwnerSeat != attack.SourceSeat || parent.NextActualUseAdjustment is not { Stage: ProgramNextActualUseAdjustmentStage.Applied } receipt || receipt.CardUseFrameId != typed.CardUseFrameId || receipt.Grant.ProgramFrameId != typed.GrantProgramFrameId || !HasExactNextActualUseProgramSelection(parent, ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!)))
            throw new InvalidOperationException("Adjusted Slash completion lost its exact typed original producer.");
        return parent.Id;
    }

    internal bool HasExactNextActualUseAdjustedProgramReturn(CardUseFrame use)
    {
        if (use.AdjustedSlashReturn is not { IsZhangba: false, ParentProgramFrameId: { } parentId } typed)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == parentId);
        if (parent is null || typed.CardUseFrameId != use.Id || parent.OwnerSeat != use.SourceSeat || parent.GameplayHash != typed.ParentGameplayHash || parent.SkillInstanceId != typed.ParentSkillInstanceId || use.CardAttack?.AdjustedSlashReturn != typed || use.CardKind is not (CardKind.Slash or CardKind.FireSlash) || parent.NextActualUseAdjustment is not { Stage: ProgramNextActualUseAdjustmentStage.Applied } receipt || receipt.CardUseFrameId != use.Id || receipt.Grant.ProgramFrameId != typed.GrantProgramFrameId || !HasExactNextActualUseProgramSelection(parent, ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!)))
            throw new InvalidOperationException("The completed adjusted Slash cannot bypass the older return without its exact already-captured parent.");
        return true;
    }

    internal bool IsExactNextActualUseMaterialSlash(global::CardGame.Core.NativeOwners.FangtianHalberdHandle multi, global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { AdjustedSlashReturn: { } typed, Action: { Type: CardActionType.Use } action } use || typed.CardUseFrameId != use.Id || _coreCardAttackStateOperations.GetCardAttackState(use.Id).AdjustedSlashReturn != typed || multi.ResolutionId != use.Id || multi.SourceSeat != use.SourceSeat || action.ActorSeat != use.SourceSeat || action.ProviderSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || attack.EffectiveCardKind != use.CardKind || multi.ConversionSource != action.ConversionChain.SingleOrDefault() || use.PhysicalCardIds is not { Count: >= 2 } cards || !cards.SequenceEqual(attack.PhysicalCards.Select(c => c.Id)) || !cards.SequenceEqual(action.PhysicalCards.Select(c => c.CardId)) || cards[0] != multi.Card.Id || typed.PaidMovementFirstSequence <= 0 || typed.PaidMovementLastSequence < typed.PaidMovementFirstSequence || action.PhysicalCards.Any(cost => _coreTrustedMovementDiagnosticsOperations.CardMovements.Count(m => m.Sequence >= typed.PaidMovementFirstSequence && m.Sequence <= typed.PaidMovementLastSequence && m.CardId == cost.CardId && m.From == cost.From && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use && m.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber) != 1) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NextActualUseTargetAdjustmentConsumedEvent>().Any(e => e.GrantProgramFrameId == typed.GrantProgramFrameId && e.OriginFrameId == use.Id && e.CardActionId == action.ActionId && e.Source.OwnerSeat == action.ActorSeat && e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && !e.IsNullificationUse))
            return false;
        if (typed.IsZhangba)
        {
            if (typed.ParentProgramFrameId is not null || typed.ParentGameplayHash is not null || typed.ParentSkillInstanceId is not null || cards.Count != 2 || action.ConversionChain.Count != 0 || action.EffectiveKind != CardKind.Slash || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetsConfirmedEvent>().Any(e => e.ResolutionId == use.Id && e.TargetSeats.SequenceEqual(multi.TargetSeats)) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhangbaSerpentSpearConvertedEvent>().Any(e => e.ResolutionId == use.Id && e.UserSeat == action.ActorSeat && e.IsUse && e.PhysicalCardIds.SequenceEqual(cards)))
                return false;
        }
        else if (NextActualUseAdjustedSlashParent(attack)is not { } parentId || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == parentId)is not { } parent || !cards.Order().SequenceEqual(parent.SelectedCardIds.Order()) || !multi.TargetSeats.SequenceEqual(new[] { parent.NextActualUseAdjustment!.OriginalTargetSeat, parent.NextActualUseAdjustment.ChangedTargetSeat }))
            return false;
        var retained = cards.Where(id => !_coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(use.Id, id)).ToHashSet();
        return processing.All(c => retained.Contains(c.Id)) && retained.All(id => _cardZonesOperations._cardZones.GetLocation(id).Zone is CardZoneKind.Processing or CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile);
    }
}
