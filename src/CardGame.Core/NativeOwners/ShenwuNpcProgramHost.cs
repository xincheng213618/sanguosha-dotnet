#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenwuNpcProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreShenwuNpcProgramsEntries> getCoreShenwuNpcProgramsRuntime) : IShenwuNpcProgramHost
{
    public SkillProgramStepOutcome ExecuteShenwuNpc(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreShenwuNpcProgramsRuntime().ExecuteShenwuNpc(frame, effect);
}
