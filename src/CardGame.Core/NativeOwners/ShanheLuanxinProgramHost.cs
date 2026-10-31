#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheLuanxinProgramHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreProgramShanheLuanxinRuntime>> getCoreProgramShanheLuanxinRuntime) : IShanheLuanxinProgramHost
{
    public void QueueNextDraw(ProgramSkillFrame frame) => getCoreProgramShanheLuanxinRuntime().Value.QueueShanheLuanxinNextDraw(frame);
}
