#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouHuangYueyingCapability(Func<RuntimeEntries.CharacterHuangYueyingOlMouEntries> getCharacterHuangYueyingOlMouRuntime) : IMouHuangYueyingHost
{
    public SkillProgramStepOutcome ResolveMouHuangYueyingLixian(ProgramSkillFrame frame) => getCharacterHuangYueyingOlMouRuntime().BeginMouLixian(frame);
    public SkillProgramStepOutcome ResolveMouHuangYueyingBingcai(ProgramSkillFrame frame) => getCharacterHuangYueyingOlMouRuntime().BeginMouBingcai(frame);
}
