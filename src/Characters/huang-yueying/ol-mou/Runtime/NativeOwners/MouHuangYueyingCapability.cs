#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouHuangYueyingCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangYueyingOlMouEntries> getCharacterHuangYueyingOlMouRuntime) : IMouHuangYueyingHost
{
    public SkillProgramStepOutcome ResolveMouHuangYueyingLixian(ProgramSkillFrame frame) => getCharacterHuangYueyingOlMouRuntime().BeginMouLixian(frame);
    public SkillProgramStepOutcome ResolveMouHuangYueyingBingcai(ProgramSkillFrame frame) => getCharacterHuangYueyingOlMouRuntime().BeginMouBingcai(frame);
}
