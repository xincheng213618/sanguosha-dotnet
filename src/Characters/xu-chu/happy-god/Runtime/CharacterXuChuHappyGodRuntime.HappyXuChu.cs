#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuChuHappyGodRuntime
{
    internal int HappyXuChuPreviousAward(ProgramSkillFrame frame) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().TakeWhile(fact => fact is not ZhengqingSettledEvent settled || settled.FrameId != frame.Id).OfType<ZhengqingSettledEvent>().Where(e => e.WinnerSeat == frame.OwnerSeat && ShenXuChuComposition.IsZhengqing(e.Source.SkillId)).Select(e => e.MaximumDamage).DefaultIfEmpty().Max();
    internal IReadOnlyList<ShenXuChuDraw> ShenXuChuRoundDraws(ProgramSkillFrame frame, int winner, int maximum)
    {
        if (winner < 0)
            return[];
        if (winner != frame.OwnerSeat)
            return[new(winner, 1), new(frame.OwnerSeat, 1)];
        if (frame.SkillId != ShenXuChuComposition.HappyZhengqing || maximum >= HappyXuChuPreviousAward(frame))
            return[new(frame.OwnerSeat, Math.Min(5, maximum))];
        // When the replacement condition fails, the winner and the owner each
        // keep their original one-card entitlement, even when they are one seat.
        return[new(frame.OwnerSeat, 1), new(frame.OwnerSeat, 1)];
    }

    internal static IReadOnlyList<ShenXuChuDraw> ShenXuChuTargetDraws(ProgramSkillFrame frame, bool marked) => marked && frame.SkillId == ShenXuChuComposition.Zhuangpo ? [new(frame.OwnerSeat, 1)] : [];
}
