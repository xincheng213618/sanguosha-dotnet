#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DuelHandle : global::CardGame.Core.GameEngine.ICardContinuationHandle
{
    private readonly global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private DuelState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).Duel ?? throw new InvalidOperationException("The Duel continuation lost its owner state.");

    private void Update(Func<DuelState, DuelState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { Duel = update(state.Duel!) });
    internal DuelHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { Duel = new DuelState { Active = false, ResponderSeat = attack.TargetSeat, } });
    }

    internal DuelHandle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public global::CardGame.Core.NativeOwners.CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
    public long ResolutionId => Attack.ResolutionId;
    public int SourceSeat => Attack.SourceSeat;
    public int TargetSeat => Attack.TargetSeat;
    public int ResponderSeat { get => State.ResponderSeat; set => Update(state => state with { ResponderSeat = value }); }
    public int OpponentSeat => ResponderSeat == SourceSeat ? TargetSeat : SourceSeat;
    public bool FactionSlashAttempted { get => State.FactionSlashAttempted; set => Update(state => state with { FactionSlashAttempted = value }); }
    public int SuccessfulSlashResponses { get => State.SuccessfulSlashResponses; private set => Update(state => state with { SuccessfulSlashResponses = value }); }

    public bool RegisterSlashResponse(int requiredCount)
    {
        if (requiredCount < 1 || SuccessfulSlashResponses >= requiredCount)
        {
            throw new InvalidOperationException("The Duel Slash response count is invalid.");
        }

        SuccessfulSlashResponses++;
        FactionSlashAttempted = false;
        if (SuccessfulSlashResponses < requiredCount)
        {
            return false;
        }

        SuccessfulSlashResponses = 0;
        ResponderSeat = OpponentSeat;
        return true;
    }
}
