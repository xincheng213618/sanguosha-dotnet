#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PrepDiscardEndingProgramCapability(Func<RuntimeEntries.CoreGameEnginePrepDiscardPaymentsEntries> getCoreGameEnginePrepDiscardPaymentsRuntime, Func<RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries> getCoreGameEnginePrepDiscardEndingPromisesRuntime) : IPrepDiscardEndingProgramHost
{
    public SkillProgramStepOutcome ResolvePrepDiscardOrEnding(ProgramSkillFrame f, string binding) => getCoreGameEnginePrepDiscardPaymentsRuntime().ResolvePrepDiscardOrEnding(f, binding);
    public SkillProgramStepOutcome ResolvePrepDiscardOrImmediateDraw(ProgramSkillFrame f) => getCoreGameEnginePrepDiscardPaymentsRuntime().ResolvePrepDiscardOrEnding(f, "");
    public SkillProgramStepOutcome DrawPrepDiscardEnding(ProgramSkillFrame f) => getCoreGameEnginePrepDiscardEndingPromisesRuntime().DrawPrepDiscardEnding(f);
}
