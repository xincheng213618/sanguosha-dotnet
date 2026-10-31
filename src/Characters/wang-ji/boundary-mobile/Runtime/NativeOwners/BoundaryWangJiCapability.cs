#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryWangJiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWangJiBoundaryMobileRuntime>> getCharacterWangJiBoundaryMobileRuntime) : IBoundaryWangJiHost, IBoundaryWangJiSelection
{
    public bool CanSpend(int owner, int target) => getCharacterWangJiBoundaryMobileRuntime().Value.CanSpendBoundaryWj(owner, target);
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterWangJiBoundaryMobileRuntime().Value.BeginBoundaryWj(frame, operation);
}
