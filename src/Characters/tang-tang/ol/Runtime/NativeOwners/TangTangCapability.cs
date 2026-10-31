#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class TangTangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTangTangOlEntries> getCharacterTangTangOlRuntime) : ITangTangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterTangTangOlRuntime().BeginTangTang(f, op);
}
