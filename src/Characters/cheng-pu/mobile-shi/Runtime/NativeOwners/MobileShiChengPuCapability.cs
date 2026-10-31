#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiChengPuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChengPuMobileShiEntries> getCharacterChengPuMobileShiRuntime) : IMobileShiChengPuProgramHost
{
    public SkillProgramStepOutcome ResolveDuzuoGrant(ProgramSkillFrame frame) => getCharacterChengPuMobileShiRuntime().BeginMobileShiDuzuoGrantProgram(frame);
    public SkillProgramStepOutcome ResolveBihanGuard(ProgramSkillFrame frame) => getCharacterChengPuMobileShiRuntime().BeginMobileShiBihanGuardProgram(frame);
}
