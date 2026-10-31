#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ConfiguredConversionProgramCapability(Func<RuntimeEntries.CoreGameEngineConfiguredConversionsEntries> getCoreGameEngineConfiguredConversionsRuntime, Func<RuntimeEntries.CoreGameEngineProgramSkillChangesEntries> getCoreGameEngineProgramSkillChangesRuntime) : IConfiguredConversionProgramHost
{
    public SkillProgramStepOutcome DeclareBoundCardName(ProgramSkillFrame frame, string sourceBind, string stateId) => getCoreGameEngineConfiguredConversionsRuntime().BeginConfiguredCardDeclaration(frame, sourceBind, stateId);
    public void UpgradeConversionTier(ProgramSkillFrame frame, string stateId) => getCoreGameEngineProgramSkillChangesRuntime().UpgradeConfiguredConversionTier(frame, stateId);
}
