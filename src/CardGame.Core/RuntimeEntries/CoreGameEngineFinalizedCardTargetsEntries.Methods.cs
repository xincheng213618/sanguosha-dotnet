#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalizedCardTargetsEntries
{
    internal IReadOnlyList<int> GetImplicitSelfCardUseTargets(CardKind kind, int sourceSeat, IReadOnlyList<int> targets) => _resolve().GetImplicitSelfCardUseTargets(kind, sourceSeat, targets);
    internal bool HasFinalizedTargetProgram(CardKind kind) => _resolve().HasFinalizedTargetProgram(kind);
    internal bool TryBeginEquipmentTargetPrograms(CharacterState source, Card equipment, long frameId) => _resolve().TryBeginEquipmentTargetPrograms(source, equipment, frameId);
    internal bool TryBeginFinalizedSimpleCardPrograms(long frameId, ProgramSimpleCardContinuation continuation) => _resolve().TryBeginFinalizedSimpleCardPrograms(frameId, continuation);
}
