namespace CardGame.Core;
public sealed partial class GameEngine
{
    private bool IsBoundaryZhouFeiUseQuery(CharacterState actor, int? selectedId)
    {
        if (selectedId is { } paidId && _resolutionStack.LastOrDefault() is CardUseFrame { BoundaryZhouFeiUse: { } issued } use &&
            issued.CardId == paidId && _cardZones.GetLocation(paidId) == issued.From && _resolutionStack.SingleOrDefault(frame => frame.Id == issued.ParentId) is ProgramSkillFrame parent &&
            parent.OwnerSeat == actor.Seat && ValidBoundaryZf(parent) && BoundaryZfNativeRidesOn(use, parent)) return true;
        if (selectedId is not { } id || _resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryZhouFei: { } receipt } frame || actor.Seat != frame.OwnerSeat || !actor.IsAlive ||
            receipt.Op != SkillProgramEffectOp.ReturnBoundaryZhouFeiKongsheng || receipt.Stage is not (BoundaryZhouFeiStage.UseChoice or BoundaryZhouFeiStage.IssuingUse) || receipt.Cursor >= receipt.Deposits.Count || !ValidBoundaryZf(frame)) return false;
        var deposit = BoundaryZfDeposit(receipt.Deposits[receipt.Cursor]);
        return _cardZones.GetLocation(id) == deposit.Location && _cardZones.CardsAt(deposit.Location).Any(card => card.Id == id) &&
            (receipt.Stage == BoundaryZhouFeiStage.UseChoice || receipt.Native is { UseId: 0, ActionId: 0 } native && native.CardId == id && native.From == deposit.Location && native.DepositId == deposit.FrameId && native.ParentId == frame.Id);
    }
    private Card? FindBoundaryZhouFeiUseCard(CharacterState actor, int? id) => IsBoundaryZhouFeiUseQuery(actor, id) ? GetAdvancedCard(id!.Value) : null;
    private IReadOnlyList<LegalAction> BoundaryZfNativeActions(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryZhouFei!;
        if (receipt.Stage != BoundaryZhouFeiStage.UseChoice || receipt.Cursor >= receipt.Deposits.Count || !_players[frame.OwnerSeat].IsAlive) return [];
        var cards = _cardZones.CardsAt(BoundaryZfDeposit(receipt.Deposits[receipt.Cursor]).Location);
        return cards.SelectMany(card => BuildLegalActionsCore(_players[frame.OwnerSeat], false, card.Id, card)).Where(action => action.CardId is not null && cards.Any(card => card.Id == action.CardId) &&
            action.ConversionSource is null && action.Kind is not (LegalActionKind.EndPlay or LegalActionKind.Recast or LegalActionKind.UseEquipmentEffect or LegalActionKind.UseProgramSkill or LegalActionKind.RevealGeneral)).ToArray();
    }
    private void IssueBoundaryZfUse(ProgramSkillFrame frame, int index)
    {
        var receipt = frame.BoundaryZhouFei!; var actions = BoundaryZfNativeActions(frame);
        if (index < 0 || index >= actions.Count) throw new InvalidOperationException("Kongsheng cannot use an absent native candidate.");
        var action = actions[index]; var id = action.CardId!.Value; var deposit = BoundaryZfDeposit(receipt.Deposits[receipt.Cursor]);
        var native = new BoundaryZhouFeiNative(frame.Id, receipt.Source, receipt.Hash, deposit.FrameId, id, deposit.Location, action, 0, 0);
        ReplaceRuntimeTop(frame with { BoundaryZhouFei = receipt with { Stage = BoundaryZhouFeiStage.IssuingUse, Native = native }, PendingMovementContinuation = null });
        ExecuteAction(_players[frame.OwnerSeat], action);
    }
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 54700)]
    private void BindBoundaryZfUse(CardUseDeclaredEvent declared)
    {
        if (_resolutionStack.Count < 2 || _resolutionStack[^2] is not ProgramSkillFrame { BoundaryZhouFei: { Stage: BoundaryZhouFeiStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } native } receipt } parent ||
            LifecycleCardUse(declared.ResolutionId) is not { Action: { } action } use) return;
        if (!ValidBoundaryZf(parent) || declared.SourceSeat != parent.OwnerSeat || use.SourceSeat != parent.OwnerSeat || use.CardId != native.CardId || action.ActorSeat != parent.OwnerSeat || action.ProviderSeat != receipt.Holder || action.RequesterSeat != (receipt.Holder == parent.OwnerSeat ? (int?)null : parent.OwnerSeat) ||
            action.Type != CardActionType.Use || action.PhysicalCards is not [var cost] || cost.CardId != native.CardId || cost.From != native.From || action.ConversionChain.Count != 0 ||
            action.EffectiveKind != (native.Action.PlayedCardKind ?? GetAdvancedCard(native.CardId).Kind)) throw new InvalidOperationException("Kongsheng must issue its exact concealed original entity as a true native Use.");
        var bound = native with { UseId = use.Id, ActionId = action.ActionId };
        ReplaceRuntimeFrame(parent.Id, parent with { BoundaryZhouFei = receipt with { Stage = BoundaryZhouFeiStage.NativeUse, Native = bound } });
        UpdateLifecycleCardUse(use.Id, current => current with { BoundaryZhouFeiUse = bound });
        AdvanceEventRulesAndQueueFact(new BoundaryZhouFeiNativeEvent(parent.Id, native.DepositId, use.Id, action.ActionId, native.CardId));
    }
    private bool BoundaryZfNativeRidesOn(CardUseFrame use, ProgramSkillFrame frame) => frame.BoundaryZhouFei is { Native: { } native } receipt &&
        use.BoundaryZhouFeiUse == native && native.ParentId == frame.Id && native.Source == receipt.Source && native.Hash == frame.GameplayHash &&
        receipt.Deposits.Contains(native.DepositId) && native.From == BoundaryZfDeposit(native.DepositId).Location && native.UseId == use.Id &&
        use.Action is { } action && action.ActionId == native.ActionId && ShownEntityUseActorMatches(use, frame.OwnerSeat, receipt.Holder) &&
        action.PhysicalCards is [var cost] && cost.CardId == native.CardId && cost.From == native.From &&
        ProgramEventHistory<BoundaryZhouFeiNativeEvent>().Count(entry => entry == new BoundaryZhouFeiNativeEvent(frame.Id, native.DepositId, use.Id, native.ActionId, native.CardId)) == 1;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 54700)]
    private bool BoundaryZfNonPlaySlash(long id, int actor) => LifecycleCardUse(id) is { BoundaryZhouFeiUse: { } native } use &&
        _resolutionStack.SingleOrDefault(frame => frame.Id == native.ParentId) is ProgramSkillFrame parent && parent.OwnerSeat == actor && ValidBoundaryZf(parent) && BoundaryZfNativeRidesOn(use, parent);
    [NativeUseValidation(54700)]
    private void AssertBoundaryZfUse(CardUseFrame use)
    {
        if (use.BoundaryZhouFeiUse is not { } native) return;
        if (use.Step != ResolutionFrameStep.Completed || _resolutionStack.LastOrDefault()?.Id != use.Id || _resolutionStack.SingleOrDefault(frame => frame.Id == native.ParentId) is not ProgramSkillFrame parent || !ValidBoundaryZf(parent) || !BoundaryZfNativeRidesOn(use, parent))
            throw new InvalidOperationException("Kongsheng's native use must return its exact original ending owner and physical payment.");
    }
    [NativeUseReturn(54700)]
    private bool ReturnBoundaryZfUse(CardUseFrame use)
    {
        if (use.BoundaryZhouFeiUse is not { } native) return false;
        if (_resolutionStack.Any(frame => frame.Id == use.Id) || _resolutionStack.SingleOrDefault(frame => frame.Id == native.ParentId) is not ProgramSkillFrame parent || !BoundaryZfNativeRidesOn(use, parent) ||
            ProgramEventHistory<BoundaryZhouFeiReturnedUseEvent>().Any(entry => entry.UseId == use.Id)) throw new InvalidOperationException("Kongsheng cannot return or pay the original entity twice.");
        ReplaceRuntimeFrame(parent.Id, parent with { BoundaryZhouFei = parent.BoundaryZhouFei! with { Stage = BoundaryZhouFeiStage.Used } });
        AdvanceEventRulesAndQueueFact(new BoundaryZhouFeiReturnedUseEvent(parent.Id, use.Id, native.ActionId));
        if (use.CardAttack is not { Active: true }) AdvanceRuntimeProgram(parent.Id); return true;
    }
    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 54700)]
    private bool ResumeBoundaryZfAttack()
    { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryZhouFei: { Stage: BoundaryZhouFeiStage.Used, Native: not null } } frame) return false; AdvanceRuntimeProgram(frame.Id); return true; }
}
