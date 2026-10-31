namespace CardGame.Core;

public sealed record ProgramVirtualBasicDraft(int InstructionIndex, long? ChildFrameId = null);
public sealed record ProgramVirtualBasicReturn(long ParentFrameId, int InstructionIndex);
