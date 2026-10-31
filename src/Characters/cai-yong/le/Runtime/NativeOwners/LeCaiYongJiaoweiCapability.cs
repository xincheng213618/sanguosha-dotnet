#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeCaiYongJiaoweiCapability(Func<RuntimeEntries.CharacterCaiYongLeEntries> getCharacterCaiYongLeRuntime) : ILeCaiYongJiaoweiHost
{
    public SkillProgramStepOutcome Initialize(ProgramSkillFrame frame) => getCharacterCaiYongLeRuntime().InitializeJiaowei(frame);
}
