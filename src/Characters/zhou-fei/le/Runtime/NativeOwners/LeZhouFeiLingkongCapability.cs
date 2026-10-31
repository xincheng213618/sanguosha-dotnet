#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeZhouFeiLingkongCapability(Func<RuntimeEntries.CharacterZhouFeiLeEntries> getCharacterZhouFeiLeRuntime) : ILeZhouFeiLingkongHost
{
    public SkillProgramStepOutcome InitializeLingkong(ProgramSkillFrame frame) => getCharacterZhouFeiLeRuntime().InitializeLingkong(frame);
    public SkillProgramStepOutcome MarkLingkongGain(ProgramSkillFrame frame) => getCharacterZhouFeiLeRuntime().MarkLingkongGain(frame);
}
