#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoXiaoQiaoHappyGodRuntime
{
    internal bool TryBeginHappyQiaoCardMaximumHealth(long attackId)
    {
        if (_coreCardAttackStateOperations.GetCardAttackState(attackId).MaximumHpReplacement is not { HappyQiaoMaximumProof: { } proof } replacement)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != attackId || use.HappyQiaoCardMaximumHealth is not null || !MatchesHappyQiaoCardMaximumHp(replacement) || _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(attackId)is not global::CardGame.Core.NativeOwners.CardAttackHandle original || original.SourceSeat != replacement.SourceSeat || original.TargetSeat != replacement.TargetSeat || original.DamageWasApplied || original.DamageAmountFinalized)
            throw new InvalidOperationException("Qiao card maximum observers require the exact unpaid native Use completion.");
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoCardMaximumHealthReturnedEvent>().Any(e => e.Receipt.UseId == attackId))
            return false;
        var changes = _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Where(c => c.ParentFrameId == attackId && c.TargetSeat == replacement.TargetSeat && c.Kind == HpChangeKind.MaximumHp).ToArray();
        if (proof.Before.MaximumHp == proof.After.MaximumHp)
        {
            if (changes.Length != 0)
                throw new InvalidOperationException("An unchanged Qiao maximum cannot invent pool changes.");
            return false;
        }

        var da = changes.SingleOrDefault(c => c.HappyQiaoPool == HappyQiaoPool.DaQiao);
        var xiao = changes.SingleOrDefault(c => c.HappyQiaoPool == HappyQiaoPool.XiaoQiao);
        if (changes.Length != 2 || da is null || xiao is null || !HappyQiaoMaximumChangeMatches(attackId, replacement, da) || !HappyQiaoMaximumChangeMatches(attackId, replacement, xiao) || da.Id >= xiao.Id)
            throw new InvalidOperationException("Card maximum replacement lost one of its original ordered pool invoices.");
        var receipt = new HappyQiaoCardMaximumHealthReceipt(use.Id, use.Action?.ActionId, replacement, da.Id, xiao.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { HappyQiaoCardMaximumHealth = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoCardMaximumHealthStartedEvent(receipt));
        if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(use.Id, PostEventContinuation.HappyQiaoCardMaximumHealth))
            return true;
        // No registered observer qualified; both real invoices were drained by
        // the native window collector, so ordinary card completion can continue.
        _coreRuntimeOperations.ReplaceRuntimeTop(((CardUseFrame)_resolutionStackOperations._resolutionStack[^1])with { HappyQiaoCardMaximumHealth = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoCardMaximumHealthReturnedEvent(receipt, null));
        return false;
    }

    internal bool HappyQiaoMaximumChangeMatches(long useId, CardDamageMaximumHpReplacementReceipt replacement, HpChangeContext change) => replacement.HappyQiaoMaximumProof is { } proof && change is { HappyQiaoPool: { } pool, Kind: HpChangeKind.MaximumHp } && change.ParentFrameId == useId && change.TargetSeat == replacement.TargetSeat && change.SourceSeat is null && change.Amount == Math.Abs(proof.After.MaximumHp - proof.Before.MaximumHp) && change.HpBefore == proof.Before.Hp(pool) && change.HpAfter == proof.After.Hp(pool);
    internal bool IsHappyQiaoCardMaximumHealthReturn(ResolutionFrame parent, HpChangedTriggerWindowFrame health) => parent is CardUseFrame { HappyQiaoCardMaximumHealth: { } receipt } use && receipt.UseId == use.Id && use.Action?.ActionId == receipt.ActionId && use.CardAttack?.MaximumHpReplacement == receipt.Replacement && MatchesHappyQiaoCardMaximumHp(receipt.Replacement) && receipt.DaQiaoChangeId > use.Id && receipt.XiaoQiaoChangeId > receipt.DaQiaoChangeId && health.Continuation == PostEventContinuation.HappyQiaoCardMaximumHealth && health.ResumeFrameId == use.Id && health.CardId is null && health.CardKind is null && health.Id == health.Change.Id && health.Id == (health.Change.HappyQiaoPool == HappyQiaoPool.DaQiao ? receipt.DaQiaoChangeId : receipt.XiaoQiaoChangeId) && HappyQiaoMaximumChangeMatches(use.Id, receipt.Replacement, health.Change) && _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(use.Id)is global::CardGame.Core.NativeOwners.CardAttackHandle original && original.SourceSeat == receipt.Replacement.SourceSeat && original.TargetSeat == receipt.Replacement.TargetSeat && !original.DamageWasApplied && !original.DamageAmountFinalized && _coreProgramEventHistoryOperations.ProgramEventHistory<CardDamageReplacedWithMaximumHpLossEvent>().Count(e => e.ParentResolutionId == use.Id && e.Receipt == receipt.Replacement) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoCardMaximumHealthStartedEvent>().Count(e => e.Receipt == receipt) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoCardMaximumHealthReturnedEvent>().Any(e => e.Receipt == receipt);
    internal bool HappyQiaoCardMaximumHealthStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => child is HpChangedTriggerWindowFrame hp && IsHappyQiaoCardMaximumHealthReturn(parent, hp);

    internal void ReturnHappyQiaoCardMaximumHealth(HpChangedTriggerWindowFrame health)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || !IsHappyQiaoCardMaximumHealthReturn(use, health) || _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Any(c => c.ParentFrameId == use.Id && c.TargetSeat == use.HappyQiaoCardMaximumHealth!.Replacement.TargetSeat && c.Kind == HpChangeKind.MaximumHp) || _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(use.Id)is not global::CardGame.Core.NativeOwners.CardAttackHandle original)
            throw new InvalidOperationException("The last Qiao maximum observer must return to its original replaced native attack.");
        var receipt = use.HappyQiaoCardMaximumHealth!;
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { HappyQiaoCardMaximumHealth = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoCardMaximumHealthReturnedEvent(receipt, health.Change.Id));
        _coreNativeDamageCompletionOperations.CompleteDamageAttack(original);
    }
}
