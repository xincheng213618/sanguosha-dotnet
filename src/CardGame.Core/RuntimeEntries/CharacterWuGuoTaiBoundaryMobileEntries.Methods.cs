#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuGuoTaiBoundaryMobileEntries
{
    internal int? BoundaryWuGuoTaiBuyiDiscardActor(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations) => _resolve().BoundaryWuGuoTaiBuyiDiscardActor(frame, bind, ids, locations);
    internal bool IsBoundaryWuGuoTaiGanluPair(int ownerSeat, int firstSeat, int secondSeat) => _resolve().IsBoundaryWuGuoTaiGanluPair(ownerSeat, firstSeat, secondSeat);
}
