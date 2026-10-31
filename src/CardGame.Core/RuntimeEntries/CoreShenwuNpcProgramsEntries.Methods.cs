#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreShenwuNpcProgramsEntries
{
    internal void BindShenwuParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, int? seat, string choice) => _resolve().BindShenwuParticipant(frame, effect, seat, choice);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectShenwuRecoveryCandidates(global::CardGame.Core.HpChangeContext change) => _resolve().CollectShenwuRecoveryCandidates(change);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateShenwuRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateShenwuRecoveryContext(change, candidate);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteShenwuNpc(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteShenwuNpc(supplied, effect);
    internal bool IsShenwuRecoveryCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsShenwuRecoveryCandidate(candidate, context);
    internal bool? ProveShenwuRecovery(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveShenwuRecovery(trigger, candidate, context);
    internal global::CardGame.Core.GameEngine.IDamageAttempt ShenwuAppliedDamage(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShenwuAppliedDamage(frame);
    internal bool ShenwuCannotTurnOver(global::CardGame.Core.CharacterState target) => _resolve().ShenwuCannotTurnOver(target);
    internal bool ShenwuEnabled(global::CardGame.Core.CharacterState player, string skillId) => _resolve().ShenwuEnabled(player, skillId);
    internal bool ShenwuFutaiCardUseForbidden(global::CardGame.Core.ProgramCardUseQuery query) => _resolve().ShenwuFutaiCardUseForbidden(query);
    internal bool ShenwuFutaiForbidsPeach(int actorSeat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => _resolve().ShenwuFutaiForbidsPeach(actorSeat, kind, type);
    internal bool ShenwuMingwanDirectedTargetProhibited(global::CardGame.Core.ProgramDirectedTargetQuery query) => _resolve().ShenwuMingwanDirectedTargetProhibited(query);
    internal bool ShenwuMingwanForbidsTarget(int actorSeat, int targetSeat) => _resolve().ShenwuMingwanForbidsTarget(actorSeat, targetSeat);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShenwuMingwanVictims(int ownerSeat) => _resolve().ShenwuMingwanVictims(ownerSeat);
    internal int ShenwuNpcDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShenwuNpcDamageBonus(attack);
    internal bool ShenwuOwnActualTurn(int seat) => _resolve().ShenwuOwnActualTurn(seat);
}
