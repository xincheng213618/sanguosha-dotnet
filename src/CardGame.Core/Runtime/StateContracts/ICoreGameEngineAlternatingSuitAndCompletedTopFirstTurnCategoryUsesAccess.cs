#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int Turn, int Actor, string Skill, global::CardGame.Core.SkillProgramCardCategory Category), long> _firstTurnCategoryUses { get; }
}
