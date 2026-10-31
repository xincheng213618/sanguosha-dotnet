#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShiSunChenHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunChenMobileShiRuntime>> getCharacterSunChenMobileShiRuntime) : IShiSunChenHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunChenMobileShiRuntime().Value.BeginShiSc(f, op);
}
