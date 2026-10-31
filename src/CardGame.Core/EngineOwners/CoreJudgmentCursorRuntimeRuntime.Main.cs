#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreJudgmentCursorRuntimeRuntime
{
    internal JudgmentFrame? ActiveJudgment => _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().LastOrDefault(frame => frame.CardAttack is null);
}
