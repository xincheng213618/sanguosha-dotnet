#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundDyingAlcoholReturnsEntries
{
    internal void IssueBoundDyingAlcoholReturn(long useId, global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.DyingFrame dying, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason) => _resolve().IssueBoundDyingAlcoholReturn(useId, program, dying, card, from, reason);
    internal void RecordBoundDyingAlcoholPayment(long useId, int recovered, long? recoveryAttemptId = default) => _resolve().RecordBoundDyingAlcoholPayment(useId, recovered, recoveryAttemptId);
    internal void RequireBoundDyingAlcoholUse(global::CardGame.Core.CardUseFrame use, bool requirePayment, bool requireFinished) => _resolve().RequireBoundDyingAlcoholUse(use, requirePayment, requireFinished);
    internal bool ReturnBoundDyingAlcoholUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnBoundDyingAlcoholUse(use);
    internal void ValidateBoundDyingAlcoholReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateBoundDyingAlcoholReturn(use);
}
