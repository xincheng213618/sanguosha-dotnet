#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class KanZeProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterKanZeOlEntries> getCharacterKanZeOlRuntime) : IKanZeProgramHost
{
    public SkillProgramStepOutcome BeginXiashu(ProgramSkillFrame f, int target) => getCharacterKanZeOlRuntime().BeginXiashu(f, target);
    public SkillProgramStepOutcome ArmKuanshi(ProgramSkillFrame f, int target) => getCharacterKanZeOlRuntime().ArmKuanshi(f, target);
}
