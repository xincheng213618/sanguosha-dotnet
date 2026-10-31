#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WenYangCapability(Func<RuntimeEntries.CharacterWenYangOlEntries> getCharacterWenYangOlRuntime) : IWenYangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWenYangOlRuntime().BeginWenYang(f, op);
}
