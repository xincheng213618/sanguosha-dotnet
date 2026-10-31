using System.Collections.ObjectModel;

namespace CardGame.Core;

internal sealed class SkillStateApplications(
    IReadOnlyDictionary<string, IReadOnlyList<SkillStateDeclaration>> templates,
    IReadOnlyList<CharacterState> players, SkillAvailability availability, ProgramSkillChangeRuntime changes,
    IProgramEventJournalQueries journal, IProgramRuntimeFrames frames)
{
    internal static IReadOnlyDictionary<string, IReadOnlyList<SkillStateDeclaration>> Compile(ContentRegistry content) =>
        new ReadOnlyDictionary<string, IReadOnlyList<SkillStateDeclaration>>(content.Skills.Values
            .Where(skill => skill.Program?.SkillStates.Count > 0)
            .ToDictionary(skill => skill.Id, skill => skill.Program!.SkillStates, StringComparer.Ordinal));

    internal static void Apply(int ownerSeat, SkillGrant grant,
        IReadOnlyDictionary<string, IReadOnlyList<SkillStateDeclaration>> templates, IReadOnlyList<CharacterState> recipients)
    {
        if (!templates.TryGetValue(grant.SkillId, out var declarations)) return;
        var owner = recipients.FirstOrDefault(player => player.Seat == ownerSeat);
        if (!AcceptedSource(owner, grant)) return;
        for (var index = 0; index < declarations.Count; index++)
        {
            var declaration = declarations[index];
            if (declaration.On is not null) continue;
            var source = new SkillStateDeclarationSource(grant.SkillId, grant.GrantId, index, declaration);
            foreach (var target in recipients)
            {
                if (declaration.Target == SkillStateTarget.Owner && target.Seat != ownerSeat) continue;
                var id = $"declared-skill-state:{ownerSeat}:{grant.GrantId.Length}:{grant.GrantId}:{index}:{target.Seat}";
                if (target.SkillGrants.Restrictions.Any(state => state.Id == id)) continue;
                target.SkillGrants.SetRestriction(new(id, id, declaration.State.Action, declaration.State.Match,
                    new(ownerSeat, target.Seat), new(null, []), declaration.State.While, source));
            }
        }
    }

    internal void ApplyEvent(IGameEvent fact, SkillStateBoundaryTiming timing)
    {
        var boundary = fact switch
        {
            TurnStartedEvent turn => new SkillStateBoundary(SkillStateBoundaryEvent.TurnStarted, turn.ActorSeat, timing, turn.TurnNumber),
            TurnEndedEvent turn => new SkillStateBoundary(SkillStateBoundaryEvent.TurnEnded, turn.ActorSeat, timing, turn.TurnNumber),
            PlayerDyingEvent dying => new SkillStateBoundary(SkillStateBoundaryEvent.DyingStarted, null, timing, dying.ResolutionId),
            DyingResolvedEvent dying => new SkillStateBoundary(SkillStateBoundaryEvent.DyingEnded, null, timing, dying.ResolutionId),
            _ => null
        };
        if (boundary is null) return;
        changes.ProcessSkillStateBoundary(boundary);
        if (timing != SkillStateBoundaryTiming.Before || boundary.Event is not
            (SkillStateBoundaryEvent.TurnStarted or SkillStateBoundaryEvent.DyingStarted)) return;
        var eventTarget = fact is PlayerDyingEvent entered ? entered.VictimSeat : (int?)null;
        var sources = players.Where(player => player.IsAlive && player.SkillGrants.Grants.Any(grant => grant.IsEnabled &&
                templates.TryGetValue(grant.SkillId, out var declarations) && declarations.Any(declaration => declaration.On?.Event == boundary.Event)))
            .SelectMany(player => availability.Query(player).EffectiveGrants.Where(grant =>
                templates.TryGetValue(grant.SkillId, out var declarations) && declarations.Any(declaration => declaration.On?.Event == boundary.Event))
                .Select(grant => (player.Seat, Grant: grant))).ToArray();
        foreach (var (ownerSeat, grant) in sources) ApplyOn(ownerSeat, grant, boundary, eventTarget);
    }

    internal void Admit(int ownerSeat, SkillGrant grant)
    {
        Apply(ownerSeat, grant, templates, players);
        if (!templates.TryGetValue(grant.SkillId, out var declarations) || !declarations.Any(declaration => declaration.On is not null)) return;
        var owner = players.FirstOrDefault(player => player.Seat == ownerSeat);
        if (!AcceptedSource(owner, grant) || owner is not { IsAlive: true } ||
            !availability.Query(owner).EffectiveGrants.Any(value => value.GrantId == grant.GrantId &&
                value.SkillInstanceId == grant.SkillInstanceId && value.SkillId == grant.SkillId)) return;
        var turn = changes.AdvancingFacts.FirstOrDefault(fact => fact is TurnStartedEvent or TurnEndedEvent) ??
            journal.LastMatching(fact => fact is TurnStartedEvent or TurnEndedEvent);
        if (turn is TurnStartedEvent started)
            ApplyOn(ownerSeat, grant, new(SkillStateBoundaryEvent.TurnStarted, started.ActorSeat,
                SkillStateBoundaryTiming.Before, started.TurnNumber), null);
        var resolved = journal.Complete().Concat(changes.AdvancingFacts).OfType<DyingResolvedEvent>()
            .Select(fact => fact.ResolutionId).ToHashSet();
        foreach (var dying in frames.OfType<DyingFrame>().Where(frame => !resolved.Contains(frame.Id)))
            ApplyOn(ownerSeat, grant, new(SkillStateBoundaryEvent.DyingStarted, null,
                SkillStateBoundaryTiming.Before, dying.Id), dying.VictimSeat);
    }

    private void ApplyOn(int ownerSeat, SkillGrant grant, SkillStateBoundary boundary, int? eventTarget)
    {
        if (!templates.TryGetValue(grant.SkillId, out var declarations)) return;
        for (var index = 0; index < declarations.Count; index++)
        {
            var declaration = declarations[index];
            if (declaration.On is not { } on || on.Event != boundary.Event || on.Timing != boundary.Timing ||
                on.Participant == SkillStateSubject.Owner && boundary.Seat != ownerSeat) continue;
            foreach (var target in players)
            {
                if (declaration.Target == SkillStateTarget.Owner && target.Seat != ownerSeat) continue;
                var id = $"declared-skill-state:{ownerSeat}:{grant.GrantId.Length}:{grant.GrantId}:{index}:{target.Seat}:{boundary.Event}:{boundary.Occurrence}";
                if (target.SkillGrants.Restrictions.Any(state => state.Id == id)) continue;
                var context = new SkillStateConditionContext(ownerSeat, target.Seat,
                    IssuedTurnNumber: boundary.Event == SkillStateBoundaryEvent.TurnStarted ? (int)boundary.Occurrence : changes.ActualTurnNumber,
                    IssuedDyingFrameId: boundary.Event == SkillStateBoundaryEvent.DyingStarted ? boundary.Occurrence : null,
                    EventTargetSeat: eventTarget);
                if (declaration.When is not null && ProgramExpressions.EvaluateBool(declaration.When,
                    availability.CreateConditionWorld(context, target)) != true) continue;
                var lifetime = changes.CreateApplicationLifetime(ownerSeat, target.Seat, declaration.State.Until, context);
                if (lifetime is null) continue;
                target.SkillGrants.SetRestriction(new(id, id, declaration.State.Action, declaration.State.Match,
                    context, lifetime, declaration.State.While, new(grant.SkillId, grant.GrantId, index, declaration)));
            }
        }
    }

    private static bool AcceptedSource(CharacterState? owner, SkillGrant grant) => owner is not null && grant.IsEnabled &&
        (grant.SourceId != CharacterState.PrimarySkillSource || owner.GeneralSelected && owner.GeneralRevealed) &&
        (grant.SourceId != CharacterState.SecondarySkillSource || owner.SecondaryGeneralSelected && owner.SecondaryGeneralRevealed);

    internal static IEnumerable<ContentSkillDefinition> AppliedDefinitions(CharacterSkillSet skills, ContentRegistry content) =>
        skills.Restrictions.Where(state => state.DeclarationSource is not null)
            .Select(state => state.DeclarationSource!.SkillId).Distinct(StringComparer.Ordinal).Select(content.GetSkill);
}
