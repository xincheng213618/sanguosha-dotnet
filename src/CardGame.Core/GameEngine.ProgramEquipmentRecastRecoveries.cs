namespace CardGame.Core;

public sealed record ProgramEquipmentRecastReceipt(int InstructionIndex, int CardId, CardKind CardKind,
    bool DrawApplied = false, int DrawCount = 0);
