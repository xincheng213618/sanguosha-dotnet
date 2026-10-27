using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int ShanheEmptyCampaignSkillSlotCount(int seat)
    {
        if (seat != 0 || _modeDefinition.Id != ShanheBattleSetup.BattleModeId || _options.ShanheBattle is not { } setup)
            return 0;
        if (_shanheMarket is { } market)
            return Math.Max(0, market.Snapshot.ExtraSkillSlots - market.Snapshot.ExtraSkillIds.Count);
        var occupied = setup.ExtraSkillIds.Count(id => id != ShanheGeneralRanks.RebirthSkillId);
        return Math.Max(0, setup.ExtraSkillSlots - occupied);
    }
}
