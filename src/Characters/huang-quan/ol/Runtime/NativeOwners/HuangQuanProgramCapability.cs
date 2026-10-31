#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class HuangQuanProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangQuanOlEntries> getCharacterHuangQuanOlRuntime) : IHuangQuanProgramHost
{
    public SkillProgramStepOutcome ArmDianhuRelation(ProgramSkillFrame f, int target) => getCharacterHuangQuanOlRuntime().ArmDianhuRelation(f, target);
    public SkillProgramStepOutcome DrawForDianhuActualOccurrence(ProgramSkillFrame f) => getCharacterHuangQuanOlRuntime().DrawForDianhuActualOccurrence(f);
    public SkillProgramStepOutcome DrawAndOfferJianjiExactCard(ProgramSkillFrame f, int target) => getCharacterHuangQuanOlRuntime().DrawAndOfferJianjiExactCard(f, target);
}
