#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRedAdditionalTargetsRuntime
{
    internal void GrantRedAdditionalTargets(ProgramSkillFrame frame, int maximum, bool? persistAfterSkillLoss = null)
    {
        var source = new CardConversionSource(frame.SkillId, "red-additional-targets", frame.OwnerSeat, frame.SkillInstanceId);
        _coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess._redAdditionalTargetGrants[frame.OwnerSeat] = (source, maximum, persistAfterSkillLoss);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RedAdditionalTargetsGrantedEvent(frame.OwnerSeat, source, maximum) { PersistAfterSkillLoss = persistAfterSkillLoss });
    }

    internal bool HasRedAdditionalTargets(CharacterState actor) => _coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess._redAdditionalTargetGrants.TryGetValue(actor.Seat, out var g) && (g.PersistAfterSkillLoss == true || _coreNationalSkillsOperations.HasRuntimeSkillInstance(actor, g.Source.SkillId, g.Source.SkillInstanceId));
    internal bool HasNextCardTargetAdjustment(CharacterState actor) => _coreNextCardTargetAdjustmentsOperations.HasLegacyNextCardTargetAdjustment(actor) || HasRedAdditionalTargets(actor) || _coreNextActualUseTargetAdjustmentsOperations.HasNextActualUseAdjustment(actor) || _characterZhouYuMouOperations.HasMouZyFireTargets(actor);
    internal bool IsTargetAdjustmentAction(CharacterState actor, LegalAction action) => action.ProgramActivationId == "next-card-target-adjustment" && _coreNextCardTargetAdjustmentsOperations.HasLegacyNextCardTargetAdjustment(actor) || action.ProgramActivationId == "red-additional-targets" && HasRedAdditionalTargets(actor) || _coreNextActualUseTargetAdjustmentsOperations.IsNextActualUseAdjustmentAction(actor, action) || _characterZhouYuMouOperations.IsMouZyTargetAction(actor, action) || _characterZhangFeiAnniversaryGodOperations.IsShenZfAdjustmentAction(actor, action);
    internal int GetAdditionalTargetAdjustmentLimit(CharacterState actor, LegalAction action) => _characterZhangFeiAnniversaryGodOperations.IsShenZfAdjustmentAction(actor, action) ? _coreMatchParticipantsPlayersAccess._players.Count : _characterZhouYuMouOperations.IsMouZyTargetAction(actor, action) ? _characterZhouYuMouOperations.MouZyTargetLimit(actor) : action.ProgramActivationId == "red-additional-targets" ? _coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess._redAdditionalTargetGrants[actor.Seat].Maximum : 1;
    [ProgramCardAction(ProgramCardActionPhase.AfterProgram, 50)]
    internal void AddRedAdditionalTargetActions(List<LegalAction> actions, CharacterState actor)
    {
        if (!HasRedAdditionalTargets(actor))
            return;
        var grant = _coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess._redAdditionalTargetGrants[actor.Seat];
        var actionKeys = actions.Select(action => (action.CardId, action.Kind, action.PlayedCardKind, action.ConversionSource, action.TargetCardId, Targets: string.Join(",", action.TargetSeats))).ToHashSet();
        foreach (var action in actions.ToArray().Where(a => a.CardId is not null && a.ProgramActivationId is null && a.Kind != LegalActionKind.Recast))
        {
            var(kind, suit) = _coreNextActualUseTargetAdjustmentsOperations.TargetAdjustmentAppearance(actor, action);
            if (suit is not (Suit.Heart or Suit.Diamond) || (CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) != SkillProgramCardCategory.Basic && !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind)))
                continue;
            var normal = action.Kind is LegalActionKind.DrawTwo or LegalActionKind.Peach or LegalActionKind.Alcohol ? new[]
            {
                actor.Seat
            }

            : _coreDirectedCardPolicyQueriesOperations.GetDeclaredCardTargets(actor, action.Kind, action.TargetSeats).ToArray();
            if (normal.Length == 0)
                continue;
            if (kind == CardKind.BorrowedSword)
            {
                normal = action.TargetSeats.ToArray();
                var originalHolders = normal.Where((_, index) => index % 2 == 0).ToArray();
                var pairs = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != actor.Seat && !originalHolders.Contains(p.Seat) && _coreNativeBorrowedSwordOperations.GetWeapon(p)is not null && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, p.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(suit)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, suit)).SelectMany(p => _coreMatchParticipantsPlayersAccess._players.Where(v => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(p, v)).Select(v => new[] { p.Seat, v.Seat })).ToArray();
                foreach (var pair in pairs)
                    Add([..normal, ..pair]);
                if (grant.Maximum == 2)
                    foreach (var a in pairs)
                        foreach (var b in pairs.Where(b => b[0] > a[0]))
                            Add([..normal, ..a, ..b]);
                continue;
            }

            var extra = _coreMatchParticipantsPlayersAccess._players.Where(p => !normal.Contains(p.Seat) && _coreNextCardTargetAdjustmentsOperations.CanBeExtraNextCardTarget(actor, p, action, kind, suit) && !(p.Seat == actor.Seat && action.ConversionSource is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)?.ExcludeOwnerEffects == true)).Select(p => p.Seat).ToArray();
            foreach (var seat in extra)
                Add([..normal, seat]);
            if (grant.Maximum == 2)
                for (var a = 0; a < extra.Length; a++)
                    for (var b = a + 1; b < extra.Length; b++)
                        Add([..normal, extra[a], extra[b]]);
            void Add(IReadOnlyList<int> targets)
            {
                if (!actionKeys.Add((action.CardId, action.Kind, action.PlayedCardKind, action.ConversionSource, action.TargetCardId, string.Join(",", targets))))
                    return;
                actions.Add(action with { TargetSeat = targets[0], TargetSeats = targets, ProgramSkillId = grant.Source.SkillId, ProgramActivationId = grant.Source.BindingId, Description = action.Description + "（增加目标）" });
            }
        }
    }
}
