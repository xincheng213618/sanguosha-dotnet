#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheDoubleStealHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheDoubleStealRuntime>> getCoreGameEngineShanheDoubleStealRuntime) : IShanheDoubleStealHost
{
    public void RepeatCurrentSnatch(ProgramSkillFrame frame) => getCoreGameEngineShanheDoubleStealRuntime().Value.IssueShanheDoubleSteal(frame);
}
