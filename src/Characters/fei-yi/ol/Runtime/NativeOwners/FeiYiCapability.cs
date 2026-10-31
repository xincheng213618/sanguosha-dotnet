#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class FeiYiCapability(Func<RuntimeEntries.CharacterFeiYiOlEntries> getCharacterFeiYiOlRuntime) : IFeiYiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterFeiYiOlRuntime().BeginFeiYi(f, op);
}
