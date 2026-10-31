#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAttackAttemptEntries
{
    internal void CompleteProgramAttack(GameEngine.IDamageAttempt attack) => _resolve().CompleteProgramAttack(attack);
    internal AttackAttemptState GetProgramAttackState(long ownerFrameId) => _resolve().GetProgramAttackState(ownerFrameId);
    internal bool TryAdvanceChainedDamageTarget(GameEngine.IDamageAttempt attack, scoped out int fromSeat) => _resolve().TryAdvanceChainedDamageTarget(attack, out fromSeat);
    internal void UpdateProgramAttackState(long ownerFrameId, Func<AttackAttemptState, AttackAttemptState> update) => _resolve().UpdateProgramAttackState(ownerFrameId, update);
    internal GameEngine.IDamageAttempt? CurrentDamageAttempt { get => _resolve().CurrentDamageAttempt; }
    internal bool IsCardAttackSuspendedByProgramDamage { get => _resolve().IsCardAttackSuspendedByProgramDamage; }
}
