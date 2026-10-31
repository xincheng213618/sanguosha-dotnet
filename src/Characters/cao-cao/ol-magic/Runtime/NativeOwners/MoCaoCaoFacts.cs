#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MoCaoCaoFacts(Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, EngineStates.CoreMatchOutcomeState matchOutcomeState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries> getCoreGameEnginePrivateOfferReturnsRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, Func<RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries> getCoreGameEngineAdvancedWeaponsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoCaoOlMagicRuntime>> getCharacterCaoCaoOlMagicRuntime, EngineStates.CoreGameEngineAdvancedLifecycleState gameEngineAdvancedLifecycleState, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot) : IMoCaoCaoFacts
{
    public bool Enabled => getContentRegistry().ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeMoCaoCaoFuzai);
    public IReadOnlyList<int> Seats => getCoreGameEngineTableOrderRuntime().CurrentTableOrder();

    public MoCaoCaoParticipant Participant(int seat)
    {
        var p = matchParticipantsState.ReferenceToPlayers[seat];
        return new(seat, p.IsAlive, p.IsHuman, p.Hp, getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(p).Count, p.EquipmentSlotCapacity(EquipmentSlot.Weapon) > 0, p.EquipmentSlotCapacity(EquipmentSlot.Armor) > 0);
    }

    public MoCaoCaoBinding? FuzaiBinding(int seat)
    {
        if (!Enabled || !getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) || !matchParticipantsState.ReferenceToPlayers[seat].IsAlive)
            return null;
        var c = getCoreGameEngineProgramLifecycleRuntime().CollectProgramTriggerCandidates(matchParticipantsState.ReferenceToPlayers[seat], SkillProgramTriggerWindow.GameStarting).FirstOrDefault(c => c.SkillId == MoCaoCaoComposition.Fuzai);
        return c is null ? null : new(new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash);
    }

    public ContentSkillDefinition Skill(string id) => getContentRegistry().GetSkill(id);
    public SkillProgram? Program(string id) => getContentRegistry().Skills.TryGetValue(id, out var skill) ? skill.Program : null;
    public ActualDiscardRecoveryPhaseKey? Phase => getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase();

    public IReadOnlyList<int> Hand(int seat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Select(c => c.Id).ToArray());
    public CardLocation Location(int id) => getCardZoneStore().GetLocation(id);
    public CardKind Kind(int id) => getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id).Kind;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public bool Held(ProgramSkillFrame f) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat], f.SkillId, f.SkillInstanceId);
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;

    public bool IsExactStart(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.WindowContext is not { } w || getCoreGameEnginePrivateOfferReturnsRuntime().GetPausedPrivateOfferEffect(f)?.Op != op || Program(f.SkillId)?.GameplayHash != f.GameplayHash)
            return false;
        var index = getFrameStore().FindIndex(p => p.Id == f.Id);
        if (index < 1 || getFrameStore()[index - 1].Id != w.ParentFrameId)
            return false;
        if (op == SkillProgramEffectOp.ResolveMoCaoCaoBaibi)
            return getFrameStore()[index - 1] is DamageTriggerWindowFrame damage && damage.TriggerWindow == SkillProgramTriggerWindow.AfterDamageApplied && damage.CandidateIndex >= 0 && damage.CandidateIndex < damage.Candidates.Count && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, damage.Candidates[damage.CandidateIndex].ToProgramCandidate());
        if (op == SkillProgramEffectOp.ResolveMoCaoCaoBachao)
            return w.Window == SkillProgramTriggerWindow.PlayPhaseStarting && w.OwnerSeat == f.OwnerSeat && w.SourceSeat == f.OwnerSeat && w.TargetSeat == f.OwnerSeat && getFrameStore()[index - 1] is PlayPhaseStartingBoundaryFrame starting && starting.OwnerSeat == f.OwnerSeat && starting.ItemIndex >= 0 && starting.ItemIndex < starting.Items.Count && starting.Items[starting.ItemIndex] is { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate } && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
        return op == SkillProgramEffectOp.InitializeMoCaoCaoFuzai && getFrameStore()[index - 1] is ProgramLifecycleTriggerWindowFrame parent && parent.Window == SkillProgramTriggerWindow.GameStarting && w.Window == parent.Window && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]);
    }

    public int ResolveHandCard(ProgramSkillFrame f, int chooser, PromptChoice c)
    {
        if (c.Cards is [var id])
            return id;
        var subject = f.MoCaoCaoBaibi?.Claim.TargetSeat ?? chooser;
        var zone = Enum.Parse<CardZoneKind>(c.Parameters["source-zone"], true);
        var slot = int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture);
        return getCardZoneStore().CardsAt(new(zone, subject))[slot].Id;
    }

    public IReadOnlyList<PromptChoice> PaymentChoices(ProgramSkillFrame f, int chooser, int subject, bool handOnly) => getCoreGameEngineProgramCardPaymentsRuntime().BuildOwnedCardPaymentChoices(f.Id, chooser, subject, handOnly ? [CardZoneKind.Hand] : [CardZoneKind.Hand, CardZoneKind.Equipment], handOnly ? OwnedCardMoveIntent.Obtain : OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.OwnerHand, destinationSeat: f.OwnerSeat);
    public MoCaoCaoDamageOrigin? DamageOrigin
    {
        get
        {
            if (getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt is not { IsChainPropagation: false, IsSourceLess: false } a || a.CardUserSeat != a.SourceSeat || a.EffectiveCardKind is not { } kind || getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(a.ResolutionId)?.Action is not { Type: CardActionType.Use } action || action.ActorSeat != a.SourceSeat || !EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || getFrameStore().OfType<DamageFrame>().LastOrDefault(d => d.ParentFrameId == a.ResolutionId && d.SourceSeat == a.SourceSeat && d.TargetSeat == a.TargetSeat)is not { } damage)
                return null;
            return new(damage.Id, a.ResolutionId, action.ActionId, a.SourceSeat, a.TargetSeat, kind, a.DamageAmount, a.IsSourceLess);
        }
    }

    public MoCaoCaoWeaponAuthorization? WeaponAuthorization(int seat, CardKind kind)
    {
        if (!getCoreGameEngineAdvancedWeaponsRuntime().HasWeaponAbility(matchParticipantsState.ReferenceToPlayers[seat], kind))
            return null;
        var physical = getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[seat]).FirstOrDefault(c => !c.IsGeneralWeapon && getCoreGameEngineAdvancedWeaponsRuntime().GetWeaponAbilityKinds(c).Contains(kind));
        if (physical is not null)
            return new(physical.Id, 0, $"equipment:{physical.Id}:{kind}");
        if (getCharacterCaoCaoOlMagicRuntime().Value.MoCaoCaoEquipment.VirtualKind(seat, EquipmentSlot.Weapon) == kind && getCharacterCaoCaoOlMagicRuntime().Value.MoCaoCaoEquipment.LastIssue(seat)is { } issue)
            return new(0, issue.IssueId, $"fuzai:{issue.IssueId}:{kind}");
        return (gameEngineAdvancedLifecycleState.ReferenceToInheritedWeaponAbilities.GetValueOrDefault(seat) ?? []).Contains(kind) ? new(0, 0, $"inherited:{seat}:{kind}") : null;
    }

    public bool ExactIssuedCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (getFrameStore().OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || !window.Candidates.Any(c => c.ToProgramCandidate() == candidate))
            return false;
        return (getCoreGameEngineProgramLifecycleRuntime().CreateAfterDamageProgramContext(window, candidate)with
        {
            Facts = null
        }

        ) == (context with
        {
            Facts = null
        }

        );
    }

    public bool NativeChild(ProgramSkillFrame f, ResolutionFrame child) => getCharacterCaoCaoOlMagicRuntime().Value.MoCaoCaoFirstNativeChild(f, child);
    public bool Friendly(int chooser, int target) => aiBrainsState.ReferenceToAiBrains[chooser].ScoreProgramTarget(createSnapshot(chooser), target, new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public int KeepValue(int chooser, int card) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(card), matchParticipantsState.ReferenceToPlayers[chooser]);
}
