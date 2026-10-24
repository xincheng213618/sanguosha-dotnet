using CardGame.Core.Campaigns;

namespace CardGame.Core;

/// <summary>Read-only participant facts for mode-owned tier, distance and encounter rules.</summary>
public sealed record StandardBattlePlayerFacts(int Seat, string GeneralId, string GeneralName,
    bool IsAlive, bool GeneralSelected, int Hp, int MaxHp, string? TeamId);

public sealed record ShenwuBattleDraftFacts(IReadOnlyList<string> SharedGods, string LeaderId)
{
    public IReadOnlyList<string> SharedGods { get; } = Array.AsReadOnly(SharedGods.ToArray());
}

/// <summary>Queries plus atomic native mutations; mode qualification and sequencing live outside Core.</summary>
public interface IStandardBattleHost
{
    int InitialHandSize { get; }
    bool Started { get; }
    IReadOnlyList<StandardBattlePlayerFacts> Players { get; }
    StandardBattlePlayerFacts Player(int seat);
    bool IsWounded(int seat);
    bool HasEffectiveSkill(int seat, string skillId);
    ShenwuBattleDraftFacts? ShenwuDraft();
    void GrantSkill(int seat, SkillGrant grant);
    void PublishShanheStartingResourceOrigins();
    void PlaceStartingCard(ShanheStartingCard card, int seat, bool equipment);
}
