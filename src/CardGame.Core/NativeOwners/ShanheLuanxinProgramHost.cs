#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheLuanxinProgramHost(Func<RuntimeEntries.CoreProgramShanheLuanxinEntries> getCoreProgramShanheLuanxinRuntime) : IShanheLuanxinProgramHost
{
    public void QueueNextDraw(ProgramSkillFrame frame) => getCoreProgramShanheLuanxinRuntime().QueueShanheLuanxinNextDraw(frame);
}
