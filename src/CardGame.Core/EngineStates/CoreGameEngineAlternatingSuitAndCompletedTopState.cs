#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAlternatingSuitAndCompletedTopState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess
{
    private global::System.Collections.Generic.Dictionary<(int Turn, int Actor, string Skill, global::CardGame.Core.SkillProgramCardCategory Category), long> _firstTurnCategoryUses = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Turn, int Actor, string Skill, global::CardGame.Core.SkillProgramCardCategory Category), long> ReferenceToFirstTurnCategoryUses => ref _firstTurnCategoryUses;

    ref readonly global::System.Collections.Generic.Dictionary<(int Turn, int Actor, string Skill, global::CardGame.Core.SkillProgramCardCategory Category), long> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess._firstTurnCategoryUses => ref _firstTurnCategoryUses;
}
