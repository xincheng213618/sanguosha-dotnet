#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheChanrangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheChanrangEdictEntries> getCoreGameEngineShanheChanrangEdictRuntime) : IShanheChanrangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCoreGameEngineShanheChanrangEdictRuntime().BeginShanheChanrang(frame);
}
