#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoCaoOlMagicRuntime
{
    internal MoCaoCaoFuzaiRuntime MoCaoCaoEquipment => _moCaoCaoFuzaiOperations.Get();
    internal MoCaoCaoBachaoRuntime MoCaoCaoBachao => _moCaoCaoBachaoOperations.Get();

    [ProgramPipelineHook(ProgramPipelinePhase.VirtualEquipment, 33800)]
    internal CardKind? MoCaoCaoVirtualEquipment(int seat, EquipmentSlot slot) => MoCaoCaoEquipment.VirtualKind(seat, slot);
    bool IMoCaoCaoFacts.Enabled => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeMoCaoCaoFuzai);

    IReadOnlyList<int> IMoCaoCaoFacts.Seats => _coreTableOrderOperations.CurrentTableOrder();

    MoCaoCaoParticipant IMoCaoCaoFacts.Participant(int seat)
    {
        var p = _coreMatchParticipantsPlayersAccess._players[seat];
        return new(seat, p.IsAlive, p.IsHuman, p.Hp, _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count, p.EquipmentSlotCapacity(EquipmentSlot.Weapon) > 0, p.EquipmentSlotCapacity(EquipmentSlot.Armor) > 0);
    }

    MoCaoCaoBinding? IMoCaoCaoFacts.FuzaiBinding(int seat)
    {
        if (!((IMoCaoCaoFacts)this).Enabled || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            return null;
        var c = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramTriggerWindow.GameStarting).FirstOrDefault(c => c.SkillId == MoCaoCaoComposition.Fuzai);
        return c is null ? null : new(new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash);
    }

    ContentSkillDefinition IMoCaoCaoFacts.Skill(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id);
    SkillProgram? IMoCaoCaoFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(id, out var skill) ? skill.Program : null;
    ActualDiscardRecoveryPhaseKey? IMoCaoCaoFacts.Phase => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();

    IReadOnlyList<int> IMoCaoCaoFacts.Hand(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select(c => c.Id).ToArray());
    CardLocation IMoCaoCaoFacts.Location(int id) => _cardZonesOperations._cardZones.GetLocation(id);
    CardKind IMoCaoCaoFacts.Kind(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind;
    IEnumerable<T> IMoCaoCaoFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    bool IMoCaoCaoFacts.Held(ProgramSkillFrame f) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId);
    bool IMoCaoCaoFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    long IMoCaoCaoFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMoCaoCaoFacts.IsExactStart(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.WindowContext is not { } w || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op || ((IMoCaoCaoFacts)this).Program(f.SkillId)?.GameplayHash != f.GameplayHash)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != w.ParentFrameId)
            return false;
        if (op == SkillProgramEffectOp.ResolveMoCaoCaoBaibi)
            return _resolutionStackOperations._resolutionStack[index - 1] is DamageTriggerWindowFrame damage && damage.TriggerWindow == SkillProgramTriggerWindow.AfterDamageApplied && damage.CandidateIndex >= 0 && damage.CandidateIndex < damage.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, damage.Candidates[damage.CandidateIndex].ToProgramCandidate());
        if (op == SkillProgramEffectOp.ResolveMoCaoCaoBachao)
            return w.Window == SkillProgramTriggerWindow.PlayPhaseStarting && w.OwnerSeat == f.OwnerSeat && w.SourceSeat == f.OwnerSeat && w.TargetSeat == f.OwnerSeat && _resolutionStackOperations._resolutionStack[index - 1] is PlayPhaseStartingBoundaryFrame starting && starting.OwnerSeat == f.OwnerSeat && starting.ItemIndex >= 0 && starting.ItemIndex < starting.Items.Count && starting.Items[starting.ItemIndex] is { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate } && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
        return op == SkillProgramEffectOp.InitializeMoCaoCaoFuzai && _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame parent && parent.Window == SkillProgramTriggerWindow.GameStarting && w.Window == parent.Window && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]);
    }

    int IMoCaoCaoFacts.ResolveHandCard(ProgramSkillFrame f, int chooser, PromptChoice c)
    {
        if (c.Cards is [var id])
            return id;
        var subject = f.MoCaoCaoBaibi?.Claim.TargetSeat ?? chooser;
        var zone = Enum.Parse<CardZoneKind>(c.Parameters["source-zone"], true);
        var slot = int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture);
        return _cardZonesOperations._cardZones.CardsAt(new(zone, subject))[slot].Id;
    }

    IReadOnlyList<PromptChoice> IMoCaoCaoFacts.PaymentChoices(ProgramSkillFrame f, int chooser, int subject, bool handOnly) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, chooser, subject, handOnly ? [CardZoneKind.Hand] : [CardZoneKind.Hand, CardZoneKind.Equipment], handOnly ? OwnedCardMoveIntent.Obtain : OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.OwnerHand, destinationSeat: f.OwnerSeat);
    MoCaoCaoDamageOrigin? IMoCaoCaoFacts.DamageOrigin
    {
        get
        {
            if (_coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsChainPropagation: false, IsSourceLess: false } a || a.CardUserSeat != a.SourceSeat || a.EffectiveCardKind is not { } kind || _coreCardUseLifecycleOperations.LifecycleCardUse(a.ResolutionId)?.Action is not { Type: CardActionType.Use } action || action.ActorSeat != a.SourceSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault(d => d.ParentFrameId == a.ResolutionId && d.SourceSeat == a.SourceSeat && d.TargetSeat == a.TargetSeat)is not { } damage)
                return null;
            return new(damage.Id, a.ResolutionId, action.ActionId, a.SourceSeat, a.TargetSeat, kind, a.DamageAmount, a.IsSourceLess);
        }
    }

    MoCaoCaoWeaponAuthorization? IMoCaoCaoFacts.WeaponAuthorization(int seat, CardKind kind)
    {
        if (!_coreAdvancedWeaponsOperations.HasWeaponAbility(_coreMatchParticipantsPlayersAccess._players[seat], kind))
            return null;
        var physical = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).FirstOrDefault(c => !c.IsGeneralWeapon && _coreAdvancedWeaponsOperations.GetWeaponAbilityKinds(c).Contains(kind));
        if (physical is not null)
            return new(physical.Id, 0, $"equipment:{(physical.Id)}:{(kind)}");
        if (MoCaoCaoEquipment.VirtualKind(seat, EquipmentSlot.Weapon) == kind && MoCaoCaoEquipment.LastIssue(seat)is { } issue)
            return new(0, issue.IssueId, $"fuzai:{(issue.IssueId)}:{(kind)}");
        return (_coreGameEngineAdvancedLifecycleInheritedWeaponAbilitiesAccess._inheritedWeaponAbilities.GetValueOrDefault(seat) ?? []).Contains(kind) ? new(0, 0, $"inherited:{(seat)}:{(kind)}") : null;
    }

    bool IMoCaoCaoFacts.ExactIssuedCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || !window.Candidates.Any(c => c.ToProgramCandidate() == candidate))
            return false;
        return (_coreProgramLifecycleOperations.CreateAfterDamageProgramContext(window, candidate)with
        {
            Facts = null
        }

        ) == (context with
        {
            Facts = null
        }

        );
    }

    bool IMoCaoCaoFacts.NativeChild(ProgramSkillFrame f, ResolutionFrame child) => MoCaoCaoFirstNativeChild(f, child);
    bool IMoCaoCaoFacts.Friendly(int chooser, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[chooser].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(chooser), target, new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false)) > 0;
    int IMoCaoCaoFacts.KeepValue(int chooser, int card) => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(card), _coreMatchParticipantsPlayersAccess._players[chooser]);
    int IMoCaoCaoOperations.NextRandom(int count) => _randomOperations._random.Next(count);
    void IMoCaoCaoOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IMoCaoCaoOperations.LogEquipment(int seat, CardKind weapon, CardKind armor) => _coreMatchLogOperations.AddLog("Fuzai", $"{(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 的覆载装备为【{(_coreEquipmentContentOperations.EquipmentRules.Get(weapon).DisplayName)}】、【{(_coreEquipmentContentOperations.EquipmentRules.Get(armor).DisplayName)}】。", seat);
    void IMoCaoCaoOperations.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    bool IMoCaoCaoOperations.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    MoCaoCaoMovement IMoCaoCaoOperations.Transfer(ProgramSkillFrame f, int payer, int id, string reason)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        if (from.OwnerSeat != payer || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("Bachao/Baibi lost the actual HE payment.");
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), from, CardLocation.Hand(f.OwnerSeat), new CardMoveReason(reason), m =>
        {
            if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == f.Id)is not { } parent || !MoCaoCaoBachao.Valid(parent) || m.CardId != id || m.From != from || m.Reason.Value != reason || m.Sequence <= before)
                throw new InvalidOperationException("Magic Cao Cao payment lost its owning ancestor or actual movement.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent.MoCaoCaoBachao is { } r ? parent with { MoCaoCaoBachao = r with { After = m.Sequence } } : parent with { MoCaoCaoBaibi = parent.MoCaoCaoBaibi!with { After = m.Sequence } });
            if (parent.MoCaoCaoBachao is not null)
                _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, payer, f.OwnerSeat, [id], before);
        }, disclosure: from.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        return new(before, after, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.From == from && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == reason && m.Sequence > before && m.Sequence <= after));
    }

    void IMoCaoCaoOperations.Damage(ProgramSkillFrame f, int target) => _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, 1);
    void IMoCaoCaoOperations.EnterRumo(ProgramSkillFrame f) => _characterSharedGameEngineOlMoRumoOperations.EnterOlMoRumo(f);
    bool IMoCaoCaoOperations.AdvanceObserverChildren() => _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 33800)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeMoCaoCaoBaibi(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> candidates) => MoCaoCaoBachao.IncludeBaibi(owner.Seat, window, candidates);
}
