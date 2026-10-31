#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PangJiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPangJiOlEntries> getCharacterPangJiOlRuntime) : IPangJiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPangJiOlRuntime().BeginPangJi(f, op);
}
