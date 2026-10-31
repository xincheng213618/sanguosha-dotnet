#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreShenwuNpcProgramsEntries
{
    internal void BindShenwuParticipant(ProgramSkillFrame frame, SkillProgramEffect effect, int? seat, string choice) => _resolve().BindShenwuParticipant(frame, effect, seat, choice);
    internal IEnumerable<ProgramTriggerCandidate> CollectShenwuRecoveryCandidates(HpChangeContext change) => _resolve().CollectShenwuRecoveryCandidates(change);
    internal ProgramSkillWindowContext CreateShenwuRecoveryContext(HpChangeContext change, ProgramTriggerCandidate candidate) => _resolve().CreateShenwuRecoveryContext(change, candidate);
    internal SkillProgramStepOutcome ExecuteShenwuNpc(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ExecuteShenwuNpc(supplied, effect);
    internal bool IsShenwuRecoveryCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsShenwuRecoveryCandidate(candidate, context);
    internal bool? ProveShenwuRecovery(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveShenwuRecovery(trigger, candidate, context);
    internal GameEngine.IDamageAttempt ShenwuAppliedDamage(ProgramSkillFrame frame) => _resolve().ShenwuAppliedDamage(frame);
    internal bool ShenwuCannotTurnOver(CharacterState target) => _resolve().ShenwuCannotTurnOver(target);
    internal bool ShenwuEnabled(CharacterState player, string skillId) => _resolve().ShenwuEnabled(player, skillId);
    internal bool ShenwuFutaiCardUseForbidden(ProgramCardUseQuery query) => _resolve().ShenwuFutaiCardUseForbidden(query);
    internal bool ShenwuFutaiForbidsPeach(int actorSeat, CardKind kind, CardActionType type) => _resolve().ShenwuFutaiForbidsPeach(actorSeat, kind, type);
    internal bool ShenwuMingwanDirectedTargetProhibited(ProgramDirectedTargetQuery query) => _resolve().ShenwuMingwanDirectedTargetProhibited(query);
    internal bool ShenwuMingwanForbidsTarget(int actorSeat, int targetSeat) => _resolve().ShenwuMingwanForbidsTarget(actorSeat, targetSeat);
    internal IReadOnlyList<int> ShenwuMingwanVictims(int ownerSeat) => _resolve().ShenwuMingwanVictims(ownerSeat);
    internal int ShenwuNpcDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ShenwuNpcDamageBonus(attack);
    internal bool ShenwuOwnActualTurn(int seat) => _resolve().ShenwuOwnActualTurn(seat);
}
