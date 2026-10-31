namespace CardGame.Core;

public enum ProgramProvenanceAlcoholStage { FaceCost, WineUse }
public sealed record ProgramProvenanceAlcoholReceipt(int InstructionIndex, ProgramProvenanceAlcoholStage Stage, long? ChildFrameId=null);
