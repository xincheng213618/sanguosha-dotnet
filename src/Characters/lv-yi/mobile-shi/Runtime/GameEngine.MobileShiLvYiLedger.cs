namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HasShiLyCapability =>
        _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiLyHuilv);

    // The virtual SongYan ExNihilo is cardless; the shared trick representation
    // chain recognizes it through this committed issued-use fold.
    private bool IsShiLySongyanDrawTwo(long id) => LifecycleCardUse(id) is
    {
        CardId: 0, CardKind: CardKind.DrawTwo, PhysicalCardIds: { Count: 0 }, ShiLySongyanOrigin: { UseId: var use },
    } && use == id;

    // A SongYan use issued inside an outer damage resolution shares the outer
    // card's Processing occupancy until that outer resolution finishes.
    private int[]? ShiLySongyanAncestorProcessing(long id) =>
        LifecycleCardUse(id) is { ShiLySongyanOrigin: { UseId: var use } origin } && use == id
            ? origin.OuterProcessing : null;

    // 恃势: players who ever dealt damage to the owner cannot respond to the
    // owner's cards until one of the owner's used cards has been responded to.
    private bool ShiLyShishiCannotRespond(int sourceSeat, int responderSeat)
    {
        if (!HasShiLyCapability || _winner != Winner.None || sourceSeat == responderSeat ||
            !IsValidPlayerSeat(sourceSeat) || !IsValidPlayerSeat(responderSeat) ||
            !HasRuntimeSkill(_players[sourceSeat], ShiLyShishi))
            return false;
        if (!ProgramEventHistory<ShiLyShishiRecordedEvent>().Any(entry =>
                entry.OwnerSeat == sourceSeat && entry.SourceSeat == responderSeat))
            return false;
        return ShiLyShishiUseUnanswered(sourceSeat);
    }

    // The latest relevant fact decides: a response to the owner's card lifts the
    // suppression, the owner's newest use declaration arms it again.
    private bool ShiLyShishiUseUnanswered(int ownerSeat)
    {
        var history = CompleteProgramEventHistory().ToArray();
        for (var i = history.Length - 1; i >= 0; i--)
        {
            if (history[i] is CardRespondedEvent { SourceSeat: var responder } && responder == ownerSeat)
                return false;
            if (history[i] is CardUseDeclaredEvent { SourceSeat: var user } && user == ownerSeat)
                return true;
        }
        return false;
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 68621)]
    private void ObserveShiLyShishiDamage(DamageAppliedEvent damage)
    {
        if (!HasShiLyCapability || damage.Amount <= 0 || damage.SourceLess ||
            !IsValidPlayerSeat(damage.SourceSeat) || !IsValidPlayerSeat(damage.TargetSeat) ||
            damage.SourceSeat == damage.TargetSeat || !HasRuntimeSkill(_players[damage.TargetSeat], ShiLyShishi) ||
            ProgramEventHistory<ShiLyShishiRecordedEvent>().Any(entry =>
                entry.OwnerSeat == damage.TargetSeat && entry.SourceSeat == damage.SourceSeat))
            return;
        AdvanceEventRulesAndQueueFact(new ShiLyShishiRecordedEvent(damage.TargetSeat, damage.SourceSeat, _roundNumber));
    }

    // ---- trigger qualification gates --------------------------------------------

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68600, SkillProgramEffectOp.ResolveShiLyHuilv)]
    private bool CanOfferShiLyHuilv(ProgramTriggerLegalityContext c) =>
        c.SkillId == ShiLvYiComposition.HuilvSkill && c.Owner.IsAlive && _roundNumber > 0 &&
        c.Context is { Window: SkillProgramTriggerWindow.PlayPhaseStarting, TargetSeat: { } actor } &&
        IsValidPlayerSeat(actor) && actor != c.OwnerSeat && _players[actor].IsAlive &&
        _winner == Winner.None && _cardZones.Count(CardLocation.DrawPile) > 0 &&
        ProgramEventHistory<ShiLyHuilvGrantedEvent>().Count(entry =>
            entry.OwnerSeat == c.OwnerSeat && entry.Round == _roundNumber) < 2;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68601, SkillProgramEffectOp.ResolveShiLyHuilvSettle)]
    private bool CanOfferShiLyHuilvSettle(ProgramTriggerLegalityContext c) =>
        c.SkillId == ShiLvYiComposition.HuilvSkill && c.Owner.IsAlive && _roundNumber > 0 &&
        c.Context is { Window: SkillProgramTriggerWindow.PlayEnding, TargetSeat: { } actor } &&
        IsValidPlayerSeat(actor) && _players[actor].IsAlive && _winner == Winner.None &&
        ShiLyOpenGrant(c.OwnerSeat, actor) is not null;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68602, SkillProgramEffectOp.ResolveShiLySongyan)]
    private bool CanOfferShiLySongyan(ProgramTriggerLegalityContext c) =>
        c.SkillId == ShiLvYiComposition.SongyanSkill && c.Owner.IsAlive && _winner == Winner.None &&
        c.Context is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } or
            { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 };
}
