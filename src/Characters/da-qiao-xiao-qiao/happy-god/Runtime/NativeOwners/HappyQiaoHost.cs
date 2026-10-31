#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyQiaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries> getCharacterDaQiaoXiaoQiaoHappyGodRuntime) : IHappyQiaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterDaQiaoXiaoQiaoHappyGodRuntime().BeginHappyQiaoSkill(frame, operation);
}
