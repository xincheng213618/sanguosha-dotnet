#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoZhenHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoZhenBoundaryMobileRuntime>> getCharacterCaoZhenBoundaryMobileRuntime) : IBoundaryCaoZhenHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterCaoZhenBoundaryMobileRuntime().Value.BeginBoundarySidi(frame, effect);
}
