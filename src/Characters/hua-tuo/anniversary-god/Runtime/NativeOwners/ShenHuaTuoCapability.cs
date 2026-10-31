#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenHuaTuoCapability(Func<RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries> getCharacterHuaTuoAnniversaryGodRuntime) : IShenHuaTuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuaTuoAnniversaryGodRuntime().BeginShenHuaTuo(f, op);
}
