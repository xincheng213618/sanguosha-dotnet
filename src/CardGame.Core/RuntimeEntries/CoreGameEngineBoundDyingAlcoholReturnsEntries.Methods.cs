#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundDyingAlcoholReturnsEntries
{
    internal void IssueBoundDyingAlcoholReturn(long useId, ProgramSkillFrame program, DyingFrame dying, Card card, CardLocation from, CardMoveReason reason) => _resolve().IssueBoundDyingAlcoholReturn(useId, program, dying, card, from, reason);
    internal void RecordBoundDyingAlcoholPayment(long useId, int recovered, long? recoveryAttemptId = default) => _resolve().RecordBoundDyingAlcoholPayment(useId, recovered, recoveryAttemptId);
    internal void RequireBoundDyingAlcoholUse(CardUseFrame use, bool requirePayment, bool requireFinished) => _resolve().RequireBoundDyingAlcoholUse(use, requirePayment, requireFinished);
    internal bool ReturnBoundDyingAlcoholUse(CardUseFrame use) => _resolve().ReturnBoundDyingAlcoholUse(use);
    internal void ValidateBoundDyingAlcoholReturn(CardUseFrame use) => _resolve().ValidateBoundDyingAlcoholReturn(use);
}
