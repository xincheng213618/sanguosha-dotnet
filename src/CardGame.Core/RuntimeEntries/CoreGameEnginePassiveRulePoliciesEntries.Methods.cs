#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePassiveRulePoliciesEntries
{
    internal bool ApplyDelayedJudgmentOutcomePolicies(JudgmentFrame frame, bool succeeded) => _resolve().ApplyDelayedJudgmentOutcomePolicies(frame, succeeded);
    internal RuleQueryEvaluation ApplyPersistentOutgoingDistancePolicy(CharacterState source, CharacterState target, RuleQueryEvaluation original) => _resolve().ApplyPersistentOutgoingDistancePolicy(source, target, original);
    internal bool HasEligiblePublicEnemyDamageObserver(GameEngine.IDamageAttempt attack, IReadOnlyList<int> propagatedTargets) => _resolve().HasEligiblePublicEnemyDamageObserver(attack, propagatedTargets);
    internal bool HasPerOccurrenceSourceDamageModifiers(int sourceSeat) => _resolve().HasPerOccurrenceSourceDamageModifiers(sourceSeat);
    internal bool HasPublicEnemyParticipantDamageModifiers { get => _resolve().HasPublicEnemyParticipantDamageModifiers; }
}
