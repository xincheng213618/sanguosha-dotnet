#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class TaoQianCapability(Func<RuntimeEntries.CharacterTaoQianOlEntries> getCharacterTaoQianOlRuntime) : ITaoQianHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterTaoQianOlRuntime().BeginTaoQian(f, op);
}
