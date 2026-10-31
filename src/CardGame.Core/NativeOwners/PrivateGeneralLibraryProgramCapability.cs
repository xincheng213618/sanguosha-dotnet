#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PrivateGeneralLibraryProgramCapability(Func<RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries> getCoreGameEnginePrivateGeneralLibrariesRuntime) : IPrivateGeneralLibraryProgramHost
{
    public SkillProgramStepOutcome ExecutePrivateGeneralLibrary(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEnginePrivateGeneralLibrariesRuntime().ExecutePrivateGeneralLibrary(e, f);
}
