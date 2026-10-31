#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HanFuCapability(Func<RuntimeEntries.CharacterHanFuOlEntries> getCharacterHanFuOlRuntime) : IHanFuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHanFuOlRuntime().BeginHanFu(f, op);
}
