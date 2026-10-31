#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiChenDaoHost(Func<RuntimeEntries.CharacterChenDaoMobileShiEntries> getCharacterChenDaoMobileShiRuntime) : IMobileShiChenDaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterChenDaoMobileShiRuntime().ResolveShiCd(f, op);
}
