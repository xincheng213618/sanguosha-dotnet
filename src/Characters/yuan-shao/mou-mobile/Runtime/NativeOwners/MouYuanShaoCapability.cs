#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouYuanShaoCapability(Func<RuntimeEntries.CharacterYuanShaoMouMobileEntries> getCharacterYuanShaoMouMobileRuntime) : IMouYuanShaoHost
{
    public SkillProgramStepOutcome Reserve(ProgramSkillFrame frame) => getCharacterYuanShaoMouMobileRuntime().ReserveMouYuanShaoReward(frame);
}
