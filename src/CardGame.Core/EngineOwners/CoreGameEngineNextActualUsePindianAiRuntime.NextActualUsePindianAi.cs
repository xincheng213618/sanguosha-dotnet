#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNextActualUsePindianAiRuntime
{
    internal bool HasNextActualUsePindianStructure(SkillProgramActivation activation) => NextActualUsePindianSelectionAi.HasStructure(activation, global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory);
}
