#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageArmorPaymentsRuntime
{
    internal DamageArmorPaymentReceipt? PrepareDamageArmorPayment(long frameId, global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (target.Armor == 0 || _coreNativeDamageArmorPoliciesOperations.BypassesNativeDamageArmor(attack))
            return null;
        if (target.Armor < 0 || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.Id != frameId || damage.ParentFrameId != attack.ResolutionId || damage.SourceSeat != attack.SourceSeat || damage.TargetSeat != attack.TargetSeat || damage.Amount != attack.DamageAmount || damage.Amount < 0 || !attack.DamageAmountFinalized || damage.ArmorPayment is not null || !IsExactArmorDamageRequest(damage, attack, requireUnpaid: true))
            throw new InvalidOperationException("Armor payment requires the exact unpaid native damage segment.");
        var multiplier = _coreNativeDamageArmorPoliciesOperations.NativeDamageArmorMultiplier(attack);
        var absorbed = Math.Min(target.Armor, checked(damage.Amount * multiplier));
        var hpLoss = damage.Amount - (absorbed + multiplier - 1) / multiplier;
        var unclamped = checked(target.Hp - hpLoss);
        var receipt = new DamageArmorPaymentReceipt(damage.Id, damage.ParentFrameId, damage.SourceSeat, damage.TargetSeat, damage.Amount, damage.Nature, attack.IsSourceLess, target.Armor, absorbed, target.Armor - absorbed, target.Hp, hpLoss, unclamped, unclamped)
        {
            ArmorDamageMultiplier = multiplier
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(damage with { ArmorPayment = receipt });
        return receipt;
    }

    internal global::CardGame.Core.GameEngine.IDamageAttempt? DamageArmorPaymentOwner(DamageFrame damage) => _resolutionStackOperations._resolutionStack.FirstOrDefault(frame => frame.Id == damage.ParentFrameId) switch
    {
        ProgramSkillFrame { AttackAttempt: not null } program => _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(program.Id),
        CardUseFrame { CardAttack: not null } use => _kernelCoreMainOperations.CreateCardAttackHandle(use.Id),
        ProgramSkillFrame { CardAttack: not null } program => _kernelCoreMainOperations.CreateCardAttackHandle(program.Id),
        JudgmentFrame { CardAttack: not null } judgment => _kernelCoreMainOperations.CreateCardAttackHandle(judgment.Id),
        _ => null
    };
    internal bool IsExactArmorDamageRequest(DamageFrame damage, global::CardGame.Core.GameEngine.IDamageAttempt attack, bool requireUnpaid = false)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<DamageRequestedEvent>().Where(request => request.ResolutionId == damage.Id).ToArray()is not [var request] || request.SourceSeat != damage.SourceSeat || request.TargetSeat != damage.TargetSeat || request.Amount != damage.Amount || request.SourceCard != attack.EffectiveCardKind || request.Nature != damage.Nature || request.SourceLess != attack.IsSourceLess)
            return false;
        // DamageWasApplied belongs to the whole attack/use and remains true
        // after the first chained recipient. The new segment's own request,
        // empty payment slot and unpaid event suffix are the payment boundary.
        return !requireUnpaid || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().SkipWhile(fact => fact is not DamageRequestedEvent issued || issued.ResolutionId != damage.Id).Skip(1).Any(fact => fact is DamageAppliedEvent);
    }

    internal bool IsValidDamageArmorPayment(DamageFrame damage, bool requireApplied)
    {
        if (damage.ArmorPayment is not { } payment || DamageArmorPaymentOwner(damage)is not { } attack || payment.DamageFrameId != damage.Id || payment.AttackFrameId != damage.ParentFrameId || payment.SourceSeat != damage.SourceSeat || payment.TargetSeat != damage.TargetSeat || payment.DamageAmount != damage.Amount || payment.DamageAmount < 0 || payment.Nature != damage.Nature || payment.ArmorDamageMultiplier is < 1 or > 2 || payment.ArmorBefore <= 0 || payment.ArmorAbsorbed != Math.Min(payment.ArmorBefore, checked(payment.DamageAmount * payment.ArmorDamageMultiplier)) || payment.ArmorAfter != payment.ArmorBefore - payment.ArmorAbsorbed || payment.HpLossAmount != payment.DamageAmount - (payment.ArmorAbsorbed + payment.ArmorDamageMultiplier - 1) / payment.ArmorDamageMultiplier || (long)payment.UnclampedHpAfter != (long)payment.HpBefore - payment.HpLossAmount || payment.HpAfter != payment.UnclampedHpAfter || attack.SourceSeat != payment.SourceSeat || attack.TargetSeat != payment.TargetSeat || attack.DamageAmount != payment.DamageAmount || !attack.DamageAmountFinalized || attack.IsSourceLess != payment.SourceLess || requireApplied && !attack.DamageWasApplied)
            return false;
        return IsExactArmorDamageRequest(damage, attack);
    }

    internal static bool DamageAppliedMatchesArmorPayment(DamageAppliedEvent applied, DamageArmorPaymentReceipt payment) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamageArmorPayments.DamageAppliedMatchesArmorPayment(applied, payment);
    internal bool IsIssuedDamageArmorPayment(DamageFrame damage) => IsValidDamageArmorPayment(damage, requireApplied: true) && _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Where(applied => applied.ArmorPayment == damage.ArmorPayment).ToArray()is [var applied] && DamageAppliedMatchesArmorPayment(applied, damage.ArmorPayment!);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 101)]
    internal void RequireExactDamageArmorPayment(IGameEvent payload)
    {
        if (payload is not DamageAppliedEvent applied)
            return;
        if (applied.ArmorPayment is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame { ArmorPayment: not null })
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.ArmorPayment is not { } payment || !IsValidDamageArmorPayment(damage, requireApplied: true) || !DamageAppliedMatchesArmorPayment(applied, payment) || _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Any(previous => previous.ArmorPayment == payment))
            throw new InvalidOperationException("Damage lost its exact once-only Armor and HP payment.");
    }

    internal void AssertDamageArmorPayments()
    {
        foreach (var damage in _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Where(frame => frame.ArmorPayment is not null))
            if (!IsIssuedDamageArmorPayment(damage))
                throw new InvalidOperationException("An Armor-paid damage segment lost its issued native payment receipt.");
    }
}
