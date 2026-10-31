#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HanFuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHanFuOlEntries> getCharacterHanFuOlRuntime) : IHanFuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHanFuOlRuntime().BeginHanFu(f, op);
}
