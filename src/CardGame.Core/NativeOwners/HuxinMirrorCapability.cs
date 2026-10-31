#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HuxinMirrorCapability(Func<RuntimeEntries.CoreGameEngineHuxinMirrorEntries> getCoreGameEngineHuxinMirrorRuntime) : IHuxinMirrorHost
{
    public SkillProgramStepOutcome PayAndPrevent(ProgramSkillFrame frame) => getCoreGameEngineHuxinMirrorRuntime().PayHuxinMirrorAndPrevent(frame);
}
