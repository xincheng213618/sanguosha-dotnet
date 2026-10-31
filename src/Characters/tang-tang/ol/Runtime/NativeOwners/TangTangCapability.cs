#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class TangTangCapability(Func<RuntimeEntries.CharacterTangTangOlEntries> getCharacterTangTangOlRuntime) : ITangTangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterTangTangOlRuntime().BeginTangTang(f, op);
}
