#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<SkillProgram, SkillProgram> _activityOrdinaryPrograms { get; }
}
