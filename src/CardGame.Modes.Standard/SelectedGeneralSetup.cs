using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature
{
    public ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts seat, string generalId)
    {
        var general = _registry.Generals[generalId];
        var extras = new List<string>();
        int? maximum = null, hp = null;
        if (_mode.TeamActivity is { } teamActivity)
        {
            if (ActivityPlayerSlot(seat.Seat)) extras.AddRange(teamActivity.PlayerSkillIds[seat.TeamId!]);
            var tier = ActivityTier(seat.Seat);
            var bonus = ActivityPlayerSlot(seat.Seat) ? (tier >= 2 ? 1 : 0) + (tier >= 5 ? 1 : 0) :
                (tier >= 2 ? 1 : 0) + (tier >= 3 ? 1 : 0) + (tier >= 4 ? 1 : 0);
            maximum = checked(general.BaseHp + bonus);
            hp = checked((general.InitialHp ?? general.BaseHp) + bonus);
        }
        if (_mode.BossActivity is { } boss)
        {
            if (seat.TeamId == boss.PlayerTeamId) extras.AddRange(boss.PlayerSkillIds);
            var tier = boss.PlayerTier;
            var bonus = _mode.HellActivity is { } hell ? seat.TeamId == boss.EnemyTeamId
                ? hell.BossTier switch { >= 5 => 2, >= 3 => 1, _ => 0 } : 2
                : seat.Seat == BossSeat ? 0 : (tier >= 2 ? 1 : 0) + (tier >= 5 ? 1 : 0);
            maximum = checked(general.BaseHp + bonus);
            hp = checked((general.InitialHp ?? general.BaseHp) + bonus);
        }
        if (_mode.ShenwuActivity is { } shenwu)
        {
            var allied = ShenwuPlayerSlot(seat.Seat);
            extras.AddRange(allied ? shenwu.PlayerSkillIds : shenwu.Edition != 0 ?
                new[] { "shenwu:enemy-tier" }.Concat(shenwu.EnemyTier == 5 ? ["shenwu:kunshou"] : Array.Empty<string>()) : []);
            var bonus = allied ? (shenwu.PlayerTier >= (shenwu.Edition == 0 ? 2 : 3) ? 1 : 0) + (shenwu.PlayerTier >= 5 ? 1 : 0) :
                shenwu.Edition == 2020 ? shenwu.EnemyTier switch { >= 4 => 2, >= 2 => 1, _ => 0 } : 0;
            maximum = checked(general.BaseHp + bonus);
            hp = checked((general.InitialHp ?? general.BaseHp) + bonus);
        }
        // This is the primary-slot rule. Dual-general combination is applied later by the shared setup owner.
        if (UsesGeneralBaseHp)
        {
            maximum = general.BaseHp + (seat.Role == Role.Lord ? 1 : 0);
            hp = Math.Min(maximum.Value, (general.InitialHp ?? general.BaseHp) + (seat.Role == Role.Lord ? 1 : 0));
        }
        if (_mode.WenheActivity is { } wenhe)
        {
            var bonus = (wenhe.PlayerTier >= 2 ? 1 : 0) + (wenhe.PlayerTier >= 5 ? 1 : 0);
            maximum = general.BaseHp + bonus;
            hp = (general.InitialHp ?? general.BaseHp) + bonus;
        }
        if (_mode.GuanduActivity is { } guandu)
        {
            var bonus = (guandu.PlayerTier >= 2 ? 1 : 0) + (guandu.PlayerTier >= 5 ? 1 : 0) + (seat.IsGuanduCommander ? 1 : 0);
            maximum = general.BaseHp + bonus;
            hp = (general.InitialHp ?? general.BaseHp) + bonus;
        }
        if (_mode.DragonBoatActivity is { } dragon)
        {
            var bonus = (dragon.PlayerTier >= 2 ? 1 : 0) + (dragon.PlayerTier >= 5 ? 1 : 0);
            maximum = general.BaseHp + bonus;
            hp = (general.InitialHp ?? general.BaseHp) + bonus;
        }
        return new(extras, maximum, hp);
    }
}
