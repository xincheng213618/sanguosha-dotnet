using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    // The source gives absolute limits. Related source IDs do not add the two limits together.
    private int ShanheDismantlementSelectionLimit(int sourceSeat) =>
        _options.ShanheBattle is not null && _modeDefinition.Id == ShanheBattleSetup.BattleModeId && IsValidPlayerSeat(sourceSeat)
            ? CardPolicies(_players[sourceSeat], SkillProgramCardPolicyKind.DismantlementSelectionLimit, CardKind.Dismantlement)
                .Select(item => item.Policy.Value).DefaultIfEmpty(1).Max()
            : 1;
}
