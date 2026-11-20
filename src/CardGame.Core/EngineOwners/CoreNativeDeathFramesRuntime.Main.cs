#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDeathFramesRuntime
{
    internal DeathFrame BeginDeath(long parentFrameId, CharacterState victim, int? killerSeat, DeathReturnKind returnKind)
    {
        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var death = new DeathFrame(frameId, parentFrameId, victim.Seat, killerSeat)
        {
            ReturnKind = returnKind,
            HpAtDeath = victim.Hp
        };
        if (victim.MaxHp > 0 && victim.DeathRestRule?.Invoke(death) == true)
            death = death with { WillRest = true };
        _coreRuntimeOperations.PushRuntimeFrame(death);
        return death;
    }
}
