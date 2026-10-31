#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheDoubleStealHost(Func<RuntimeEntries.CoreGameEngineShanheDoubleStealEntries> getCoreGameEngineShanheDoubleStealRuntime) : IShanheDoubleStealHost
{
    public void RepeatCurrentSnatch(ProgramSkillFrame frame) => getCoreGameEngineShanheDoubleStealRuntime().IssueShanheDoubleSteal(frame);
}
