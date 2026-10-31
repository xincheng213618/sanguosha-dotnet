#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyGuoJiaHost(Func<RuntimeEntries.CharacterGuoJiaHappyGodEntries> getCharacterGuoJiaHappyGodRuntime) : IHappyGuoJiaHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoJiaHappyGodRuntime().BeginHappyGuoJia(f, op);
}
