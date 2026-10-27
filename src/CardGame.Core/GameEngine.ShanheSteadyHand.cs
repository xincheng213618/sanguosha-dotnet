using CardGame.Core.Campaigns;

namespace CardGame.Core;

public static class ShanheSteadyHandPrograms
{
    public const string SkillId = "shanhe:current-xinshou-nianlai-2166";
}

public sealed partial class GameEngine
{
    private bool IsShanheSteadyHandModifierQualified(int ownerSeat, string skillId) =>
        skillId != ShanheSteadyHandPrograms.SkillId ||
        _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null &&
        IsValidPlayerSeat(ownerSeat) && _players[ownerSeat].IsAlive;
}
