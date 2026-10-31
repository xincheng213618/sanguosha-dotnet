#nullable enable
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace CardGame.Core.NativeOwners;
internal sealed class ConfiguredConversionProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineConfiguredConversionsEntries> getCoreGameEngineConfiguredConversionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries> getCoreGameEngineProgramSkillChangesRuntime) : IConfiguredConversionProgramHost
{
    public SkillProgramStepOutcome DeclareBoundCardName(ProgramSkillFrame frame, string sourceBind, string stateId) => getCoreGameEngineConfiguredConversionsRuntime().BeginConfiguredCardDeclaration(frame, sourceBind, stateId);
    public void UpgradeConversionTier(ProgramSkillFrame frame, string stateId) => getCoreGameEngineProgramSkillChangesRuntime().UpgradeConfiguredConversionTier(frame, stateId);
}
