using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum NativeCardEffectEpisodeScope { WholeUse, FrozenTarget, ExecutionOccurrence, ExecutionPair }
public enum NativeCardEffectEpisodeReplay { Original, BoundSelfRescueRecovery, ConsumePhysicalPlacement }
public enum NativeCardEffectEpisodeTargetAdmission { RetainIssued, RequireCurrentTargetsLegal }

public sealed record NativeCardEffectEpisodeSpec(int Priority, NativeCardEffectEpisodeScope Scope = NativeCardEffectEpisodeScope.WholeUse,
    int Count = 1, int? TargetSeat = null, int? TargetIndex = null,
    NativeCardEffectEpisodeReplay Replay = NativeCardEffectEpisodeReplay.Original,
    NativeCardEffectEpisodeTargetAdmission TargetAdmission = NativeCardEffectEpisodeTargetAdmission.RetainIssued,
    bool ContinueAfterActorDeath = true,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool ContinueOriginalAfterActorDeath = false);

public sealed record NativeCardEffectEpisodeIssue(long IssuerFrameId, int InstructionIndex, int IssuanceSlot,
    CardConversionSource Source, string GameplayHash, NativeCardEffectEpisodeSpec Spec);

public sealed record NativeCardEffectEpisode(long Id, NativeCardEffectEpisodeIssue Issue, int Started = 0, int Completed = 0,
    bool Cancelled = false);
public sealed record NativeCardEffectAttackSeed(int TargetIndex, CardAttackState Attack);

// One original native continuation and attack sample set belongs to the use.
// Episode declarations never copy another source's seeds or group-source fact.
public sealed record NativeCardEffectOriginalState(long? ActionId, ProgramTrickContinuation? Trick = null,
    ProgramSimpleCardContinuation? Simple = null)
{
    private readonly ProgramSimpleCardContinuation? _simple = FreezeSimple(Simple);
    public ProgramSimpleCardContinuation? Simple
    { get => _simple; init => _simple = FreezeSimple(value); }
    private readonly IReadOnlyList<NativeCardEffectAttackSeed> _attacks = [];
    public IReadOnlyList<NativeCardEffectAttackSeed> Attacks
    { get => _attacks; init => _attacks = Array.AsReadOnly(value.Select(seed => seed with { Attack = seed.Attack with
        {
            PhysicalCardIds = Array.AsReadOnly(seed.Attack.PhysicalCardIds.ToArray()),
            ChainedTargetSeats = Array.AsReadOnly(seed.Attack.ChainedTargetSeats.ToArray()),
            ResponseProhibitingSkillNames = Array.AsReadOnly(seed.Attack.ResponseProhibitingSkillNames.ToArray()),
            RangePreventionVisitedTargets = seed.Attack.RangePreventionVisitedTargets is { } visited ? Array.AsReadOnly(visited.ToArray()) : null
        } }).ToArray()); }
    private readonly FangtianHalberdState? _fangtian;
    public FangtianHalberdState? Fangtian
    { get => _fangtian; init => _fangtian = value is null ? null : value with { TargetSeats = Array.AsReadOnly(value.TargetSeats.ToArray()) }; }
    public int? GroupDamageSourceSeat { get; init; }
    private static ProgramSimpleCardContinuation? FreezeSimple(ProgramSimpleCardContinuation? value) => value is null ? null : value with
        { RecoveryPolicySources = value.RecoveryPolicySources is { } sources ? Array.AsReadOnly(sources.ToArray()) : null };
}

public sealed record NativeCardEffectEpisodeCursor(long EpisodeId, int CopyIndex, int Ordinal,
    int TargetIndex, IReadOnlyList<int> TargetSeats)
{
    private readonly IReadOnlyList<int> _targets = Array.AsReadOnly(TargetSeats.ToArray());
    public IReadOnlyList<int> TargetSeats
    { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
}

public sealed record NativeCardEffectEpisodesState(NativeCardEffectOriginalState Original)
{
    private readonly IReadOnlyList<NativeCardEffectEpisode> _episodes = [];
    public IReadOnlyList<NativeCardEffectEpisode> Episodes
    { get => _episodes; init => _episodes = Array.AsReadOnly(value.ToArray()); }
    public NativeCardEffectEpisodeCursor? Current { get; init; }
    public int StartedOrdinal { get; init; }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeCardEffectEpisodesState? EffectEpisodes { get; init; }
}

public sealed record NativeCardEffectEpisodeIssuedEvent(long UseFrameId, long? ActionId, NativeCardEffectEpisode Episode) : IGameEvent;
public sealed record NativeCardEffectEpisodeStartedEvent(long UseFrameId, long? ActionId, NativeCardEffectEpisode Episode,
    NativeCardEffectEpisodeCursor Cursor) : IGameEvent;
public sealed record NativeCardEffectEpisodeCompletedEvent(long UseFrameId, long? ActionId, NativeCardEffectEpisode Episode,
    NativeCardEffectEpisodeCursor Cursor, bool Executed) : IGameEvent;
public sealed record NativeCardEffectEpisodeCancelledEvent(long UseFrameId, long? ActionId, NativeCardEffectEpisode Episode) : IGameEvent;

internal interface INativeCardEffectEpisodes : IProgramRuntimeOperations
{
    NativeCardEffectEpisode Issue(long useId, NativeCardEffectEpisodeIssue issue,
        ProgramTrickContinuation? trick = null, ProgramSimpleCardContinuation? simple = null);
    NativeCardEffectEpisode IssueCurrent(ProgramSkillFrame frame, ProgramCardEffectEpisode declaration);
    NativeCardEffectEpisode? Get(long useId, long episodeId);
    bool IsCompleted(long useId, long episodeId);
    bool IsSettled(long useId, long episodeId);
    int Ordinal(long useId);
    NativeCardEffectEpisodeCursor? Current(long useId);
    bool IsActive(long useId);
    bool IsTargetScoped(long useId);
    // An active episode uses its own continuation right. Outside an episode,
    // only an explicitly issued original-use right permits the native suffix.
    bool CanContinueAfterActorDeath(long useId);
    // Future copies that have not started. The current native copy has its
    // own lifetime through IsActive and does not retain materials for another.
    bool HasPending(long useId);
    NativeCardEffectOriginalState? Original(long useId);
    void CaptureAttack(long useId, int targetIndex, CardAttackState attack, FangtianHalberdState? fangtian = null);
    void CaptureGroupDamageSource(long useId, int sourceSeat);
    bool TryBegin(long useId, bool afterAttack = false);
    void CompleteCurrent(long useId, bool executed = true);
}

public sealed record ProgramCardEffectEpisode(NativeCardEffectEpisodeSpec Spec, ProgramParticipantReference? TargetRef = null);

// The actual native adapter owns native walker readiness, cancellation and
// dispatch. Its one implementation may use native capabilities, never sources.
internal interface INativeCardEffectEpisodeExecution : IProgramRuntimeOperations
{
    bool IsMatchCompleted { get; }
    bool HasPendingTargets(CardUseFrame use);
    bool IsUseIneffective(CardUseFrame use);
    bool TargetsRemainLegal(CardUseFrame use);
    bool Begin(CardUseFrame use, NativeCardEffectOriginalState original, NativeCardEffectEpisode episode,
        NativeCardEffectEpisodeCursor cursor, bool afterAttack);
    bool ContinueSlashTargets(CardUseFrame use, NativeCardEffectOriginalState original,
        NativeCardEffectEpisodeCursor cursor);
}
