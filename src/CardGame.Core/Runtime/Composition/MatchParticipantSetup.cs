namespace CardGame.Core;

// Used only during synchronous setup application; no engine or second participant store is retained.
internal interface IMatchParticipantSetupQueries
{
    int SeatCount { get; }
    bool IsHuman(int seat);
    bool IsGuanduCommander(int seat);
    IReadOnlyList<string>? SharedGods { get; }
    string? LeaderId { get; }
    GuanduBattleConfiguredEvent? GuanduConfiguration { get; }
}

internal interface IMatchParticipantSetupState
{
    void SetInitialActor(int? seat);
    void SetSharedDraft(IReadOnlyList<string> sharedGods, string leaderId);
    void SetGuanduConfiguration(GuanduBattleConfiguredEvent? configuration);
}

/// <summary>Applies mode recipes to the match's existing participants and draft stores.</summary>
internal sealed class MatchParticipantSetup(IProgramContentQueries content, IReadOnlyList<GeneralDefinition> generals)
{
    internal GeneralDefinition ResolveGeneral(string? id) => id is null ? RuntimeSkills.RuntimeRules_Core_CommonRules.CreateHiddenGeneral() :
        generals.SingleOrDefault(general => general.Id == id) ??
            throw new InvalidOperationException($"The mode setup references an unavailable general '{id}'.");

    private GeneralDefinition AddModeSetupSkills(GeneralDefinition general, IReadOnlyList<string> skillIds) =>
        skillIds.Count == 0 ? general : general with
        {
            Skills = general.Skills.Concat(skillIds.Select(id =>
                RuntimeSkills.RuntimeRules_Core_NationalSkills.ToRuntimeSkillDefinition(content.Skill(id)))).ToArray()
        };

    internal void ApplyPlan<TContext>(ModeSetupPlan plan, List<CharacterState> players,
        List<GeneralDefinition> availableGenerals, List<int> selectionOrder, TContext context)
        where TContext : struct, IMatchParticipantSetupQueries, IMatchParticipantSetupState
    {
        ArgumentNullException.ThrowIfNull(plan);
        if (players.Count != 0 || plan.Seats.Count != context.SeatCount ||
            !plan.Seats.Select(seat => seat.Seat).Order().SequenceEqual(Enumerable.Range(0, context.SeatCount)) ||
            plan.SelectionOrder.Any(seat => !IsValidSeat(seat, context.SeatCount)) ||
            plan.InitialActorSeat is { } initialActor && !IsValidSeat(initialActor, context.SeatCount))
            throw new InvalidOperationException("The mode setup must describe exactly the current match's seats.");
        foreach (var seat in plan.Seats.OrderBy(seat => seat.Seat))
        {
            if (seat.IsHuman != context.IsHuman(seat.Seat))
                throw new InvalidOperationException("The mode setup cannot change the match's human-seat ownership.");
            var player = new CharacterState
            {
                Seat = seat.Seat, Name = seat.Name, IsHuman = seat.IsHuman, Role = seat.Role,
                TeamId = seat.TeamId, TeamRevealed = seat.TeamRevealed, RoleRevealed = seat.RoleRevealed,
                NationalFactionId = seat.NationalFactionId, FactionRevealed = seat.FactionRevealed,
                General = AddModeSetupSkills(ResolveGeneral(seat.GeneralId), seat.ExtraGeneralSkillIds),
                GeneralSelected = seat.GeneralSelected, GeneralRevealed = seat.GeneralRevealed,
                SecondaryGeneral = seat.SecondaryGeneralId is null ? null : ResolveGeneral(seat.SecondaryGeneralId),
                SecondaryGeneralSelected = seat.SecondaryGeneralSelected,
                SecondaryGeneralRevealed = seat.SecondaryGeneralRevealed,
                MaxHp = seat.MaxHp, Hp = seat.Hp, IsAlive = seat.IsAlive, Armor = seat.InitialArmor,
                GeneralDraftCandidates = seat.GeneralDraftCandidateIds is null ? null :
                    Array.AsReadOnly(seat.GeneralDraftCandidateIds.Select(ResolveGeneral).ToArray())
            };
            foreach (var grant in seat.SkillGrants)
            {
                _ = content.Skill(grant.SkillId);
                player.SkillGrants.Grant(new SkillGrant(grant.GrantId, grant.SkillId,
                    grant.SkillInstanceId, grant.SourceId) { IsModeRule = grant.IsModeRule });
            }
            players.Add(player);
        }
        availableGenerals.AddRange(plan.AvailableGeneralIds.Select(ResolveGeneral));
        selectionOrder.AddRange(plan.SelectionOrder);
        context.SetInitialActor(plan.InitialActorSeat);
        if (plan.ShenwuSharedGodIds is { } shared)
            context.SetSharedDraft(Array.AsReadOnly(shared.ToArray()), plan.ShenwuLeaderGeneralId ??
                throw new InvalidOperationException("A shared god draft requires its configured encounter leader."));
        context.SetGuanduConfiguration(plan.GuanduConfiguration);
    }

    internal static ModeSeatSetupFacts CaptureSeat<TContext>(CharacterState player, TContext context)
        where TContext : struct, IMatchParticipantSetupQueries => new(player.Seat,
        player.IsHuman, player.Role, player.TeamId, player.NationalFactionId,
        player.GeneralSelected ? player.General.Id : null, player.GeneralSelected,
        player.SecondaryGeneralSelected ? player.SecondaryGeneral?.Id : null, player.SecondaryGeneralSelected)
        { IsGuanduCommander = context.IsGuanduCommander(player.Seat), IsAlive = player.IsAlive };

    internal static IReadOnlyList<ModeSeatSetupFacts> CaptureSeats<TContext>(
        IReadOnlyList<CharacterState> players, TContext context)
        where TContext : struct, IMatchParticipantSetupQueries =>
        Array.AsReadOnly(players.Select(player => CaptureSeat(player, context)).ToArray());

    internal IReadOnlyList<(GeneralDefinition General, string CandidateId)> GetOffers<TContext>(
        IMatchModeSetupFeature mode, CharacterState player, IReadOnlyList<GeneralDefinition> availableGenerals,
        IReadOnlyList<CharacterState> players, bool fullRoster, TContext context)
        where TContext : struct, IMatchParticipantSetupQueries
    {
        var plan = mode.GetGeneralOffers(new(CaptureSeat(player, context),
            availableGenerals.Select(general => general.Id), CaptureSeats(players, context),
            player.GeneralDraftCandidates?.Select(general => general.Id), fullRoster, context.SharedGods,
            context.LeaderId, context.GuanduConfiguration));
        if (plan.FrozenCandidateIds is { } frozen)
            player.GeneralDraftCandidates = Array.AsReadOnly(frozen.Select(ResolveGeneral).ToArray());
        return plan.Offers.Select(offer => (ResolveGeneral(offer.GeneralId), offer.CandidateId)).ToArray();
    }

    internal void ApplySelectedGeneral<TContext>(IMatchModeSetupFeature mode,
        CharacterState player, GeneralDefinition selected, TContext context)
        where TContext : struct, IMatchParticipantSetupQueries
    {
        var plan = mode.PrepareSelectedGeneral(CaptureSeat(player, context), selected.Id);
        player.General = AddModeSetupSkills(selected, plan.ExtraSkillIds);
        if (plan.MaximumHp is { } maximum) player.MaxHp = maximum;
        if (plan.Hp is { } hp) player.Hp = hp;
    }

    private static bool IsValidSeat(int seat, int count) => seat >= 0 && seat < count;
}
