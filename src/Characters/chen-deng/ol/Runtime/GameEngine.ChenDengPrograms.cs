namespace CardGame.Core;

// 丰积 evidence: one scalar fact per resolved option of the owner's round
// ledger; the redistribution target is public once committed.
public enum FengjiOptionKind { Draw = 1, Slash = 2 }
public sealed record ProgramFengjiOptionChosenEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int RoundNumber, FengjiOptionKind Option, bool Accepted, int RecipientSeat) : IGameEvent;

// 旋回 evidence: the round effects swap is public and the skill disables until
// a character dies; the disable state derives from this fact versus deaths.
public sealed record ProgramXuanhuiEffectsSwappedEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int RoundNumber, int SwappedOptionCount) : IGameEvent;

public sealed partial class GameEngine
{
}
