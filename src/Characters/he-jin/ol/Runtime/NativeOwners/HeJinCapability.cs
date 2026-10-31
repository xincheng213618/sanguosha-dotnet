#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HeJinCapability(Func<RuntimeEntries.CharacterHeJinOlEntries> getCharacterHeJinOlRuntime) : IHeJinProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouzhuHandDemandAndAssault ? getCharacterHeJinOlRuntime().BeginHeJinMouzhu(f) : getCharacterHeJinOlRuntime().BeginHeJinYanhuo(f);
}
