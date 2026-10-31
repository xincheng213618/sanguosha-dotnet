namespace CardGame.Core;

public sealed record ProgramOwnedDamagePointJudgment(int InstructionIndex, long DamageWindowId, long DamageOwnerFrameId,
    int OccurrenceIndex, long JudgmentFrameId, int DamageSourceSeat, int DamageTargetSeat, string Reason, string ResultBind);
