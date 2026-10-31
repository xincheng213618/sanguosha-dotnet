#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LvKuangLvXiangProgramCapability(Func<RuntimeEntries.CharacterLvKuangLvXiangOlEntries> getCharacterLvKuangLvXiangOlRuntime) : ILvKuangLvXiangProgramHost
{
    public SkillProgramStepOutcome ExecuteLvKuangLvXiang(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLvKuangLvXiangOlRuntime().ExecuteLvKuangLvXiang(f, op);
}
