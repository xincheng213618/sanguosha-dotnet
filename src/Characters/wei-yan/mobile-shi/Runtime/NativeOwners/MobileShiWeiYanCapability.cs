#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiWeiYanCapability(Func<RuntimeEntries.CharacterWeiYanMobileShiEntries> getCharacterWeiYanMobileShiRuntime) : IMobileShiWeiYanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterWeiYanMobileShiRuntime().BeginMobileShiWeiYan(frame, operation);
}
