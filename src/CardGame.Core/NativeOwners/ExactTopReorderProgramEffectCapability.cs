#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ExactTopReorderProgramEffectCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramTopReorderEntries> getCoreGameEngineProgramTopReorderRuntime) : IExactTopReorderProgramEffectHost
{
    public SkillProgramStepOutcome ReorderTopCardsExactly(ProgramSkillFrame frame, int maximumCards, SkillProgramNumberExpression? numberExpression, int exactTopCount) => getCoreGameEngineProgramTopReorderRuntime().BeginProgramTopReorder(frame, maximumCards, numberExpression, exactTopCount);
}
