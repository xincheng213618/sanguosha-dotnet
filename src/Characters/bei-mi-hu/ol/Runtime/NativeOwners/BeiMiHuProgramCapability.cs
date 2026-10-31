#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class BeiMiHuProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries> getCharacterBeiMiHuOlRuntime) : IBeiMiHuProgramHost
{
    public SkillProgramStepOutcome BeginBeiMiHuOperation(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterBeiMiHuOlRuntime().BeginBeiMiHuOperation(f, op);
}
