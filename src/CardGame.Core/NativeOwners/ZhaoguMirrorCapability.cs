#nullable enable
using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ZhaoguMirrorCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineZhaoguMirrorEntries> getCoreGameEngineZhaoguMirrorRuntime) : IZhaoguMirrorHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCoreGameEngineZhaoguMirrorRuntime().BeginZhaoguMirror(frame);
}
