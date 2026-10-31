#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeXiaoQiaoQiqinCapability(Func<RuntimeEntries.CharacterXiaoQiaoLeEntries> getCharacterXiaoQiaoLeRuntime) : ILeXiaoQiaoQiqinHost
{
    public SkillProgramStepOutcome Initialize(ProgramSkillFrame f) => getCharacterXiaoQiaoLeRuntime().InitializeQiqin(f);
    public SkillProgramStepOutcome Collect(ProgramSkillFrame f) => getCharacterXiaoQiaoLeRuntime().BeginQiqinCollect(f);
}
