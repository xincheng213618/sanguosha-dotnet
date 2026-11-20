#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDeathFramesEntries
{
    internal DeathFrame BeginDeath(long parentFrameId, CharacterState victim, int? killerSeat, DeathReturnKind returnKind) => _resolve().BeginDeath(parentFrameId, victim, killerSeat, returnKind);
}
