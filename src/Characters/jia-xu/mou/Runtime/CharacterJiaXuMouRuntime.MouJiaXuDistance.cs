#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiaXuMouRuntime
{
    internal bool MouJxHandDistanceQuery(CharacterState actor, IReadOnlyList<int>? physical)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJiaXu: { Yin: true, Stage: MouJiaXuStage.Use or MouJiaXuStage.IssuingUse } r } f || r.Holder != actor.Seat || !actor.IsAlive || !MouJiaXu.Valid(f))
            return false;
        if (physical is { Count: > 0 })
            return physical is [var id] && IsMouJxExactCardQuery(actor, id);
        return r.Stage == MouJiaXuStage.IssuingUse ? r.Native is { UseId: 0, ActionId: 0 } n && IsMouJxExactCardQuery(actor, n.CardId) : r.Remaining.Any(id => IsMouJxExactCardQuery(actor, id));
    }

    internal bool MouJxBoundDistance(long id, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouJiaXuFumou: { } n } u && n.Actor == actor && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is { } parent && MouJxNativeRidesOn(u, parent);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 36500)]
    internal bool MouJxCardDistance(ProgramCardDistanceQuery q) => q.CardUseFrameId is { } id ? MouJxBoundDistance(id, q.Actor.Seat) : MouJxHandDistanceQuery(q.Actor, q.PhysicalCardIds);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 36500)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 36500)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 36500)]
    internal bool MouJxSlashDistance(ProgramSlashDistanceQuery q) => q.ExistingUseFrameId is { } id ? MouJxBoundDistance(id, q.Actor.Seat) : MouJxHandDistanceQuery(q.Actor, q.PhysicalCardIds ?? (q.PhysicalCard is { } card ? [card.Id] : null));
}
