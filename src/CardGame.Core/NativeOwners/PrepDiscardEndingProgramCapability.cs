#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PrepDiscardEndingProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardPaymentsEntries> getCoreGameEnginePrepDiscardPaymentsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries> getCoreGameEnginePrepDiscardEndingPromisesRuntime) : IPrepDiscardEndingProgramHost
{
    public SkillProgramStepOutcome ResolvePrepDiscardOrEnding(ProgramSkillFrame f, string binding) => getCoreGameEnginePrepDiscardPaymentsRuntime().ResolvePrepDiscardOrEnding(f, binding);
    public SkillProgramStepOutcome ResolvePrepDiscardOrImmediateDraw(ProgramSkillFrame f) => getCoreGameEnginePrepDiscardPaymentsRuntime().ResolvePrepDiscardOrEnding(f, "");
    public SkillProgramStepOutcome DrawPrepDiscardEnding(ProgramSkillFrame f) => getCoreGameEnginePrepDiscardEndingPromisesRuntime().DrawPrepDiscardEnding(f);
}
