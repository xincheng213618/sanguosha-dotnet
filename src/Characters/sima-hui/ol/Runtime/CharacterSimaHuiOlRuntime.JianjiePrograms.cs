#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaHuiOlRuntime
{
    internal CardConversionSource SimaHuiIssuer(ProgramSkillFrame f) => new("ol:jianjie", "seals", f.OwnerSeat, f.SkillInstanceId);
    internal static string SimaHuiSealSource(CardConversionSource issuer, PlayerMarkerKind marker) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sima_hui_ol_Jianjie_Programs.SimaHuiSealSource(issuer, marker);
    internal int? SimaHuiHolder(CardConversionSource issuer, PlayerMarkerKind marker) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SimaHuiSealMovedEvent>().LastOrDefault(e => e.Issuer == issuer && e.Marker == marker)?.Holder;
    internal int SimaHuiOwnTurns(int owner) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnStartedEvent>().Count(e => e.ActorSeat == owner);
    internal void SetSimaHuiSeal(ProgramSkillFrame f, CardConversionSource issuer, PlayerMarkerKind marker, int? from, int? to)
    {
        if (SimaHuiHolder(issuer, marker) != from || to == from || to is { } recipient && !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
            throw new InvalidOperationException("Jianjie seal transfer lost its exact issuer or recipient.");
        void Change(int seat, int delta)
        {
            var p = _coreMatchParticipantsPlayersAccess._players[seat];
            var key = (marker, issuer.OwnerSeat);
            var attributed = p.MarkerSourceCounts.GetValueOrDefault(key);
            var total = p.Markers.GetValueOrDefault(marker);
            if (attributed + delta < 0 || total + delta < 0)
                throw new InvalidOperationException("Jianjie cannot remove another source's seal.");
            if (attributed + delta == 0)
                p.MarkerSourceCounts.Remove(key);
            else
                p.MarkerSourceCounts[key] = attributed + delta;
            if (total + delta == 0)
                p.Markers.Remove(marker);
            else
                p.Markers[marker] = total + delta;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, seat, marker, delta, total + delta, issuer.OwnerSeat, "program.sima-hui.seal"));
        }

        if (from is { } old)
            Change(old, -1);
        if (to is { } next)
            Change(next, 1);
        var sourceId = SimaHuiSealSource(issuer, marker);
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            foreach (var grant in p.SkillGrants.Grants.Where(g => g.SourceId == sourceId).ToArray())
                p.SkillGrants.RemoveGrant(grant.GrantId);
        if (to is { } holder)
        {
            var id = marker == PlayerMarkerKind.DragonSeal ? "ol:huoji-jianjie" : "ol:lianhuan-jianjie";
            var grantId = sourceId + ":" + id;
            _coreMatchParticipantsPlayersAccess._players[holder].SkillGrants.Grant(new(grantId, id, grantId, sourceId));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SimaHuiSealMovedEvent(f.Id, issuer, f.GameplayHash, f.SimaHui!.ActualTurn, marker, from, to, sourceId));
        RefreshSimaHuiCombinedGrants();
    }

    internal void RefreshSimaHuiCombinedGrants()
    {
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
        {
            var source = $"sima-hui:combined:{(p.Seat)}";
            var id = source + ":ol:yeyan-jianjie";
            var both = global::CardGame.Core.EngineOwners.CharacterSimaHuiOlDefinitions.SimaHuiSeals.All(m => p.Markers.GetValueOrDefault(m) > 0);
            if (both)
                p.SkillGrants.Grant(new(id, "ol:yeyan-jianjie", id, source));
            else
                p.SkillGrants.RemoveGrant(id);
        }
    }
}
