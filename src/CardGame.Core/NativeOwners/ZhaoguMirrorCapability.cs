#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ZhaoguMirrorCapability(Func<RuntimeEntries.CoreGameEngineZhaoguMirrorEntries> getCoreGameEngineZhaoguMirrorRuntime) : IZhaoguMirrorHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCoreGameEngineZhaoguMirrorRuntime().BeginZhaoguMirror(frame);
}
