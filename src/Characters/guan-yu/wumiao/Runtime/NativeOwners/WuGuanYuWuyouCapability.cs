#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WuGuanYuWuyouCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries> getCharacterGuanYuWumiaoRuntime) : IWuGuanYuWuyouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterGuanYuWumiaoRuntime().BeginWuyou(frame);
}
