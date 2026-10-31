#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheXiashengHost(Func<RuntimeEntries.CoreGameEngineShanheXiashengEntries> getCoreGameEngineShanheXiashengRuntime) : IShanheXiashengHost
{
    public SkillProgramStepOutcome ResolveXiasheng(ProgramSkillFrame frame, string stage) => getCoreGameEngineShanheXiashengRuntime().BeginShanheXiasheng(frame, stage);
}
