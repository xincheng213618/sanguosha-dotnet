#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundarySunLuBanHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunLuBanBoundaryMobileRuntime>> getCharacterSunLuBanBoundaryMobileRuntime) : IBoundarySunLuBanHost
{
    public SkillProgramStepOutcome Choose(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterSunLuBanBoundaryMobileRuntime().Value.BeginBoundarySunLuBanChoice(frame, effect);
}
