#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyPangTongHost(Func<RuntimeEntries.CharacterPangTongHappyGodEntries> getCharacterPangTongHappyGodRuntime) : IHappyPangTongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPangTongHappyGodRuntime().BeginHappyPangTong(f, op);
}
