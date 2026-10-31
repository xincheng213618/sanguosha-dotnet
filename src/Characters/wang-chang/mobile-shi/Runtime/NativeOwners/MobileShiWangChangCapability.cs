#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiWangChangCapability(Func<RuntimeEntries.CharacterWangChangMobileShiEntries> getCharacterWangChangMobileShiRuntime) : IMobileShiWangChangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterWangChangMobileShiRuntime().ResolveShiWangChang(frame, op);
}
