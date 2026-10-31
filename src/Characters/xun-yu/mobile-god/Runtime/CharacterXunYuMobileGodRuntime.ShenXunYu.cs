#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXunYuMobileGodRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IShenXunYuHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => ResolveShenXunYu(frame, op);
    internal bool HasDinghanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RecordAndCancelDinghanTarget);

    internal HashSet<CardKind> DinghanNames(int seat)
    {
        var result = new HashSet<CardKind>();
        foreach (var e in _coreProgramEventHistoryOperations.ProgramEventHistory<DinghanNameChangedEvent>().Where(e => e.Owner == seat))
            if (e.Added)
                result.Add(e.Kind);
            else
                result.Remove(e.Kind);
        return result;
    }

    internal CardActionContext? ShenXunYuAction(ProgramSkillWindowContext? context) => context is { ParentFrameId: > 0 } c ? _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)?.Action : null;
    internal bool CanLingce(int seat, CardActionContext? a) => a is not null && CardCatalog.Get(a.EffectiveKind).CategoryName == "锦囊牌" && a.PhysicalCards.Count == 1 && a.ConversionChain.Count == 0 && a.PhysicalCards[0].CardKind == a.EffectiveKind && !_coreProgramEventHistoryOperations.ProgramEventHistory<LingceDrawIssuedEvent>().Any(e => e.Owner == seat && e.ActionId == a.ActionId) && (a.EffectiveKind is CardKind.DrawTwo or CardKind.Dismantlement or CardKind.Nullification or CardKind.QizhengXiangsheng || DinghanNames(seat).Contains(a.EffectiveKind));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 38300, SkillProgramEffectOp.DrawLingceActualTrick, SkillProgramEffectOp.RecordAndCancelDinghanTarget)]
    internal bool CanTriggerShenXunYu(ProgramTriggerLegalityContext gate)
    {
        var a = ShenXunYuAction(gate.WindowContext);
        if (gate.Features.HasOperation(SkillProgramEffectOp.DrawLingceActualTrick))
            return CanLingce(gate.OwnerSeat, a);
        return a is not null && CardCatalog.Get(a.EffectiveKind).CategoryName == "锦囊牌" && !DinghanNames(gate.OwnerSeat).Contains(a.EffectiveKind) && a.TargetSeats.Contains(gate.OwnerSeat);
    }

    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 38300)]
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 38300)]
    internal bool DinghanRequiresFinalizedTrickWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RecordAndCancelDinghanTarget);
    internal SkillProgramStepOutcome ResolveShenXunYu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (op == SkillProgramEffectOp.DrawLingceActualTrick)
        {
            var a = ShenXunYuAction(f.WindowContext);
            if (!CanLingce(f.OwnerSeat, a))
                throw new InvalidOperationException("Lingce requires an unpaid qualifying actual native trick use.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingceDrawIssuedEvent(f.Id, f.OwnerSeat, a!.ActionId, a.EffectiveKind));
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.EditDinghanNames)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenXunYuEditing = true });
            PublishDinghanEdit(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (op == SkillProgramEffectOp.AddTianzuoCards)
        {
            if (f.TriggerId != "eight-qizheng" || f.WindowContext?.Window != SkillProgramTriggerWindow.GameStarting || _coreProgramEventHistoryOperations.ProgramEventHistory<TianzuoCardCreatedEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Tianzuo requires its original unpaid game-start instruction.");
            var next = checked(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1);
            var cards = new List<Card>();
            for (var i = 0; i < 8; i++)
            {
                var card = new Card(next + i, CardKind.QizhengXiangsheng, i < 4 ? Suit.Spade : Suit.Club, i < 4 ? 2 + 2 * i : 3 + 2 * (i - 4));
                _cardZonesOperations._cardZones.AddGeneratedCard(card);
                _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(card.Id);
                cards.Add(card);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianzuoCardCreatedEvent(f.Id, f.OwnerSeat, card.Id, card.Suit, card.Rank));
            }

            _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.OutsideGame, CardLocation.DrawPile, new("skill-program.mobile:tianzuo.add"), disclosure: CardMovementDisclosure.FaceDown);
            var before = _randomOperations._random.State;
            _cardZonesOperations._cardZones.Shuffle(CardLocation.DrawPile, _randomOperations._random);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianzuoDeckShuffledEvent(f.Id, before, _randomOperations._random.State, _cardZonesOperations._cardZones.Count(CardLocation.DrawPile)));
            return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat);
        }

        if (op != SkillProgramEffectOp.RecordAndCancelDinghanTarget || f.WindowContext is not { ParentFrameId: > 0 } context || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { } action } use || !action.TargetSeats.Contains(f.OwnerSeat) || DinghanNames(f.OwnerSeat).Contains(action.EffectiveKind))
            throw new InvalidOperationException("Dinghan requires its actual new incoming trick designation.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DinghanNameChangedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, action.EffectiveKind, true));
        var targets = use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Chunk(2).Where(pair => pair[0] != f.OwnerSeat).SelectMany(pair => pair).ToArray() : use.TargetSeats.Where(seat => seat != f.OwnerSeat).ToArray();
        var updated = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(action, targets);
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = updated, TargetIndex = 0, TargetsAdjusted = true, SequentialTrick = targets.Length == 0 ? null : use.SequentialTrick }, updated);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DinghanTargetCanceledEvent(f.Id, use.Id, action.ActionId, f.OwnerSeat, action.EffectiveKind, action));
        return SkillProgramStepOutcome.Continue;
    }

    internal IReadOnlyList<PromptChoice> DinghanEditChoices(ProgramSkillFrame f)
    {
        var recorded = DinghanNames(f.OwnerSeat);
        var choices = new List<PromptChoice>();
        foreach (var kind in CardCatalog.ImplementedCards.Where(card => card.CategoryName == "锦囊牌").Select(card => card.Kind).Order())
            choices.Add(new(new($"dinghan.{(f.Id)}.{(kind)}"), $"{((recorded.Contains(kind) ? "移除" : "增加"))}【{(CardCatalog.Get(kind).DisplayName)}】", [], [], new Dictionary<string, string> { ["program-action"] = "dinghan-edit", ["kind"] = kind.ToString() }));
        choices.Add(new(new($"dinghan.{(f.Id)}.cancel"), "不修改记录", [], [], new Dictionary<string, string> { ["program-action"] = "dinghan-edit", ["kind"] = "cancel" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishDinghanEdit(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, DinghanEditChoices(f), "【定汉】你可以增加或移除一种锦囊牌名。", f.OwnerSeat);
    }

    [ProgramAnswer("dinghan-edit")]
    internal void AnswerDinghanEdit(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenXunYuEditing: true } f || f.SkillId != "mobile:dinghan" || f.TriggerId != "edit-trick-name" || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], DinghanEditChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Dinghan requires its current owner edit choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters["kind"] != "cancel")
        {
            var kind = Enum.Parse<CardKind>(selected.Parameters["kind"]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DinghanNameChangedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, kind, !DinghanNames(f.OwnerSeat).Contains(kind)));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenXunYuEditing = false });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 38300)]
    internal bool ResumeDinghanEdit(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenXunYuEditing: true } f || f.Id != id)
            return false;
        PublishDinghanEdit(f);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.EditDinghanNames)]
    internal PromptChoice ChooseDinghanEdit(PendingDecision prompt) => prompt.Choices.First(c => c.Parameters.GetValueOrDefault("kind") == "cancel");
    internal static int[] DinghanRetainedTargets(CardKind kind, IReadOnlyList<int> before, int owner) => kind == CardKind.BorrowedSword ? before.Chunk(2).Where(pair => pair[0] != owner).SelectMany(pair => pair).ToArray() : before.Where(seat => seat != owner).ToArray();
    internal bool IsDinghanCancellationFact(CardUseFrame use, DinghanTargetCanceledEvent e, IReadOnlyList<int> before) => e.UseId == use.Id && e.ActionId == use.Action?.ActionId && e.Kind == use.CardKind && e.OriginalAction.TargetSeats.SequenceEqual(before) && before.Contains(e.Owner) && _coreCompletedCategoryUseProofOperations.ActualHandGainMutationBinding(e.FrameId, "mobile:dinghan", e.Owner, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramEffectOp.RecordAndCancelDinghanTarget) && _coreProgramEventHistoryOperations.ProgramEventHistory<DinghanNameChangedEvent>().Count(n => n.FrameId == e.FrameId && n.Owner == e.Owner && n.Kind == e.Kind && n.Added) == 1;
    internal bool HasDinghanCancellation(CardUseFrame use) => _coreProgramEventHistoryOperations.ProgramEventHistory<DinghanTargetCanceledEvent>().Any(e => e.UseId == use.Id && IsDinghanCancellationFact(use, e, e.OriginalAction.TargetSeats) && !use.TargetSeats.Contains(e.Owner));
}
