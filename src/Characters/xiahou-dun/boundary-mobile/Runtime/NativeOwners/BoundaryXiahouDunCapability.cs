#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryXiahouDunCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXiahouDunBoundaryMobileRuntime>> getCharacterXiahouDunBoundaryMobileRuntime) : IBoundaryXiahouDunHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXiahouDunBoundaryMobileRuntime().Value.BeginBoundaryXiahouDun(f, op);
}
