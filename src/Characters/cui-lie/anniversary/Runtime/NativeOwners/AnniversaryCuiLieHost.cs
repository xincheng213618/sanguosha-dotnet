#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryCuiLieHost(Func<RuntimeEntries.CharacterCuiLieAnniversaryEntries> getCharacterCuiLieAnniversaryRuntime) : IAnniversaryCuiLieHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCuiLieAnniversaryRuntime().BeginCuiLie(f, op);
}
