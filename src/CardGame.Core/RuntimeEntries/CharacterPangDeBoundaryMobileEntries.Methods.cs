#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangDeBoundaryMobileEntries
{
    internal void CaptureBoundaryPangDeDiscard(CardMovedEvent moved) => _resolve().CaptureBoundaryPangDeDiscard(moved);
    internal bool HasBoundaryPangDePaidDiscard(ProgramSkillFrame f) => _resolve().HasBoundaryPangDePaidDiscard(f);
    internal bool IsBoundaryPangDeUnrespondable(long frameId) => _resolve().IsBoundaryPangDeUnrespondable(frameId);
    internal bool KeepBoundaryPangDePaidDiscard(ProgramSkillFrame f) => _resolve().KeepBoundaryPangDePaidDiscard(f);
    internal bool ReturnBoundaryPangDePaidDiscard(ProgramSkillFrame f) => _resolve().ReturnBoundaryPangDePaidDiscard(f);
}
