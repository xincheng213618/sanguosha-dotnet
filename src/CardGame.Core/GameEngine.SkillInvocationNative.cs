namespace CardGame.Core;

public sealed record NativeFactionSkillInvocationCanceledEvent(long InvocationId, long ParentFrameId,
    long RequestOwnerFrameId, SkillInvocationReturnKind ReturnKind, int ActorSeat, string SkillKey,
    string SkillInstanceId, string OccurrenceKey) : IGameEvent;
