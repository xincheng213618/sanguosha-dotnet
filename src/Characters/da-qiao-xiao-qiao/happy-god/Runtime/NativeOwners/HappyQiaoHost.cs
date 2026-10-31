#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyQiaoHost(Func<RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries> getCharacterDaQiaoXiaoQiaoHappyGodRuntime) : IHappyQiaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterDaQiaoXiaoQiaoHappyGodRuntime().BeginHappyQiaoSkill(frame, operation);
}
