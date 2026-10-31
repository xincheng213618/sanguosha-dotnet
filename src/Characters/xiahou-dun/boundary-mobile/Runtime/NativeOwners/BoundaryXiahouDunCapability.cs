#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryXiahouDunCapability(Func<RuntimeEntries.CharacterXiahouDunBoundaryMobileEntries> getCharacterXiahouDunBoundaryMobileRuntime) : IBoundaryXiahouDunHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXiahouDunBoundaryMobileRuntime().BeginBoundaryXiahouDun(f, op);
}
