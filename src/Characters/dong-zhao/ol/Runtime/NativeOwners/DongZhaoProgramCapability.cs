#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class DongZhaoProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDongZhaoOlEntries> getCharacterDongZhaoOlRuntime) : IDongZhaoProgramHost
{
    public SkillProgramStepOutcome XianlueRecord(ProgramSkillFrame f) => getCharacterDongZhaoOlRuntime().BeginXianlueRecord(f);
    public SkillProgramStepOutcome XianlueRedistribute(ProgramSkillFrame f) => getCharacterDongZhaoOlRuntime().BeginXianlueRedistribute(f);
    public SkillProgramStepOutcome ZaowangResolve(ProgramSkillFrame f) => getCharacterDongZhaoOlRuntime().ArmZaowang(f);
}
