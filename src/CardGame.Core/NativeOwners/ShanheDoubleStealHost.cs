#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheDoubleStealHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheDoubleStealEntries> getCoreGameEngineShanheDoubleStealRuntime) : IShanheDoubleStealHost
{
    public void RepeatCurrentSnatch(ProgramSkillFrame frame) => getCoreGameEngineShanheDoubleStealRuntime().IssueShanheDoubleSteal(frame);
}
