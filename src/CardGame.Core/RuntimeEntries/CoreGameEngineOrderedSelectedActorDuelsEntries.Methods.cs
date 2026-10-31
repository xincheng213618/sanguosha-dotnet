#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOrderedSelectedActorDuelsEntries
{
    internal void AssertOrderedSelectedActorDuelPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertOrderedSelectedActorDuelPayment(frame);
    internal void AssertOrderedSelectedActorDuelUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin) => _resolve().AssertOrderedSelectedActorDuelUse(use, parent, origin);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOrderedSelectedActorDuel(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginOrderedSelectedActorDuel(frame);
    internal bool CanContinuePaidOrderedSelectedActorDuel(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinuePaidOrderedSelectedActorDuel(frame);
    internal void CompleteOrderedSelectedActorDuel(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin, long useId) => _resolve().CompleteOrderedSelectedActorDuel(frame, origin, useId);
    internal bool HasExactOrderedSelectedActorDuelPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasExactOrderedSelectedActorDuelPayment(frame);
    internal bool IsOrderedSelectedActorDuelCannotNullify(long useId) => _resolve().IsOrderedSelectedActorDuelCannotNullify(useId);
    internal bool IsOrderedSelectedActorDuelProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsOrderedSelectedActorDuelProgram(frame);
    internal bool MatchesOrderedSelectedActorDuel(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin, long useId) => _resolve().MatchesOrderedSelectedActorDuel(frame, origin, useId);
    internal bool ReturnOrderedSelectedActorDuelAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnOrderedSelectedActorDuelAttack(completion);
    internal bool ReturnOrderedSelectedActorDuelPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnOrderedSelectedActorDuelPayment(frame);
    internal bool TryPayOrderedSelectedActorDuel(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardMoveReason reason) => _resolve().TryPayOrderedSelectedActorDuel(frame, ids, reason);
}
