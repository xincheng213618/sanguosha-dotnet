#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryGaoShunHost(Func<RuntimeEntries.CharacterGaoShunBoundaryMobileEntries> getCharacterGaoShunBoundaryMobileRuntime) : IBoundaryGaoShunHost
{
    public void Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGaoShunBoundaryMobileRuntime().ExecuteBoundaryGaoShun(f, op);
}
