#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class NiuFuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterNiuFuOlEntries> getCharacterNiuFuOlRuntime) : INiuFuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterNiuFuOlRuntime().BeginNiuFu(f, op);
}
