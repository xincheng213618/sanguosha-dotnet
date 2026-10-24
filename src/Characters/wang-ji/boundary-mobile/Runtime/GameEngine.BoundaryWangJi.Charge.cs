namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Existing editions retain their own capacities, source ledgers and overflow rules.
    // Jinqu adds a point; it does not grant a skill or create a charge pool.
    private IEnumerable<string> BoundaryWjChargeSources(int seat)
    {
        foreach (var skill in new[] { BoundaryWjQizhi, MouJiangWeiComposition.Tiaoxin, MouGongsunZanMobileComposition.Yicong,
            MouMengHuoComposition.Zaiqi, ShiZhHuiComposition.SiziSkill, ShiDaTuntian })
            if (HasRuntimeSkill(_players[seat], skill)) yield return skill;
    }
    private void BoundaryWjChangeMarkerCharge(long frameId, int owner, PlayerMarkerKind marker, int delta, int capacity, string reason)
    {
        var player = _players[owner]; var before = player.Markers.GetValueOrDefault(marker); var after = Math.Clamp(checked(before + delta), 0, capacity);
        if (before == after) return;
        var key = (marker, owner);
        if (after == 0) { player.Markers.Remove(marker); player.MarkerSourceCounts.Remove(key); }
        else { player.Markers[marker] = after; player.MarkerSourceCounts[key] = after; }
        AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frameId, owner, marker, after - before, after, owner, reason));
    }
    private void GrantBoundaryWjCharge(ProgramSkillFrame frame, int target, string skill)
    {
        if (!_players[target].IsAlive || !BoundaryWjChargeSources(target).Contains(skill)) throw new InvalidOperationException("Jinqu requires its selected living holder's actual charge skill.");
        switch (skill)
        {
            case BoundaryWjQizhi: BoundaryWjChangeMarkerCharge(frame.Id, target, PlayerMarkerKind.BoundaryWangJiCharge, 1, 2, BoundaryWjJinqu); break;
            case MouJiangWeiComposition.Tiaoxin: ChangeMouJwCharge(frame.Id, target, 1); break;
            case MouGongsunZanMobileComposition.Yicong: ChangeMouGszMobileCharge(frame.Id, target, 1); break;
            case MouMengHuoComposition.Zaiqi: BoundaryWjChangeMarkerCharge(frame.Id, target, PlayerMarkerKind.MouMengHuoCharge, 1, 7, BoundaryWjJinqu); break;
            case ShiZhHuiComposition.SiziSkill: ChangeShiZhHuiCharge(frame.Id, target, 1, BoundaryWjJinqu, out _); break;
            case ShiDaTuntian:
                var instance = GetSkillBindingShard(_players[target]).ProgramInstances.First(binding => binding.SkillId == skill && HasRuntimeSkillInstance(_players[target], skill, binding.SkillInstanceId));
                ChangeShiDaCharge(frame.Id, new(skill, "external-charge", target, instance.SkillInstanceId), _contentRegistry.GetSkill(skill).Program!.GameplayHash, 1, 0, 0); break;
            default: throw new InvalidOperationException("Unsupported actual charge source.");
        }
        AdvanceEventRulesAndQueueFact(new BoundaryWangJiChargeGrantedEvent(frame.Id, target, skill));
    }
}
