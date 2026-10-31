#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryWuYiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWuYiBoundaryMobileEntries> getCharacterWuYiBoundaryMobileRuntime) : IBoundaryWuYiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWuYiBoundaryMobileRuntime().ExecuteBoundaryWuYi(f, op);
}
