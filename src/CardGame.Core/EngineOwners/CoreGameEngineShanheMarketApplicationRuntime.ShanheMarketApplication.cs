#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheMarketApplicationRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IShanheMarketPurchaseHost.ApplyShanheMarketPurchase(ProgramSkillFrame frame) => BeginShanheMarketApplication(frame);
    internal ShanheMarketPurchaseFrame RequireShanheMarketProgram(ProgramSkillFrame program)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == program.Id);
        if (program.SkillId != ShanheMarketPrograms.ExecutionSkillId || program.OwnerSeat != 0 || program.TriggerId != ShanheMarketPrograms.PurchaseTriggerId || program.InstructionIndex != 1 || program.SkillInstanceId != ShanheMarketPrograms.ExecutionGrantId || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ShanheMarketPurchaseFrame market || market.ProgramFrameId is { } existing && existing != program.Id || program.WindowContext?.ParentFrameId != market.Id || !_coreShanheMarketOperations.ValidShanheMarketPurchase(market))
            throw new InvalidOperationException("The market effect lost its exact paid original instruction.");
        return market;
    }

    internal ShanheRewardDefinition MarketPurchaseReward(ShanheMarketPurchaseFrame market) => _coreMatchDefinitionOptionsAccess._options.ShanheBattle!.MarketSetup!.Definition.Rewards.Single(r => r.Id == market.RewardId);
    internal SkillProgramStepOutcome BeginShanheMarketApplication(ProgramSkillFrame supplied)
    {
        var program = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var market = RequireShanheMarketProgram(program);
        if (program.ShanheMarketApplication is not null)
            throw new InvalidOperationException("A market payment cannot apply twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(market.Id, market with { ProgramFrameId = program.Id });
        _coreRuntimeOperations.ReplaceRuntimeTop(program with { ShanheMarketApplication = new(market.Id, program.InstructionIndex, market.RewardId, ShanheMarketApplicationStage.PayingHealth) });
        ContinueShanheMarketApplication(program.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62030)]
    internal bool ResumeShanheMarketApplication(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheMarketApplication: not null } p || p.Id != frameId)
            return false;
        ContinueShanheMarketApplication(frameId);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62030)]
    internal bool ReturnShanheMarketMovement(ProgramSkillFrame frame)
    {
        if (frame.ShanheMarketApplication is null)
            return false;
        _ = RequireShanheMarketProgram(frame);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }
    internal bool DrainShanheMarketChildren(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id);
    internal void ContinueShanheMarketApplication(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShanheMarketApplication: { } receipt } program && program.Id == id)
        {
            var market = RequireShanheMarketProgram(program);
            var reward = MarketPurchaseReward(market);
            if (receipt.PurchaseFrameId != market.Id || receipt.InstructionIndex != program.InstructionIndex || receipt.RewardId != market.RewardId)
                throw new InvalidOperationException("The market application receipt changed its paid source.");
            if (DrainShanheMarketChildren(program))
                return;
            program = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            if (program.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(program = program with { PendingMovementContinuation = null });
            switch (receipt.Stage)
            {
                case ShanheMarketApplicationStage.PayingHealth:
                    _coreRuntimeOperations.ReplaceRuntimeTop(program = program with { ShanheMarketApplication = receipt with { Stage = ShanheMarketApplicationStage.Granting } });
                    if (market.HpCost > 0)
                    {
                        _ = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(id, ShanheMarketPrograms.ExecutionSkillId, 0, market.HpCost);
                        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
                            return;
                    }

                    continue;
                case ShanheMarketApplicationStage.Granting:
                    var instances = ApplyShanheMarketSkillSources(program, market, reward);
                    if (reward.Bonuses is { MaxHp: > 0 } bonus)
                        _coreProgramLifecycleOperations.ChangeProgramMaximumHp(program, bonus.MaxHp);
                    _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { ShanheMarketApplication = receipt with { Stage = ShanheMarketApplicationStage.Opening, NewSkillInstanceIds = instances } });
                    continue;
                case ShanheMarketApplicationStage.Opening:
                    _coreRuntimeOperations.ReplaceRuntimeTop(program = program with { ShanheMarketApplication = receipt with { Stage = ShanheMarketApplicationStage.Materials } });
                    if (_coreShanheMarketOpeningOperations.TryBeginShanheMarketOpening(program, market, receipt.NewSkillInstanceIds))
                        return;
                    continue;
                case ShanheMarketApplicationStage.Materials:
                    _coreRuntimeOperations.ReplaceRuntimeTop(program = program with { ShanheMarketApplication = receipt with { Stage = ShanheMarketApplicationStage.Recovery }, PendingMovementContinuation = new(0, 0, null) });
                    PlaceShanheMarketMaterials(program, market, reward);
                    if (DrainShanheMarketChildren(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
                        return;
                    continue;
                case ShanheMarketApplicationStage.Recovery:
                    _coreRuntimeOperations.ReplaceRuntimeTop(program = program with { ShanheMarketApplication = receipt with { Stage = ShanheMarketApplicationStage.Children } });
                    if (reward.Heal > 0)
                        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(id, 0, 0, reward.Heal, null, null);
                    if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
                        return;
                    continue;
                case ShanheMarketApplicationStage.Children:
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheMarketPurchaseAppliedEvent(market.Id, market.RewardId));
                    _coreRuntimeOperations.ReplaceRuntimeTop(program with { ShanheMarketApplication = null, PendingMovementContinuation = null });
                    _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                    return;
                default:
                    throw new InvalidOperationException("A market application has an invalid stage.");
            }
        }
    }

    internal IReadOnlyList<string> ApplyShanheMarketSkillSources(ProgramSkillFrame program, ShanheMarketPurchaseFrame market, ShanheRewardDefinition reward)
    {
        var newInstances = new List<string>();
        var owner = _coreMatchParticipantsPlayersAccess._players[0];
        if (reward.SkillId is { } skill)
        {
            var revoked = market.ReplaceSkillId is { } old ? owner.SkillGrants.Grants.Where(g => g.SkillId == old && g.SourceId == "campaign:shanhe").Select(g => g.GrantId).ToArray() : [];
            var id = $"campaign:shanhe:0:{(skill)}";
            var grant = _coreProgramSkillChangesOperations.CommitProgramExplicitSkillSource(program, new(id, skill, id, "campaign:shanhe"), revoked);
            newInstances.Add(grant.SkillInstanceId);
        }

        var setup = _coreMatchDefinitionOptionsAccess._options.ShanheBattle!.MarketSetup!;
        var desired = _shanheMarketOperations._shanheMarket!.GetBattleTacticSkills(setup.State.Gold, setup.BossBattle).GroupBy(id => id, StringComparer.Ordinal).ToDictionary(g => g.Key, g => g.Count(), StringComparer.Ordinal);
        var existing = owner.SkillGrants.Grants.Where(g => g.SourceId == "campaign:shanhe-tactic").GroupBy(g => g.SkillId).ToDictionary(g => g.Key, g => g.OrderBy(x => x.GrantId, StringComparer.Ordinal).ToArray(), StringComparer.Ordinal);
        foreach (var(id, grants)in existing)
            if (grants.Length > desired.GetValueOrDefault(id))
                _coreProgramSkillChangesOperations.RemoveProgramExplicitSkillSources(program, grants.Skip(desired.GetValueOrDefault(id)).Select(g => g.GrantId).ToArray());
        foreach (var(id, count)in desired)
            for (var ordinal = existing.GetValueOrDefault(id)?.Length ?? 0; ordinal < count; ordinal++)
            {
                var grantId = $"campaign:shanhe-tactic:market:{(market.MarketRevision)}:{(ordinal)}:{(id)}";
                var grant = _coreProgramSkillChangesOperations.CommitProgramExplicitSkillSource(program, new(grantId, id, grantId, "campaign:shanhe-tactic"));
                newInstances.Add(grant.SkillInstanceId);
            }

        return Array.AsReadOnly(newInstances.ToArray());
    }

    internal void PlaceShanheMarketMaterials(ProgramSkillFrame program, ShanheMarketPurchaseFrame market, ShanheRewardDefinition reward)
    {
        var cards = new List<Card>();
        foreach (var equipment in new[]
        {
            false,
            true
        }

        )
        {
            var recipe = equipment ? reward.StartingEquipment : reward.StartingCards;
            for (var index = 0; index < (recipe?.Count ?? 0); index++)
            {
                var item = recipe![index];
                var card = new Card(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1, item.Kind, item.Suit, item.Rank);
                _cardZonesOperations._cardZones.AddGeneratedCard(card);
                _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(card.Id);
                cards.Add(card);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheMarketCardCreatedEvent(market.Id, reward.Id, index, equipment, card.Id));
            }
        }

        if (cards.Count > 0)
            _corePhysicalMovementFacadesOperations.MoveCards(Array.AsReadOnly(cards.ToArray()), CardLocation.OutsideGame, CardLocation.Hand(program.OwnerSeat), global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketDefinitions.ShanheMarketCardGain);
    }

    internal bool IsShanheMarketResource(int id)
    {
        if (_coreMatchDefinitionOptionsAccess._options.ShanheBattle?.StartingResourcesExpireOnUse != true || !_coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Contains(id) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheMarketCardCreatedEvent>().Where(e => e.CardId == id).ToArray()is not [var created] || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheMarketPurchaseCommittedEvent>().Where(e => e.FrameId == created.PurchaseFrameId && e.RewardId == created.RewardId).ToArray()is not [var paid] || _coreMatchDefinitionOptionsAccess._options.ShanheBattle.MarketSetup?.Definition.Rewards.SingleOrDefault(r => r.Id == paid.RewardId)is not { } reward)
            return false;
        var recipe = created.EquipmentResource ? reward.StartingEquipment : reward.StartingCards;
        if (created.RecipeIndex < 0 || recipe is null || created.RecipeIndex >= recipe.Count)
            return false;
        var item = recipe[created.RecipeIndex];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        return card.Kind == item.Kind && card.Suit == item.Suit && card.Rank == item.Rank;
    }
}
