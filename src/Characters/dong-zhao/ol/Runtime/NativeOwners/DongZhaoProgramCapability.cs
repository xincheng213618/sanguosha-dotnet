#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DongZhaoProgramCapability(Func<RuntimeEntries.CharacterDongZhaoOlEntries> getCharacterDongZhaoOlRuntime) : IDongZhaoProgramHost
{
    public SkillProgramStepOutcome XianlueRecord(ProgramSkillFrame f) => getCharacterDongZhaoOlRuntime().BeginXianlueRecord(f);
    public SkillProgramStepOutcome XianlueRedistribute(ProgramSkillFrame f) => getCharacterDongZhaoOlRuntime().BeginXianlueRedistribute(f);
    public SkillProgramStepOutcome ZaowangResolve(ProgramSkillFrame f) => getCharacterDongZhaoOlRuntime().ArmZaowang(f);
}
