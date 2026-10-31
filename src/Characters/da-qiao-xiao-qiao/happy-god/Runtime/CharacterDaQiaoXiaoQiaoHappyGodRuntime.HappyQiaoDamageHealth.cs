#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoXiaoQiaoHappyGodRuntime
{
    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 42400)]
    internal bool TryBeginHappyQiaoDamageHealth(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId)
    {
        if ((!HasHappyQiaoHealth(attack.TargetSeat) && !HappyQiaoHealthSubject(attack.TargetSeat)) || !attack.DamageWasApplied || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.Id != damageFrameId || damage.ParentFrameId != attack.ResolutionId || damage.HappyQiaoDamageHealth is not null)
            return false;
        var change = _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.FirstOrDefault(c => c.ParentFrameId == damage.Id && c.TargetSeat == damage.TargetSeat && c.Kind == HpChangeKind.Damage && c.Amount > 0 && c.HpBefore > c.HpAfter);
        if (change is null)
            return false;
        var use = HappyQiaoCausingUse(change);
        var red = use?.Action is { } action && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoRedHealthQualifiedEvent>().Any(e => e.ChangeId == change.Id && e.UseId == use.Id && e.ActionId == action.ActionId && e.Owner == action.ActorSeat && HappyQiaoRedQualification(e.Owner, e.Instance, change));
        var shutu = HappyQiaoShutuChange(damage.TargetSeat, change) ? _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[damage.TargetSeat], SkillProgramTriggerWindow.AfterHealthChanged).Where(c => c.SkillId == HappyQiaoComposition.Shutu && c.BindingId == "pool-health").Select(c => new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId)).FirstOrDefault() : null;
        if (!red && shutu is null)
            return false;
        var receipt = new HappyQiaoDamageHealthReceipt(damage.Id, attack.ResolutionId, change.Id, damage.SourceSeat, damage.TargetSeat, damage.Amount, damage.Nature, attack.IsSourceLess, change.HpBefore, change.HpAfter, use?.Id, use?.Action?.ActionId, change.HappyQiaoPool, shutu, red);
        _coreRuntimeOperations.ReplaceRuntimeTop(damage with { HappyQiaoDamageHealth = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoDamageHealthStartedEvent(receipt));
        if (!_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(damage.Id, PostEventContinuation.HappyQiaoDamageHealth))
            throw new InvalidOperationException("Happy Qiao lost the original paid damage's qualified Shutu or Tongxin health-change child.");
        return true;
    }

    internal bool IsHappyQiaoDamageHealthReturn(DamageFrame damage, HpChangedTriggerWindowFrame health) => damage.HappyQiaoDamageHealth is { } r && r.DamageFrameId == damage.Id && r.AttackFrameId == damage.ParentFrameId && r.SourceSeat == damage.SourceSeat && r.TargetSeat == damage.TargetSeat && r.Amount == damage.Amount && r.Nature == damage.Nature && health.Continuation == PostEventContinuation.HappyQiaoDamageHealth && health.ResumeFrameId == damage.Id && health.Change.ParentFrameId == damage.Id && health.Id == health.Change.Id && health.Id == r.ChangeId && health.Change.TargetSeat == r.TargetSeat && health.Change.Kind == HpChangeKind.Damage && health.Change.Amount == (damage.ArmorPayment?.ActualHpLost ?? r.Amount) && health.Change.HpBefore == r.HpBefore && health.Change.HpAfter == r.HpAfter && r.HpBefore > r.HpAfter && health.Change.HappyQiaoPool == r.ChangedPool && (damage.ArmorPayment is null || damage.ArmorPayment.HpBefore == r.HpBefore && damage.ArmorPayment.HpAfter == r.HpAfter && _coreDamageArmorPaymentsOperations.IsIssuedDamageArmorPayment(damage)) && _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(r.AttackFrameId)is { DamageWasApplied: true } original && original.SourceSeat == r.SourceSeat && original.TargetSeat == r.TargetSeat && original.IsSourceLess == r.SourceLess && original.DamageAmount == r.Amount && _coreNativeCardClassificationsOperations.GetDamageNature(original) == r.Nature && (r.UseId is null ? r.ActionId is null : _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId.Value)is { Action: { } action } use && use.Id == r.AttackFrameId && action.ActionId == r.ActionId) && (r.ShutuSource is not null || r.RedUseQualified) && (r.ShutuSource is null || r.ShutuSource is { SkillId: HappyQiaoComposition.Shutu, BindingId: "pool-health" } shutu && shutu.OwnerSeat == r.TargetSeat && HappyQiaoShutuChange(shutu.OwnerSeat, health.Change) && health.Candidates.Any(c => c.OwnerSeat == shutu.OwnerSeat && c.SkillId == shutu.SkillId && c.BindingId == shutu.BindingId && c.SkillInstanceId == shutu.SkillInstanceId)) && (!r.RedUseQualified || HappyQiaoDamageRedUseQualification(r, health.Change)) && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoDamageHealthStartedEvent>().Count(e => e.Receipt == r) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoDamageHealthReturnedEvent>().Any(e => e.Receipt == r);
    internal bool HappyQiaoDamageRedUseQualification(HappyQiaoDamageHealthReceipt r, HpChangeContext change) => r.UseId is { } useId && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { } action } && action.ActionId == r.ActionId && (action.Type == CardActionType.Use || action.Type == CardActionType.Response && action.EffectiveKind is CardKind.Dodge or CardKind.Nullification) && (action.EffectiveIsRed ?? (action.EffectiveSuit is Suit.Heart or Suit.Diamond)) && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoRedHealthQualifiedEvent>().Any(e => e.ChangeId == r.ChangeId && e.UseId == useId && e.ActionId == r.ActionId && e.Owner == action.ActorSeat && e.Target == r.TargetSeat && e.Kind == HpChangeKind.Damage && HappyQiaoRedQualification(e.Owner, e.Instance, change));
    internal bool HappyQiaoDamageHealthStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => parent is DamageFrame damage && child is HpChangedTriggerWindowFrame health && IsHappyQiaoDamageHealthReturn(damage, health) || parent is ProgramSkillFrame { HappyQiao: not null } program && HappyQiaoFirstSkillChild(program, child) || parent is CardUseFrame { HappyQiaoShutuUse: { } native } use && child is DamageFrame applied && applied.ParentFrameId == use.Id && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == native.FrameId)is { } producer && HappyQiaoNativeUseRidesOn(use, producer) && _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(use.Id)is { } attack && applied.SourceSeat == attack.SourceSeat && applied.TargetSeat == attack.TargetSeat && applied.Amount == attack.DamageAmount && applied.Nature == _coreNativeCardClassificationsOperations.GetDamageNature(attack) && _coreProgramEventHistoryOperations.ProgramEventHistory<DamageRequestedEvent>().Count(e => e.ResolutionId == applied.Id && e.SourceSeat == applied.SourceSeat && e.TargetSeat == applied.TargetSeat && e.Amount == applied.Amount && e.Nature == applied.Nature) == 1 || HappyQiaoHealthChoiceRidesOn(child, parent);
    internal bool HasHappyQiaoDamageHealthContinuation(long attackId)
    {
        for (var i = 0; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
            if (_resolutionStackOperations._resolutionStack[i] is DamageFrame damage && damage.ParentFrameId == attackId && _resolutionStackOperations._resolutionStack[i + 1] is HpChangedTriggerWindowFrame health && IsHappyQiaoDamageHealthReturn(damage, health))
                return Enumerable.Range(i + 2, _resolutionStackOperations._resolutionStack.Count - i - 2).All(index => HappyQiaoDamageHealthStructuralEdge(_resolutionStackOperations._resolutionStack[index - 1], _resolutionStackOperations._resolutionStack[index]) || _coreObserverSubtreesOperations.IsMovementObserverEdge(index));
        return false;
    }

    internal void AssertHappyQiaoDamageHealth()
    {
        foreach (var damage in _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Where(d => d.HappyQiaoDamageHealth is not null))
            if (!HasHappyQiaoDamageHealthContinuation(damage.ParentFrameId))
                throw new InvalidOperationException("Happy Qiao lost the exact original paid damage and its continuously typed health observer subtree.");
    }

    internal IReadOnlyList<Card> SeparateHappyQiaoDamageHealthChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (processing.Count == 0 || !HasHappyQiaoDamageHealthContinuation(attack.ResolutionId))
            return processing;
        var root = _resolutionStackOperations._resolutionStack.FindIndex(f => f is DamageFrame { HappyQiaoDamageHealth: not null } d && d.ParentFrameId == attack.ResolutionId);
        var childIds = new HashSet<int>();
        for (var i = root + 2; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (_resolutionStackOperations._resolutionStack[i] is not ProgramSkillFrame { HappyQiao: { Stage: HappyQiaoStage.NativeUse or HappyQiaoStage.ReturnedUse } receipt } frame || _resolutionStackOperations._resolutionStack[i + 1] is not CardUseFrame use || !HappyQiaoNativeUseRidesOn(use, frame) || use.Action is not { } action)
                continue;
            foreach (var cost in action.PhysicalCards)
            {
                if (_cardZonesOperations._cardZones.GetLocation(cost.CardId) != CardLocation.Processing)
                    continue;
                var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId);
                if (movement is null || movement.Sequence <= receipt.Before || movement.From != cost.From || movement.To != CardLocation.Processing || movement.CardKind != cost.CardKind || movement.Reason != CardMoveReasons.Use && movement.Reason != CardMoveReasons.EquipmentUse || attack.PhysicalCards.Any(c => c.Id == cost.CardId))
                    return processing;
                childIds.Add(cost.CardId);
            }
        }

        return childIds.Count == 0 ? processing : processing.Where(card => !childIds.Contains(card.Id)).ToArray();
    }

    internal void ReturnHappyQiaoDamageHealth(HpChangedTriggerWindowFrame health)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || !IsHappyQiaoDamageHealthReturn(damage, health) || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.ResolutionId != damage.ParentFrameId || !attack.DamageWasApplied || attack.SourceSeat != damage.SourceSeat || attack.TargetSeat != damage.TargetSeat || attack.IsSourceLess != damage.HappyQiaoDamageHealth!.SourceLess)
            throw new InvalidOperationException("Happy Qiao's paid health observers must return to their exact original damage.");
        var receipt = damage.HappyQiaoDamageHealth!;
        _coreRuntimeOperations.ReplaceRuntimeTop(damage with { HappyQiaoDamageHealth = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoDamageHealthReturnedEvent(receipt));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreNativeDamageWindowsOperations.TryBeginDamageTriggerWindow(attack, damage.Id, SkillProgramTriggerWindow.DamageAppliedBeforeDying))
            return;
        var target = _coreMatchParticipantsPlayersAccess._players[damage.TargetSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && target.IsAlive && target.Hp <= 0)
        {
            _coreNativeDyingOperations.BeginDying(attack, damage.Id, target, _coreMatchParticipantsPlayersAccess._players[damage.SourceSeat]);
            return;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreNativeDamageWindowsOperations.TryBeginDamageTriggerWindow(attack, damage.Id, SkillProgramTriggerWindow.AfterDamageApplied))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AfterDamageEvent(damage.Id, damage.SourceSeat, damage.TargetSeat, damage.Amount, target.Hp, damage.Nature) { SourceLess = receipt.SourceLess });
        _coreRuntimeOperations.PopResolutionFrame(damage.Id, ResolutionFrameKind.Damage);
        _coreNativeDamageCompletionOperations.CompleteDamageAttack(attack);
    }
}
