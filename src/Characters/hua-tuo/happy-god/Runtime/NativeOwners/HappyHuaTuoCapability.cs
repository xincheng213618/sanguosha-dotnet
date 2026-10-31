#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyHuaTuoCapability(Func<RuntimeEntries.CharacterHuaTuoHappyGodEntries> getCharacterHuaTuoHappyGodRuntime) : IHappyHuaTuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuaTuoHappyGodRuntime().ResolveHappyHuaTuo(f, op);
}
