#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCompletedCategoryUseProofRuntime
{
    internal bool HasExactAcceptedActualHandGainUse(CardUseFrame use, CardActionContext completed)
    {
        if (use.Action is not { Type: CardActionType.Use } current || completed.Type != CardActionType.Use || current.ActionId != completed.ActionId || current.ActorSeat != completed.ActorSeat || current.ProviderSeat != completed.ProviderSeat || current.EffectiveKind != completed.EffectiveKind || !current.TargetSeats.SequenceEqual(completed.TargetSeats) || !current.PhysicalCards.SequenceEqual(completed.PhysicalCards) || !current.ConversionChain.SequenceEqual(completed.ConversionChain))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var declarations = history.OfType<CardUseDeclaredEvent>().Where(e => e.ResolutionId == use.Id && e.CardId == use.CardId).ToArray();
        if (declarations is not [var declared] || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, declared.SourceSeat, completed.ProviderSeat))
            return false;
        for (var i = history.Length - 1; i >= 0; i--)
        {
            if (history[i] is not CardActionAcceptedEvent { Action: var accepted } || accepted.ActionId != completed.ActionId || accepted.Type != CardActionType.Use || accepted.ProviderSeat != completed.ProviderSeat || accepted.ParentActionId != completed.ParentActionId || accepted.RequesterSeat != completed.RequesterSeat || accepted.ResponderSeat != completed.ResponderSeat || accepted.OpponentSeat != completed.OpponentSeat || accepted.EffectiveSuit != completed.EffectiveSuit || accepted.EffectiveRank != completed.EffectiveRank || !accepted.PhysicalCards.SequenceEqual(completed.PhysicalCards))
                continue;
            if (RebuildAcceptedActualHandGainUse(use, accepted, completed, history, i))
                return true;
        }

        return false;
    }

    internal bool ActualHandGainMutationBinding(long frameId, string skill, int owner, SkillProgramTriggerWindow window, SkillProgramEffectOp operation)
    {
        var started = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Where(e => e.FrameId == frameId && e.SkillId == skill && e.OwnerSeat == owner && e.Window == window).ToArray();
        return started is [var fact] && _contentRegistryOperations._contentRegistry.GetSkill(skill).Program is { } program && ProgramInstructionResolver.Default.FindTrigger(program, fact.BindingId)is { } trigger && ProgramInstructionResolver.Default.Features(trigger).HasOperation(operation);
    }

    internal bool RebuildAcceptedActualHandGainUse(CardUseFrame use, CardActionContext accepted, CardActionContext completed, IReadOnlyList<IGameEvent> history, int acceptedIndex)
    {
        var actor = accepted.ActorSeat;
        var kind = accepted.EffectiveKind;
        var chain = accepted.ConversionChain.ToList();
        var designated = accepted.TargetSeats.ToList();
        var actual = accepted.TargetSeats.ToList();
        // A later native finalized acceptance already proves its complete targets;
        // actual redirect targets may remain distinct from the designated action.
        if (actor == completed.ActorSeat && kind == completed.EffectiveKind && chain.SequenceEqual(completed.ConversionChain) && designated.SequenceEqual(completed.TargetSeats))
            return true;
        foreach (var fact in history.Skip(acceptedIndex + 1))
        {
            if (fact is SoleTargetReplacedEvent replacement && replacement.CardUseFrameId == use.Id)
            {
                if (!_coreSoleTargetQuarantineOperations.IsSoleTargetReplacementFact(use, replacement, designated))
                    return false;
                designated = replacement.AfterTargets.ToList();
                actual = replacement.AfterTargets.ToList();
            }
            else if (fact is SoleTargetAfterProtectionRedirectedEvent protectedRedirect && protectedRedirect.CardUseFrameId == use.Id)
            {
                if (!actual.SequenceEqual(designated) || !_coreSoleTargetRedirectOperations.IsSoleTargetAfterProtectionRedirectFact(use, protectedRedirect, actual))
                    return false;
                designated = protectedRedirect.AfterTargets.ToList();
                actual = protectedRedirect.AfterTargets.ToList();
            }
            else if (fact is ProgramCardUseActorReplacedEvent changedActor && changedActor.CardUseFrameId == use.Id)
            {
                if (changedActor.PreviousActorSeat != actor || changedActor.OwnerSeat != actor || changedActor.ProviderSeat != completed.ProviderSeat || !ActualHandGainMutationBinding(changedActor.FrameId, changedActor.SkillId, changedActor.OwnerSeat, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramEffectOp.ReplaceCurrentCardUseActor))
                    return false;
                actor = changedActor.ActorSeat;
            }
            else if (fact is ShortRangeSlashTargetResolvedEvent shortRange && shortRange.CardUseFrameId == use.Id && shortRange.Added)
            {
                if (!_coreShortRangeSlashTargetsOperations.IsShortRangeSlashTargetFact(use, shortRange, designated))
                    return false;
                var issued = shortRange.Receipt!;
                designated.Add(issued.AddedTargetSeat);
                actual.Add(issued.AddedTargetSeat);
            }
            else if (fact is MoQuanyuTargetAddedEvent mo && mo.UseId == use.Id)
            {
                if (!actual.SequenceEqual(designated) || mo.TargetIndex != actual.Count || actual.Contains(mo.TargetSeat) || !_characterSunQuanOlMagicOperations.IsMoQuanyuAddedTargetFact(use, mo))
                    return false;
                designated.Add(mo.TargetSeat);
                actual.Add(mo.TargetSeat);
            }
            else if (fact is SameTypeAidTargetAddedEvent aid && aid.Use.CardUseFrameId == use.Id)
            {
                if (aid.Use.CardActionId != completed.ActionId || !_coreSameTypeActualUseAidOperations.IsSameTypeAidTargetFact(use, aid) || actual.Contains(aid.RecipientSeat))
                    return false;
                designated.Add(aid.RecipientSeat);
                actual.Add(aid.RecipientSeat);
            }
            else if (fact is TargetedOrdinaryTrickResolvedEvent targeted && targeted.CardUseFrameId == use.Id)
            {
                if (!_coreTargetedOrdinaryTrickOperations.IsTargetedOrdinaryTrickResolvedFact(use, targeted, actual))
                    return false;
                actual = targeted.ResultTargets.ToList();
                designated = targeted.ResultTargets.ToList();
            }
            else if (fact is SouyingNativeMutationEvent souying && souying.NativeUseId == use.Id && souying.Cancelled)
            {
                if (!_characterHuaManOlOperations.IsHuaManSouyingMutationFact(use, souying, actual))
                    return false;
                designated.Clear();
                actual.Clear();
            }
            else if (fact is ChenglieTargetsAppliedEvent chenglie && chenglie.UseId == use.Id)
            {
                if (!actual.SequenceEqual(designated) || !_characterMaChengOlOperations.IsChenglieAppliedTargetFact(use, chenglie, designated, out var chenglieTargets))
                    return false;
                designated = chenglieTargets.ToList();
                actual = chenglieTargets.ToList();
            }
            else if (fact is BingzhongTargetsAppliedEvent { AddedCount: > 0 } bingzhong && bingzhong.UseId == use.Id)
            {
                IReadOnlyList<int> preceding = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && use.BingzhongTail?.OriginalTargets is [var self] && self == actor)
                    preceding = [actor];
                if (!actual.SequenceEqual(designated) || !IsBingzhongAcceptedUseTargetFact(use, bingzhong, preceding, actor, completed.ProviderSeat, history, out var resultTargets))
                    return false;
                designated = resultTargets.ToList();
                actual = resultTargets.ToList();
            }
            else if (fact is PijingTargetsAppliedEvent pijing && pijing.UseId == use.Id)
            {
                IReadOnlyList<int> preceding = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0)
                    preceding = [actor];
                if (!actual.SequenceEqual(designated) || !_characterLiuPanOlOperations.IsPijingAppliedTargetFact(use, pijing, preceding, out var resultTargets))
                    return false;
                designated = resultTargets.ToList();
                actual = resultTargets.ToList();
            }
            else if (fact is WeifuTargetAddedEvent weifu && weifu.UseId == use.Id)
            {
                IReadOnlyList<int> preceding = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && use.WeifuUse?.OriginalTargets is [var self] && self == actor)
                    preceding = [actor];
                if (!actual.SequenceEqual(designated) || !IsWeifuAcceptedUseTargetFact(use, weifu, preceding, actor, completed.ProviderSeat, history))
                    return false;
                designated = preceding.Append(weifu.TargetSeat).ToList();
                actual = designated.ToList();
            }
            else if (fact is MouLidanTargetsAppliedEvent lidan && lidan.UseId == use.Id && lidan.AddedCount > 0)
            {
                IReadOnlyList<int> originalTargets = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && use.MouLidan?.Original is [var self] && self == actor)
                    originalTargets = [actor];
                if (!actual.SequenceEqual(designated) || lidan.Source.OwnerSeat != actor || !_characterChengYuMouOlOperations.IsMouLidanAppliedTargetFact(use, lidan, originalTargets, out var resultTargets))
                    return false;
                designated = resultTargets.ToList();
                actual = resultTargets.ToList();
            }
            else if (fact is OverflowUseTargetsCanceledEvent canceled && canceled.Receipt.CardUseFrameId == use.Id)
            {
                if (canceled.Receipt.Source.OwnerSeat != actor || !_coreOverflowTargetCancellationObserversOperations.IsOverflowTargetCancellationFact(use, canceled, actual))
                    return false;
                designated = canceled.Receipt.ResultTargetSeats.ToList();
                actual = canceled.Receipt.ResultTargetSeats.ToList();
            }
            else if (fact is AnniversaryCuiLieUseChangedEvent changed && changed.UseId == use.Id)
            {
                if (!actual.SequenceEqual(designated) || !_characterCuiLieAnniversaryOperations.IsAnniversaryCuiLieTargetMutationFact(use, changed, actual, actor))
                    return false;
                designated = changed.AfterTargets.ToList();
                actual = changed.AfterTargets.ToList();
            }
            else if (fact is ProgramCardUseTargetAddedEvent added && added.CardUseFrameId == use.Id)
            {
                // These producers also emit a typed issuance proof; consume that
                // proof once instead of counting its public summary a second time.
                if (_characterChengYuMouOlOperations.IsMouLidanTargetSummary(use, added))
                    continue;
                if (history.OfType<ShortRangeSlashTargetResolvedEvent>().Any(e => e.CardUseFrameId == use.Id && e.Added && e.Receipt is { } issued && issued.ProgramFrameId == added.FrameId && issued.AddedTargetSeat == added.TargetSeat) || history.OfType<SameTypeAidTargetAddedEvent>().Any(e => e.Use.CardUseFrameId == use.Id && e.ProgramFrameId == added.FrameId && e.RecipientSeat == added.TargetSeat) || history.OfType<CompletedCategoryCompoundTargetsIssuedEvent>().Any(e => e.CardUseFrameId == use.Id && e.ProgramFrameId == added.FrameId && e.Targets.Count == e.OriginalTargets.Count + 2 && e.Targets[^2] == added.TargetSeat) || history.OfType<DesignatedExtraTargetResolvedEvent>().Any(e => e.CardUseFrameId == use.Id && e.ProgramFrameId == added.FrameId && e.AddedTargetSeats.Count > 0 && e.AddedTargetSeats[0] == added.TargetSeat) || history.OfType<RoundGainedTrickTargetResolvedEvent>().Any(e => e.Qualification.CardUseFrameId == use.Id && e.ProgramFrameId == added.FrameId && e.AddedTargetSeats.Count > 0 && e.AddedTargetSeats[0] == added.TargetSeat) || history.OfType<UniqueLeaderTrickTargetResolvedEvent>().Any(e => e.CardUseFrameId == use.Id && e.ProgramFrameId == added.FrameId && e.AddedTargetSeats.Count > 0 && e.AddedTargetSeats[0] == added.TargetSeat) || history.OfType<RecipientCategorySlashTargetsResolvedEvent>().Any(e => e.CardUseFrameId == use.Id && e.FrameId == added.FrameId && e.AddedTargets.Contains(added.TargetSeat)) || history.OfType<DamageTargetBudgetResolvedEvent>().Any(e => e.Receipt.CardUseFrameId == use.Id && e.ProgramFrameId == added.FrameId && e.Receipt.AddedTargets.Where((_, i) => kind != CardKind.BorrowedSword || i % 2 == 0).Contains(added.TargetSeat)) || history.OfType<AdjacentRedTargetAddedEvent>().Any(e => e.CardUseFrameId == use.Id && e.ProgramFrameId == added.FrameId && e.Source.OwnerSeat == added.OwnerSeat && e.Source.SkillId == added.SkillId && e.AddedTargets[0] == added.TargetSeat))
                    continue;
                if (actual.Contains(added.TargetSeat) || !ActualHandGainMutationBinding(added.FrameId, added.SkillId, added.OwnerSeat, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramEffectOp.AddCurrentCardUseTarget))
                    return false;
                designated.Add(added.TargetSeat);
                actual.Add(added.TargetSeat);
                if (kind == CardKind.BorrowedSword)
                    return false;
            }
            else if (fact is CompletedCategoryCompoundTargetsIssuedEvent compound && compound.CardUseFrameId == use.Id)
            {
                if (kind != CardKind.BorrowedSword || compound.ActionId != completed.ActionId || compound.Source.OwnerSeat != actor || !designated.SequenceEqual(compound.OriginalTargets) || !actual.SequenceEqual(compound.OriginalTargets) || compound.Targets.Count != compound.OriginalTargets.Count + 2 || !compound.Targets.Take(compound.OriginalTargets.Count).SequenceEqual(compound.OriginalTargets) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(compound.Targets[^2]) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(compound.Targets[^1]) || compound.Targets[^2] == compound.Targets[^1] || _contentRegistryOperations._contentRegistry.GetSkill(compound.Source.SkillId).Program is not { } program || program.GameplayHash != compound.GameplayHash || ProgramInstructionResolver.Default.Find(program, ProgramInstructionSourceKind.Trigger, compound.Source.BindingId)is not { } plan || plan.Trigger?.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || compound.InstructionIndex < 1 || compound.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(compound.InstructionIndex).Effect.Op != SkillProgramEffectOp.AddCurrentCardUseTarget || history.OfType<CompletedCategoryCompoundTargetsIssuedEvent>().Count(e => e.ProgramFrameId == compound.ProgramFrameId && e.CardUseFrameId == use.Id && e.InstructionIndex == compound.InstructionIndex) != 1 || history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == compound.ProgramFrameId && e.SkillId == compound.Source.SkillId && e.BindingId == compound.Source.BindingId && e.SkillInstanceId == compound.Source.SkillInstanceId && e.OwnerSeat == compound.Source.OwnerSeat && e.Window == plan.Trigger.Window) != 1 || history.OfType<ProgramCardUseTargetAddedEvent>().Count(e => e.FrameId == compound.ProgramFrameId && e.SkillId == compound.Source.SkillId && e.OwnerSeat == compound.Source.OwnerSeat && e.CardUseFrameId == use.Id && e.TargetSeat == compound.Targets[^2]) != 1)
                    return false;
                designated = compound.Targets.ToList();
                actual = compound.Targets.ToList();
            }
            else if (fact is DesignatedExtraTargetResolvedEvent designatedExtra && designatedExtra.CardUseFrameId == use.Id && designatedExtra.AddedTargetSeats.Count > 0)
            {
                IReadOnlyList<int> originalTargets = designated;
                // Only this exact issuance may normalize the old implicit self
                // DrawTwo acceptance before extending its designated targets.
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && designatedExtra.OriginalTargetSeats is [var self] && self == actor)
                    originalTargets = [actor];
                if (designatedExtra.Source.OwnerSeat != actor || !actual.SequenceEqual(designated) || !_coreDesignatedExtraTargetsOperations.IsDesignatedExtraTargetFact(use, designatedExtra, originalTargets))
                    return false;
                designated = designatedExtra.ResultTargetSeats.ToList();
                actual = designatedExtra.ResultTargetSeats.ToList();
            }
            else if (fact is UniqueLeaderTrickTargetResolvedEvent leaderTarget && leaderTarget.CardUseFrameId == use.Id && (leaderTarget.AddedTargetSeats.Count > 0 || kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && leaderTarget.OriginalTargetSeats is [var implicitSelf] && implicitSelf == actor))
            {
                IReadOnlyList<int> originalTargets = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && leaderTarget.OriginalTargetSeats is [var self] && self == actor)
                    originalTargets = [actor];
                if (!actual.SequenceEqual(designated) || !_coreUniqueLeaderTrickTargetsOperations.IsUniqueLeaderTrickTargetFact(use, leaderTarget, originalTargets))
                    return false;
                designated = leaderTarget.ResultTargetSeats.ToList();
                actual = leaderTarget.ResultTargetSeats.ToList();
            }
            else if (fact is NeifaTargetsResolvedEvent neifa && neifa.UseId == use.Id)
            {
                IReadOnlyList<int> originalTargets = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && neifa.OriginalTargets is [var neifaSelf] && neifaSelf == actor)
                    originalTargets = [actor];
                if (!actual.SequenceEqual(designated) || !_characterYuanTanYuanShangOlOperations.IsNeifaTargetsResolvedFact(use, neifa, originalTargets))
                    return false;
                designated = neifa.ResultTargets.ToList();
                actual = neifa.ResultTargets.ToList();
            }
            else if (fact is RoundGainedTrickTargetResolvedEvent roundTarget && roundTarget.Qualification.CardUseFrameId == use.Id)
            {
                IReadOnlyList<int> originalTargets = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && roundTarget.OriginalTargetSeats is [var roundSelf] && roundSelf == actor)
                    originalTargets = [actor];
                if (!actual.SequenceEqual(designated) || !_coreRoundGainedTrickTargetsOperations.IsRoundGainedTrickTargetFact(use, roundTarget, originalTargets))
                    return false;
                designated = roundTarget.ResultTargetSeats.ToList();
                actual = roundTarget.ResultTargetSeats.ToList();
            }
            else if (fact is DinghanTargetCanceledEvent dinghan && dinghan.UseId == use.Id)
            {
                IReadOnlyList<int> before = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && dinghan.OriginalAction.TargetSeats is [var self] && self == actor)
                    before = [actor];
                if (!actual.SequenceEqual(designated) || !_characterXunYuMobileGodOperations.IsDinghanCancellationFact(use, dinghan, before))
                    return false;
                designated = global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime.DinghanRetainedTargets(kind, before, dinghan.Owner).ToList();
                actual = designated.ToList();
            }
            else if (fact is BeiMiHuTargetsPaidEvent kuiTargets && kuiTargets.Receipt.CardUseId == use.Id)
            {
                var receipt = kuiTargets.Receipt;
                IReadOnlyList<int> originalTargets = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && receipt.BeforeTargets is [var self] && self == actor)
                    originalTargets = [actor];
                if (!actual.SequenceEqual(designated) || !originalTargets.SequenceEqual(receipt.BeforeTargets) || receipt.ActionId != completed.ActionId || use.BeiMiHuTargetMutation is not { } mutation || !global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime.BeiMiHuMutationEqual(mutation, receipt) || !_characterBeiMiHuOlOperations.ValidBeiMiHuTargetMutation(use))
                    return false;
                designated = receipt.AfterTargets.ToList();
                actual = receipt.AfterTargets.ToList();
            }
            else if (fact is RecipientCategorySlashTargetsResolvedEvent categoryTargets && categoryTargets.CardUseFrameId == use.Id)
            {
                if (!actual.SequenceEqual(designated) || !_coreRecipientCategorySlashTargetsOperations.IsRecipientCategorySlashTargetFact(use, categoryTargets, designated))
                    return false;
                designated = categoryTargets.ResultTargets.ToList();
                actual = categoryTargets.ResultTargets.ToList();
            }
            else if (fact is DamageTargetBudgetResolvedEvent budget && budget.Receipt.CardUseFrameId == use.Id)
            {
                IReadOnlyList<int> original = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && budget.Receipt.OriginalTargets is [var self] && self == actor)
                    original = [actor];
                if (!actual.SequenceEqual(designated) || !_coreDamageTargetBudgetOperations.IsDamageBudgetResolvedFact(use, budget, original, actor))
                    return false;
                designated = budget.ResultTargets.ToList();
                actual = budget.ResultTargets.ToList();
            }
            else if (fact is AdjacentRedTargetAddedEvent joined && joined.CardUseFrameId == use.Id)
            {
                IReadOnlyList<int> original = designated;
                if (kind == CardKind.DrawTwo && designated.Count == 0 && actual.Count == 0 && joined.OriginalTargets is [var self] && self == actor)
                    original = [actor];
                if (!actual.SequenceEqual(designated) || !_coreAdjacentRedTargetsOperations.IsAdjacentRedTargetFact(use, joined, original, actor))
                    return false;
                designated = joined.ResultTargets.ToList();
                actual = joined.ResultTargets.ToList();
            }
            else if (fact is CurrentCardEnhancedEvent { Enhancement: CurrentCardEnhancement.ExtraTarget, ExtraTargetSeat: { } target } enhanced && enhanced.CardUseFrameId == use.Id && enhanced.CardActionId == completed.ActionId)
            {
                if (actual.Contains(target) || (use.Enhancements & CurrentCardEnhancement.ExtraTarget) == 0)
                    return false;
                designated.Add(target);
                actual.Add(target);
            }
            else if (fact is OriginalTargetAdditionResolvedEvent { Added: true } original && original.CardUseFrameId == use.Id && original.ActionId == completed.ActionId)
            {
                if (actual.Contains(original.TargetSeat) || original.ActorSeat != actor || use.OriginalTargetAddition is not { Added: true } receipt || !receipt.Grants.Any(g => g.Source.OwnerSeat == actor && g.TargetSeat == original.TargetSeat && history.OfType<OriginalTargetAdditionIssuedEvent>().Count(e => e.CardUseFrameId == use.Id && e.ActionId == completed.ActionId && e.Grant == g) == 1))
                    return false;
                if (kind == CardKind.DrawTwo && designated.Count == 0)
                {
                    designated.Add(actor);
                    actual.Add(actor);
                }

                designated.Add(original.TargetSeat);
                actual.Add(original.TargetSeat);
            }
            else if (fact is ProgramCurrentSlashFireChangedEvent changedKind && changedKind.CardUseFrameId == use.Id)
            {
                if (use.CurrentSlashFirePolicy is not { } policy || changedKind.ActionId != completed.ActionId || changedKind.ProgramFrameId != policy.ProgramFrameId || changedKind.Source != policy.Source || changedKind.OriginalKind != kind || changedKind.OriginalKind != policy.OriginalKind || changedKind.FinalKind != CardKind.FireSlash || changedKind.ExtraTargetSeat != policy.ExtraTargetSeat || policy.OriginalAction.ActionId != completed.ActionId || policy.OriginalAction.ActorSeat != actor || policy.OriginalAction.ProviderSeat != completed.ProviderSeat || !designated.SequenceEqual(policy.OriginalAction.TargetSeats) || !chain.SequenceEqual(policy.OriginalAction.ConversionChain) || history.OfType<ProgramCurrentSlashFireChangedEvent>().Count(e => e.CardUseFrameId == use.Id) != 1 || history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == policy.ProgramFrameId && e.SkillId == policy.Source.SkillId && e.OwnerSeat == policy.Source.OwnerSeat && e.BindingId == policy.Source.BindingId && e.SkillInstanceId == policy.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseCommitted) != 1 || !ActualHandGainMutationBinding(policy.ProgramFrameId, policy.Source.SkillId, policy.Source.OwnerSeat, SkillProgramTriggerWindow.CardUseCommitted, SkillProgramEffectOp.OfferCurrentSlashFireAndExtraTarget))
                    return false;
                if (policy.Converted)
                    chain.Add(policy.Source);
                kind = CardKind.FireSlash;
                if (policy.ExtraTargetSeat is { } extra)
                {
                    if (actual.Contains(extra))
                        return false;
                    designated.Add(extra);
                    actual.Add(extra);
                }
            }
            else if (fact is SlashTargetsReplacedEvent replaced && replaced.CardUseFrameId == use.Id)
            {
                if (!actual.SequenceEqual(replaced.PreviousTargets) || !_characterMaXiuMaTieOlOperations.IsKenshangTargetsReplacedFact(use, replaced) && !ActualHandGainMutationBinding(replaced.FrameId, replaced.SkillId, replaced.OwnerSeat, SkillProgramTriggerWindow.CardUseCommitted, SkillProgramEffectOp.ReplaceAllSlashTargets))
                    return false;
                designated = replaced.Targets.ToList();
                actual = replaced.Targets.ToList();
            }
            else if (fact is ProgramActualSlashTargetRedirectedEvent redirected && redirected.CardUseFrameId == use.Id)
            {
                if (_coreSoleTargetProtectionOperations.HasSoleTargetProtectionCapability && history.OfType<SoleTargetAfterProtectionRedirectedEvent>().Any(e => e.ProgramFrameId == redirected.ProgramFrameId && e.CardUseFrameId == use.Id && e.ActionId == redirected.ActionId && e.Source == redirected.Source && e.GameplayHash == redirected.GameplayHash && e.InstructionIndex == redirected.InstructionIndex && e.TargetIndex == redirected.TargetIndex && e.PreviousTargetSeat == redirected.OriginalTargetSeat && e.NewTargetSeat == redirected.NewTargetSeat))
                    continue;
                if (!_coreShortRangeSlashTargetsOperations.IsShortRangeSlashRedirectFact(use, redirected, actual))
                    return false;
                if (history.OfType<DesignatedExtraTargetRedirectedEvent>().Any(e => e.ProgramFrameId == redirected.ProgramFrameId && e.CardUseFrameId == redirected.CardUseFrameId && e.ActionId == redirected.ActionId && e.Source == redirected.Source && e.GameplayHash == redirected.GameplayHash && e.InstructionIndex == redirected.InstructionIndex && e.TargetIndex == redirected.TargetIndex && e.OriginalTargetSeat == redirected.OriginalTargetSeat && e.NewTargetSeat == redirected.NewTargetSeat))
                    continue;
                actual[redirected.TargetIndex] = redirected.NewTargetSeat;
            }
            else if (fact is ProgramCurrentSlashFireRedirectedEvent fire && fire.CardUseFrameId == use.Id)
            {
                if (_coreSoleTargetProtectionOperations.HasSoleTargetProtectionCapability && history.OfType<SoleTargetAfterProtectionRedirectedEvent>().Any(e => e.ProgramFrameId == fire.ProgramFrameId && e.CardUseFrameId == use.Id && e.ActionId == fire.ActionId && e.Source == fire.Source && e.TargetIndex == fire.TargetIndex && e.PreviousTargetSeat == fire.OriginalTargetSeat && e.NewTargetSeat == fire.NewTargetSeat))
                    continue;
                if (use.CurrentSlashFirePolicy is null || fire.ActionId != completed.ActionId || fire.TargetIndex < 0 || fire.TargetIndex >= actual.Count || actual[fire.TargetIndex] != fire.OriginalTargetSeat || fire.Source.OwnerSeat != fire.OriginalTargetSeat || !ActualHandGainMutationBinding(fire.ProgramFrameId, fire.Source.SkillId, fire.Source.OwnerSeat, SkillProgramTriggerWindow.SlashTargetRedirecting, SkillProgramEffectOp.RedirectCurrentAttack))
                    return false;
                if (history.OfType<DesignatedExtraTargetRedirectedEvent>().Any(e => e.ProgramFrameId == fire.ProgramFrameId && e.CardUseFrameId == fire.CardUseFrameId && e.ActionId == fire.ActionId && e.Source == fire.Source && e.TargetIndex == fire.TargetIndex && e.OriginalTargetSeat == fire.OriginalTargetSeat && e.NewTargetSeat == fire.NewTargetSeat))
                    continue;
                actual[fire.TargetIndex] = fire.NewTargetSeat;
            }
            else if (fact is DesignatedExtraTargetRedirectedEvent redirectedExtra && redirectedExtra.CardUseFrameId == use.Id)
            {
                if (_coreSoleTargetProtectionOperations.HasSoleTargetProtectionCapability && history.OfType<SoleTargetAfterProtectionRedirectedEvent>().Any(e => e.ProgramFrameId == redirectedExtra.ProgramFrameId && e.CardUseFrameId == use.Id && e.ActionId == redirectedExtra.ActionId && e.Source == redirectedExtra.Source && e.GameplayHash == redirectedExtra.GameplayHash && e.InstructionIndex == redirectedExtra.InstructionIndex && e.TargetIndex == redirectedExtra.TargetIndex && e.PreviousTargetSeat == redirectedExtra.OriginalTargetSeat && e.NewTargetSeat == redirectedExtra.NewTargetSeat))
                    continue;
                if (!_coreDesignatedExtraTargetsOperations.IsDesignatedExtraTargetRedirectFact(use, redirectedExtra, actual))
                    return false;
                actual[redirectedExtra.TargetIndex] = redirectedExtra.NewTargetSeat;
            }
            else if (fact is BingzhongRedirectedEvent bingzhongRedirect && bingzhongRedirect.UseId == use.Id)
            {
                if (!_characterYangFuOlOperations.HasBingzhongTargetTail(use) || bingzhongRedirect.ActionId != completed.ActionId || history.OfType<BingzhongRedirectedEvent>().Count(e => e.FrameId == bingzhongRedirect.FrameId && e.UseId == use.Id && e.InstructionIndex == bingzhongRedirect.InstructionIndex) != 1)
                    return false;
                // A native redirect may also acknowledge a fire/short-range or
                // designated-target receipt. Its existing branch owns the mutation.
                if (history.OfType<ProgramActualSlashTargetRedirectedEvent>().Any(e => e.ProgramFrameId == bingzhongRedirect.FrameId && e.CardUseFrameId == use.Id && e.ActionId == completed.ActionId && e.Source == bingzhongRedirect.Source && e.GameplayHash == bingzhongRedirect.GameplayHash && e.InstructionIndex == bingzhongRedirect.InstructionIndex && e.TargetIndex == bingzhongRedirect.TargetIndex && e.OriginalTargetSeat == bingzhongRedirect.PreviousSeat && e.NewTargetSeat == bingzhongRedirect.Seat) || history.OfType<ProgramCurrentSlashFireRedirectedEvent>().Any(e => e.ProgramFrameId == bingzhongRedirect.FrameId && e.CardUseFrameId == use.Id && e.ActionId == completed.ActionId && e.Source == bingzhongRedirect.Source && e.TargetIndex == bingzhongRedirect.TargetIndex && e.OriginalTargetSeat == bingzhongRedirect.PreviousSeat && e.NewTargetSeat == bingzhongRedirect.Seat) || history.OfType<DesignatedExtraTargetRedirectedEvent>().Any(e => e.ProgramFrameId == bingzhongRedirect.FrameId && e.CardUseFrameId == use.Id && e.ActionId == completed.ActionId && e.Source == bingzhongRedirect.Source && e.GameplayHash == bingzhongRedirect.GameplayHash && e.InstructionIndex == bingzhongRedirect.InstructionIndex && e.TargetIndex == bingzhongRedirect.TargetIndex && e.OriginalTargetSeat == bingzhongRedirect.PreviousSeat && e.NewTargetSeat == bingzhongRedirect.Seat) || history.OfType<SoleTargetAfterProtectionRedirectedEvent>().Any(e => e.ProgramFrameId == bingzhongRedirect.FrameId && e.CardUseFrameId == use.Id && e.ActionId == completed.ActionId && e.Source == bingzhongRedirect.Source && e.GameplayHash == bingzhongRedirect.GameplayHash && e.InstructionIndex == bingzhongRedirect.InstructionIndex && e.TargetIndex == bingzhongRedirect.TargetIndex && e.PreviousTargetSeat == bingzhongRedirect.PreviousSeat && e.NewTargetSeat == bingzhongRedirect.Seat))
                    continue;
                if (bingzhongRedirect.TargetIndex < 0 || bingzhongRedirect.TargetIndex >= actual.Count || actual[bingzhongRedirect.TargetIndex] != bingzhongRedirect.PreviousSeat)
                    return false;
                actual[bingzhongRedirect.TargetIndex] = bingzhongRedirect.Seat;
            }
            else if (fact is WeifuRedirectedEvent weifuRedirect && weifuRedirect.UseId == use.Id)
            {
                if (!_characterQianZhaoOlOperations.HasWeifuTargetTail(use) || weifuRedirect.ActionId != completed.ActionId || history.OfType<WeifuRedirectedEvent>().Count(e => e.FrameId == weifuRedirect.FrameId && e.UseId == use.Id && e.InstructionIndex == weifuRedirect.InstructionIndex) != 1)
                    return false;
                // Companion receipts acknowledge this same native redirect; the
                // existing dedicated branch consumes its actual target change.
                if (history.OfType<ProgramActualSlashTargetRedirectedEvent>().Any(e => e.ProgramFrameId == weifuRedirect.FrameId && e.CardUseFrameId == use.Id && e.ActionId == completed.ActionId && e.Source == weifuRedirect.Source && e.GameplayHash == weifuRedirect.GameplayHash && e.InstructionIndex == weifuRedirect.InstructionIndex && e.TargetIndex == weifuRedirect.TargetIndex && e.OriginalTargetSeat == weifuRedirect.PreviousSeat && e.NewTargetSeat == weifuRedirect.TargetSeat) || history.OfType<ProgramCurrentSlashFireRedirectedEvent>().Any(e => e.ProgramFrameId == weifuRedirect.FrameId && e.CardUseFrameId == use.Id && e.ActionId == completed.ActionId && e.Source == weifuRedirect.Source && e.TargetIndex == weifuRedirect.TargetIndex && e.OriginalTargetSeat == weifuRedirect.PreviousSeat && e.NewTargetSeat == weifuRedirect.TargetSeat) || history.OfType<DesignatedExtraTargetRedirectedEvent>().Any(e => e.ProgramFrameId == weifuRedirect.FrameId && e.CardUseFrameId == use.Id && e.ActionId == completed.ActionId && e.Source == weifuRedirect.Source && e.GameplayHash == weifuRedirect.GameplayHash && e.InstructionIndex == weifuRedirect.InstructionIndex && e.TargetIndex == weifuRedirect.TargetIndex && e.OriginalTargetSeat == weifuRedirect.PreviousSeat && e.NewTargetSeat == weifuRedirect.TargetSeat) || history.OfType<SoleTargetAfterProtectionRedirectedEvent>().Any(e => e.ProgramFrameId == weifuRedirect.FrameId && e.CardUseFrameId == use.Id && e.ActionId == completed.ActionId && e.Source == weifuRedirect.Source && e.GameplayHash == weifuRedirect.GameplayHash && e.InstructionIndex == weifuRedirect.InstructionIndex && e.TargetIndex == weifuRedirect.TargetIndex && e.PreviousTargetSeat == weifuRedirect.PreviousSeat && e.NewTargetSeat == weifuRedirect.TargetSeat))
                    continue;
                if (weifuRedirect.TargetIndex < 0 || weifuRedirect.TargetIndex >= actual.Count || actual[weifuRedirect.TargetIndex] != weifuRedirect.PreviousSeat)
                    return false;
                actual[weifuRedirect.TargetIndex] = weifuRedirect.TargetSeat;
            }
            else if (fact is CardActionAcceptedEvent finalized && finalized.Action.ActionId == completed.ActionId)
            {
                if (finalized.Action.ActorSeat != actor || finalized.Action.ProviderSeat != completed.ProviderSeat || finalized.Action.EffectiveKind != kind || !finalized.Action.ConversionChain.SequenceEqual(chain) || !finalized.Action.PhysicalCards.SequenceEqual(completed.PhysicalCards) || !actual.SequenceEqual(finalized.Action.TargetSeats))
                    return false;
                designated = actual.ToList();
            }
        }

        return actor == completed.ActorSeat && kind == completed.EffectiveKind && chain.SequenceEqual(completed.ConversionChain) && designated.SequenceEqual(completed.TargetSeats) && actual.SequenceEqual(use.TargetSeats);
    }

    internal bool IsBingzhongAcceptedUseTargetFact(CardUseFrame use, BingzhongTargetsAppliedEvent fact, IReadOnlyList<int> precedingTargets, int actor, int provider, IReadOnlyList<IGameEvent> history, out IReadOnlyList<int> resultTargets)
    {
        resultTargets = [];
        if (use.BingzhongTail is not { AddedTargets.Count: > 0 } tail || tail.OriginalActor != actor || tail.ProviderSeat != provider || !tail.OriginalTargets.SequenceEqual(precedingTargets) || fact != new BingzhongTargetsAppliedEvent(tail.FrameId, tail.UseId, tail.ActionId, tail.Source, tail.GameplayHash, tail.X, tail.Ordinal, tail.OriginalActor, tail.ProviderSeat, tail.OriginalTargets.Count, tail.AddedTargets.Count) || !_characterYangFuOlOperations.HasBingzhongTargetTail(use) || history.OfType<BingzhongTargetsAppliedEvent>().Where(e => e.UseId == use.Id).ToArray()is not [var applied] || applied != fact || history.OfType<ProgramBindingStartedEvent>().Where(e => e.FrameId == tail.FrameId).ToArray()is not [var binding] || binding.SkillId != tail.Source.SkillId || binding.BindingId != tail.Source.BindingId || binding.SkillInstanceId != tail.Source.SkillInstanceId || binding.OwnerSeat != actor || binding.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || !ActualHandGainMutationBinding(tail.FrameId, tail.Source.SkillId, actor, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramEffectOp.ResolveBingzhongExtraTargets) || history.OfType<YangFuStartedEvent>().Where(e => e.FrameId == tail.FrameId).ToArray()is not [var start] || start.Source != tail.Source || start.GameplayHash != tail.GameplayHash || start.Op != SkillProgramEffectOp.ResolveBingzhongExtraTargets || start.TargetSeat != actor || start.X != tail.X || start.Ordinal != tail.Ordinal || start.UseId != use.Id || start.ActionId != tail.ActionId || start.OriginalTargetCount != precedingTargets.Count || start.ActualTurn < 1 || start.ParentId <= 0)
            return false;
        var original = history.OfType<BingzhongOriginalTargetEvent>().Where(e => e.FrameId == tail.FrameId).OrderBy(e => e.Index).ToArray();
        var selected = history.OfType<BingzhongTargetSelectedEvent>().Where(e => e.FrameId == tail.FrameId).OrderBy(e => e.Index).ToArray();
        if (!original.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, original.Length)) || !original.Select(e => e.Seat).SequenceEqual(precedingTargets) || !selected.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, selected.Length)) || !selected.Select(e => e.Seat).SequenceEqual(tail.AddedTargets))
            return false;
        resultTargets = tail.OriginalTargets.Concat(tail.AddedTargets).ToArray();
        return true;
    }

    internal bool IsWeifuAcceptedUseTargetFact(CardUseFrame use, WeifuTargetAddedEvent fact, IReadOnlyList<int> precedingTargets, int actor, int provider, IReadOnlyList<IGameEvent> history)
    {
        if (use.WeifuUse is not { Offered: true, AddedTargets.Count: > 0 } receipt || use.SourceSeat != actor || receipt.ProviderSeat != provider || !_characterQianZhaoOlOperations.HasWeifuTargetTail(use) || fact.ActionId != receipt.ActionId || fact.Source.OwnerSeat != receipt.ActorSeat || fact.Source.SkillId != "ol:weifu" || fact.Source.BindingId != "issued-category-targets" || fact.Index < 0 || fact.Index >= receipt.AddedTargets.Count || receipt.AddedTargets[fact.Index] != fact.TargetSeat || !precedingTargets.SequenceEqual(receipt.OriginalTargets.Concat(receipt.AddedTargets.Take(fact.Index))) || history.OfType<ProgramBindingStartedEvent>().Where(e => e.FrameId == fact.FrameId).ToArray()is not [var binding] || binding.SkillId != fact.Source.SkillId || binding.BindingId != fact.Source.BindingId || binding.SkillInstanceId != fact.Source.SkillInstanceId || binding.OwnerSeat != receipt.ActorSeat || binding.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || !ActualHandGainMutationBinding(fact.FrameId, fact.Source.SkillId, receipt.ActorSeat, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramEffectOp.OfferWeifuCategoryTargets))
            return false;
        var added = history.OfType<WeifuTargetAddedEvent>().Where(e => e.UseId == use.Id).ToArray();
        return added.Length == receipt.AddedTargets.Count && added[fact.Index] == fact && added.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, added.Length)) && added.Select(e => e.TargetSeat).SequenceEqual(receipt.AddedTargets) && added.All(e => e.FrameId == fact.FrameId && e.ActionId == receipt.ActionId && e.Source == fact.Source);
    }
}
