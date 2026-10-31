#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAlternatingSuitAndCompletedTopState : Contracts.ICoreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess
{
    private Dictionary<(int Turn, int Actor, string Skill, SkillProgramCardCategory Category), long> _firstTurnCategoryUses = [];
    internal ref readonly Dictionary<(int Turn, int Actor, string Skill, SkillProgramCardCategory Category), long> ReferenceToFirstTurnCategoryUses => ref _firstTurnCategoryUses;

    ref readonly Dictionary<(int Turn, int Actor, string Skill, SkillProgramCardCategory Category), long> Contracts.ICoreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess._firstTurnCategoryUses => ref _firstTurnCategoryUses;
}
