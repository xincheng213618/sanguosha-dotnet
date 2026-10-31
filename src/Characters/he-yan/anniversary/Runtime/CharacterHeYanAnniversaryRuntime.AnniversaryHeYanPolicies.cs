#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeYanAnniversaryRuntime
{
    internal bool IsAnniversaryHeYanHandUseForbidden(int seat, IReadOnlyList<int> ids) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryHeYanYachai) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHeYanHandUseBannedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && e.AffectedSeat == seat) && ids.Any(id => id > 0 && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(seat));
    internal bool HeYanResponseIsUse(int actor, CardKind kind, bool dying = false) => dying || kind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[actor], kind);
    [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 44200)]
    internal bool HeYanMaterialBan(ProgramCardMaterialQuery q) => (!q.IsResponse || HeYanResponseIsUse(q.ActorSeat, q.Kind)) && IsAnniversaryHeYanHandUseForbidden(q.ActorSeat, q.CardIds.ToArray());
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 44200)]
    internal bool HeYanNativeResponseBan(ProgramNativeResponseQuery q) => HeYanResponseIsUse(q.Provider.Seat, q.Kind, q.DyingUse) && IsAnniversaryHeYanHandUseForbidden(q.Provider.Seat, q.CardIds);
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 44200)]
    internal bool HeYanViewAsBan(ProgramViewAsLegalityQuery q) => (!q.ForResponse || q.RequestedUseTarget is not null || HeYanResponseIsUse(q.Owner.Seat, q.Kind, q.DyingUse)) && IsAnniversaryHeYanHandUseForbidden(q.Owner.Seat, q.CardIds);
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 44200)]
    internal bool HeYanLegalUseBan(ProgramLegalActionQuery q) => q.Action.Kind is not (LegalActionKind.Recast or LegalActionKind.UseProgramSkill or LegalActionKind.EndPlay or LegalActionKind.UseEquipmentEffect) && q.Action.CardId is { } id && IsAnniversaryHeYanHandUseForbidden(q.Actor.Seat, [id]);
    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 44200)]
    internal void HeYanSubmittedUseBan(ProgramSubmittedUseQuery q)
    {
        if (IsAnniversaryHeYanHandUseForbidden(q.ActorSeat, q.CardIds))
            throw new InvalidOperationException("Yachai prohibits using Hand materials for the remainder of this actual turn.");
    }
}
