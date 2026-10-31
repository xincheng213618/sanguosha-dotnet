#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiWenJiLeRuntime
{
    internal BeifenUseQualification? CaptureBeifenUseQualification(CharacterState actor)
    {
        if (!HasLeCaiWenJiBeifenCapability || !_coreMatchSetupSetupCompleteAccess._setupComplete || !actor.IsAlive)
            return null;
        var source = _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.SelectMany(i => i.Program.Triggers.Where(IsBeifenLossTrigger).Select(t => new BeifenLossSource(new(i.SkillId, t.Id, actor.Seat, i.SkillInstanceId), i.Program.GameplayHash))).OrderBy(s => s.Source.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
        if (source is null)
            return null;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(actor);
        var marked = hand.Count(c => HasShuangjiaEntityMark(c.Id));
        return marked < hand.Count - marked ? new(source.Source, source.GameplayHash, actor.Seat, marked, hand.Count - marked, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) : null;
    }

    internal bool HasBeifenLiveUsePrivilege(CharacterState actor) => CaptureBeifenUseQualification(actor)is not null;
    internal bool HasBeifenPreUsePrivilege(CharacterState actor)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is CardDeclarationFrame { Stage: CardDeclarationStage.Applying } declaration && declaration.Return.ActorSeat == actor.Seat && BeifenDeclarationIsTrueUse(declaration.Return))
            return declaration.Payment.Beifen is { } q && ValidBeifenDeclaration(declaration.Payment, q);
        var current = _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (current is ResponseWindowFrame response)
            current = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == response.ParentFrameId);
        if (current?.AcceptedDeclarationPayment is { Claimed: false } accepted && accepted.ActorSeat == actor.Seat && _cardZonesOperations._cardZones.GetLocation(accepted.Cost.CardId) == CardLocation.Processing)
            return accepted.Beifen is { } paid && ValidBeifenDeclaration(accepted, paid);
        return HasBeifenLiveUsePrivilege(actor);
    }

    internal bool HasBeifenUseDistance(CharacterState actor, long? existingUseFrameId = null) => existingUseFrameId is { } id ? HasIssuedBeifenUse(id, actor.Seat) : HasBeifenPreUsePrivilege(actor);
    internal static bool BeifenDeclarationIsTrueUse(CardDeclarationParentReturn parent) => parent.Purpose is CardDeclarationPurpose.Use or CardDeclarationPurpose.Dodge or CardDeclarationPurpose.Counterspell or CardDeclarationPurpose.Recovery or CardDeclarationPurpose.BorrowedSword or CardDeclarationPurpose.ProvidedSlash or CardDeclarationPurpose.BorrowedSwordProvidedSlash or CardDeclarationPurpose.QinglongProvidedSlash;
    internal DeclaredCardPayment CaptureBeifenDeclarationPayment(DeclaredCardPayment payment, CardDeclarationParentReturn parent)
    {
        if (!BeifenDeclarationIsTrueUse(parent) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(parent.ActorSeat) || payment.ActorSeat != parent.ActorSeat || payment.Claimed || payment.Beifen is not null || _cardZonesOperations._cardZones.GetLocation(payment.Cost.CardId) != payment.Cost.From || CaptureBeifenUseQualification(_coreMatchParticipantsPlayersAccess._players[parent.ActorSeat])is not { } q)
            return payment;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeifenDeclarationQualifiedEvent(payment.DeclarationId, payment.OwnerFrameId, payment.ProviderSeat, payment.Cost.CardId, payment.Cost.From, payment.DeclaredKind, q));
        return payment with
        {
            Beifen = q
        };
    }

    internal bool ValidBeifenQualification(BeifenUseQualification q) => q.Source.SkillId == "le:beifen" && q.Source.BindingId == "lost-hujia-suits" && q.Source.OwnerSeat == q.ActorSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(q.ActorSeat) && q.MarkedHandCount >= 0 && q.OtherHandCount > q.MarkedHandCount && q.SequenceBefore <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program?.GameplayHash == q.GameplayHash;
    internal bool ValidBeifenDeclaration(DeclaredCardPayment payment, BeifenUseQualification q) => ValidBeifenQualification(q) && payment.ActorSeat == q.ActorSeat && payment.Beifen == q && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == payment.OwnerFrameId)is { } owner && (owner.AcceptedDeclarationPayment == payment || owner is CardDeclarationFrame declaration && declaration.Payment == payment) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BeifenDeclarationQualifiedEvent>().Where(e => e.DeclarationId == payment.DeclarationId).ToArray()is [var fact] && fact.OwnerFrameId == payment.DeclarationId && fact.Qualification == q && fact.ProviderSeat == payment.ProviderSeat && fact.CardId == payment.Cost.CardId && fact.From == payment.Cost.From && fact.DeclaredKind == payment.DeclaredKind;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28420)]
    internal void CaptureBeifenZeroMaterialNativeUse(IGameEvent payload)
    {
        if (HasLeCaiWenJiBeifenCapability && payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { BeifenUse: null, Action: { Type: CardActionType.Use } action } use && use.CardId == 0 && declared.CardId == 0 && (use.PhysicalCardIds is null or { Count: 0 }) && action.PhysicalCards.Count == 0 && use.SourceSeat == declared.SourceSeat && use.CardKind == declared.CardKind)
            IssueBeifenUse(use);
    }

    internal DeclaredCardPayment? BeifenOriginalDeclaration(CardUseFrame use)
    {
        if (use.Action is not { Type: CardActionType.Use } action)
            return null;
        if (use.AcceptedDeclarationPayment is { } own && own.ActorSeat == action.ActorSeat && action.PhysicalCards.Any(c => c.CardId == own.Cost.CardId && c.CardKind == own.Cost.CardKind && c.From == own.Cost.From))
            return own;
        // Only the actual pending claim for this native action is eligible; a
        // previously claimed payment elsewhere in history cannot lend rights.
        if (action.PhysicalCards is [var cost] && _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(action.ProviderSeat, cost.CardId)is { } pending && pending.ActorSeat == action.ActorSeat && pending.Cost.CardKind == cost.CardKind && pending.Cost.From == cost.From)
            return pending;
        return null;
    }

    internal void IssueBeifenUse(CardUseFrame use)
    {
        if (!HasLeCaiWenJiBeifenCapability || use.Action is not { Type: CardActionType.Use } action)
            return;
        var declaration = BeifenOriginalDeclaration(use);
        BeifenUseQualification? q;
        if (declaration is not null)
        {
            q = declaration.Beifen;
            if (q is not null && !ValidBeifenDeclaration(declaration, q))
                throw new InvalidOperationException("Beifen's declaration right lost its exact paid original entity.");
        }
        else
            q = CaptureBeifenUseQualification(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]);
        if (q is null)
            return;
        if (!ValidBeifenQualification(q) || q.ActorSeat != action.ActorSeat || use.SourceSeat != action.ActorSeat)
            throw new InvalidOperationException("Beifen cannot lend another actor's accepted use privilege.");
        var r = new BeifenUseReceipt(use.Id, action.ActionId, use.CardKind, q, declaration?.DeclarationId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { BeifenUse = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeifenUseAcceptedEvent(use.Id, action.ActionId, q.Source, q.GameplayHash, action.ActorSeat, q.MarkedHandCount, q.OtherHandCount, use.CardKind, q.SequenceBefore, r.DeclarationId));
    }

    internal bool HasIssuedBeifenUse(long useId, int actorSeat)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { BeifenUse: { } r, Action: { Type: CardActionType.Use } action } use || r.UseId != use.Id || r.ActionId != action.ActionId || action.ActorSeat != actorSeat || use.SourceSeat != actorSeat || r.Qualification.ActorSeat != actorSeat || !ValidBeifenQualification(r.Qualification) || !(use.CardKind == r.OriginalKind || CoreNativeCardClassificationsRuntime.IsSlashCard(r.OriginalKind) && use.CardKind == CardKind.FireSlash && _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use)) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BeifenUseAcceptedEvent>().Where(e => e.UseId == useId).ToArray()is not [var accepted])
            return false;
        var q = r.Qualification;
        return accepted.ActionId == action.ActionId && accepted.Source == q.Source && accepted.GameplayHash == q.GameplayHash && accepted.ActorSeat == actorSeat && accepted.MarkedHandCount == q.MarkedHandCount && accepted.OtherHandCount == q.OtherHandCount && accepted.OriginalKind == r.OriginalKind && accepted.SequenceBefore == q.SequenceBefore && accepted.DeclarationId == r.DeclarationId;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 28420)]
    internal void AssertBeifenUses()
    {
        if (!HasLeCaiWenJiBeifenCapability)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.BeifenUse is not null))
            if (!HasIssuedBeifenUse(use.Id, use.SourceSeat))
                throw new InvalidOperationException("Beifen's native use lost its exact accepted actor, source, ratio or action invoice.");
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 50)]
    internal bool DistanceCardUseExemption50HasBeifenUseDistance(ProgramCardDistanceQuery q) => (HasBeifenUseDistance(q.Actor, q.CardUseFrameId) || _characterHuangGaiMouMobileOperations.HasMouZhaJiangUseDistance(q.Actor, q.CardUseFrameId));
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 10)]
    internal bool DistanceBorrowedPaidWeaponSlash10HasBeifenPreUsePrivilege(ProgramSlashDistanceQuery q) => (HasBeifenPreUsePrivilege(q.Actor) || _characterHuangGaiMouMobileOperations.HasMouZhaJiangPreUsePrivilege(q.Actor));
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 10)]
    internal bool BeifenSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => HasBeifenPreUsePrivilege(query.Actor);
}
