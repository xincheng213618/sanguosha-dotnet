#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ConditionalDiscardDuelProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineConditionalDiscardDuelsEntries> getCoreGameEngineConditionalDiscardDuelsRuntime) : IConditionalDiscardDuelProgramHost
{
    public SkillProgramStepOutcome DiscardSlashThenOtherCardAndUseDuel(ProgramSkillFrame frame) => getCoreGameEngineConditionalDiscardDuelsRuntime().BeginConditionalDiscardDuel(frame);
}
