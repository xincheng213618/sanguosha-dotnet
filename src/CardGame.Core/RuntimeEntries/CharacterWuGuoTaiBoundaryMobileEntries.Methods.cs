#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuGuoTaiBoundaryMobileEntries
{
    internal int? BoundaryWuGuoTaiBuyiDiscardActor(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> locations) => _resolve().BoundaryWuGuoTaiBuyiDiscardActor(frame, bind, ids, locations);
    internal bool IsBoundaryWuGuoTaiGanluPair(int ownerSeat, int firstSeat, int secondSeat) => _resolve().IsBoundaryWuGuoTaiGanluPair(ownerSeat, firstSeat, secondSeat);
}
