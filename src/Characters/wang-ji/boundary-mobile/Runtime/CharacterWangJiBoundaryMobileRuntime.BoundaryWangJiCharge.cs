#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangJiBoundaryMobileRuntime
{
    internal IEnumerable<string> BoundaryWjChargeSources(int seat)
    {
        foreach (var skill in new[]
        {
            BoundaryWjQizhi,
            MouJiangWeiComposition.Tiaoxin,
            MouGongsunZanMobileComposition.Yicong,
            MouMengHuoComposition.Zaiqi,
            ShiZhHuiComposition.SiziSkill,
            "mobile:shi-tuntian"
        }

        )
            if (_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], skill))
                yield return skill;
    }

    internal void BoundaryWjChangeMarkerCharge(long frameId, int owner, PlayerMarkerKind marker, int delta, int capacity, string reason)
    {
        var player = _coreMatchParticipantsPlayersAccess._players[owner];
        var before = player.Markers.GetValueOrDefault(marker);
        var after = Math.Clamp(checked(before + delta), 0, capacity);
        if (before == after)
            return;
        var key = (marker, owner);
        if (after == 0)
        {
            player.Markers.Remove(marker);
            player.MarkerSourceCounts.Remove(key);
        }
        else
        {
            player.Markers[marker] = after;
            player.MarkerSourceCounts[key] = after;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frameId, owner, marker, after - before, after, owner, reason));
    }

    internal void GrantBoundaryWjCharge(ProgramSkillFrame frame, int target, string skill)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !BoundaryWjChargeSources(target).Contains(skill))
            throw new InvalidOperationException("Jinqu requires its selected living holder's actual charge skill.");
        switch (skill)
        {
            case BoundaryWjQizhi:
                BoundaryWjChangeMarkerCharge(frame.Id, target, PlayerMarkerKind.BoundaryWangJiCharge, 1, 2, BoundaryWjJinqu);
                break;
            case MouJiangWeiComposition.Tiaoxin:
                _characterJiangWeiMouMobileOperations.ChangeMouJwCharge(frame.Id, target, 1);
                break;
            case MouGongsunZanMobileComposition.Yicong:
                _characterGongsunZanMouMobileOperations.ChangeMouGszMobileCharge(frame.Id, target, 1);
                break;
            case MouMengHuoComposition.Zaiqi:
                BoundaryWjChangeMarkerCharge(frame.Id, target, PlayerMarkerKind.MouMengHuoCharge, 1, 7, BoundaryWjJinqu);
                break;
            case ShiZhHuiComposition.SiziSkill:
                _characterZhongHuiMobileShiOperations.ChangeShiZhHuiCharge(frame.Id, target, 1, BoundaryWjJinqu, out _);
                break;
            case "mobile:shi-tuntian":
                var instance = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[target]).ProgramInstances.First(binding => binding.SkillId == skill && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[target], skill, binding.SkillInstanceId));
                _characterDengAiMobileShiOperations.ChangeShiDaCharge2(frame.Id, new(skill, "external-charge", target, instance.SkillInstanceId), _contentRegistryOperations._contentRegistry.GetSkill(skill).Program!.GameplayHash, 1, 0, 0);
                break;
            default:
                throw new InvalidOperationException("Unsupported actual charge source.");
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWangJiChargeGrantedEvent(frame.Id, target, skill));
    }
}
