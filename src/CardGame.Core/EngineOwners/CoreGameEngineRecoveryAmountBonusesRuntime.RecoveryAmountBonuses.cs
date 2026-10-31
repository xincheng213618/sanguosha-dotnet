#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecoveryAmountBonusesRuntime
{
    internal bool TracksRecoveryAmountBonuses => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.OwnRecoveryAmountBonus);

    internal bool HasOwnRecoveryAmountSupport(int targetSeat) => TracksRecoveryAmountBonuses && _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && _coreMatchParticipantsPlayersAccess._players[targetSeat] is { IsAlive: true, HasHpValue: true } target && target.Hp < target.MaxHp && _coreProgramCardPoliciesOperations.CardPolicies(target, SkillProgramCardPolicyKind.OwnRecoveryAmountBonus).Any();
    internal static int? RecoveryOrdinalAt(IReadOnlyList<IGameEvent> history, int endExclusive, int targetSeat, int turnNumber, int turnOwnerSeat) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_RecoveryAmountBonuses.RecoveryOrdinalAt(history, endExclusive, targetSeat, turnNumber, turnOwnerSeat);
    internal RecoveryAmountBonusReceipt? PrepareOwnRecoveryAmountBonus(PreparedRecoveryApplication application)
    {
        if (!HasOwnRecoveryAmountSupport(application.TargetSeat) || application.RequestedAmount <= 0 || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return null;
        var target = _coreMatchParticipantsPlayersAccess._players[application.TargetSeat];
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var ordinal = RecoveryOrdinalAt(history, history.Length, target.Seat, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
        var sources = _coreProgramCardPoliciesOperations.CardPolicies(target, SkillProgramCardPolicyKind.OwnRecoveryAmountBonus).Where(item => item.Policy.MaximumRecoveryOrdinal is null || ordinal is { } n && n <= item.Policy.MaximumRecoveryOrdinal).Select(item => new RecoveryAmountBonusSource(new(item.Source.SkillId, item.Policy.Id, target.Seat, item.Source.SkillInstanceId), item.Source.Program.GameplayHash, item.Policy.Value, item.Policy.MaximumRecoveryOrdinal)).ToArray();
        if (sources.Length == 0)
            return null;
        var requested = checked(application.RequestedAmount + sources.Sum(s => s.Amount));
        var actual = Math.Min(requested, Math.Max(0, target.MaxHp - target.Hp));
        if (actual <= 0)
            return null;
        return new(application.SourceSeat, target.Seat, application.RequestedAmount, requested, target.Hp, checked(target.Hp + actual), _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, ordinal)
        {
            Sources = sources,
            MaxHpAtApplication = target.MaxHp
        };
    }

    internal int SupportedRecoveryAmount(RecoveryReplacementFrame recovery, int baseAmount)
    {
        var supported = _characterCaoSongOlOperations.SupportedCaoSongRecoveryAmount(recovery, baseAmount);
        if (recovery.AmountBonus is not { } bonus)
            return supported;
        RequireRecoveryAmountBonus(recovery, bonus);
        return Math.Max(supported, bonus.ModifiedRequestedAmount);
    }

    internal void RequireRecoveryAmountBonus(RecoveryReplacementFrame frame, RecoveryAmountBonusReceipt bonus)
    {
        if (frame.CandidateIndex is { } invalid && (invalid < 0 || invalid >= frame.Attempt.Candidates.Count))
            throw new InvalidOperationException("Recovery amount lost its selected recipient.");
        var candidate = frame.CandidateIndex is { } selected ? frame.Attempt.Candidates[selected] : null;
        var targetSeat = candidate?.OwnerSeat ?? frame.Attempt.TargetSeat;
        var sourceSeat = candidate is null ? frame.Attempt.SourceSeat : frame.Attempt.TargetSeat;
        var baseAmount = _characterCaoSongOlOperations.SupportedCaoSongRecoveryAmount(frame, candidate?.RecoveryAmount ?? frame.Attempt.Amount);
        if (bonus.SourceSeat != sourceSeat || bonus.TargetSeat != targetSeat || bonus.BaseRequestedAmount != baseAmount || bonus.BaseRequestedAmount <= 0 || bonus.Sources.Count == 0 || bonus.Sources.Select(s => s.Source).Distinct().Count() != bonus.Sources.Count || bonus.ModifiedRequestedAmount != checked(bonus.BaseRequestedAmount + bonus.Sources.Sum(s => s.Amount)) || bonus.HpAfter <= bonus.HpBefore || bonus.MaxHpAtApplication < 1 || bonus.AppliedRecoveryFrameId <= frame.Id || frame.OriginalRecovered != (candidate is null ? bonus.HpAfter - bonus.HpBefore : 0) || bonus.HpAfter != Math.Min((long)bonus.MaxHpAtApplication, (long)bonus.HpBefore + bonus.ModifiedRequestedAmount) || bonus.ActualTurnNumber < 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(bonus.ActualTurnOwnerSeat) || bonus.Sources.Any(s => s.Source.OwnerSeat != targetSeat || s.Amount <= 0 || !_contentRegistryOperations._contentRegistry.Skills.TryGetValue(s.Source.SkillId, out var skill) || skill.Program is not { } program || program.GameplayHash != s.GameplayHash || !program.CardPolicies.Any(p => p.Id == s.Source.BindingId && p.Kind == SkillProgramCardPolicyKind.OwnRecoveryAmountBonus && p.Value == s.Amount && p.MaximumRecoveryOrdinal == s.MaximumRecoveryOrdinal) || s.MaximumRecoveryOrdinal is { } max && (bonus.ActualTurnRecoveryOrdinal is not { } ordinal || ordinal > max)))
            throw new InvalidOperationException("Recovery amount lost its exact recipient, source policies or atomic payment.");
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var facts = history.Select((e, i) => (e, i)).Where(item => item.e is RecoveryAmountBonusAppliedEvent paid && paid.RecoveryFrameId == frame.Id).ToArray();
        if (facts is not [var fact] || fact.e is not RecoveryAmountBonusAppliedEvent issued || !RecoveryAmountBonusEquals(issued.Receipt, bonus) || RecoveryOrdinalAt(history, fact.i, targetSeat, bonus.ActualTurnNumber, bonus.ActualTurnOwnerSeat) != bonus.ActualTurnRecoveryOrdinal)
            throw new InvalidOperationException("Recovery amount lost its unique issued historical payment.");
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27920)]
    internal void RequireAtomicRecoveryAmountBonus(RecoveryAmountBonusAppliedEvent paid)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not RecoveryFrame actual || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == paid.RecoveryFrameId)is not RecoveryReplacementFrame owner || owner.AmountBonus is not { } bonus || !RecoveryAmountBonusEquals(bonus, paid.Receipt) || actual.Id != bonus.AppliedRecoveryFrameId || actual.ParentFrameId != owner.Id || actual.SourceSeat != bonus.SourceSeat || actual.TargetSeat != bonus.TargetSeat || actual.HpBefore != bonus.HpBefore || actual.Amount != bonus.HpAfter - bonus.HpBefore || _coreMatchParticipantsPlayersAccess._players[actual.TargetSeat].Hp != bonus.HpAfter || owner.Stage != RecoveryReplacementStage.RecoveryApplied)
            throw new InvalidOperationException("Recovery bonus must be issued by its exact once-paid native recovery child.");
    }

    // The original short-lived RecoveryFrame is still the atomic payer here.
    // No skill holder filter: later acquisition must see all earlier recoveries.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 2)]
    internal void RecordActualTurnRecovery(RecoveryAppliedEvent applied)
    {
        if (!TracksRecoveryAmountBonuses || _resolutionStackOperations._resolutionStack.LastOrDefault()is not RecoveryFrame recovery)
            return;
        var actual = _coreMatchParticipantsPlayersAccess._players[recovery.TargetSeat].Hp - recovery.HpBefore;
        if (actual <= 0)
            return;
        if (applied.SourceSeat != recovery.SourceSeat || applied.TargetSeat != recovery.TargetSeat || applied.RemainingHp != _coreMatchParticipantsPlayersAccess._players[recovery.TargetSeat].Hp || actual > recovery.Amount || applied.Amount < actual)
            throw new InvalidOperationException("Actual recovery lost its native atomic recovery frame.");
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var ordinal = RecoveryOrdinalAt(history, history.Length, recovery.TargetSeat, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
        if (ordinal is null)
            return;
        if (history.OfType<ActualTurnRecoveryRecordedEvent>().Any(e => e.RecoveryFrameId == recovery.Id))
            throw new InvalidOperationException("A recovery frame cannot pay or record its actual ordinal twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualTurnRecoveryRecordedEvent(recovery.Id, recovery.ParentFrameId, recovery.SourceSeat, recovery.TargetSeat, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, ordinal.Value, recovery.Amount, actual, recovery.HpBefore, applied.RemainingHp));
    }

    internal void AssertRecoveryAmountBonusReceipts()
    {
        foreach (var frame in _resolutionStackOperations._resolutionStack.OfType<RecoveryReplacementFrame>())
            if (frame.AmountBonus is { } bonus)
            {
                if (frame.Stage == RecoveryReplacementStage.Choosing)
                    throw new InvalidOperationException("A recovery amount receipt must already own its atomic application.");
                RequireRecoveryAmountBonus(frame, bonus);
            }
    }

    internal static bool RecoveryAmountBonusEquals(RecoveryAmountBonusReceipt a, RecoveryAmountBonusReceipt b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_RecoveryAmountBonuses.RecoveryAmountBonusEquals(a, b);
}
