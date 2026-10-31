#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeCaiYongJiaoweiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaiYongLeEntries> getCharacterCaiYongLeRuntime) : ILeCaiYongJiaoweiHost
{
    public SkillProgramStepOutcome Initialize(ProgramSkillFrame frame) => getCharacterCaiYongLeRuntime().InitializeJiaowei(frame);
}
