#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class TaoQianCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTaoQianOlEntries> getCharacterTaoQianOlRuntime) : ITaoQianHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterTaoQianOlRuntime().BeginTaoQian(f, op);
}
