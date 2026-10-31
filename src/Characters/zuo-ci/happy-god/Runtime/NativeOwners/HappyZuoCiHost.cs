#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyZuoCiHost(Func<RuntimeEntries.CharacterZuoCiHappyGodEntries> getCharacterZuoCiHappyGodRuntime) : IHappyZuoCiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZuoCiHappyGodRuntime().BeginHappyZuoCi(f, op);
}
