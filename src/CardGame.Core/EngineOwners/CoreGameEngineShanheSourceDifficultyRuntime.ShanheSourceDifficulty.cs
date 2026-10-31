#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceDifficultyRuntime
{
    internal bool RequiresShanheSourceArmorTracking => _coreMatchDefinitionOptionsAccess._options.ShanheBattle is { } setup && (setup.EnemyRoster.Concat(setup.AllyRoster).Any(member => member.ArmorRecoveryPerRound > 0) || setup.BossTransition?.Combatant is { ArmorRecoveryPerRound: > 0 });

    internal void RestoreShanheSourceRoundArmor()
    {
        // Initial armor is already in the seat recipe. Local completion starts recovery in round two.
        if (_coreActualRoundProgressRoundNumberAccess._roundNumber <= 1 || !_modeRulesOperations._modeRules.AllowsArmorGain || _coreMatchDefinitionOptionsAccess._options.ShanheBattle is not { } setup)
            return;
        var members = setup.AllyRoster.Concat(setup.EnemyRoster).ToArray();
        for (var index = 0; index < members.Length; index++)
        {
            var member = members[index];
            var player = _coreMatchParticipantsPlayersAccess._players[index + 1];
            if (setup.BossTransition is { Combatant: { } stage } transition && index == setup.AllyRoster.Count + transition.EnemyIndex && player.General.Id == transition.GeneralId)
                member = stage;
            if (!player.IsAlive || member.ArmorRecoveryPerRound <= 0 || player.Armor >= member.ArmorRecoveryCap)
                continue;
            var before = player.Armor;
            var after = Math.Min(member.ArmorRecoveryCap, checked(before + member.ArmorRecoveryPerRound));
            player.Armor = after;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceArmorRestoredEvent(_coreActualRoundProgressRoundNumberAccess._roundNumber, player.Seat, after - before, before, after, member.ArmorRecoveryCap));
            _coreMatchLogOperations.AddLog("ShanheSourceArmor", $"{(player.Name)} 回复 {(after - before)} 点护甲，现有 {(after)} 点。", player.Seat);
        }
    }
}
