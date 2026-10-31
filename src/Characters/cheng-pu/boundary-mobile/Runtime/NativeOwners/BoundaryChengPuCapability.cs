#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryChengPuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterChengPuBoundaryMobileRuntime>> getCharacterChengPuBoundaryMobileRuntime) : IBoundaryChengPuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterChengPuBoundaryMobileRuntime().Value.BeginBoundaryChengPuLihuo(f);
}
