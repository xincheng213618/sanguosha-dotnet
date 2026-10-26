using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheMarketPurchaseCapability(GameEngine engine) : IShanheMarketPurchaseHost
    {
        public SkillProgramStepOutcome ApplyShanheMarketPurchase(ProgramSkillFrame frame) =>
            engine.BeginShanheMarketApplication(frame);
    }

    private ShanheMarketPurchaseFrame RequireShanheMarketProgram(ProgramSkillFrame program)
    {
        var index = _resolutionStack.FindIndex(f => f.Id == program.Id);
        if (program.SkillId != ShanheMarketPrograms.ExecutionSkillId || program.OwnerSeat != 0 ||
            program.TriggerId != ShanheMarketPrograms.PurchaseTriggerId || program.InstructionIndex != 1 ||
            program.SkillInstanceId != ShanheMarketPrograms.ExecutionGrantId || index < 1 ||
            _resolutionStack[index - 1] is not ShanheMarketPurchaseFrame market ||
            market.ProgramFrameId is { } existing && existing != program.Id ||
            program.WindowContext?.ParentFrameId != market.Id || !ValidShanheMarketPurchase(market))
            throw new InvalidOperationException("The market effect lost its exact paid original instruction.");
        return market;
    }

    private ShanheRewardDefinition MarketPurchaseReward(ShanheMarketPurchaseFrame market) =>
        _options.ShanheBattle!.MarketSetup!.Definition.Rewards.Single(r => r.Id == market.RewardId);

    private SkillProgramStepOutcome BeginShanheMarketApplication(ProgramSkillFrame supplied)
    {
        var program = GetActiveProgramFrame(supplied.Id);
        var market = RequireShanheMarketProgram(program);
        if (program.ShanheMarketApplication is not null) throw new InvalidOperationException("A market payment cannot apply twice.");
        ReplaceRuntimeFrame(market.Id, market with { ProgramFrameId = program.Id });
        ReplaceRuntimeTop(program with { ShanheMarketApplication = new(market.Id, program.InstructionIndex,
            market.RewardId, ShanheMarketApplicationStage.PayingHealth) });
        ContinueShanheMarketApplication(program.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62030)]
    private bool ResumeShanheMarketApplication(long frameId)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheMarketApplication: not null } p || p.Id != frameId) return false;
        ContinueShanheMarketApplication(frameId);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62030)]
    private bool ReturnShanheMarketMovement(ProgramSkillFrame frame)
    {
        if (frame.ShanheMarketApplication is null) return false;
        _ = RequireShanheMarketProgram(frame);
        AdvanceRuntimeProgram(frame.Id); return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 62030)]
    private bool MatchesShanheMarketMovement(ProgramSkillFrame frame, SkillProgramEffect? effect,
        ProgramMovementContinuation pending) =>
        effect?.Op == SkillProgramEffectOp.ApplyShanheMarketPurchase && frame.ShanheMarketApplication is { } receipt &&
        receipt.InstructionIndex == frame.InstructionIndex && pending.SubjectSeat == 0 && pending.BeforeCount == 0 &&
        pending.CoverageResultBind is null && RequireShanheMarketProgram(frame).Id == receipt.PurchaseFrameId;

    private bool DrainShanheMarketChildren(ProgramSkillFrame frame) =>
        TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) ||
        TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) ||
        TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) ||
        TryBeginCardsMovedProgramWindow(frame.Id) || TryBeginAdvancedSkillsChanged(frame.Id);

    private void ContinueShanheMarketApplication(long id)
    {
        while (_resolutionStack.LastOrDefault() is ProgramSkillFrame { ShanheMarketApplication: { } receipt } program && program.Id == id)
        {
            var market = RequireShanheMarketProgram(program); var reward = MarketPurchaseReward(market);
            if (receipt.PurchaseFrameId != market.Id || receipt.InstructionIndex != program.InstructionIndex || receipt.RewardId != market.RewardId)
                throw new InvalidOperationException("The market application receipt changed its paid source.");
            if (DrainShanheMarketChildren(program)) return;
            program = GetActiveProgramFrame(id);
            if (program.PendingMovementContinuation is not null) ReplaceRuntimeTop(program = program with { PendingMovementContinuation = null });
            switch (receipt.Stage)
            {
                case ShanheMarketApplicationStage.PayingHealth:
                    ReplaceRuntimeTop(program = program with { ShanheMarketApplication = receipt with { Stage = ShanheMarketApplicationStage.Granting } });
                    if (market.HpCost > 0)
                    {
                        _ = new ProgramSkillHost(this).LoseHp(id, ShanheMarketPrograms.ExecutionSkillId, 0, market.HpCost);
                        if (_resolutionStack.LastOrDefault()?.Id != id) return;
                    }
                    continue;
                case ShanheMarketApplicationStage.Granting:
                    var instances = ApplyShanheMarketSkillSources(program, market, reward);
                    if (reward.Bonuses is { MaxHp: > 0 } bonus) ChangeProgramMaximumHp(program, bonus.MaxHp);
                    ReplaceRuntimeTop(GetActiveProgramFrame(id) with { ShanheMarketApplication = receipt with
                        { Stage = ShanheMarketApplicationStage.Opening, NewSkillInstanceIds = instances } });
                    continue;
                case ShanheMarketApplicationStage.Opening:
                    ReplaceRuntimeTop(program = program with { ShanheMarketApplication = receipt with { Stage = ShanheMarketApplicationStage.Materials } });
                    if (TryBeginShanheMarketOpening(program, market, receipt.NewSkillInstanceIds)) return;
                    continue;
                case ShanheMarketApplicationStage.Materials:
                    ReplaceRuntimeTop(program = program with { ShanheMarketApplication = receipt with { Stage = ShanheMarketApplicationStage.Recovery },
                        PendingMovementContinuation = new(0, 0, null) });
                    PlaceShanheMarketMaterials(program, market, reward);
                    if (DrainShanheMarketChildren(GetActiveProgramFrame(id))) return;
                    continue;
                case ShanheMarketApplicationStage.Recovery:
                    ReplaceRuntimeTop(program = program with { ShanheMarketApplication = receipt with { Stage = ShanheMarketApplicationStage.Children } });
                    if (reward.Heal > 0) new ProgramSkillHost(this).Recover(id, 0, 0, reward.Heal, null, null);
                    if (_resolutionStack.LastOrDefault()?.Id != id) return;
                    continue;
                case ShanheMarketApplicationStage.Children:
                    AdvanceEventRulesAndQueueFact(new ShanheMarketPurchaseAppliedEvent(market.Id, market.RewardId));
                    ReplaceRuntimeTop(program with { ShanheMarketApplication = null, PendingMovementContinuation = null });
                    AdvanceRuntimeProgram(id);
                    return;
                default: throw new InvalidOperationException("A market application has an invalid stage.");
            }
        }
    }

    private IReadOnlyList<string> ApplyShanheMarketSkillSources(ProgramSkillFrame program,
        ShanheMarketPurchaseFrame market, ShanheRewardDefinition reward)
    {
        var newInstances = new List<string>(); var owner = _players[0];
        if (reward.SkillId is { } skill)
        {
            var revoked = market.ReplaceSkillId is { } old ? owner.SkillGrants.Grants.Where(g => g.SkillId == old &&
                g.SourceId == "campaign:shanhe").Select(g => g.GrantId).ToArray() : [];
            var id = $"campaign:shanhe:0:{skill}";
            var grant = CommitProgramExplicitSkillSource(program, new(id, skill, id, "campaign:shanhe"), revoked);
            newInstances.Add(grant.SkillInstanceId);
        }
        var definition = _options.ShanheBattle!.MarketSetup!.Definition;
        var boss = _options.ShanheBattle.MarketSetup.BossBattle;
        var desired = _shanheMarket!.Snapshot.TacticIds.Select(id => definition.Rewards.Single(r => r.Id == id))
            .SelectMany(r => new[] { !r.BossBattleOnly || boss ? r.TacticSkillId : null,
                r.CampaignEconomy is { BattleSkillId: { } battleSkill, GoldAbove: { } threshold } &&
                _options.ShanheBattle.MarketSetup.State.Gold > threshold ? battleSkill : null }.OfType<string>())
            .Concat(_shanheMarket.GetSchoolProgress().Where(progress => progress.IsComplete)
                .Select(progress => ShanheSchoolEffects.SkillId(progress.School)).OfType<string>())
            .GroupBy(id => id, StringComparer.Ordinal).ToDictionary(g => g.Key, g => g.Count(), StringComparer.Ordinal);
        var existing = owner.SkillGrants.Grants.Where(g => g.SourceId == "campaign:shanhe-tactic")
            .GroupBy(g => g.SkillId).ToDictionary(g => g.Key, g => g.OrderBy(x => x.GrantId, StringComparer.Ordinal).ToArray(), StringComparer.Ordinal);
        foreach (var (id, grants) in existing)
            if (grants.Length > desired.GetValueOrDefault(id)) RemoveProgramExplicitSkillSources(program,
                grants.Skip(desired.GetValueOrDefault(id)).Select(g => g.GrantId).ToArray());
        foreach (var (id, count) in desired)
            for (var ordinal = existing.GetValueOrDefault(id)?.Length ?? 0; ordinal < count; ordinal++)
            {
                var grantId = $"campaign:shanhe-tactic:market:{market.MarketRevision}:{ordinal}:{id}";
                var grant = CommitProgramExplicitSkillSource(program, new(grantId, id, grantId, "campaign:shanhe-tactic"));
                newInstances.Add(grant.SkillInstanceId);
            }
        return Array.AsReadOnly(newInstances.ToArray());
    }

    private void PlaceShanheMarketMaterials(ProgramSkillFrame program, ShanheMarketPurchaseFrame market, ShanheRewardDefinition reward)
    {
        var cards = new List<Card>();
        foreach (var equipment in new[] { false, true })
        {
            var recipe = equipment ? reward.StartingEquipment : reward.StartingCards;
            for (var index = 0; index < (recipe?.Count ?? 0); index++)
            {
                var item = recipe![index];
                var card = new Card(CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1,
                    item.Kind, item.Suit, item.Rank);
                _cardZones.AddGeneratedCard(card); _generatedPhysicalCardIds.Add(card.Id); cards.Add(card);
                AdvanceEventRulesAndQueueFact(new ShanheMarketCardCreatedEvent(market.Id, reward.Id, index, equipment, card.Id));
            }
        }
        if (cards.Count > 0) MoveCards(Array.AsReadOnly(cards.ToArray()), CardLocation.OutsideGame,
            CardLocation.Hand(program.OwnerSeat), ShanheMarketCardGain);
    }

    private bool IsShanheMarketResource(int id)
    {
        if (_options.ShanheBattle?.StartingResourcesExpireOnUse != true || !_generatedPhysicalCardIds.Contains(id) ||
            ProgramEventHistory<ShanheMarketCardCreatedEvent>().Where(e => e.CardId == id).ToArray() is not [var created] ||
            ProgramEventHistory<ShanheMarketPurchaseCommittedEvent>().Where(e => e.FrameId == created.PurchaseFrameId && e.RewardId == created.RewardId).ToArray() is not [var paid] ||
            _options.ShanheBattle.MarketSetup?.Definition.Rewards.SingleOrDefault(r => r.Id == paid.RewardId) is not { } reward) return false;
        var recipe = created.EquipmentResource ? reward.StartingEquipment : reward.StartingCards;
        if (created.RecipeIndex < 0 || recipe is null || created.RecipeIndex >= recipe.Count) return false;
        var item = recipe[created.RecipeIndex]; var card = GetAdvancedCard(id);
        return card.Kind == item.Kind && card.Suit == item.Suit && card.Rank == item.Rank;
    }
}
