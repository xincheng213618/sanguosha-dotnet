#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreJudgmentCursorRuntimeRuntime
{
    internal JudgmentFrame? ActiveJudgment => _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().LastOrDefault(frame => frame.CardAttack is null);
}
