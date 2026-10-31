#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineUnnullifiableOrdinaryTricksRuntime
{
    internal bool HasOrdinaryTrickCannotNullifyCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.CannotNullifyOwnOrdinaryTrick);

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 370)]
    internal void ObserveUnnullifiableOrdinaryTrick(IGameEvent payload)
    {
        if (!HasOrdinaryTrickCannotNullifyCapability || payload is not CardUseDeclaredEvent declared || _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is not { Action: { Type: CardActionType.Use } action } use || action.EffectiveKind == CardKind.Nullification || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(action.EffectiveKind))
            return;
        var binding = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], SkillProgramCardPolicyKind.CannotNullifyOwnOrdinaryTrick, action.EffectiveKind).FirstOrDefault();
        if (binding.Source is null)
            return;
        if (use.OrdinaryTrickCannotNullify is not null || use.SourceSeat != action.ActorSeat || use.CardKind != action.EffectiveKind || declared.SourceSeat != action.ActorSeat || declared.CardKind != action.EffectiveKind)
            throw new InvalidOperationException("Cannot-nullify policy lost its real original trick declaration.");
        var receipt = new OrdinaryTrickCannotNullifyReceipt(new(binding.Source.SkillId, binding.Policy.Id, action.ActorSeat, binding.Source.SkillInstanceId), use.Id, action.ActionId, action.ActorSeat, action.EffectiveKind);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { OrdinaryTrickCannotNullify = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryTrickCannotNullifyIssuedEvent(receipt));
    }

    internal bool IsOriginalOrdinaryTrickCannotNullify(long useId)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.OrdinaryTrickCannotNullify is not { } r)
            return false;
        AssertOrdinaryTrickCannotNullify(_coreCardUseLifecycleOperations.LifecycleCardUse(useId)!);
        return true;
    }

    internal void AssertOrdinaryTrickCannotNullify(CardUseFrame use)
    {
        if (use.OrdinaryTrickCannotNullify is not { } r)
            return;
        if (!HasOrdinaryTrickCannotNullifyCapability || r.CardUseFrameId != use.Id || use.Action is not { Type: CardActionType.Use } action || r.ActionId != action.ActionId || r.ActorSeat != action.ActorSeat || r.ActorSeat != use.SourceSeat || r.Source.OwnerSeat != r.ActorSeat || r.EffectiveKind != use.CardKind || r.EffectiveKind != action.EffectiveKind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(r.EffectiveKind) || r.EffectiveKind == CardKind.Nullification || string.IsNullOrWhiteSpace(r.Source.SkillInstanceId) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(r.Source.SkillId)?.Program?.CardPolicies.SingleOrDefault(p => p.Id == r.Source.BindingId && p.Kind == SkillProgramCardPolicyKind.CannotNullifyOwnOrdinaryTrick)is not { } policy || !policy.CardKinds.Contains(r.EffectiveKind) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OrdinaryTrickCannotNullifyIssuedEvent>().Count(e => e.Receipt == r) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == r.ActorSeat && e.CardKind == r.EffectiveKind && e.CardId == use.CardId) != 1)
            throw new InvalidOperationException("An issued cannot-nullify ordinary trick lost its exact original use/facts.");
    }
}
