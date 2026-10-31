#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class LeDiaoChanDiouCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries> getCharacterDiaoChanLeRuntime) : ILeDiaoChanDiouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterDiaoChanLeRuntime().BeginDiou(frame);
}
