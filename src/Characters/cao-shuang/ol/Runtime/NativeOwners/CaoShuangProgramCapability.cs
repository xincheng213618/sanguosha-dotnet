#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CaoShuangProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoShuangOlRuntime>> getCharacterCaoShuangOlRuntime) : ICaoShuangProgramHost
{
    public SkillProgramStepOutcome InheritDeadGeneralChosenSkill(ProgramSkillFrame f) => getCharacterCaoShuangOlRuntime().Value.BeginCaoShuangTuogu(f);
    public SkillProgramStepOutcome PlaceDamagedCardAsDelayedTrick(ProgramSkillFrame f) => getCharacterCaoShuangOlRuntime().Value.BeginCaoShuangShanzhuan(f);
}
