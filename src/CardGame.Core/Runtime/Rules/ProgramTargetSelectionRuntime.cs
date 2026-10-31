namespace CardGame.Core;

internal interface IProgramTargetQueries : IProgramSelectionQuery
{
    IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, SkillProgramTargetKind targetKind,
        ProgramSkillWindowContext? windowContext = null, PlayerMarkerKind? marker = null,
        ProgramParticipantReference? actorReference = null);
    bool IsProgramTargetEligible(int ownerSeat, SkillProgramTargetKind targetKind,
        IReadOnlyList<CardZoneKind> zones, int targetSeat, PlayerMarkerKind? marker = null,
        ProgramSkillWindowContext? windowContext = null, ProgramParticipantReference? actorReference = null);
}

/// <summary>Owns live target domains, prompt combinations and submission on the original program frame.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed partial class ProgramTargetSelectionRuntime(IProgramRuntimeFrames frames, ICardZoneQueries zones,
    IMatchParticipantStateAccess state, IProgramContentQueries content,
    IProgramTurnQueries turn, IProgramRuntimePrompts prompts, IProgramRuntimeEvents events,
    IProgramPindianPolicyQueries pindianPolicies,
    IProgramTargetFacts facts, IProgramTargetResolvers resolvers, IProgramTargetSelectionProtocols protocols) : IProgramTargetQueries
{
    private IReadOnlyList<CharacterState> Players => state.Players;
    private CharacterState Player(int seat) => state.At(seat);
    private IReadOnlyList<Card> Hand(CharacterState player) => zones.CardsAt(CardLocation.Hand(player.Seat));
    private IReadOnlyList<Card> Equipment(CharacterState player) => zones.CardsAt(CardLocation.Equipment(player.Seat));
    private IReadOnlyList<Card> Judgment(CharacterState player) => zones.CardsAt(CardLocation.Judgment(player.Seat));

    public IReadOnlyList<int> GetProgramTargetSeats(
        int ownerSeat,
        SkillProgramTargetKind targetKind,
        ProgramSkillWindowContext? windowContext = null,
        PlayerMarkerKind? marker = null,
        ProgramParticipantReference? actorReference = null)
    {
        if (targetKind == SkillProgramTargetKind.OtherLivingVirtualDuelTarget)
            return facts.VirtualDuelTargets(Player(ownerSeat));
        if (targetKind == SkillProgramTargetKind.LivingWhoseAttackRangeIncludesLord)
        {
            var lord = Players.SingleOrDefault(p => p.IsAlive && p.Role == Role.Lord);
            return lord is null ? [] : Players.Where(p => p.IsAlive && p.Seat != lord.Seat && facts.IsWithinAttackRange(p.Seat, lord.Seat)).Select(p => p.Seat).ToArray();
        }
        if (targetKind == SkillProgramTargetKind.OtherLivingNonLord)
            return Players.Where(p => p.IsAlive && p.Seat != ownerSeat && p.Role != Role.Lord).Select(p => p.Seat).ToArray();
        if (resolvers.TryResolve(new(ownerSeat, targetKind), out var resolvedTargets))
            return resolvedTargets;
        if (targetKind == SkillProgramTargetKind.CurrentTurnPlayer)
            return Player(turn.CurrentSeat).IsAlive ? [turn.CurrentSeat] : [];
        windowContext ??= frames.Top switch
        {
            ProgramSkillFrame program => program.WindowContext,
            ProgramCardTriggerWindowFrame cardWindow when cardWindow.CandidateIndex < cardWindow.Candidates.Count =>
                facts.CreateCardActionProgramContext(cardWindow, cardWindow.Candidates[cardWindow.CandidateIndex]),
            TurnEndingBoundaryFrame ending when ending.ItemIndex < ending.Items.Count &&
                ending.Items[ending.ItemIndex].Candidate is { } candidate =>
                facts.CreateTurnEndingProgramContext(ending, candidate, ending.Items[ending.ItemIndex].Facts),
            _ => null
        };
        if (facts.CaoSongSourceTargetSeats(ownerSeat, targetKind, windowContext) is { } caoSongTargets)
            return caoSongTargets;
        if (targetKind == SkillProgramTargetKind.EventTargetWithHand)
            return windowContext?.TargetSeat is { } eventSeat && Player(eventSeat).IsAlive &&
                Hand(Player(eventSeat)).Count > 0 ? [eventSeat] : [];
        if (targetKind == SkillProgramTargetKind.EventTarget)
        {
            if (facts.IsExactHellPreparationTargetContext(ownerSeat, windowContext)) return [windowContext!.TargetSeat!.Value];
            if (windowContext is { Window: SkillProgramTriggerWindow.DyingResponse, TargetSeat: { } victimSeat })
                return facts.ActiveDying is { } dying && dying.FrameId == windowContext.ParentFrameId &&
                    dying.VictimSeat == victimSeat && dying.ResponderSeat == ownerSeat &&
                    Player(victimSeat).IsAlive && Player(victimSeat).Hp <= 0 ? [victimSeat] : [];
            if (windowContext is { Window: SkillProgramTriggerWindow.CharacterTurnedOver or SkillProgramTriggerWindow.CharacterTurnedFaceUp or
                SkillProgramTriggerWindow.CharacterEnteredChain or SkillProgramTriggerWindow.DyingEntering or
                SkillProgramTriggerWindow.CardsMoved or SkillProgramTriggerWindow.CardsGained or
                SkillProgramTriggerWindow.DiscardPileReceived, TargetSeat: { } changedSeat })
                return Player(changedSeat).IsAlive ? [changedSeat] : [];
            if (windowContext is { Window: SkillProgramTriggerWindow.TurnEnding, TargetSeat: { } endingSeat } &&
                frames.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(item =>
                    item.Id == windowContext.ParentFrameId && item.OwnerSeat == endingSeat) is not null)
                return Player(endingSeat).IsAlive ? [endingSeat] : [];
            if (windowContext is
                {
                    Window: SkillProgramTriggerWindow.AfterDamageApplied or
                        SkillProgramTriggerWindow.DamageAppliedBeforeDying,
                    TargetSeat: { } damageSeat
                })
                return Player(damageSeat).IsAlive ? [damageSeat] : [];
            var actionId = windowContext?.CardUse?.CardActionId ??
                throw new InvalidOperationException("Event-target selection requires card-action context.");
            var parent = frames.OfType<ProgramCardTriggerWindowFrame>()
                .SingleOrDefault(item => item.Id == windowContext.ParentFrameId && item.Action.ActionId == actionId) ??
                throw new InvalidOperationException("Event-target selection lost its parent card action.");
            var frozenTargets = parent.Action.TargetSeats.Count > 0
                ? parent.Action.TargetSeats
                : parent.Candidates.Select(candidate => candidate.OpponentSeat)
                    .Where(seat => seat >= 0).Distinct().ToArray();
            return frozenTargets.Distinct().Where(seat => Player(seat).IsAlive).Order().ToArray();
        }
        if (targetKind == SkillProgramTargetKind.OtherLegalCurrentCardTarget)
            return facts.GetProgramCardUseRoleTargets(ownerSeat, windowContext);
        if (targetKind == SkillProgramTargetKind.CurrentArrowBarrageTargets)
            return facts.GetCurrentArrowBarrageTargets(ownerSeat, windowContext);
        if (targetKind == SkillProgramTargetKind.CurrentCardUseTargets)
        {
            if (windowContext is not
                {
                    Window: SkillProgramTriggerWindow.CardUseBeforeTargetEffects or SkillProgramTriggerWindow.CardUseTargetsFinalized,
                    CardUse: { } cardUse
                } ||
                frames.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault() is not { } window ||
                window.Id != windowContext.ParentFrameId ||
                window.Action.ActionId != cardUse.CardActionId ||
                window.ParentFrameId != cardUse.ParentCardUseFrameId ||
                window.Action.Type != CardActionType.Use ||
                frames.OfType<CardUseFrame>().SingleOrDefault(item => item.Id == window.ParentFrameId) is not { } parent ||
                parent.Action?.ActionId != cardUse.CardActionId)
                throw new InvalidOperationException("Current card-use targets require their frozen active card action.");
            return (cardUse.DesignatedTargetSeats ?? window.Action.EffectiveDesignatedTargetSeats)
                .Distinct().Where(seat => Player(seat).IsAlive &&
                    parent.IneffectiveTargetSeats?.Contains(seat) != true).ToArray();
        }
        if (targetKind == SkillProgramTargetKind.OtherLivingInBoundParticipantAttackRange)
        {
            if (actorReference is not { } reference ||
                frames.Top is not ProgramSkillFrame frame)
                throw new InvalidOperationException("Attack-range target selection lost its bound participant.");
            var actorSeat = facts.ResolveProgramParticipant(frame, reference);
            if (!Player(actorSeat).IsAlive) return [];
            return Players.Where(target => target.IsAlive && target.Seat != actorSeat &&
                    facts.IsWithinAttackRange(actorSeat, target.Seat))
                .Select(target => target.Seat).Order().ToArray();
        }
        if (targetKind is SkillProgramTargetKind.MaximumAttributedMarker or SkillProgramTargetKind.MaximumOtherAttributedMarker)
        {
            if (marker is not { } markerKind)
                throw new InvalidOperationException("Maximum attributed-marker selection requires a marker kind.");
            return GameRules.GetMaximumMarkerCandidates(Players
                .Where(player => targetKind != SkillProgramTargetKind.MaximumOtherAttributedMarker || player.Seat != ownerSeat)
                .Select(player =>
                new PlayerMarkerCandidateState(
                    player.Seat,
                    player.IsAlive,
                    facts.GetMarkerSourceCount(player, markerKind, ownerSeat))));
        }
        return Players
            .Where(target => target.IsAlive && targetKind switch
            {
                SkillProgramTargetKind.OtherLivingHpAtLeastOwner => target.Seat != ownerSeat && target.HasHpValue &&
                    Player(ownerSeat).HasHpValue && target.Hp >= Player(ownerSeat).Hp,
                SkillProgramTargetKind.OtherLivingChained => target.Seat != ownerSeat && target.IsChained,
                SkillProgramTargetKind.AnyLivingChained => target.IsChained,
                SkillProgramTargetKind.PublicTeamEnemies => facts.PublicTeamEnemy(ownerSeat, target),
                SkillProgramTargetKind.PublicTeamWoundedAllies => facts.SamePublicTeam(Player(ownerSeat), target) && facts.IsRuleWounded(target),
                SkillProgramTargetKind.OtherPublicTeamWoundedAllies => target.Seat != ownerSeat && facts.SamePublicTeam(Player(ownerSeat), target) && facts.IsRuleWounded(target),
                SkillProgramTargetKind.OtherPublicTeamAllies => facts.OtherRevealedPublicTeamAlly(Player(ownerSeat), target),
                SkillProgramTargetKind.PublicTeamUnchainedEnemies => facts.PublicTeamEnemy(ownerSeat, target) && !target.IsChained,
                SkillProgramTargetKind.PublicTeamEnemiesWithFaceDownAlly => facts.PublicTeamEnemy(ownerSeat, target) &&
                    Players.Any(p => p.IsAlive && p.IsFaceDown && facts.SamePublicTeam(Player(ownerSeat), p)),
                SkillProgramTargetKind.OtherLiving => target.Seat != ownerSeat,
                SkillProgramTargetKind.OtherLivingShanheS5Jixian => facts.IsShanheS5JixianTarget(ownerSeat, target),
                SkillProgramTargetKind.OtherLivingMale =>
                    target.Seat != ownerSeat && target.Gender == GeneralGender.Male,
                SkillProgramTargetKind.AnyLivingMale => target.Gender == GeneralGender.Male,
                SkillProgramTargetKind.OtherWoundedMale =>
                    target.Seat != ownerSeat && target.Gender == GeneralGender.Male &&
                    facts.IsRuleWounded(target),
                SkillProgramTargetKind.OtherLivingInAttackRange =>
                    target.Seat != ownerSeat &&
                    facts.IsWithinAttackRange(ownerSeat, target.Seat),
                SkillProgramTargetKind.OtherLivingWhoseAttackRangeIncludesOwner =>
                    target.Seat != ownerSeat &&
                    facts.IsWithinAttackRange(target.Seat, ownerSeat),
                SkillProgramTargetKind.OtherLivingWithQinggangSword =>
                    target.Seat != ownerSeat &&
                    facts.HasWeaponAbility(target, CardKind.QinggangSword),
                SkillProgramTargetKind.OtherLivingSlashable =>
                    facts.CanUseProvidedSlashTarget(Player(ownerSeat), target),
                SkillProgramTargetKind.SlashRedirectable =>
                    facts.IsSlashRedirectTarget(ownerSeat, target.Seat),
                SkillProgramTargetKind.OtherLivingVirtualSlashTarget =>
                    target.Seat != ownerSeat &&
                    (windowContext?.Window != SkillProgramTriggerWindow.RoundStarting ||
                     Player(ownerSeat).IsAlive &&
                     !facts.IsCardUseForbidden(ownerSeat, CardKind.Slash, CardActionType.Use) &&
                     !facts.HasTurnCardTargetRestriction(ownerSeat, SkillProgramCardTargetRestriction.SelfOnly)) &&
                    !facts.IsDirectedCardTargetProhibited(ownerSeat, target.Seat, CardKind.Slash) &&
                    !facts.IsSlashProhibited(target),
                SkillProgramTargetKind.OtherLivingDelayedTrickTarget =>
                    facts.IsProgramDiscardedDelayedTrickTarget(ownerSeat, target, windowContext),
                SkillProgramTargetKind.AnyLivingWithHand => Hand(target).Count > 0,
                SkillProgramTargetKind.OtherLivingWithHand =>
                    target.Seat != ownerSeat && Hand(target).Count > 0,
                SkillProgramTargetKind.OtherLivingWithMoreHandCards => target.Seat != ownerSeat && Hand(target).Count > Hand(Player(ownerSeat)).Count,
                SkillProgramTargetKind.OtherLivingWithFewerHandCardsAndOwnedCards => target.Seat != ownerSeat &&
                    Hand(target).Count < Hand(Player(ownerSeat)).Count && Hand(target).Count + Equipment(target).Count > 0,
                SkillProgramTargetKind.OtherLivingWithFewerHandCards =>
                    target.Seat != ownerSeat && Hand(target).Count > 0 &&
                    Hand(target).Count < Hand(Player(ownerSeat)).Count,
                SkillProgramTargetKind.OtherLivingWuFactionWithHand =>
                    target.Seat != ownerSeat && Hand(target).Count > 0 &&
                    facts.GetEffectiveFactionId(target) == "wu",
                SkillProgramTargetKind.OtherLivingWithHandOrEquipment =>
                    target.Seat != ownerSeat && Hand(target).Count + Equipment(target).Count > 0,
                SkillProgramTargetKind.OtherLivingWithDiscardableHandOrEquipment =>
                    target.Seat != ownerSeat && facts.HasDiscardableHeBy(ownerSeat, target.Seat),
                SkillProgramTargetKind.OtherLivingEmptyHand =>
                    target.Seat != ownerSeat && Hand(target).Count == 0,
                SkillProgramTargetKind.OtherLivingHandAtMostOwner =>
                    target.Seat != ownerSeat && Hand(target).Count <= Hand(Player(ownerSeat)).Count,
                SkillProgramTargetKind.OtherLivingWithHandHpGreaterThanOwner =>
                                    target.Seat != ownerSeat && Hand(target).Count > 0 &&
                                    target.HasHpValue && Player(ownerSeat).HasHpValue && target.Hp > Player(ownerSeat).Hp,
                SkillProgramTargetKind.OtherLivingHandAtLeastOwner =>
                                    target.Seat != ownerSeat && Hand(target).Count > 0 &&
                                    Hand(target).Count >= Hand(Player(ownerSeat)).Count,
                SkillProgramTargetKind.OtherLivingUnequalHandPair => target.Seat != ownerSeat &&
                    Players.Any(peer => peer.IsAlive && peer.Seat != ownerSeat && peer.Seat != target.Seat &&
                        Hand(peer).Count != Hand(target).Count),
                SkillProgramTargetKind.OtherLivingPair => target.Seat != ownerSeat &&
                    Players.Count(peer => peer.IsAlive && peer.Seat != ownerSeat) >= 2,
                SkillProgramTargetKind.LivingPairDistinct =>
                    Players.Count(peer => peer.IsAlive) >= 2,
                SkillProgramTargetKind.EquipmentExchangePair =>
                    Players.Any(peer => facts.IsEquipmentExchangePair(ownerSeat, target.Seat, peer.Seat)),
                SkillProgramTargetKind.OtherLivingLeastHandCount => target.Seat != ownerSeat &&
                    Hand(target).Count ==
                    Players.Where(peer => peer.IsAlive && peer.Seat != ownerSeat)
                        .Select(peer => Hand(peer).Count).Min(),
                SkillProgramTargetKind.OtherLivingAtDistanceOne =>
                    target.Seat != ownerSeat && facts.GetCombatDistance(ownerSeat, target.Seat) == 1,
                SkillProgramTargetKind.AnyLivingHighestHp => target.HasHpValue && target.Hp == Players.Where(player => player.IsAlive && player.HasHpValue).Max(player => player.Hp),
                SkillProgramTargetKind.AnyLivingLeastHandCount => Hand(target).Count == Players.Where(player => player.IsAlive).Min(player => Hand(player).Count),
                SkillProgramTargetKind.OtherLivingHighestHand => target.Seat != ownerSeat && Hand(target).Count == Players.Where(player => player.IsAlive && player.Seat != ownerSeat).Max(player => Hand(player).Count),
                SkillProgramTargetKind.AnyLivingHighestHand => Hand(target).Count == Players.Where(player => player.IsAlive).Max(player => Hand(player).Count),
                SkillProgramTargetKind.AnyLiving => true,
                SkillProgramTargetKind.OtherWounded =>
                    target.Seat != ownerSeat && facts.IsRuleWounded(target),
                SkillProgramTargetKind.AnyWounded => facts.IsRuleWounded(target),
                SkillProgramTargetKind.AnyLivingHandBelowMaxHp =>
                    target.HasHpValue && Hand(target).Count < target.MaxHp,
                SkillProgramTargetKind.OtherLivingExceptSource =>
                    target.Seat != ownerSeat && target.Seat != windowContext?.SourceSeat,
                SkillProgramTargetKind.EventSource =>
                    target.Seat == windowContext?.SourceSeat,
                _ => false
            })
            .Select(target => target.Seat)
            .Order()
            .ToArray();
    }

    public bool IsProgramTargetEligible(int ownerSeat, SkillProgramTargetKind targetKind,
        IReadOnlyList<CardZoneKind> zones, int targetSeat, PlayerMarkerKind? marker = null,
        ProgramSkillWindowContext? windowContext = null,
        ProgramParticipantReference? actorReference = null)
    {
        if (!GetProgramTargetSeats(ownerSeat, targetKind, windowContext, marker, actorReference).Contains(targetSeat)) return false;
        if (zones.Count == 0) return true;
        var target = Player(targetSeat);
        return zones.Any(zone => zone switch
        {
            CardZoneKind.Hand => Hand(target).Count > 0,
            CardZoneKind.Equipment => Equipment(target).Count > 0,
            CardZoneKind.Judgment => Judgment(target).Count > 0,
            _ => false
        });
    }

}
