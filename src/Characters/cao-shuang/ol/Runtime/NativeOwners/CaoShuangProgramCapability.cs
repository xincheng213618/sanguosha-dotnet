#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CaoShuangProgramCapability(Func<RuntimeEntries.CharacterCaoShuangOlEntries> getCharacterCaoShuangOlRuntime) : ICaoShuangProgramHost
{
    public SkillProgramStepOutcome InheritDeadGeneralChosenSkill(ProgramSkillFrame f) => getCharacterCaoShuangOlRuntime().BeginCaoShuangTuogu(f);
    public SkillProgramStepOutcome PlaceDamagedCardAsDelayedTrick(ProgramSkillFrame f) => getCharacterCaoShuangOlRuntime().BeginCaoShuangShanzhuan(f);
}
