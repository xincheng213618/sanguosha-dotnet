#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DuelHandle : GameEngine.ICardContinuationHandle
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly CardAttackHandleFactory createCardAttackHandle;
    public long OwnerFrameId { get; }
    private DuelState State => getCoreGameEngineCardAttackStateRuntime().GetCardContinuations(OwnerFrameId).Duel ?? throw new InvalidOperationException("The Duel continuation lost its owner state.");

    private void Update(Func<DuelState, DuelState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { Duel = update(state.Duel!) });
    internal DuelHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, CardAttackHandle attack)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = attack.ResolutionId;
        getCoreGameEngineCardAttackStateRuntime().UpdateCardContinuations(OwnerFrameId, state => state with { Duel = new DuelState { Active = false, ResponderSeat = attack.TargetSeat, } });
    }

    internal DuelHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, CardAttackHandleFactory createCardAttackHandle, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.createCardAttackHandle = createCardAttackHandle;
        OwnerFrameId = ownerId;
    }

    public CardAttackHandle Attack => createCardAttackHandle(OwnerFrameId);
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
