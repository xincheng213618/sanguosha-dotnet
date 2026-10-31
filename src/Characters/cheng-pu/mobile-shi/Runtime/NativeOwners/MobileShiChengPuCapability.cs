#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiChengPuCapability(Func<RuntimeEntries.CharacterChengPuMobileShiEntries> getCharacterChengPuMobileShiRuntime) : IMobileShiChengPuProgramHost
{
    public SkillProgramStepOutcome ResolveDuzuoGrant(ProgramSkillFrame frame) => getCharacterChengPuMobileShiRuntime().BeginMobileShiDuzuoGrantProgram(frame);
    public SkillProgramStepOutcome ResolveBihanGuard(ProgramSkillFrame frame) => getCharacterChengPuMobileShiRuntime().BeginMobileShiBihanGuardProgram(frame);
}
