#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheQianlongRuntime
{
    internal int ShanheEmptyCampaignSkillSlotCount(int seat)
    {
        if (seat != 0 || _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id != ShanheBattleSetup.BattleModeId || _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not { } setup)
            return 0;
        if (_shanheMarketOperations._shanheMarket is { } market)
            return Math.Max(0, market.Snapshot.ExtraSkillSlots - market.Snapshot.ExtraSkillIds.Count);
        var occupied = setup.ExtraSkillIds.Count(id => id != ShanheGeneralRanks.RebirthSkillId);
        return Math.Max(0, setup.ExtraSkillSlots - occupied);
    }
}
