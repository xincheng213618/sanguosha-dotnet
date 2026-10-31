namespace CardGame.Core;
public sealed partial class GameEngine
{
internal sealed record NativeBuZhiDiscardInvocationDraft(long Id, long FrameId, CardConversionSource Source, string GameplayHash,
        int InstructionIndex, int Turn, int TurnOwner, CardMoveReason Reason, long Before,
        IReadOnlyList<CardZoneKind> Zones, IReadOnlyList<NativeBuZhiDiscardMaterial> Eligible) : IProgramOwnedZoneDiscardInvocation;
}
