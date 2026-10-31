#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FactionDefenseHandle : global::CardGame.Core.GameEngine.ICardContinuationHandle
{
    private readonly global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime;
    private readonly global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private FactionDefenseState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).FactionDefense ?? throw new InvalidOperationException("The FactionDefense continuation lost its owner state.");

    private void Update(Func<FactionDefenseState, FactionDefenseState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { FactionDefense = update(state.FactionDefense!) });
    internal FactionDefenseHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, global::CardGame.Core.NativeOwners.CardAttackHandle attack, int ownerSeat, IReadOnlyList<int> candidateSeats, FactionResponsePolicySource source)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCoreGameEngineProgramEventHistoryRuntime = getCoreGameEngineProgramEventHistoryRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { FactionDefense = new FactionDefenseState { InvocationOccurrenceKey = $"faction-defense:{OwnerFrameId}:{(getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<FactionDefenseRequestedEvent>().Count + 1)}", Active = false, Source = source, OwnerSeat = ownerSeat, CandidateSeats = Array.AsReadOnly(candidateSeats.ToArray()), } });
    }

    internal FactionDefenseHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCoreGameEngineProgramEventHistoryRuntime = getCoreGameEngineProgramEventHistoryRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public FactionResponsePolicySource Source { get => State.Source; }
    public string InvocationOccurrenceKey => State.InvocationOccurrenceKey ?? throw new InvalidOperationException("A faction defense lost its accepted skill occurrence.");
    public global::CardGame.Core.NativeOwners.CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
    public int OwnerSeat { get => State.OwnerSeat; }
    public IReadOnlyList<int> CandidateSeats { get => State.CandidateSeats; }
    public int CandidateIndex { get => State.CandidateIndex; set => Update(state => state with { CandidateIndex = value }); }
    public int CurrentCandidateSeat => CandidateIndex < CandidateSeats.Count ? CandidateSeats[CandidateIndex] : -1;
}
