#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DyingSuitsAndEndingHistoryCapability(Func<RuntimeEntries.CoreGameEngineDyingSuitsPeachProgramsEntries> getCoreGameEngineDyingSuitsPeachProgramsRuntime, Func<RuntimeEntries.CoreGameEngineEndingHistoricalUsesEntries> getCoreGameEngineEndingHistoricalUsesRuntime) : IDyingSuitsAndEndingHistoryHost
{
    public SkillProgramStepOutcome DrawDiscardSuits(ProgramSkillFrame f) => getCoreGameEngineDyingSuitsPeachProgramsRuntime().BeginDyingSuits(f);
    public SkillProgramStepOutcome EndingHistoricalUses(ProgramSkillFrame f) => getCoreGameEngineEndingHistoricalUsesRuntime().BeginEndingHistoricalUses(f);
}
