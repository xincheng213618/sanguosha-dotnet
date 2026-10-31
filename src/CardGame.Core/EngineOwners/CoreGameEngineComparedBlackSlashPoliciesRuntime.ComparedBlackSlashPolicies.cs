#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineComparedBlackSlashPoliciesRuntime
{
    internal bool HasComparedBlackSlashPolicies => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.NullifyBlackSlashByCurrentHp) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.ProhibitBlackSlashResponseByCurrentHand);

    internal bool ApplyComparedBlackSlashPolicies(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (!HasComparedBlackSlashPolicies || attack.IsSourceLess || attack.IsChainPropagation || attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { Type: CardActionType.Use } action } use || action.ActionId < 1 || action.ActorSeat != use.SourceSeat || attack.CardUserSeat != use.SourceSeat || action.EffectiveKind != kind || !use.TargetSeats.Contains(attack.TargetSeat) || action.EffectiveIsRed != false || (action.PhysicalCards.Count == 0 && action.EffectiveSuit is not (Suit.Spade or Suit.Club)))
            return false;
        var existing = _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ComparedBlackSlash;
        if (existing is not null)
        {
            if (existing.CardUseFrameId != use.Id || existing.ActionId != action.ActionId || existing.ActorSeat != attack.CardUserSeat || existing.TargetSeat != attack.TargetSeat || existing.EffectiveKind != kind || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ComparedBlackSlashAppliedEvent>().Count(e => e.Receipt == existing) != 1)
                throw new InvalidOperationException("A black Slash comparison lost its exact original use and current target.");
            if (existing.CannotRespond && existing.ActorPolicySource is { } paidSource)
                attack.ProhibitDodgeBy(_contentRegistryOperations._contentRegistry.GetSkill(paidSource.SkillId).Name);
            return existing.Ineffective;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var incoming = target.IsAlive ? _coreProgramCardPoliciesOperations.CardPolicies(target, SkillProgramCardPolicyKind.NullifyBlackSlashByCurrentHp, kind).FirstOrDefault() : default;
        var outgoing = actor.IsAlive ? _coreProgramCardPoliciesOperations.CardPolicies(actor, SkillProgramCardPolicyKind.ProhibitBlackSlashResponseByCurrentHand, kind).FirstOrDefault() : default;
        if (incoming.Source is null && outgoing.Source is null)
            return false;
        var actorHand = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Count;
        var targetHand = _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count;
        CardConversionSource? incomingSource = incoming.Source is null ? null : new(incoming.Source.SkillId, incoming.Policy.Id, target.Seat, incoming.Source.SkillInstanceId);
        CardConversionSource? outgoingSource = outgoing.Source is null ? null : new(outgoing.Source.SkillId, outgoing.Policy.Id, actor.Seat, outgoing.Source.SkillInstanceId);
        var receipt = new ComparedBlackSlashReceipt(use.Id, action.ActionId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, actor.Seat, action.ProviderSeat, target.Seat, kind, action.EffectiveSuit ?? Suit.None, false, actor.Hp, target.Hp, actorHand, targetHand, incomingSource, incoming.Source?.Program.GameplayHash, outgoingSource, outgoing.Source?.Program.GameplayHash, incomingSource is not null && actor.Hp >= target.Hp, outgoingSource is not null && targetHand <= actorHand);
        _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => (state ?? throw new InvalidOperationException("Missing Slash owner."))with { ComparedBlackSlash = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ComparedBlackSlashAppliedEvent(receipt));
        if (receipt.CannotRespond && outgoingSource is { } source)
            attack.ProhibitDodgeBy(_contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Name);
        if (receipt.Ineffective)
            _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(use.Id, target.Seat);
        return receipt.Ineffective;
    }

    internal static void ValidateComparedBlackSlashPolicy(string path, SkillProgramCardPolicy policy) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ComparedBlackSlashPolicies.ValidateComparedBlackSlashPolicy(path, policy);
    internal void AssertComparedBlackSlashReceipt(CardUseFrame use)
    {
        if (use.CardAttack?.ComparedBlackSlash is not { } r)
            return;
        var attack = use.CardAttack;
        var action = use.Action;
        if (action is null || action.Type != CardActionType.Use || action.EffectiveIsRed != false || r.CardUseFrameId != use.Id || r.ActionId != action.ActionId || r.ActorSeat != use.SourceSeat || r.ActorSeat != action.ActorSeat || r.ProviderSeat != action.ProviderSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || !use.TargetSeats.Contains(r.TargetSeat) || r.EffectiveKind != action.EffectiveKind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(r.EffectiveKind) || r.EffectiveSuit != (action.EffectiveSuit ?? Suit.None) || r.EffectiveIsRed || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || r.ActorHandCount < 0 || r.TargetHandCount < 0 || r.ActorHp <= 0 || r.TargetHp <= 0 || r.TargetPolicySource is null && r.ActorPolicySource is null || (action.PhysicalCards.Count == 0 && r.EffectiveSuit is not (Suit.Spade or Suit.Club)) || r.Ineffective != (r.TargetPolicySource is not null && r.ActorHp >= r.TargetHp) || r.CannotRespond != (r.ActorPolicySource is not null && r.TargetHandCount <= r.ActorHandCount) || !ComparedBlackSlashPolicyMatches(r.TargetPolicySource, r.TargetPolicyHash, r.TargetSeat, SkillProgramCardPolicyKind.NullifyBlackSlashByCurrentHp) || !ComparedBlackSlashPolicyMatches(r.ActorPolicySource, r.ActorPolicyHash, r.ActorSeat, SkillProgramCardPolicyKind.ProhibitBlackSlashResponseByCurrentHand) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ComparedBlackSlashAppliedEvent>().Count(e => e.Receipt == r) != 1 || r.CannotRespond && (!attack.ProhibitsDodge || !attack.ResponseProhibitingSkillNames.Contains(_contentRegistryOperations._contentRegistry.GetSkill(r.ActorPolicySource!.SkillId).Name, StringComparer.Ordinal)) || r.Ineffective && use.IneffectiveTargetSeats?.Contains(r.TargetSeat) != true || attack.TargetSeat != r.TargetSeat && (!attack.DamageRedirected || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramDamageTransferredEvent>().Any(e => e.ResolutionId == use.Id && e.OwnerSeat == r.TargetSeat && e.TargetSeat == attack.TargetSeat && e.SourceSeat == attack.SourceSeat)))
            throw new InvalidOperationException("A black Slash conclusion changed its frozen same-use appearance, current actor, source metadata or original target.");
    }

    internal bool ComparedBlackSlashPolicyMatches(CardConversionSource? source, string? hash, int owner, SkillProgramCardPolicyKind kind)
    {
        if (source is null)
            return hash is null;
        return source.OwnerSeat == owner && !string.IsNullOrWhiteSpace(source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(source.SkillId)?.Program is { } program && program.GameplayHash == hash && program.CardPolicies.Any(p => p.Id == source.BindingId && p.Kind == kind);
    }
}
