#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryGaoShunHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGaoShunBoundaryMobileEntries> getCharacterGaoShunBoundaryMobileRuntime) : IBoundaryGaoShunHost
{
    public void Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGaoShunBoundaryMobileRuntime().ExecuteBoundaryGaoShun(f, op);
}
