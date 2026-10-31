using System.Globalization;

namespace CardGame.Core;

public sealed record ProgramVirtualOrdinaryTrickOrigin(long ParentProgramFrameId, int InstructionIndex,
    long LifecycleFrameId, int OwnerSeat, int InitialActorSeat, int InitialTargetSeat,
    string SkillId, string BindingId, string SkillInstanceId, string GameplayHash, int TurnNumber,
    CardKind OutputKind, SkillProgramConditionKind ConditionKind, string? PindianBind, bool? FrozenSourceWon,
    long CardActionId = 0);
public sealed record ProgramVirtualOrdinaryAttackReturnedEvent(long UseId, ProgramVirtualOrdinaryTrickOrigin Origin) : IGameEvent;

public sealed partial class GameEngine
{
}
