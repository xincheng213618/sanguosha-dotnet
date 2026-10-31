#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundarySunLuBanHost(Func<RuntimeEntries.CharacterSunLuBanBoundaryMobileEntries> getCharacterSunLuBanBoundaryMobileRuntime) : IBoundarySunLuBanHost
{
    public SkillProgramStepOutcome Choose(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterSunLuBanBoundaryMobileRuntime().BeginBoundarySunLuBanChoice(frame, effect);
}
