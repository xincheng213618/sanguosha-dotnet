#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiChengPuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterChengPuMobileShiRuntime>> getCharacterChengPuMobileShiRuntime) : IMobileShiChengPuProgramHost
{
    public SkillProgramStepOutcome ResolveDuzuoGrant(ProgramSkillFrame frame) => getCharacterChengPuMobileShiRuntime().Value.BeginMobileShiDuzuoGrantProgram(frame);
    public SkillProgramStepOutcome ResolveBihanGuard(ProgramSkillFrame frame) => getCharacterChengPuMobileShiRuntime().Value.BeginMobileShiBihanGuardProgram(frame);
}
