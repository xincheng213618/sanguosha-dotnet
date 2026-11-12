#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChengYuMouRuntime
{
    internal MouCyUse? MouCyUse(ProgramSkillWindowContext c)
    {
        if (c.Window != SkillProgramTriggerWindow.CardUseCommitted || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } w || CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) != c.Window)
            return null;
        var a = w.Action;
        if (a.Type == CardActionType.Response)
            return w.CompletedResponseReturn is { IsCommitted: true } typed && _coreResponseUseCompletionOperations.IsCompletedResponseUse2(a, typed.Kind) ? new(w.Id, w.ParentFrameId, a, true) : null;
        return a.Type == CardActionType.Use && _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is { Action: { } actual } u && MouCySameAction(a, actual) && u.CardKind == a.EffectiveKind ? new(w.Id, w.ParentFrameId, a, false) : null;
    }

    internal static bool MouCySameAction(CardActionContext a, CardActionContext b) => (a.ActionId, a.ParentActionId, a.Type, a.ActorSeat, a.ProviderSeat, a.RequesterSeat, a.ResponderSeat, a.OpponentSeat, a.EffectiveKind, a.EffectiveSuit, a.EffectiveRank) == (b.ActionId, b.ParentActionId, b.Type, b.ActorSeat, b.ProviderSeat, b.RequesterSeat, b.ResponderSeat, b.OpponentSeat, b.EffectiveKind, b.EffectiveSuit, b.EffectiveRank) && a.PhysicalCards.SequenceEqual(b.PhysicalCards) && a.ConversionChain.SequenceEqual(b.ConversionChain);
    internal void ApplyMouCyInvalidation(ProgramSkillFrame f)
    {
        if (!MouChengYuAnniversary.Valid(f) || f.MouChengYuAnniversary is not { Stage: MouCyStage.Invalidate, Use: { } u } || MouCyUse(f.WindowContext!)is not { } live || live.WindowId != u.WindowId || live.ParentId != u.ParentId || !MouCySameAction(live.Action, u.Action))
            throw new InvalidOperationException("Shizha cannot invalidate a different committed native use.");
        if (!u.Response)
            _nativeCardActionInvalidations.IssueUse(u.ParentId, u.Action, new(SimpleCancellation: new(
                NativeSimpleCardCancellationBoundary.EarlySimple, [SimpleCardUseEffect.EquipmentPlacement],
                new("skill-program.mou:shizha.ineffective"), Priority: 10)));
        else if (u.Action.EffectiveKind == CardKind.Dodge)
            _nativeCardActionInvalidations.IssueDodge(u.ParentId, u.Action);
        else if (u.Action.EffectiveKind == CardKind.Nullification && _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().SingleOrDefault(p => p.Id == u.ParentId)is { NamedAmbushResponse: { } r } counter && r.ActionId == u.Action.ActionId)
            _nativeCardActionInvalidations.IssueCounterspell(counter.Id, u.Action);
        else
            throw new InvalidOperationException("Shizha response lost its original typed native owner.");
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动【识诈】，令【{(CardCatalog.Get(u.Action.EffectiveKind).DisplayName)}】无效。", f.OwnerSeat, u.Action.ActorSeat);
    }

    internal bool HasMouCyMatchingName(int actor, CardKind kind) => HasMouChengYuAnniversary && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Any(owner => _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCommitted).Any(b => b.SkillId == MouChengYuAnniversaryRuntime.Shizha && b.Trigger.Id == "consume-name" && MouChengYuAnniversary.Names(owner.Seat, b.SkillInstanceId).Any(n => n.Name == CoreGameEngineNamedAmbushRuntime.AmbushCardName(kind))));
    internal bool TryMouCyEquipmentCommitted(long id, Card equipment) => HasMouCyMatchingName(_coreCardUseLifecycleOperations.LifecycleCardUse(id)!.SourceSeat, equipment.Kind) && _coreCardUseLifecycleOperations.TryBeginCommittedCardUse(id, ProgramCardContinuation.CommittedSimpleCard, simple: new(equipment.Id, SimpleCardUseEffect.EquipmentPlacement));
}
