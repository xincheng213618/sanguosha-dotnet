#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalizedCardTargetsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<int> GetImplicitSelfCardUseTargets(global::CardGame.Core.CardKind kind, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().GetImplicitSelfCardUseTargets(kind, sourceSeat, targets);
    internal bool HasFinalizedTargetProgram(global::CardGame.Core.CardKind kind) => _resolve().HasFinalizedTargetProgram(kind);
    internal bool TryBeginEquipmentTargetPrograms(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long frameId) => _resolve().TryBeginEquipmentTargetPrograms(source, equipment, frameId);
    internal bool TryBeginFinalizedSimpleCardPrograms(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().TryBeginFinalizedSimpleCardPrograms(frameId, continuation);
}
