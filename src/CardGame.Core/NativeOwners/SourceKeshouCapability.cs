#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SourceKeshouCapability(Func<RuntimeEntries.CoreGameEngineShanheSourceKeshouEntries> getCoreGameEngineShanheSourceKeshouRuntime) : ISourceKeshouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, string step) => getCoreGameEngineShanheSourceKeshouRuntime().ResolveSourceKeshou(f, step);
}
