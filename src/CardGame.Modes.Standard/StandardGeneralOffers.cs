using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature
{
    private List<ContentGeneralDefinition[]> GeneralFamilies(IEnumerable<ContentGeneralDefinition> generals)
    {
        var keys = _mode.GeneralVariantGroups?.SelectMany(group => group.Value.Select(id => (Id: id, Family: group.Key)))
            .ToDictionary(entry => entry.Id, entry => entry.Family, StringComparer.Ordinal);
        return generals.GroupBy(g => keys?.GetValueOrDefault(g.Id, g.Id) ?? g.Id, StringComparer.Ordinal)
            .Select(group => group.ToArray()).ToList();
    }
    private IReadOnlyList<ContentGeneralDefinition> DraftFamilies(IEnumerable<ContentGeneralDefinition> generals, int count)
    {
        var families = GeneralFamilies(generals);
        var result = new List<ContentGeneralDefinition>();
        while (result.Count < count && families.Count > 0)
        {
            var index = _random.Next(families.Count);
            var family = families[index];
            result.Add(family[_random.Next(family.Length)]);
            families.RemoveAt(index);
        }
        return Array.AsReadOnly(result.ToArray());
    }
    public ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest request)
    {
        if (_mode.GuanduActivity is not null) return GuanduOffers(request);
        if (_mode.DragonBoatActivity is not null)
            return new((request.HeldDraftCandidateIds ?? []).Select(id => new ModeGeneralOffer(id, id)));
        if (_mode.ShenwuActivity is not null) return ShenwuOffers(request);
        var available = request.AvailableGeneralIds.Select(id => _registry.Generals[id]).ToArray();
        if (UsesFamilyDraft) return FamilyOffers(request, available);
        var families = _mode.GeneralVariantGroups;
        var keys = families?.SelectMany(group => group.Value.Select(id => (Id: id, Family: group.Key)))
            .ToDictionary(entry => entry.Id, entry => entry.Family, StringComparer.Ordinal);
        var candidates = available
            .Where(g => _mode.TeamActivity is null || ActivitySeatPool(request.Seat.Seat, request.Seat.TeamId!).Contains(g.Id, StringComparer.Ordinal))
            .Where(g => _mode.BossActivity is null || BossSeatPool(request.Seat.Seat).Contains(g.Id, StringComparer.Ordinal))
            .Where(g => _mode.ModeKind != ContentModeKind.NationalWarLite || string.Equals(g.FactionId, request.Seat.NationalFactionId, StringComparison.Ordinal))
            .DistinctBy(g => keys?.GetValueOrDefault(g.Id, g.Id) ?? g.Id, StringComparer.Ordinal)
            .Take(request.FullRoster ? int.MaxValue : _mode.RoleGeneralCandidateCounts?.GetValueOrDefault(
                request.Seat.Role.ToString(), _mode.GeneralCandidateCount) ?? _mode.GeneralCandidateCount).ToArray();
        return new(candidates.SelectMany(candidate => new[] { candidate }.Concat(
            keys is not null && keys.TryGetValue(candidate.Id, out var family)
                ? available.Where(g => g.Id != candidate.Id && families![family].Contains(g.Id, StringComparer.Ordinal)) : []),
            (candidate, general) => new ModeGeneralOffer(general.Id, candidate.Id)));
    }
    private ModeGeneralOfferPlan FamilyOffers(ModeGeneralOfferRequest request, IReadOnlyList<ContentGeneralDefinition> available)
    {
        var eligible = available.Where(SelectionEligible).ToArray();
        var groups = GeneralFamilies(eligible);
        IReadOnlyList<ContentGeneralDefinition> candidates;
        IReadOnlyList<string>? frozen = null;
        if (request.FullRoster) candidates = groups.Select(family => family[0]).ToArray();
        else if (request.HeldDraftCandidateIds is { } held) candidates = held.Select(id => _registry.Generals[id]).ToArray();
        else
        {
            var count = _mode.RoleGeneralCandidateCounts?.GetValueOrDefault(request.Seat.Role.ToString(), _mode.GeneralCandidateCount) ?? _mode.GeneralCandidateCount;
            candidates = DraftFamilies(eligible, count);
            frozen = candidates.Select(g => g.Id).ToArray();
        }
        var families = groups.SelectMany(family => family.Select(g => (g.Id, Family: family)))
            .ToDictionary(entry => entry.Id, entry => entry.Family, StringComparer.Ordinal);
        return new(candidates.SelectMany(candidate => new[] { candidate }.Concat(families[candidate.Id].Where(g =>
            g.Id != candidate.Id && (request.Seat.IsHuman || _mode.AiGeneralSwapExcludedIds?.Contains(g.Id, StringComparer.Ordinal) != true))),
            (candidate, general) => new ModeGeneralOffer(general.Id, candidate.Id)), frozen);
    }
}
