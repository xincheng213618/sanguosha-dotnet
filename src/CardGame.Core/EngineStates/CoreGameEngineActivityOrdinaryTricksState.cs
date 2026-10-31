#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineActivityOrdinaryTricksState : Contracts.ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess
{
    // Projections and their cache belong to this match. Registered source programs keep their content identity.
    private Dictionary<SkillProgram, SkillProgram> _activityOrdinaryPrograms = [];
    internal ref readonly Dictionary<SkillProgram, SkillProgram> ReferenceToActivityOrdinaryPrograms => ref _activityOrdinaryPrograms;

    ref readonly Dictionary<SkillProgram, SkillProgram> Contracts.ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess._activityOrdinaryPrograms => ref _activityOrdinaryPrograms;
}
