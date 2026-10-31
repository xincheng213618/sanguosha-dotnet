#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouYuanShaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYuanShaoMouMobileEntries> getCharacterYuanShaoMouMobileRuntime) : IMouYuanShaoHost
{
    public SkillProgramStepOutcome Reserve(ProgramSkillFrame frame) => getCharacterYuanShaoMouMobileRuntime().ReserveMouYuanShaoReward(frame);
}
