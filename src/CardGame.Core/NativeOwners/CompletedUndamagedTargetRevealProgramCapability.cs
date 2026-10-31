#nullable enable
using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class CompletedUndamagedTargetRevealProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUndamagedTargetRevealEntries> getCoreGameEngineCompletedUndamagedTargetRevealRuntime) : ICompletedUndamagedTargetRevealProgramHost
{
    public SkillProgramStepOutcome CompletedUndamagedTargetReveal(ProgramSkillFrame f) => getCoreGameEngineCompletedUndamagedTargetRevealRuntime().BeginCompletedUndamagedTargetReveal(f);
}
