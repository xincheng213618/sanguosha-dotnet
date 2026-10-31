#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Turn, int Actor, string Skill, SkillProgramCardCategory Category), long> _firstTurnCategoryUses { get; }
}
