#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundOnceCardlessBasicRuntime
{
    internal bool RoundOnceCardlessBasicUsed(int ownerSeat, string skillId, string group) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessBasicQuotaPaidEvent>().Any(e => e.Receipt.Source.OwnerSeat == ownerSeat && e.Receipt.Source.SkillId == skillId && e.Receipt.UsageGroup == group && e.Receipt.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
    internal bool CanUseRoundOnceCardlessBasic(CharacterState actor, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool forResponse)
    {
        if (rule.RoundOnceCardlessBasic is not { } p)
            return true;
        return rule.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(actor)) && _coreMatchOutcomeWinnerAccess._winner == Winner.None && actor.IsAlive && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 1 && rule.InputCount == 0 && (rule.OutputKind is CardKind.Slash or CardKind.Dodge) && (forResponse ? rule.ForResponse : rule.ForPlay && rule.OutputKind == CardKind.Slash) && instance.SkillInstanceId is not null && _coreNationalSkillsOperations.HasRuntimeSkillInstance(actor, instance.SkillId, instance.SkillInstanceId) && !RoundOnceCardlessBasicUsed(actor.Seat, instance.SkillId, p.UsageGroup) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, rule.OutputKind, forResponse ? CardActionType.Response : CardActionType.Use) && _characterZhouYuOlMouOperations.IsMouGuquCardlessAllowed(actor, rule.OutputKind, forResponse);
    }

    internal RoundOnceCardlessBasicPaymentReceipt ConsumeRoundOnceCardlessBasic(CardConversionSource source, bool forResponse)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat];
        var rule = _corePlayCardHistoryOperations.ViewAsRule(source) ?? throw new InvalidOperationException("Cardless basic payment lost its current rule.");
        var p = rule.RoundOnceCardlessBasic ?? throw new InvalidOperationException("Cardless basic payment lost its strict policy.");
        var instance = _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.SingleOrDefault(i => i.SkillId == source.SkillId && i.SkillInstanceId == source.SkillInstanceId);
        if (instance is null || !CanUseRoundOnceCardlessBasic(actor, instance, rule, forResponse))
            throw new InvalidOperationException("Cardless basic cannot consume an invalid source or already spent shared round allowance.");
        var receipt = new RoundOnceCardlessBasicPaymentReceipt(source, instance.Program.GameplayHash, p.UsageGroup, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundOnceCardlessBasicQuotaPaidEvent(receipt));
        return receipt;
    }

    internal RoundOnceCardlessBasicUseReceipt IssueRoundOnceCardlessBasicUse(ResolutionFrame owner, CardActionContext action, CardConversionSource source)
    {
        var rule = _corePlayCardHistoryOperations.ViewAsRule(source) ?? throw new InvalidOperationException("Issued cardless basic lost its catalog source.");
        var policy = rule.RoundOnceCardlessBasic ?? throw new InvalidOperationException("Issued cardless basic lost its policy.");
        var payment = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessBasicQuotaPaidEvent>().Select(e => e.Receipt).SingleOrDefault(p => p.Source == source && p.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && p.UsageGroup == policy.UsageGroup) ?? throw new InvalidOperationException("Issued cardless basic has no accepted quota payment.");
        if (action.ActionId <= 0 || action.ActorSeat != source.OwnerSeat || action.ProviderSeat != source.OwnerSeat || action.RequesterSeat is not null || action.PhysicalCards.Count != 0 || action.ConversionChain is not [var exact] || exact != source || action.EffectiveKind != rule.OutputKind || action.Type is not (CardActionType.Use or CardActionType.Response) || action.Type == CardActionType.Response && action.ResponderSeat != source.OwnerSeat || action.Type == CardActionType.Use && !rule.ForPlay || action.Type == CardActionType.Response && !rule.ForResponse || payment.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash)
            throw new InvalidOperationException("Issued cardless basic changed its actual actor/provider/zero-material/output/native action tuple.");
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessBasicUseIssuedEvent>().Any(e => e.Receipt.Payment == payment))
            throw new InvalidOperationException("One cardless payment cannot issue a second native action.");
        var receipt = new RoundOnceCardlessBasicUseReceipt(payment, owner.Id, action.ActionId, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.Type, action.ParentActionId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundOnceCardlessBasicUseIssuedEvent(receipt));
        return receipt;
    }

    internal bool IsIssuedRoundOnceCardlessBasicUse(RoundOnceCardlessBasicUseReceipt? receipt, ResolutionFrame owner, CardActionContext action)
    {
        if (receipt is not { } r || r.OwnerFrameId != owner.Id || r.CardActionId != action.ActionId || r.ActorSeat != action.ActorSeat || r.ProviderSeat != action.ProviderSeat || r.RequesterSeat != action.RequesterSeat || r.ResponderSeat != action.ResponderSeat || r.OpponentSeat != action.OpponentSeat || r.ParentActionId != action.ParentActionId || r.ActionType != action.Type || r.EffectiveKind != action.EffectiveKind || action.PhysicalCards.Count != 0 || action.ConversionChain is not [var source] || source != r.Payment.Source || _corePlayCardHistoryOperations.ViewAsRule(source)is not { RoundOnceCardlessBasic: { } } rule || rule.OutputKind != r.EffectiveKind || _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash != r.Payment.GameplayHash)
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessBasicQuotaPaidEvent>().Count(e => e.Receipt == r.Payment) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessBasicUseIssuedEvent>().Count(e => e.Receipt == r) == 1;
    }
}
