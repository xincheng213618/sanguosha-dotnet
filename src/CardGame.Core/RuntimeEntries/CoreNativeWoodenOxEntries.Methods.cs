#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeWoodenOxEntries
{
    internal void ResolveWoodenOx(CharacterState actor, int storedCardId, int targetSeat) => _resolve().ResolveWoodenOx(actor, storedCardId, targetSeat);
}
