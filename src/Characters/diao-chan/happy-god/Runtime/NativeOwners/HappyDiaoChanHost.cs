#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyDiaoChanHost(Func<RuntimeEntries.CharacterDiaoChanHappyGodEntries> getCharacterDiaoChanHappyGodRuntime) : IHappyDiaoChanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDiaoChanHappyGodRuntime().BeginHappyDc(f, op);
}
