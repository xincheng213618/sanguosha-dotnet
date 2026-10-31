#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FactionCardRequestHandle : GameEngine.ICardContinuationHandle
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime;
    private readonly CardAttackHandleFactory createCardAttackHandle;
    private readonly BorrowedSwordHandleFactory createBorrowedSwordHandle;
    private readonly QinglongCrescentBladeHandleFactory createQinglongCrescentBladeHandle;
    public long OwnerFrameId { get; }
    private FactionCardRequestState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).FactionCardRequest ?? throw new InvalidOperationException("The FactionCardRequest continuation lost its owner state.");

    private void Update(Func<FactionCardRequestState, FactionCardRequestState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { FactionCardRequest = update(state.FactionCardRequest!) });
    internal FactionCardRequestHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, CardAttackHandleFactory createCardAttackHandle, BorrowedSwordHandleFactory createBorrowedSwordHandle, QinglongCrescentBladeHandleFactory createQinglongCrescentBladeHandle, long resolutionId, FactionCardRequestPurpose purpose, int ownerSeat, IReadOnlyList<int> candidateSeats, string providerFactionId, CardAttackHandle? responseAttack = null, int? targetSeat = null, long? programSkillFrameId = null, CardKind requiredKind = CardKind.Slash, BorrowedSwordHandle? borrowedSword = null, QinglongCrescentBladeHandle? qinglongCrescentBlade = null, FactionResponsePolicySource? policySource = null, string? assistedResultBind = null)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCoreGameEngineProgramEventHistoryRuntime = getCoreGameEngineProgramEventHistoryRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        this.createBorrowedSwordHandle = createBorrowedSwordHandle;
        this.createQinglongCrescentBladeHandle = createQinglongCrescentBladeHandle;
        OwnerFrameId = resolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { FactionCardRequest = new FactionCardRequestState { InvocationOccurrenceKey = $"faction-slash:{OwnerFrameId}:{(getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<FactionSlashRequestedEvent>().Count + 1)}", Active = false, PolicySource = policySource, AssistedResultBind = assistedResultBind, Purpose = purpose, OwnerSeat = ownerSeat, CandidateSeats = Array.AsReadOnly(candidateSeats.ToArray()), ResponseAttackOwnerId = responseAttack?.ResolutionId, TargetSeat = targetSeat, ProgramSkillFrameId = programSkillFrameId, ProviderFactionId = providerFactionId, RequiredKind = requiredKind, BorrowedSwordOwnerId = borrowedSword?.ResolutionId, QinglongCrescentBladeOwnerId = qinglongCrescentBlade?.OwnerFrameId, AwaitingProviders = true, ZhuqueFanPhysicalCardIds = [], } });
    }

    internal FactionCardRequestHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, CardAttackHandleFactory createCardAttackHandle, BorrowedSwordHandleFactory createBorrowedSwordHandle, QinglongCrescentBladeHandleFactory createQinglongCrescentBladeHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCoreGameEngineProgramEventHistoryRuntime = getCoreGameEngineProgramEventHistoryRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        this.createBorrowedSwordHandle = createBorrowedSwordHandle;
        this.createQinglongCrescentBladeHandle = createQinglongCrescentBladeHandle;
        OwnerFrameId = ownerId;
    }

    public FactionResponsePolicySource? PolicySource { get => State.PolicySource; }
    public string InvocationOccurrenceKey => State.InvocationOccurrenceKey ?? throw new InvalidOperationException("A faction slash lost its accepted skill occurrence.");
    public string? AssistedResultBind { get => State.AssistedResultBind; }
    public long ResolutionId => OwnerFrameId;
    public FactionCardRequestPurpose Purpose { get => State.Purpose; }
    public int OwnerSeat { get => State.OwnerSeat; }
    public IReadOnlyList<int> CandidateSeats { get => State.CandidateSeats; }
    public CardAttackHandle? ResponseAttack { get => State.ResponseAttackOwnerId is { } id ? createCardAttackHandle(id) : null; }
    public int? TargetSeat { get => State.TargetSeat; }
    public long? ProgramSkillFrameId { get => State.ProgramSkillFrameId; }
    public string ProviderFactionId { get => State.ProviderFactionId; }
    public CardKind RequiredKind { get => State.RequiredKind; }
    public BorrowedSwordHandle? BorrowedSword { get => State.BorrowedSwordOwnerId is { } id ? createBorrowedSwordHandle(id) : null; }
    public QinglongCrescentBladeHandle? QinglongCrescentBlade { get => State.QinglongCrescentBladeOwnerId is { } id ? createQinglongCrescentBladeHandle(id) : null; }
    public int CandidateIndex { get => State.CandidateIndex; set => Update(state => state with { CandidateIndex = value }); }
    public bool AwaitingProviders { get => State.AwaitingProviders; set => Update(state => state with { AwaitingProviders = value }); }
    public bool CostPaid { get => State.CostPaid; set => Update(state => state with { CostPaid = value }); }
    public bool ProviderRewarded { get => State.ProviderRewarded; set => Update(state => state with { ProviderRewarded = value }); }
    public bool AwaitingZhuqueFanChoice { get => State.AwaitingZhuqueFanChoice; set => Update(state => state with { AwaitingZhuqueFanChoice = value }); }
    public int? ZhuqueFanProviderSeat { get => State.ZhuqueFanProviderSeat; set => Update(state => state with { ZhuqueFanProviderSeat = value }); }
    public IReadOnlyList<Card> ZhuqueFanPhysicalCards { get => State.ZhuqueFanPhysicalCardIds.Select(getCoreGameEngineCardAttackStateRuntime().GetAttackCard).ToArray(); set => Update(state => state with { ZhuqueFanPhysicalCardIds = Array.AsReadOnly(value.Select(card => card.Id).ToArray()) }); }
    public CardAttackHandle? ActiveAttack { get => State.ActiveAttackOwnerId is { } id ? createCardAttackHandle(id) : null; set => Update(state => state with { ActiveAttackOwnerId = value?.ResolutionId }); }
    public int CurrentCandidateSeat => CandidateIndex < CandidateSeats.Count ? CandidateSeats[CandidateIndex] : -1;
    public bool IsProgramSkillUse => Purpose is FactionCardRequestPurpose.ProgramSkillUse or FactionCardRequestPurpose.AssistedProgramUse;
    public bool IsAssistedProgramUse => Purpose == FactionCardRequestPurpose.AssistedProgramUse;
    public bool IsBorrowedSwordUse => Purpose == FactionCardRequestPurpose.BorrowedSwordUse;
    public bool IsQinglongCrescentBladeUse => Purpose == FactionCardRequestPurpose.QinglongCrescentBladeUse;
}
