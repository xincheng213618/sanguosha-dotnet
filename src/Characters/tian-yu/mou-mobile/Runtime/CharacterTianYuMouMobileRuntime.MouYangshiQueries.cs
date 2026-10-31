#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianYuMouMobileRuntime
{
    [ProgramDistanceRule(ProgramDistanceStage.AttackRangeContributions, 30)]
    internal void AddMouTianYuRangeBonusAttackRangeContribution(CharacterState player, List<RuleQueryContribution> contributions) => ((MouYangshiRangeRuntime)_closedMouYangshiRangeRuntimeSource.Get()).AddRangeContribution(player.Seat, contributions);
}
