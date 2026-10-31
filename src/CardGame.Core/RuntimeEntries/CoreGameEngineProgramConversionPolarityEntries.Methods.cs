#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramConversionPolarityEntries
{
    internal global::CardGame.Core.SkillPolarity CommitProgramConversionPolarity(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CommitProgramConversionPolarity(frame);
    internal global::CardGame.Core.SkillPolarity GetProgramConversionPolarity(int ownerSeat, string skillId) => _resolve().GetProgramConversionPolarity(ownerSeat, skillId);
}
