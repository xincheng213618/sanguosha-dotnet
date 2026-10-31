#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNextActualUsePindianAiRuntime
{
    internal bool HasNextActualUsePindianStructure(SkillProgramActivation activation) => NextActualUsePindianSelectionAi.HasStructure(activation, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory);
}
