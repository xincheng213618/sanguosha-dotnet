#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LiFengCapability(Func<RuntimeEntries.CharacterLiFengOlEntries> getCharacterLiFengOlRuntime) : ILiFengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiFengOlRuntime().BeginLiFeng(f, op);
}
