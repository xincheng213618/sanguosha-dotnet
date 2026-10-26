namespace CardGame.Core;

public sealed record ShanheSourceArmorRestoredEvent(int RoundNumber, int PlayerSeat, int Amount,
    int ArmorBefore, int ArmorAfter, int RecoveryCap) : IGameEvent;

public sealed partial class GameEngine
{
    private bool RequiresShanheSourceArmorTracking => _options.ShanheBattle is { } setup &&
        (setup.EnemyRoster.Concat(setup.AllyRoster).Any(member => member.ArmorRecoveryPerRound > 0) ||
            setup.BossTransition?.Combatant is { ArmorRecoveryPerRound: > 0 });

    private void RestoreShanheSourceRoundArmor()
    {
        // Initial armor is already in the seat recipe. Local completion starts recovery in round two.
        if (_roundNumber <= 1 || !_modeRules.AllowsArmorGain || _options.ShanheBattle is not { } setup) return;
        var members = setup.AllyRoster.Concat(setup.EnemyRoster).ToArray();
        for (var index = 0; index < members.Length; index++)
        {
            var member = members[index]; var player = _players[index + 1];
            if (setup.BossTransition is { Combatant: { } stage } transition &&
                index == setup.AllyRoster.Count + transition.EnemyIndex && player.General.Id == transition.GeneralId)
                member = stage;
            if (!player.IsAlive || member.ArmorRecoveryPerRound <= 0 || player.Armor >= member.ArmorRecoveryCap) continue;
            var before = player.Armor;
            var after = Math.Min(member.ArmorRecoveryCap, checked(before + member.ArmorRecoveryPerRound));
            player.Armor = after;
            AdvanceEventRulesAndQueueFact(new ShanheSourceArmorRestoredEvent(_roundNumber, player.Seat,
                after - before, before, after, member.ArmorRecoveryCap));
            AddLog("ShanheSourceArmor", $"{player.Name} 回复 {after - before} 点护甲，现有 {after} 点。", player.Seat);
        }
    }
}
