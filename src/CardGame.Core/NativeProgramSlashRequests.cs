using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ProgramSlashRequestTargetSource { FrozenCandidates, NearestLiving, AllOtherLiving }
public enum ProgramSlashRequestReturnProtocol { NativeInstruction, ChoiceResult, FixedTarget }
public enum ProgramSlashRequestPermissionTargets { RequestTargets, AnyOtherTarget }
public enum ProgramSlashRequestFailureReturnPolicy { CancelBinding, ResumeOriginalInstruction }

/// <summary>Scope of one issued request, independently of native material and target legality.</summary>
public sealed record ProgramSlashRequestSpec
{
    public ProgramSlashRequestSpec(ProgramSlashRequestTargetSource TargetSource,
        IReadOnlyList<int>? CandidateSeats = null, string? ResultBind = null,
        bool IgnoreDistance = false, int? RangeOwnerSeat = null, bool UsesChoiceResult = false,
        bool RetainSourceLoss = false, bool RetainOwnerDeath = false,
        bool NearestStrictSnapshot = false, bool CountsTowardSlashLimit = false,
        ProgramSlashRequestReturnProtocol ReturnProtocol = ProgramSlashRequestReturnProtocol.NativeInstruction,
        bool RetainAcceptedSourceLoss = false, bool RetainAcceptedOwnerDeath = false,
        ProgramSlashRequestPermissionTargets QuotaTargets = ProgramSlashRequestPermissionTargets.RequestTargets,
        ProgramSlashRequestPermissionTargets? IssuedPhysicalSlashDistanceTargets = null, bool ExemptIssuedSlashDebit = false,
        ProgramSlashRequestFailureReturnPolicy FailureReturnPolicy = ProgramSlashRequestFailureReturnPolicy.CancelBinding)
    {
        if (!Enum.IsDefined(TargetSource) || CandidateSeats?.Any(seat => seat < 0) == true ||
            CandidateSeats is not null && CandidateSeats.Distinct().Count() != CandidateSeats.Count ||
            TargetSource == ProgramSlashRequestTargetSource.FrozenCandidates && CandidateSeats is null ||
            TargetSource == ProgramSlashRequestTargetSource.AllOtherLiving && CandidateSeats is { Count: > 0 } ||
            NearestStrictSnapshot && (TargetSource != ProgramSlashRequestTargetSource.NearestLiving || CandidateSeats is null) ||
            RangeOwnerSeat is < 0 || ResultBind is not null && string.IsNullOrWhiteSpace(ResultBind) ||
            !Enum.IsDefined(ReturnProtocol) || !Enum.IsDefined(QuotaTargets) || !Enum.IsDefined(FailureReturnPolicy) ||
            IssuedPhysicalSlashDistanceTargets is { } issuedTargets && !Enum.IsDefined(issuedTargets) ||
            (UsesChoiceResult || ReturnProtocol == ProgramSlashRequestReturnProtocol.ChoiceResult) && ResultBind is null)
            throw new InvalidOperationException("A native Slash request requires its declared actor-target scope and return permissions.");
        this.TargetSource = TargetSource;
        this.CandidateSeats = Array.AsReadOnly((CandidateSeats ?? []).ToArray());
        this.ResultBind = ResultBind;
        this.IgnoreDistance = IgnoreDistance;
        this.RangeOwnerSeat = RangeOwnerSeat;
        this.UsesChoiceResult = UsesChoiceResult || ReturnProtocol == ProgramSlashRequestReturnProtocol.ChoiceResult;
        this.RetainSourceLoss = RetainSourceLoss;
        this.RetainOwnerDeath = RetainOwnerDeath;
        this.NearestStrictSnapshot = NearestStrictSnapshot;
        this.CountsTowardSlashLimit = CountsTowardSlashLimit;
        this.ReturnProtocol = UsesChoiceResult && ReturnProtocol == ProgramSlashRequestReturnProtocol.NativeInstruction
            ? ProgramSlashRequestReturnProtocol.ChoiceResult : ReturnProtocol;
        this.RetainAcceptedSourceLoss = RetainAcceptedSourceLoss;
        this.RetainAcceptedOwnerDeath = RetainAcceptedOwnerDeath;
        this.QuotaTargets = QuotaTargets;
        this.IssuedPhysicalSlashDistanceTargets = IssuedPhysicalSlashDistanceTargets;
        this.ExemptIssuedSlashDebit = ExemptIssuedSlashDebit;
        this.FailureReturnPolicy = FailureReturnPolicy;
    }
    public ProgramSlashRequestTargetSource TargetSource { get; }
    public IReadOnlyList<int> CandidateSeats { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? ResultBind { get; }
    public bool IgnoreDistance { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int? RangeOwnerSeat { get; }
    public bool UsesChoiceResult { get; }
    public bool RetainSourceLoss { get; }
    public bool RetainOwnerDeath { get; }
    public bool NearestStrictSnapshot { get; }
    public bool CountsTowardSlashLimit { get; }
    public ProgramSlashRequestReturnProtocol ReturnProtocol { get; }
    public bool RetainAcceptedSourceLoss { get; }
    public bool RetainAcceptedOwnerDeath { get; }
    public ProgramSlashRequestPermissionTargets QuotaTargets { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramSlashRequestPermissionTargets? IssuedPhysicalSlashDistanceTargets { get; }
    public bool ExemptIssuedSlashDebit { get; }
    public ProgramSlashRequestFailureReturnPolicy FailureReturnPolicy { get; }
    public bool Equals(ProgramSlashRequestSpec? other) => other is not null && TargetSource == other.TargetSource &&
        CandidateSeats.SequenceEqual(other.CandidateSeats) && ResultBind == other.ResultBind && IgnoreDistance == other.IgnoreDistance &&
        RangeOwnerSeat == other.RangeOwnerSeat && UsesChoiceResult == other.UsesChoiceResult && RetainSourceLoss == other.RetainSourceLoss &&
        RetainOwnerDeath == other.RetainOwnerDeath && NearestStrictSnapshot == other.NearestStrictSnapshot &&
        CountsTowardSlashLimit == other.CountsTowardSlashLimit && ReturnProtocol == other.ReturnProtocol &&
        RetainAcceptedSourceLoss == other.RetainAcceptedSourceLoss && RetainAcceptedOwnerDeath == other.RetainAcceptedOwnerDeath &&
        QuotaTargets == other.QuotaTargets && IssuedPhysicalSlashDistanceTargets == other.IssuedPhysicalSlashDistanceTargets &&
        ExemptIssuedSlashDebit == other.ExemptIssuedSlashDebit && FailureReturnPolicy == other.FailureReturnPolicy;
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(TargetSource);
        foreach (var seat in CandidateSeats) hash.Add(seat);
        hash.Add(ResultBind); hash.Add(IgnoreDistance); hash.Add(RangeOwnerSeat); hash.Add(UsesChoiceResult);
        hash.Add(RetainSourceLoss); hash.Add(RetainOwnerDeath); hash.Add(NearestStrictSnapshot);
        hash.Add(CountsTowardSlashLimit); hash.Add(ReturnProtocol);
        hash.Add(RetainAcceptedSourceLoss); hash.Add(RetainAcceptedOwnerDeath);
        hash.Add(QuotaTargets); hash.Add(IssuedPhysicalSlashDistanceTargets); hash.Add(ExemptIssuedSlashDebit);
        hash.Add(FailureReturnPolicy);
        return hash.ToHashCode();
    }
}

public sealed record ProgramSlashRequestIdentity(long FrameId, int InstructionIndex, int IssuanceSlot,
    int ActorSeat, CardConversionSource Source, string GameplayHash, ProgramSlashRequestSpec Spec);
public sealed record ProgramSlashRequestAcceptance(ProgramSlashRequestIdentity Identity, int TargetSeat, int AttemptOrdinal = 0);
public sealed record ProgramSlashRequestFailure(ProgramSlashRequestAcceptance Acceptance, long ProducerFrameId, bool Consumed = false);
public sealed record ProgramSlashRequestState(ProgramSlashRequestIdentity Identity,
    ProgramSlashRequestAcceptance? Acceptance = null, bool Closed = false, int? SelectedTargetSeat = null,
    long? ExecutionFrameId = null, int LastAttemptOrdinal = 0, bool PreparingNativeUse = false,
    ProgramSlashRequestFailure? Failure = null);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramSlashRequestState? SlashRequest { get; init; }
}

internal interface IProgramSlashRequestFacts : IProgramSelectionQuery
{
    bool CanInvokeProgram(ProgramSkillFrame frame);
    int GetCombatDistance(int actorSeat, int targetSeat);
}

internal interface INativeProgramSlashRequests : IProgramRuntimeOperations
{
    // Slots are original issuance ordinals within one instruction, not seat numbers.
    ProgramSlashRequestIdentity Issue(ProgramSkillFrame frame, int actorSeat, ProgramSlashRequestSpec spec, int issuanceSlot = 0);
    // Identity remains readable after acceptance; offer reads also require its live invocation permissions.
    ProgramSlashRequestIdentity? GetIdentity(ProgramSkillFrame frame);
    bool OwnsIssuedIdentity(ProgramSkillFrame frame, ProgramSlashRequestIdentity identity, int actorSeat);
    ProgramSlashRequestIdentity? GetOffer(ProgramSkillFrame frame, int? actorSeat = null);
    IReadOnlyList<int> GetNearestLivingSeats(int actorSeat);
    // An accepted request returns its frozen target, independently of the later live offer conditions.
    IReadOnlyList<int> GetTargets(ProgramSkillFrame frame, int actorSeat);
    // Only an already issued native child or a paid typed continuation may use these reads/commit.
    IReadOnlyList<int> GetIssuedTargets(ProgramSkillFrame frame, ProgramSlashRequestIdentity identity, int actorSeat);
    bool OwnsOffer(ProgramSkillFrame frame, int actorSeat, int? targetSeat = null);
    void SelectTarget(ProgramSkillFrame frame, int actorSeat, int targetSeat);
    // Call only at real commitment/payment, never when opening a cancellable material-selection child.
    ProgramSlashRequestAcceptance Accept(ProgramSkillFrame frame, int actorSeat, int targetSeat);
    ProgramSlashRequestAcceptance AcceptIssued(ProgramSkillFrame frame, ProgramAssistedSlashAuthorization authorization,
        int actorSeat, int targetSeat);
    ProgramSlashRequestAcceptance AcceptIssued(ProgramSkillFrame frame, ProgramSlashRequestIdentity identity,
        int actorSeat, int targetSeat);
    bool OwnsAcceptance(ProgramSkillFrame frame, ProgramSlashRequestAcceptance acceptance, int actorSeat, int? targetSeat = null);
    bool CanExecuteAcceptedUse(ProgramSkillFrame frame, ProgramSlashRequestAcceptance acceptance);
    bool ReleaseAcceptance(ProgramSkillFrame frame, ProgramSlashRequestAcceptance acceptance);
    ProgramSlashRequestFailure FailAcceptedUse(ProgramSkillFrame frame, ProgramSlashRequestAcceptance acceptance, long actualProducerFrameId);
    ProgramSlashRequestFailure FailIssuedUse(ProgramSkillFrame frame, ProgramSlashRequestIdentity identity,
        int actorSeat, int targetSeat, long actualProducerFrameId);
    ProgramSlashRequestFailure? TakeFailure(ProgramSkillFrame frame);
    void Close(ProgramSkillFrame frame);
}

public sealed record ProgramSlashRequestExecution(long ProgramFrameId, long UseFrameId, long? CardActionId,
    ProgramSlashRequestAcceptance Acceptance);
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramSlashRequestExecution? ProgramSlashRequestExecution { get; init; }
}

internal interface INativeProgramSlashRequestExecution : IProgramRuntimeOperations
{
    ProgramSlashRequestAcceptance? GetAcceptance(ProgramSkillFrame frame, int actorSeat, int targetSeat);
    ProgramSkillFrame PrepareAcceptedUse(ProgramSkillFrame frame, ProgramSlashRequestAcceptance acceptance);
    CardUseFrame BindExecution(CardUseFrame use, ProgramSlashRequestAcceptance acceptance, long? originalProgramId = null);
}
