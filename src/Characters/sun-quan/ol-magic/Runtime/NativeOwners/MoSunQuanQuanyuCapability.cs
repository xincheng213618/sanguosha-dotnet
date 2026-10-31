#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MoSunQuanQuanyuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries> getCharacterSunQuanOlMagicRuntime) : IMoSunQuanQuanyuHost
{
    public SkillProgramStepOutcome BeginRound(ProgramSkillFrame f) => getCharacterSunQuanOlMagicRuntime().BeginMoQuanyuRound(f);
    public SkillProgramStepOutcome BeginTarget(ProgramSkillFrame f) => throw new InvalidOperationException("Quanyu target-only choices require the exact paid native Tianen factory.");
}
