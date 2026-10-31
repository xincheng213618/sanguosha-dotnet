#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ZhaoguMirrorCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineZhaoguMirrorRuntime>> getCoreGameEngineZhaoguMirrorRuntime) : IZhaoguMirrorHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCoreGameEngineZhaoguMirrorRuntime().Value.BeginZhaoguMirror(frame);
}
