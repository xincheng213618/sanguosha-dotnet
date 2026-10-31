#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class KanZeProgramCapability(Func<RuntimeEntries.CharacterKanZeOlEntries> getCharacterKanZeOlRuntime) : IKanZeProgramHost
{
    public SkillProgramStepOutcome BeginXiashu(ProgramSkillFrame f, int target) => getCharacterKanZeOlRuntime().BeginXiashu(f, target);
    public SkillProgramStepOutcome ArmKuanshi(ProgramSkillFrame f, int target) => getCharacterKanZeOlRuntime().ArmKuanshi(f, target);
}
