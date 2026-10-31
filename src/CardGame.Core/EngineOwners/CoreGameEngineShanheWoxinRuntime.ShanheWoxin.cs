#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheWoxinRuntime
{
    internal bool HasWoxinPolicy => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.ReserveNextDamageAfterOffTurnHit);

    internal CardUseEffectSource? WoxinSource(int owner, IReadOnlySet<string>? visible = null) => _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[owner], SkillProgramCardPolicyKind.ReserveNextDamageAfterOffTurnHit).Where(item => visible is null || visible.Contains(item.Source.SkillId)).Select(item => new CardUseEffectSource(item.Source.SkillId, item.Policy.Id, owner, item.Source.SkillInstanceId)).FirstOrDefault();
    internal ShanheWoxinReservation? WoxinReceipt(global::CardGame.Core.GameEngine.IDamageAttempt attack) => attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ShanheWoxin : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ShanheWoxin;
    internal IEnumerable<ShanheWoxinReservation> LiveWoxinReservations()
    {
        foreach (var frame in _resolutionStackOperations._resolutionStack)
        {
            if (frame is ProgramSkillFrame { AttackAttempt: { IsChainPropagation: false, ShanheWoxin: { } skill } })
                yield return skill;
            var card = frame switch
            {
                CardUseFrame use => use.CardAttack,
                ProgramSkillFrame program => program.CardAttack,
                JudgmentFrame judgment => judgment.CardAttack,
                _ => null
            };
            if (card is { IsChainPropagation: false, ShanheWoxin: { } receipt })
                yield return receipt;
            if (frame is CardUseFrame { PreparedTargetAttacks: { } prepared })
                foreach (var queued in prepared)
                    if (queued is { IsChainPropagation: false, ShanheWoxin: { } queuedReceipt })
                        yield return queuedReceipt;
        }
    }

    internal ShanheWoxinCreditEarnedEvent[] AvailableWoxinCredits(CardUseEffectSource source, string hash)
    {
        var blocked = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheWoxinReservationConsumedEvent>().Select(fact => fact.ReservationId).Concat(LiveWoxinReservations().Select(receipt => receipt.Id)).ToHashSet();
        var spent = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheWoxinCreditReservedEvent>().Where(fact => blocked.Contains(fact.ReservationId)).Select(fact => fact.CreditOrdinal).ToHashSet();
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheWoxinCreditEarnedEvent>().Where(fact => fact.Source == source && fact.GameplayHash == hash && !spent.Contains(fact.Ordinal)).ToArray();
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 63300)]
    internal int ReserveWoxinDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!HasWoxinPolicy || attack.IsSourceLess || attack.IsChainPropagation)
            return 0;
        if (WoxinReceipt(attack)is { } existing)
            return existing.Amount;
        var binding = WoxinSource(attack.SourceSeat);
        if (binding is null)
            return 0;
        var hash = _contentRegistryOperations._contentRegistry.GetSkill(binding.SkillId).Program!.GameplayHash;
        var credits = AvailableWoxinCredits(binding, hash);
        if (credits.Length == 0)
            return 0;
        var receipt = new ShanheWoxinReservation(_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheWoxinReservationIssuedEvent>().Count + 1L, attack.ResolutionId, binding, hash, attack.TargetSeat, credits.Length);
        if (attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, state => state with { ShanheWoxin = receipt });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { ShanheWoxin = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheWoxinReservationIssuedEvent(receipt));
        foreach (var credit in credits)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheWoxinCreditReservedEvent(receipt.Id, credit.Ordinal));
        return receipt.Amount;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 63300)]
    internal void RecordWoxinAppliedDamage(IGameEvent payload)
    {
        if (!HasWoxinPolicy || payload is not DamageAppliedEvent { Amount: > 0 } applied)
            return;
        var earnedBinding = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != applied.TargetSeat && _coreMatchParticipantsPlayersAccess._players[applied.TargetSeat].IsAlive ? WoxinSource(applied.TargetSeat) : null;
        if (earnedBinding is null && !LiveWoxinReservations().Any(receipt => receipt.Source.OwnerSeat == applied.SourceSeat))
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount || _coreDamageArmorPaymentsOperations.DamageArmorPaymentOwner(damage)is not { } attack)
            throw new InvalidOperationException("Woxin requires the exact native applied-damage frame.");
        if (!attack.IsSourceLess && !attack.IsChainPropagation && WoxinReceipt(attack)is { } receipt)
        {
            var reserved = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheWoxinCreditReservedEvent>().Where(fact => fact.ReservationId == receipt.Id).ToArray();
            if (receipt.AttackFrameId != damage.ParentFrameId || receipt.Source.OwnerSeat != applied.SourceSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheWoxinReservationIssuedEvent>().Count(fact => fact.Receipt == receipt) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheWoxinReservationConsumedEvent>().Any(fact => fact.ReservationId == receipt.Id) || reserved.Length != receipt.Amount || reserved.Select(fact => fact.CreditOrdinal).Distinct().Count() != receipt.Amount || reserved.Any(fact => _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheWoxinCreditEarnedEvent>().Count(credit => credit.Ordinal == fact.CreditOrdinal && credit.Source == receipt.Source && credit.GameplayHash == receipt.GameplayHash) != 1) || _contentRegistryOperations._contentRegistry.GetSkill(receipt.Source.SkillId).Program?.GameplayHash != receipt.GameplayHash)
                throw new InvalidOperationException("Woxin lost its original frame-owned credit reservation.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheWoxinReservationConsumedEvent(receipt.Id, damage.Id, applied.SourceSeat, applied.TargetSeat, applied.Amount));
        }

        // Apply the outgoing reservation first: self-damage outside the turn
        // may earn a new credit, which belongs to a later damage occurrence.
        if (earnedBinding is null)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheWoxinCreditEarnedEvent(_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheWoxinCreditEarnedEvent>().Count + 1, earnedBinding, _contentRegistryOperations._contentRegistry.GetSkill(earnedBinding.SkillId).Program!.GameplayHash, damage.Id, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, applied.Amount));
    }

    internal int WoxinPublicBonus(GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, global::CardGame.Core.GameEngine.IDamageAttempt? attack)
    {
        if (!HasWoxinPolicy || sourceLess || chain || !_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive)
            return 0;
        var visible = global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime.VisiblePredictionSkillIds(view.Players.Single(player => player.Seat == sourceSeat));
        if (attack is not null && WoxinReceipt(attack)is { } receipt)
            return visible.Contains(receipt.Source.SkillId) ? receipt.Amount : 0;
        var binding = WoxinSource(sourceSeat, visible);
        return binding is null ? 0 : AvailableWoxinCredits(binding, _contentRegistryOperations._contentRegistry.GetSkill(binding.SkillId).Program!.GameplayHash).Length;
    }
}
