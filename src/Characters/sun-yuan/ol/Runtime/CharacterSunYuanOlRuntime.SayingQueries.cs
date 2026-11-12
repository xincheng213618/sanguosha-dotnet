#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunYuanOlRuntime
{
    internal const string SayingObtainReason = "skill-program.sun-yuan.saying.obtain";
    internal bool TryGetSayingSlashRequest(ProgramSkillFrame f, out int actorSeat, out IReadOnlyList<int> targetSeats)
    {
        actorSeat = -1;
        targetSeats = [];
        if (f.SunYuan is not { Paid: true } state || _slashRequests.GetIdentity(f) is not { } identity ||
            identity.ActorSeat != state.Holder)
            return false;
        actorSeat = identity.ActorSeat;
        targetSeats = _slashRequests.GetTargets(f, actorSeat);
        return true;
    }

    internal bool IsSayingSlashRequest(ProgramSkillFrame f) => f.SunYuan?.Stage == SunYuanStage.Request && TryGetSayingSlashRequest(f, out _, out _);

}
