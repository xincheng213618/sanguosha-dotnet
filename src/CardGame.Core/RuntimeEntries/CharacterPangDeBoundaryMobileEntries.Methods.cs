#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangDeBoundaryMobileEntries
{
    internal void CaptureBoundaryPangDeDiscard(global::CardGame.Core.CardMovedEvent moved) => _resolve().CaptureBoundaryPangDeDiscard(moved);
    internal bool HasBoundaryPangDePaidDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasBoundaryPangDePaidDiscard(f);
    internal bool IsBoundaryPangDeUnrespondable(long frameId) => _resolve().IsBoundaryPangDeUnrespondable(frameId);
    internal bool KeepBoundaryPangDePaidDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryPangDePaidDiscard(f);
    internal bool ReturnBoundaryPangDePaidDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryPangDePaidDiscard(f);
}
