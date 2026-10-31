#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheLuanxinProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramShanheLuanxinEntries> getCoreProgramShanheLuanxinRuntime) : IShanheLuanxinProgramHost
{
    public void QueueNextDraw(ProgramSkillFrame frame) => getCoreProgramShanheLuanxinRuntime().QueueShanheLuanxinNextDraw(frame);
}
