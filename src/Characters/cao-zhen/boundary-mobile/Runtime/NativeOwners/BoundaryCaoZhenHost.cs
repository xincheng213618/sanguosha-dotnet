#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoZhenHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoZhenBoundaryMobileEntries> getCharacterCaoZhenBoundaryMobileRuntime) : IBoundaryCaoZhenHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterCaoZhenBoundaryMobileRuntime().BeginBoundarySidi(frame, effect);
}
