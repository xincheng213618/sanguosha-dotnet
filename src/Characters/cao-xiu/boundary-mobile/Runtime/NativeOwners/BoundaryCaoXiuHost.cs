#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoXiuHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoXiuBoundaryMobileRuntime>> getCharacterCaoXiuBoundaryMobileRuntime) : IBoundaryCaoXiuHost
{
    public SkillProgramStepOutcome BeginQingxi(ProgramSkillFrame frame) => getCharacterCaoXiuBoundaryMobileRuntime().Value.BeginBoundaryCaoXiuQingxi(frame);
}
