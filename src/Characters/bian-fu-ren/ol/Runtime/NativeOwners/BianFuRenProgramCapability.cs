#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BianFuRenProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries> getCharacterBianFuRenOlRuntime) : IBianFuRenProgramHost
{
    public void ResolveBianFuRenMovement(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterBianFuRenOlRuntime().ResolveBianFuRenMovement(frame, operation);
}
