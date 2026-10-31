#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryWuYiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWuYiBoundaryMobileRuntime>> getCharacterWuYiBoundaryMobileRuntime) : IBoundaryWuYiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWuYiBoundaryMobileRuntime().Value.ExecuteBoundaryWuYi(f, op);
}
