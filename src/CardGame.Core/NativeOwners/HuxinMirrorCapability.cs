#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HuxinMirrorCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHuxinMirrorEntries> getCoreGameEngineHuxinMirrorRuntime) : IHuxinMirrorHost
{
    public SkillProgramStepOutcome PayAndPrevent(ProgramSkillFrame frame) => getCoreGameEngineHuxinMirrorRuntime().PayHuxinMirrorAndPrevent(frame);
}
