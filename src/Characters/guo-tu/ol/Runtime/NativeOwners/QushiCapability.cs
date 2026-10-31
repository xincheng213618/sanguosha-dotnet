#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QushiCapability(Func<RuntimeEntries.CharacterGuoTuOlEntries> getCharacterGuoTuOlRuntime) : IQushiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoTuOlRuntime().BeginQushi(f, op);
}
