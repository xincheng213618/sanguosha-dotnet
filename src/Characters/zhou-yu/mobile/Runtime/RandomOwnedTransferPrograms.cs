using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum RandomOwnedTransferStage { GainChildren, RevealChildren, Complete }
public sealed record ProgramRandomOwnedTransfer(int InstructionIndex, string ResultBind, int RecipientSeat,
    int CardId, CardKind PrintedKind, int SequenceBefore, int SequenceAfter,
    RandomOwnedTransferStage Stage = RandomOwnedTransferStage.GainChildren, Suit? RevealedSuit = null);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramRandomOwnedTransfer? RandomOwnedTransfer { get; init; }
}
