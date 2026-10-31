#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDeathFramesEntries
{
    internal long BeginDeath(long parentFrameId, int victimSeat, int? killerSeat, global::CardGame.Core.DeathReturnKind returnKind) => _resolve().BeginDeath(parentFrameId, victimSeat, killerSeat, returnKind);
}
