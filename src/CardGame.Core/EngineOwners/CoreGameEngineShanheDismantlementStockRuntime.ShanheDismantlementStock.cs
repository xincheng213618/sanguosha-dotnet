#nullable enable

using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheDismantlementStockRuntime
{
    internal int ShanheDismantlementSelectionLimit(int sourceSeat) => _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null && _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) ? _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[sourceSeat], SkillProgramCardPolicyKind.DismantlementSelectionLimit, CardKind.Dismantlement).Select(item => item.Policy.Value).DefaultIfEmpty(1).Max() : 1;
}
