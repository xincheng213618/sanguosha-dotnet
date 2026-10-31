#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DyingSuitsAndEndingHistoryCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime>> getCoreGameEngineDyingSuitsPeachProgramsRuntime, Func<RuntimeEntries.CoreGameEngineEndingHistoricalUsesEntries> getCoreGameEngineEndingHistoricalUsesRuntime) : IDyingSuitsAndEndingHistoryHost
{
    public SkillProgramStepOutcome DrawDiscardSuits(ProgramSkillFrame f) => getCoreGameEngineDyingSuitsPeachProgramsRuntime().Value.BeginDyingSuits(f);
    public SkillProgramStepOutcome EndingHistoricalUses(ProgramSkillFrame f) => getCoreGameEngineEndingHistoricalUsesRuntime().BeginEndingHistoricalUses(f);
}
