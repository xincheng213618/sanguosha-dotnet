#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryWangJiCapability(Func<RuntimeEntries.CharacterWangJiBoundaryMobileEntries> getCharacterWangJiBoundaryMobileRuntime) : IBoundaryWangJiHost, IBoundaryWangJiSelection
{
    public bool CanSpend(int owner, int target) => getCharacterWangJiBoundaryMobileRuntime().CanSpendBoundaryWj(owner, target);
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterWangJiBoundaryMobileRuntime().BeginBoundaryWj(frame, operation);
}
