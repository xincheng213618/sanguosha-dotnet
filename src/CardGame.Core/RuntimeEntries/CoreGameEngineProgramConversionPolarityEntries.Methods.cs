#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramConversionPolarityEntries
{
    internal SkillPolarity CommitProgramConversionPolarity(ProgramSkillFrame frame) => _resolve().CommitProgramConversionPolarity(frame);
    internal SkillPolarity GetProgramConversionPolarity(int ownerSeat, string skillId) => _resolve().GetProgramConversionPolarity(ownerSeat, skillId);
}
