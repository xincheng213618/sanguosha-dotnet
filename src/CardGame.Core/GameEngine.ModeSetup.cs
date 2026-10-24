namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int? _modeInitialActorSeat;

    private IMatchModeSetupFeature ModeSetup => _modeRuntime.Find<IMatchModeSetupFeature>() ??
        throw new InvalidOperationException("The selected mode requires an explicitly installed setup feature.");

    private GeneralDefinition ResolveSetupGeneral(string? id) => id is null ? CreateHiddenGeneral() :
        _generalPool.SingleOrDefault(general => general.Id == id) ??
            throw new InvalidOperationException($"The mode setup references an unavailable general '{id}'.");

    private GeneralDefinition AddModeSetupSkills(GeneralDefinition general, IReadOnlyList<string> skillIds) =>
        skillIds.Count == 0 ? general : general with
        {
            Skills = general.Skills.Concat(skillIds.Select(id =>
                ToRuntimeSkillDefinition(_contentRegistry.GetSkill(id)))).ToArray()
        };

    private void ApplyModeSetupPlan(ModeSetupPlan plan)
    {
        ArgumentNullException.ThrowIfNull(plan);
        if (_players.Count != 0 || plan.Seats.Count != _playerCount ||
            !plan.Seats.Select(seat => seat.Seat).Order().SequenceEqual(Enumerable.Range(0, _playerCount)) ||
            plan.SelectionOrder.Any(seat => !IsValidPlayerSeat(seat)) ||
            plan.InitialActorSeat is { } initialActor && !IsValidPlayerSeat(initialActor))
            throw new InvalidOperationException("The mode setup must describe exactly the current match's seats.");
        foreach (var seat in plan.Seats.OrderBy(seat => seat.Seat))
        {
            if (seat.IsHuman != IsHumanControlledSeat(seat.Seat))
                throw new InvalidOperationException("The mode setup cannot change the match's human-seat ownership.");
            var player = new CharacterState
            {
                Seat = seat.Seat, Name = seat.Name, IsHuman = seat.IsHuman, Role = seat.Role,
                TeamId = seat.TeamId, TeamRevealed = seat.TeamRevealed, RoleRevealed = seat.RoleRevealed,
                NationalFactionId = seat.NationalFactionId, FactionRevealed = seat.FactionRevealed,
                General = AddModeSetupSkills(ResolveSetupGeneral(seat.GeneralId), seat.ExtraGeneralSkillIds),
                GeneralSelected = seat.GeneralSelected, GeneralRevealed = seat.GeneralRevealed,
                SecondaryGeneral = seat.SecondaryGeneralId is null ? null : ResolveSetupGeneral(seat.SecondaryGeneralId),
                SecondaryGeneralSelected = seat.SecondaryGeneralSelected,
                SecondaryGeneralRevealed = seat.SecondaryGeneralRevealed,
                MaxHp = seat.MaxHp, Hp = seat.Hp, IsAlive = seat.IsAlive,
                GeneralDraftCandidates = seat.GeneralDraftCandidateIds is null ? null :
                    Array.AsReadOnly(seat.GeneralDraftCandidateIds.Select(ResolveSetupGeneral).ToArray())
            };
            foreach (var grant in seat.SkillGrants)
            {
                _ = _contentRegistry.GetSkill(grant.SkillId);
                player.SkillGrants.Grant(new SkillGrant(grant.GrantId, grant.SkillId,
                    grant.SkillInstanceId, grant.SourceId) { IsModeRule = grant.IsModeRule });
            }
            _players.Add(player);
        }
        _availableGenerals.AddRange(plan.AvailableGeneralIds.Select(ResolveSetupGeneral));
        _selectionOrder.AddRange(plan.SelectionOrder);
        _modeInitialActorSeat = plan.InitialActorSeat;
        if (plan.ShenwuSharedGodIds is { } shared)
            _shenwuDraft = new(Array.AsReadOnly(shared.ToArray()), plan.ShenwuLeaderGeneralId ??
                throw new InvalidOperationException("A shared god draft requires its configured encounter leader."));
        _guanduInitialConfiguration = plan.GuanduConfiguration;
    }

    private ModeSeatSetupFacts CaptureModeSetupSeat(CharacterState player) => new(player.Seat,
        player.IsHuman, player.Role, player.TeamId, player.NationalFactionId,
        player.GeneralSelected ? player.General.Id : null, player.GeneralSelected,
        player.SecondaryGeneralSelected ? player.SecondaryGeneral?.Id : null, player.SecondaryGeneralSelected)
        { IsGuanduCommander = IsGuanduCommander(player.Seat), IsAlive = player.IsAlive };

    private IReadOnlyList<ModeSeatSetupFacts> CaptureModeSetupSeats() =>
        Array.AsReadOnly(_players.Select(CaptureModeSetupSeat).ToArray());

    private IReadOnlyList<(GeneralDefinition General, string CandidateId)> GetModeGeneralOffers(
        CharacterState player, bool fullRoster)
    {
        var plan = ModeSetup.GetGeneralOffers(new(CaptureModeSetupSeat(player),
            _availableGenerals.Select(general => general.Id), CaptureModeSetupSeats(),
            player.GeneralDraftCandidates?.Select(general => general.Id), fullRoster, _shenwuDraft?.SharedGods,
            _shenwuDraft?.LeaderId, GuanduConfiguration));
        if (plan.FrozenCandidateIds is { } frozen)
            player.GeneralDraftCandidates = Array.AsReadOnly(frozen.Select(ResolveSetupGeneral).ToArray());
        return plan.Offers.Select(offer => (ResolveSetupGeneral(offer.GeneralId), offer.CandidateId)).ToArray();
    }

    private void ApplyModeSelectedGeneral(CharacterState player, GeneralDefinition selected)
    {
        var plan = ModeSetup.PrepareSelectedGeneral(CaptureModeSetupSeat(player), selected.Id);
        player.General = AddModeSetupSkills(selected, plan.ExtraSkillIds);
        if (plan.MaximumHp is { } maximum) player.MaxHp = maximum;
        if (plan.Hp is { } hp) player.Hp = hp;
    }
}
