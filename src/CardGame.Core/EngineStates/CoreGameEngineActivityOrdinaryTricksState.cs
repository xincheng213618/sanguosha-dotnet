#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineActivityOrdinaryTricksState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess
{
    // Projections and their cache belong to this match. Registered source programs keep their content identity.
    private global::System.Collections.Generic.Dictionary<global::CardGame.Core.SkillProgram, global::CardGame.Core.SkillProgram> _activityOrdinaryPrograms = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.SkillProgram, global::CardGame.Core.SkillProgram> ReferenceToActivityOrdinaryPrograms => ref _activityOrdinaryPrograms;

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.SkillProgram, global::CardGame.Core.SkillProgram> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess._activityOrdinaryPrograms => ref _activityOrdinaryPrograms;
}
