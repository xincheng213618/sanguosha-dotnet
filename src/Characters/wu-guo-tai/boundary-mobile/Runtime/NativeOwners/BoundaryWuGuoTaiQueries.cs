#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryWuGuoTaiQueries(Func<RuntimeEntries.CharacterWuGuoTaiBoundaryMobileEntries> getCharacterWuGuoTaiBoundaryMobileRuntime) : IBoundaryWuGuoTaiQueries
{
    public bool CanExchange(int owner, int first, int second) => getCharacterWuGuoTaiBoundaryMobileRuntime().IsBoundaryWuGuoTaiGanluPair(owner, first, second);
}
