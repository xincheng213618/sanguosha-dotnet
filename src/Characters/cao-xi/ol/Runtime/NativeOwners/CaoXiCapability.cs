#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class CaoXiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime) : ICaoXiProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaoXiOlRuntime().BeginCaoXi(f, op);
}
