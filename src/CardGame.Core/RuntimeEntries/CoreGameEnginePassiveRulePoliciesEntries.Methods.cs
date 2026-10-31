#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePassiveRulePoliciesEntries
{
    internal bool ApplyDelayedJudgmentOutcomePolicies(global::CardGame.Core.JudgmentFrame frame, bool succeeded) => _resolve().ApplyDelayedJudgmentOutcomePolicies(frame, succeeded);
    internal global::CardGame.Core.RuleQueryEvaluation ApplyPersistentOutgoingDistancePolicy(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.RuleQueryEvaluation original) => _resolve().ApplyPersistentOutgoingDistancePolicy(source, target, original);
    internal bool HasEligiblePublicEnemyDamageObserver(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::System.Collections.Generic.IReadOnlyList<int> propagatedTargets) => _resolve().HasEligiblePublicEnemyDamageObserver(attack, propagatedTargets);
    internal bool HasPerOccurrenceSourceDamageModifiers(int sourceSeat) => _resolve().HasPerOccurrenceSourceDamageModifiers(sourceSeat);
    internal bool HasPublicEnemyParticipantDamageModifiers { get => _resolve().HasPublicEnemyParticipantDamageModifiers; }
}
