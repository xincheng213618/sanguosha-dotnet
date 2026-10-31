#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardRecoveryModifiersRuntime
{
    internal void GrantProgramCardRecoveryModifier(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, int amount, SkillProgramDamageModifierExpiration expiration = SkillProgramDamageModifierExpiration.CurrentTurnEnd)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRecoveryModifier(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), cardKinds, amount, expiration);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecoveryModifierGrantedEvent(grant));
    }

    // Every real Use declares once, including producers with no physical card or Action context.
    // The declarer's seat is the user of a supplied/converted card, not its provider or skill owner.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 190)]
    internal void ObserveIssuedCardRecoveryModifiers(IGameEvent payload)
    {
        if (payload is not CardUseDeclaredEvent declared || declared.CardKind is not (CardKind.Peach or CardKind.Alcohol))
            return;
        var modifiers = _turnCardUseEffectsOperations._turnCardUseEffects.GetRecoveryModifiers(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, declared.SourceSeat, declared.CardKind);
        if (modifiers.Count == 0)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId);
        if (use is null || use.SourceSeat != declared.SourceSeat || use.CardKind != declared.CardKind)
            throw new InvalidOperationException("A recovery use declaration lost its owning frame.");
        if (use.IssuedRecoveryModifiers is not null)
            throw new InvalidOperationException("A recovery use was issued more than once.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, frame => frame with { IssuedRecoveryModifiers = modifiers });
    }

    internal int ApplyIssuedCardRecoveryModifiers(long useId, int actorSeat, int targetSeat, int baseAmount)
    {
        baseAmount = _characterDengAiAnniversaryGodOperations.ApplyShenDengAiFengtianRecovery(useId, actorSeat, targetSeat, baseAmount);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId);
        if (use?.IssuedRecoveryModifiers is not { Count: > 0 } modifiers)
            return baseAmount;
        if (use.SourceSeat != actorSeat || modifiers.Any(item => item.Source.OwnerSeat != actorSeat))
            throw new InvalidOperationException("A recovery modifier must belong to the actual card user.");
        var previous = use.RecoveryModifierApplications?.SingleOrDefault(item => item.TargetSeat == targetSeat);
        if (previous is not null)
        {
            if (previous.BaseAmount != baseAmount)
                throw new InvalidOperationException("A resumed card recovery changed its base amount.");
            return previous.ModifiedAmount;
        }

        var amount = checked(baseAmount + modifiers.Sum(item => item.Amount));
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, frame => frame with { RecoveryModifierApplications = (frame.RecoveryModifierApplications ?? []).Append(new(targetSeat, baseAmount, amount)).ToArray() });
        var current = baseAmount;
        foreach (var modifier in modifiers)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardRecoveryModifiedEvent(useId, modifier.Source, actorSeat, targetSeat, use.CardKind, current, current + modifier.Amount));
            current += modifier.Amount;
        }

        return amount;
    }
}
