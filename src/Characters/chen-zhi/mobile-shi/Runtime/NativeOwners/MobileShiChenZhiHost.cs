#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiChenZhiHost(Func<RuntimeEntries.CharacterChenZhiMobileShiEntries> getCharacterChenZhiMobileShiRuntime) : IMobileShiChenZhiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterChenZhiMobileShiRuntime().BeginShiCz(frame, operation);
}
