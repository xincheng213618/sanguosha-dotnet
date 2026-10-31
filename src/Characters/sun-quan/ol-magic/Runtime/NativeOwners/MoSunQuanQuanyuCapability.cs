#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MoSunQuanQuanyuCapability(Func<RuntimeEntries.CharacterSunQuanOlMagicEntries> getCharacterSunQuanOlMagicRuntime) : IMoSunQuanQuanyuHost
{
    public SkillProgramStepOutcome BeginRound(ProgramSkillFrame f) => getCharacterSunQuanOlMagicRuntime().BeginMoQuanyuRound(f);
    public SkillProgramStepOutcome BeginTarget(ProgramSkillFrame f) => throw new InvalidOperationException("Quanyu target-only choices require the exact paid native Tianen factory.");
}
