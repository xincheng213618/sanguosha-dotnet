#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheChanrangCapability(Func<RuntimeEntries.CoreGameEngineShanheChanrangEdictEntries> getCoreGameEngineShanheChanrangEdictRuntime) : IShanheChanrangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCoreGameEngineShanheChanrangEdictRuntime().BeginShanheChanrang(frame);
}
