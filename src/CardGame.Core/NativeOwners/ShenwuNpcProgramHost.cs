#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenwuNpcProgramHost(Func<RuntimeEntries.CoreShenwuNpcProgramsEntries> getCoreShenwuNpcProgramsRuntime) : IShenwuNpcProgramHost
{
    public SkillProgramStepOutcome ExecuteShenwuNpc(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreShenwuNpcProgramsRuntime().ExecuteShenwuNpc(frame, effect);
}
