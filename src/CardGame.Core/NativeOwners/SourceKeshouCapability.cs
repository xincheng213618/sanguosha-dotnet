#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SourceKeshouCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceKeshouRuntime>> getCoreGameEngineShanheSourceKeshouRuntime) : ISourceKeshouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, string step) => getCoreGameEngineShanheSourceKeshouRuntime().Value.ResolveSourceKeshou(f, step);
}
