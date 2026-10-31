#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryWuGuoTaiQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWuGuoTaiBoundaryMobileEntries> getCharacterWuGuoTaiBoundaryMobileRuntime) : IBoundaryWuGuoTaiQueries
{
    public bool CanExchange(int owner, int first, int second) => getCharacterWuGuoTaiBoundaryMobileRuntime().IsBoundaryWuGuoTaiGanluPair(owner, first, second);
}
