#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheChanrangCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheChanrangEdictRuntime>> getCoreGameEngineShanheChanrangEdictRuntime) : IShanheChanrangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCoreGameEngineShanheChanrangEdictRuntime().Value.BeginShanheChanrang(frame);
}
