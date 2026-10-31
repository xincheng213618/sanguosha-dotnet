#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class CaoShuangProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoShuangOlEntries> getCharacterCaoShuangOlRuntime) : ICaoShuangProgramHost
{
    public SkillProgramStepOutcome InheritDeadGeneralChosenSkill(ProgramSkillFrame f) => getCharacterCaoShuangOlRuntime().BeginCaoShuangTuogu(f);
    public SkillProgramStepOutcome PlaceDamagedCardAsDelayedTrick(ProgramSkillFrame f) => getCharacterCaoShuangOlRuntime().BeginCaoShuangShanzhuan(f);
}
