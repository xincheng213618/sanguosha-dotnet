#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAttackAttemptEntries
{
    internal void AssertProgramAttackState() => _resolve().AssertProgramAttackState();
    internal void CompleteProgramAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CompleteProgramAttack(attack);
    internal global::CardGame.Core.AttackAttemptState GetProgramAttackState(long ownerFrameId) => _resolve().GetProgramAttackState(ownerFrameId);
    internal bool HasDamageTriggerForAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HasDamageTriggerForAttack(attack);
    internal bool TryAdvanceChainedDamageTarget(global::CardGame.Core.GameEngine.IDamageAttempt attack, scoped out int fromSeat) => _resolve().TryAdvanceChainedDamageTarget(attack, out fromSeat);
    internal void UpdateProgramAttackState(long ownerFrameId, global::System.Func<global::CardGame.Core.AttackAttemptState, global::CardGame.Core.AttackAttemptState> update) => _resolve().UpdateProgramAttackState(ownerFrameId, update);
    internal global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get => _resolve().CurrentDamageAttempt; }
    internal bool IsCardAttackSuspendedByProgramDamage { get => _resolve().IsCardAttackSuspendedByProgramDamage; }
}
