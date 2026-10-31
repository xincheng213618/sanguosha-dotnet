#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LeDiaoChanDiouCapability(Func<RuntimeEntries.CharacterDiaoChanLeEntries> getCharacterDiaoChanLeRuntime) : ILeDiaoChanDiouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterDiaoChanLeRuntime().BeginDiou(frame);
}
