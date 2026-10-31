#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheXiashengHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheXiashengEntries> getCoreGameEngineShanheXiashengRuntime) : IShanheXiashengHost
{
    public SkillProgramStepOutcome ResolveXiasheng(ProgramSkillFrame frame, string stage) => getCoreGameEngineShanheXiashengRuntime().BeginShanheXiasheng(frame, stage);
}
