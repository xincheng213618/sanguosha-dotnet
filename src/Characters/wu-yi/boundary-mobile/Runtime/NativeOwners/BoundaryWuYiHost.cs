#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryWuYiHost(Func<RuntimeEntries.CharacterWuYiBoundaryMobileEntries> getCharacterWuYiBoundaryMobileRuntime) : IBoundaryWuYiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWuYiBoundaryMobileRuntime().ExecuteBoundaryWuYi(f, op);
}
