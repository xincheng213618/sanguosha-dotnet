#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeZhugeGuoCapability(Func<RuntimeEntries.CharacterZhugeGuoLeEntries> getCharacterZhugeGuoLeRuntime) : ILeZhugeGuoHost
{
    public SkillProgramStepOutcome Initialize(ProgramSkillFrame f) => getCharacterZhugeGuoLeRuntime().InitializeLeXidi(f);
    public SkillProgramStepOutcome Reorder(ProgramSkillFrame f) => getCharacterZhugeGuoLeRuntime().ReorderLeXidi(f);
    public SkillProgramStepOutcome Chengyan(ProgramSkillFrame f) => getCharacterZhugeGuoLeRuntime().StartLeChengyan(f);
}
