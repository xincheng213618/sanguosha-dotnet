#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhenJiHost(Func<RuntimeEntries.CharacterZhenJiHappyGodEntries> getCharacterZhenJiHappyGodRuntime) : IHappyZhenJiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhenJiHappyGodRuntime().BeginHappyZj(f, op);
}
