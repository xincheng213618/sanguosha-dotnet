#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeZhouYuGuYinCapability(Func<RuntimeEntries.CharacterZhouYuLeEntries> getCharacterZhouYuLeRuntime) : ILeZhouYuGuYinHost
{
    public SkillProgramStepOutcome Initialize(ProgramSkillFrame f) => getCharacterZhouYuLeRuntime().InitializeGuYin(f);
    public SkillProgramStepOutcome Draw(ProgramSkillFrame f) => getCharacterZhouYuLeRuntime().BeginGuYinDraw(f);
}
