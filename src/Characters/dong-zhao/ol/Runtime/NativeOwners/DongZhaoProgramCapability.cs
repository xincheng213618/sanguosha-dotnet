#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DongZhaoProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDongZhaoOlRuntime>> getCharacterDongZhaoOlRuntime) : IDongZhaoProgramHost
{
    public SkillProgramStepOutcome XianlueRecord(ProgramSkillFrame f) => getCharacterDongZhaoOlRuntime().Value.BeginXianlueRecord(f);
    public SkillProgramStepOutcome XianlueRedistribute(ProgramSkillFrame f) => getCharacterDongZhaoOlRuntime().Value.BeginXianlueRedistribute(f);
    public SkillProgramStepOutcome ZaowangResolve(ProgramSkillFrame f) => getCharacterDongZhaoOlRuntime().Value.ArmZaowang(f);
}
