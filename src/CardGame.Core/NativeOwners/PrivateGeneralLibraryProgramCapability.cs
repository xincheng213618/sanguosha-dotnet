#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PrivateGeneralLibraryProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries> getCoreGameEnginePrivateGeneralLibrariesRuntime) : IPrivateGeneralLibraryProgramHost
{
    public SkillProgramStepOutcome ExecutePrivateGeneralLibrary(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEnginePrivateGeneralLibrariesRuntime().ExecutePrivateGeneralLibrary(e, f);
}
