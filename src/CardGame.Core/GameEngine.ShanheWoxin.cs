using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheWoxinCreditEarnedEvent(int Ordinal, CardUseEffectSource Source, string GameplayHash,
    long DamageFrameId, int ActualTurn, int TurnOwnerSeat, int ReceivedAmount) : IGameEvent;
public sealed record ShanheWoxinReservation(long Id, long AttackFrameId, CardUseEffectSource Source,
    string GameplayHash, int OriginalTargetSeat, int Amount);
public sealed record ShanheWoxinReservationIssuedEvent(ShanheWoxinReservation Receipt) : IGameEvent;
public sealed record ShanheWoxinCreditReservedEvent(long ReservationId, int CreditOrdinal) : IGameEvent;
public sealed record ShanheWoxinReservationConsumedEvent(long ReservationId, long DamageFrameId,
    int SourceSeat, int TargetSeat, int AppliedAmount) : IGameEvent;

public sealed partial record AttackAttemptState
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheWoxinReservation? ShanheWoxin { get; init; }
}
public sealed partial record CardAttackState
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheWoxinReservation? ShanheWoxin { get; init; }
}

public sealed partial class GameEngine
{
    private bool HasWoxinPolicy => _contentRegistry.ProgramDependencies.HasCardPolicy(
        SkillProgramCardPolicyKind.ReserveNextDamageAfterOffTurnHit);

    private CardUseEffectSource? WoxinSource(int owner, IReadOnlySet<string>? visible = null) =>
        CardPolicies(_players[owner], SkillProgramCardPolicyKind.ReserveNextDamageAfterOffTurnHit)
            .Where(item => visible is null || visible.Contains(item.Source.SkillId))
            .Select(item => new CardUseEffectSource(item.Source.SkillId, item.Policy.Id, owner, item.Source.SkillInstanceId))
            .FirstOrDefault();

    private ShanheWoxinReservation? WoxinReceipt(IDamageAttempt attack) => attack is ProgramAttackHandle
        ? GetProgramAttackState(attack.ResolutionId).ShanheWoxin
        : GetCardAttackState(attack.ResolutionId).ShanheWoxin;

    private IEnumerable<ShanheWoxinReservation> LiveWoxinReservations()
    {
        foreach (var frame in _resolutionStack)
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
            if (card is { IsChainPropagation: false, ShanheWoxin: { } receipt }) yield return receipt;
            if (frame is CardUseFrame { PreparedTargetAttacks: { } prepared })
                foreach (var queued in prepared)
                    if (queued is { IsChainPropagation: false, ShanheWoxin: { } queuedReceipt }) yield return queuedReceipt;
        }
    }

    private ShanheWoxinCreditEarnedEvent[] AvailableWoxinCredits(CardUseEffectSource source, string hash)
    {
        var blocked = ProgramEventHistory<ShanheWoxinReservationConsumedEvent>().Select(fact => fact.ReservationId)
            .Concat(LiveWoxinReservations().Select(receipt => receipt.Id)).ToHashSet();
        var spent = ProgramEventHistory<ShanheWoxinCreditReservedEvent>()
            .Where(fact => blocked.Contains(fact.ReservationId)).Select(fact => fact.CreditOrdinal).ToHashSet();
        return ProgramEventHistory<ShanheWoxinCreditEarnedEvent>()
            .Where(fact => fact.Source == source && fact.GameplayHash == hash && !spent.Contains(fact.Ordinal)).ToArray();
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 63300)]
    private int ReserveWoxinDamage(IDamageAttempt attack)
    {
        if (!HasWoxinPolicy || attack.IsSourceLess || attack.IsChainPropagation) return 0;
        if (WoxinReceipt(attack) is { } existing) return existing.Amount;
        var binding = WoxinSource(attack.SourceSeat);
        if (binding is null) return 0;
        var hash = _contentRegistry.GetSkill(binding.SkillId).Program!.GameplayHash;
        var credits = AvailableWoxinCredits(binding, hash);
        if (credits.Length == 0) return 0;
        var receipt = new ShanheWoxinReservation(ProgramEventHistory<ShanheWoxinReservationIssuedEvent>().Count + 1L,
            attack.ResolutionId, binding, hash, attack.TargetSeat, credits.Length);
        if (attack is ProgramAttackHandle)
            UpdateProgramAttackState(attack.ResolutionId, state => state with { ShanheWoxin = receipt });
        else UpdateCardAttackState(attack.ResolutionId, state => state! with { ShanheWoxin = receipt });
        AdvanceEventRulesAndQueueFact(new ShanheWoxinReservationIssuedEvent(receipt));
        foreach (var credit in credits)
            AdvanceEventRulesAndQueueFact(new ShanheWoxinCreditReservedEvent(receipt.Id, credit.Ordinal));
        return receipt.Amount;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 63300)]
    private void RecordWoxinAppliedDamage(IGameEvent payload)
    {
        if (!HasWoxinPolicy || payload is not DamageAppliedEvent { Amount: > 0 } applied) return;
        var earnedBinding = _currentSeat != applied.TargetSeat && _players[applied.TargetSeat].IsAlive
            ? WoxinSource(applied.TargetSeat)
            : null;
        if (earnedBinding is null && !LiveWoxinReservations().Any(receipt => receipt.Source.OwnerSeat == applied.SourceSeat)) return;
        if (_resolutionStack.LastOrDefault() is not DamageFrame damage ||
            damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount ||
            DamageArmorPaymentOwner(damage) is not { } attack)
            throw new InvalidOperationException("Woxin requires the exact native applied-damage frame.");
        if (!attack.IsSourceLess && !attack.IsChainPropagation && WoxinReceipt(attack) is { } receipt)
        {
            var reserved = ProgramEventHistory<ShanheWoxinCreditReservedEvent>().Where(fact => fact.ReservationId == receipt.Id).ToArray();
            if (receipt.AttackFrameId != damage.ParentFrameId || receipt.Source.OwnerSeat != applied.SourceSeat ||
                ProgramEventHistory<ShanheWoxinReservationIssuedEvent>().Count(fact => fact.Receipt == receipt) != 1 ||
                ProgramEventHistory<ShanheWoxinReservationConsumedEvent>().Any(fact => fact.ReservationId == receipt.Id) ||
                reserved.Length != receipt.Amount || reserved.Select(fact => fact.CreditOrdinal).Distinct().Count() != receipt.Amount ||
                reserved.Any(fact => ProgramEventHistory<ShanheWoxinCreditEarnedEvent>().Count(credit =>
                    credit.Ordinal == fact.CreditOrdinal && credit.Source == receipt.Source && credit.GameplayHash == receipt.GameplayHash) != 1) ||
                _contentRegistry.GetSkill(receipt.Source.SkillId).Program?.GameplayHash != receipt.GameplayHash)
                throw new InvalidOperationException("Woxin lost its original frame-owned credit reservation.");
            AdvanceEventRulesAndQueueFact(new ShanheWoxinReservationConsumedEvent(receipt.Id, damage.Id,
                applied.SourceSeat, applied.TargetSeat, applied.Amount));
        }
        // Apply the outgoing reservation first: self-damage outside the turn
        // may earn a new credit, which belongs to a later damage occurrence.
        if (earnedBinding is null) return;
        AdvanceEventRulesAndQueueFact(new ShanheWoxinCreditEarnedEvent(
            ProgramEventHistory<ShanheWoxinCreditEarnedEvent>().Count + 1, earnedBinding,
            _contentRegistry.GetSkill(earnedBinding.SkillId).Program!.GameplayHash, damage.Id, _turnNumber, _currentSeat, applied.Amount));
    }

    private int WoxinPublicBonus(GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, IDamageAttempt? attack)
    {
        if (!HasWoxinPolicy || sourceLess || chain || !_players[sourceSeat].IsAlive) return 0;
        var visible = VisiblePredictionSkillIds(view.Players.Single(player => player.Seat == sourceSeat));
        if (attack is not null && WoxinReceipt(attack) is { } receipt)
            return visible.Contains(receipt.Source.SkillId) ? receipt.Amount : 0;
        var binding = WoxinSource(sourceSeat, visible);
        return binding is null ? 0 : AvailableWoxinCredits(binding,
            _contentRegistry.GetSkill(binding.SkillId).Program!.GameplayHash).Length;
    }
}
