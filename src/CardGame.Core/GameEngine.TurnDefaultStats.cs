using System.Globalization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    internal sealed record TurnDefaultStatKey(int OwnerSeat, string SkillId, string StateId);
    internal sealed record TurnDefaultStatAssignment(int Value, long FrameId, int SlotIndex);
    internal sealed record TurnDefaultStatState(CardConversionSource Source, string GameplayHash, int ActualTurnNumber, bool HasPool, IReadOnlyList<int> Slots, int UsedSlotMask, IReadOnlyDictionary<TurnDefaultStatKind, global::CardGame.Core.GameEngine.TurnDefaultStatAssignment> Assigned, IReadOnlyList<int> NextSlots);
    // This is durable gameplay state. Choices and their frozen pool remain only
    // on ProgramSkillFrame; no pending action is retained in this dictionary.
    private ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.TurnDefaultStatKey, global::CardGame.Core.GameEngine.TurnDefaultStatState> _turnDefaultStatStates => ref _turnDefaultStats.ReferenceToTurnDefaultStatStates;
    private ref bool _turnDefaultStatLedgerStarted => ref _turnDefaultStats.ReferenceToTurnDefaultStatLedgerStarted;
}
