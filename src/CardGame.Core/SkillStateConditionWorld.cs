namespace CardGame.Core;

// Ongoing conditions read raw state only; querying skill effectiveness here
// would recurse back into the restriction being evaluated.
internal sealed class SkillStateConditionWorld(SkillStateConditionContext context, CharacterState owner,
    EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess? participants = null, CardZoneStore? cards = null,
    IActualDiscardRecoveryPhaseQueries? phases = null, IProgramRuntimeFrames? frames = null,
    RuntimeEntries.CoreGameEngineHealthQueriesEntries? health = null,
    EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess? turnState = null,
    IProgramHealthChildQueries? healthChildren = null) : IProgramExpressionWorld
{
    public int? Participant(string name)
    {
        int? seat = name switch
        {
            "owner" => context.OwnerSeat,
            "target" or "selectedTarget" => context.TargetSeat,
            "eventTarget" => context.EventTargetSeat,
            _ => null
        };
        return seat is { } value && Player(value) is not null ? value : null;
    }

    public IReadOnlyList<int>? ParticipantSet(string name) => name is "living" or "livingOthers"
        ? Players.Where(player => player.IsAlive && (name == "living" || player.Seat != context.OwnerSeat))
            .OrderBy(player => player.Seat).Select(player => player.Seat).ToArray()
        : null;

    public int? Stat(string stat, int seat)
    {
        if (Player(seat) is not { } player) return null;
        return stat switch
        {
            "hp" => health?.UnambiguousHp(player),
            "maxHp" => player.MaxHp,
            "lostHp" => health?.UnambiguousHp(player) is { } hp ? player.MaxHp - hp : null,
            "handCount" => cards?.Count(CardLocation.Hand(seat)),
            "equipmentCount" => cards?.Count(CardLocation.Equipment(seat)),
            _ => null
        };
    }

    public bool? Flag(string flag, int seat)
    {
        if (Player(seat) is not { } player) return null;
        return flag switch
        {
            "alive" => player.IsAlive,
            "hasHp" => player.HasHpValue,
            "faceDown" => player.IsFaceDown,
            "chained" => player.IsChained,
            "inIssuedPhase" => context.IssuedPhase is { } issued && issued.ActorSeat == seat && phases?.Current() == issued,
            "inUpcomingTurnPrelude" => frames is null ? null :
                frames.OfType<ResolutionFrame>().FirstOrDefault()?.UpcomingTurnPrelude?.OwnerSeat == seat,
            "isTurnOwner" => turnState is null ? null : turnState._turnNumber > 0 &&
                turnState._phase is not (TurnPhase.NotStarted or TurnPhase.Finished) && turnState._currentSeat == seat,
            "inIssuedTurn" => turnState is null ? null : context.IssuedTurnNumber == turnState._turnNumber &&
                turnState._phase is not (TurnPhase.NotStarted or TurnPhase.Finished) && turnState._currentSeat == seat,
            "inIssuedDying" => healthChildren is null ? null : healthChildren.ActiveDying is { } dying &&
                dying.Id == context.IssuedDyingFrameId && dying.VictimSeat == seat,
            _ => null
        };
    }

    private IEnumerable<CharacterState> Players => participants is null ? [owner] : participants._players;
    private CharacterState? Player(int seat) => Players.FirstOrDefault(player => player.Seat == seat);
}
