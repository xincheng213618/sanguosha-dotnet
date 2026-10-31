namespace CardGame.Core;

// 盗书 settlement evidence: the declared suit, whether a hand card was taken
// and the branch outcome are public; the taken card's identity stays concealed
// (it resolves through the owner's own hand) while a handed-back card and a
// revealed hand enter the event as public knowledge.
public sealed record ProgramDaoshuEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int TargetSeat, Suit ChosenSuit, bool TookHandCard, bool SameSuit,
    int? GivenCardId, IReadOnlyList<int> RevealedCardIds) : IGameEvent;

// The suspended guess: the take already happened; the frame either waits for
// the give-back pick or is about to finish with the reveal fallback.
public sealed record DaoshuGuessState(int TargetSeat, Suit ChosenSuit, int TakenCardId,
    Suit TakenEffectiveSuit, bool GiveBackPending);

public sealed partial record ProgramSkillFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public DaoshuGuessState? DaoshuGuess { get; init; }
}

public sealed partial class GameEngine
{
}
