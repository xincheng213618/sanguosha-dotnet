namespace CardGame.Core;

// Scalar receipt: the sole material is public only after its real discard.
public sealed record ProgramLostHpChainedPayment(int InstructionIndex, string SourceBind,
    int CardId, CardLocation From, CardLocation To, Suit EffectiveSuit, long SequenceBefore,
    long SequenceAfter, int ActualTurnNumber, int ActualTurnOwnerSeat, CardUseEffectSource Source);
public sealed record ProgramLostHpChainedCostPaidEvent(long ProgramFrameId, ProgramLostHpChainedPayment Payment) : IGameEvent;
