#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyLuBuHost(Func<RuntimeEntries.CharacterLuBuHappyGodEntries> getCharacterLuBuHappyGodRuntime) : IHappyLuBuHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuBuHappyGodRuntime().BeginHappyLuBu(f, op);
}
