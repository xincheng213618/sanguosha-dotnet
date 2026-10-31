#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiChenDaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChenDaoMobileShiEntries> getCharacterChenDaoMobileShiRuntime) : IMobileShiChenDaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterChenDaoMobileShiRuntime().ResolveShiCd(f, op);
}
