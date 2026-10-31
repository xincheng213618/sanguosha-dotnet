#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOrderedSelectedActorDuelsEntries
{
    internal SkillProgramStepOutcome BeginOrderedSelectedActorDuel(ProgramSkillFrame frame) => _resolve().BeginOrderedSelectedActorDuel(frame);
    internal bool CanContinuePaidOrderedSelectedActorDuel(ProgramSkillFrame frame) => _resolve().CanContinuePaidOrderedSelectedActorDuel(frame);
    internal void CompleteOrderedSelectedActorDuel(ProgramSkillFrame frame, ProgramSelectedActorDuelOrigin origin, long useId) => _resolve().CompleteOrderedSelectedActorDuel(frame, origin, useId);
    internal bool HasExactOrderedSelectedActorDuelPayment(ProgramSkillFrame frame) => _resolve().HasExactOrderedSelectedActorDuelPayment(frame);
    internal bool IsOrderedSelectedActorDuelCannotNullify(long useId) => _resolve().IsOrderedSelectedActorDuelCannotNullify(useId);
    internal bool IsOrderedSelectedActorDuelProgram(ProgramSkillFrame frame) => _resolve().IsOrderedSelectedActorDuelProgram(frame);
    internal bool MatchesOrderedSelectedActorDuel(ProgramSkillFrame frame, ProgramSelectedActorDuelOrigin origin, long useId) => _resolve().MatchesOrderedSelectedActorDuel(frame, origin, useId);
    internal bool ReturnOrderedSelectedActorDuelAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnOrderedSelectedActorDuelAttack(completion);
    internal bool ReturnOrderedSelectedActorDuelPayment(ProgramSkillFrame frame) => _resolve().ReturnOrderedSelectedActorDuelPayment(frame);
    internal bool TryPayOrderedSelectedActorDuel(ProgramSkillFrame frame, IReadOnlyList<int> ids, CardMoveReason reason) => _resolve().TryPayOrderedSelectedActorDuel(frame, ids, reason);
}
