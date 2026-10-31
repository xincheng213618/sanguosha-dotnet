#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryCuiLieHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries> getCharacterCuiLieAnniversaryRuntime) : IAnniversaryCuiLieHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCuiLieAnniversaryRuntime().BeginCuiLie(f, op);
}
