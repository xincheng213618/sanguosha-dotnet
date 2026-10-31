#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DyingSuitsAndEndingHistoryCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsPeachProgramsEntries> getCoreGameEngineDyingSuitsPeachProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEndingHistoricalUsesEntries> getCoreGameEngineEndingHistoricalUsesRuntime) : IDyingSuitsAndEndingHistoryHost
{
    public SkillProgramStepOutcome DrawDiscardSuits(ProgramSkillFrame f) => getCoreGameEngineDyingSuitsPeachProgramsRuntime().BeginDyingSuits(f);
    public SkillProgramStepOutcome EndingHistoricalUses(ProgramSkillFrame f) => getCoreGameEngineEndingHistoricalUsesRuntime().BeginEndingHistoricalUses(f);
}
