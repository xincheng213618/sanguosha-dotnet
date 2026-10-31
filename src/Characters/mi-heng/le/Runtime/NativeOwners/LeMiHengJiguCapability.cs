#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeMiHengJiguCapability(Func<RuntimeEntries.CharacterMiHengLeEntries> getCharacterMiHengLeRuntime) : ILeMiHengJiguHost
{
    public SkillProgramStepOutcome Initialize(ProgramSkillFrame frame) => getCharacterMiHengLeRuntime().InitializeJigu(frame);
    public SkillProgramStepOutcome Draw(ProgramSkillFrame frame) => getCharacterMiHengLeRuntime().BeginJiguDraw(frame);
}
