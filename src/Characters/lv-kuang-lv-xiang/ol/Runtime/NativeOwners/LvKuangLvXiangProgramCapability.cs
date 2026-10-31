#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LvKuangLvXiangProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLvKuangLvXiangOlEntries> getCharacterLvKuangLvXiangOlRuntime) : ILvKuangLvXiangProgramHost
{
    public SkillProgramStepOutcome ExecuteLvKuangLvXiang(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLvKuangLvXiangOlRuntime().ExecuteLvKuangLvXiang(f, op);
}
