#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreShenwuNpcProgramsRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 30)]
    internal bool ShenwuMingwanDirectedTargetProhibited(ProgramDirectedTargetQuery query) => ShenwuMingwanForbidsTarget(query.ActorSeat, query.TargetSeat);
    SkillProgramStepOutcome IShenwuNpcProgramHost.ExecuteShenwuNpc(ProgramSkillFrame frame, SkillProgramEffect effect) => ExecuteShenwuNpc(frame, effect);
    internal SkillProgramStepOutcome ExecuteShenwuNpc(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}");
        switch (effect.Op)
        {
            case SkillProgramEffectOp.DrawShenwuWuzangReplacement:
                _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, owner.Seat, Math.Max(5, owner.Hp / 2) + _coreShenwuExpandedPoliciesOperations.ShenwuReplacementDrawBonus(owner.Seat), null, null, SkillProgramCardSetVisibility.Private, reason);
                break;
            case SkillProgramEffectOp.DrawShenwuCangmuReplacement:
                _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, owner.Seat, _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive) + _coreShenwuExpandedPoliciesOperations.ShenwuReplacementDrawBonus(owner.Seat), null, null, SkillProgramCardSetVisibility.Private, reason);
                break;
            case SkillProgramEffectOp.ResolveShenwuYinzei:
            {
                GameEngine.IDamageAttempt attack = ShenwuAppliedDamage(frame);
                if (_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count != 0 || attack.IsSourceLess || attack.SourceSeat == owner.Seat || !_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive)
                    break;
                var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
                var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(source).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(source)).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(source.Seat, c, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, c), OwnedCardMoveIntent.Discard)).ToArray();
                if (cards.Length == 0)
                    break;
                var card = cards[_randomOperations._random.Next(cards.Length)];
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(owner.Seat, source.Seat, card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, card), reason, parentFrameId: frame.Id);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenwuYinzeiDiscardedEvent(frame.Id, owner.Seat, source.Seat, card.Id));
                break;
            }

            case SkillProgramEffectOp.BindShenwuOtherDamageSource:
            {
                GameEngine.IDamageAttempt attack = ShenwuAppliedDamage(frame);
                var eligible = !attack.IsSourceLess && attack.SourceSeat != owner.Seat && _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive;
                BindShenwuParticipant(frame, effect, eligible ? attack.SourceSeat : null, eligible ? "eligible" : "other");
                break;
            }

            case SkillProgramEffectOp.BindShenwuUndamagingTurnOwner:
            {
                var turn = _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().LastOrDefault();
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } context || turn is null || context.ParentFrameId != turn.Id || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(turn))
                    throw new InvalidOperationException("Yandu requires the exact completed actual turn.");
                var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
                var end = Array.FindLastIndex(history, e => e is TurnEndedEvent t && t.TurnNumber == turn.TurnNumber && t.ActorSeat == turn.OwnerSeat);
                var start = end < 0 ? -1 : Array.FindLastIndex(history, end, e => e is TurnStartedEvent t && t.TurnNumber == turn.TurnNumber && t.ActorSeat == turn.OwnerSeat);
                var eligible = start >= 0 && owner.Seat != turn.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[turn.OwnerSeat].IsAlive && !history.Skip(start + 1).Take(end - start - 1).OfType<DamageAppliedEvent>().Any(e => !e.SourceLess && e.SourceSeat == turn.OwnerSeat && e.Amount > 0);
                BindShenwuParticipant(frame, effect, eligible ? turn.OwnerSeat : null, eligible ? "eligible" : "other");
                break;
            }

            case SkillProgramEffectOp.RecordShenwuMingwanVictim:
            {
                GameEngine.IDamageAttempt attack = ShenwuAppliedDamage(frame);
                if (!ShenwuOwnActualTurn(owner.Seat) || attack.IsSourceLess || attack.SourceSeat != owner.Seat || attack.TargetSeat == owner.Seat || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || use.SourceSeat != owner.Seat || use.CardKind != attack.EffectiveCardKind)
                    break;
                if (!ShenwuMingwanVictims(owner.Seat).Contains(attack.TargetSeat))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenwuMingwanVictimRecordedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, owner.Seat, attack.TargetSeat, frame.WindowContext!.DamageFrameId!.Value, use.Id));
                break;
            }

            case SkillProgramEffectOp.DrawShenwuMingwanUse:
                if (ShenwuOwnActualTurn(owner.Seat) && frame.WindowContext?.CardUse is { ActorSeat: var actor } && actor == owner.Seat && ShenwuMingwanVictims(owner.Seat).Count != 0)
                    _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, owner.Seat, 1, null, null, SkillProgramCardSetVisibility.Private, reason);
                break;
            case SkillProgramEffectOp.PreventShenwuNitaiOwnTurnDamage:
                if (frame.WindowContext?.Window != SkillProgramTriggerWindow.BeforeDamageApplied || _resolutionStackOperations._resolutionStack.ElementAtOrDefault(_resolutionStackOperations._resolutionStack.Count - 2)is not BeforeDamageProgramWindowFrame damage || damage.Id != frame.WindowContext.ParentFrameId || damage.TargetSeat != owner.Seat)
                    throw new InvalidOperationException("Nitai requires its native before-damage window.");
                if ((frame.SkillId == "shenwu:zhaohuo" ? _coreNativeCardClassificationsOperations.GetDamageNature(_coreProgramAttackAttemptOperations.CurrentDamageAttempt!) == DamageNature.Fire : ShenwuOwnActualTurn(owner.Seat)) && !damage.Prevented)
                    _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(frame);
                break;
            case SkillProgramEffectOp.LoseShenwuTanyuHp:
                if (_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count <= _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count))
                    return _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, owner.Seat, 1);
                break;
            case SkillProgramEffectOp.BindShenwuRecoveryBeneficiary:
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterHpRecovered, HpChange: { Kind: HpChangeKind.Recovery } hp } || hp.Amount <= 0)
                    throw new InvalidOperationException("Jicai requires a real positive recovery occurrence.");
                BindShenwuParticipant(frame, effect, _coreMatchParticipantsPlayersAccess._players[hp.TargetSeat].IsAlive ? hp.TargetSeat : null, _coreMatchParticipantsPlayersAccess._players[hp.TargetSeat].IsAlive ? "eligible" : "other");
                break;
            case SkillProgramEffectOp.BindShenwuDeathBenefit:
            {
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CharacterDied, TargetSeat: { } victim } context || _coreMatchParticipantsPlayersAccess._players[victim].IsAlive)
                    throw new InvalidOperationException("Coalition supply requires an actual death window.");
                var choice = "none";
                if (owner.IsAlive && owner.TeamId is { } team)
                {
                    if (context.SourceSeat == owner.Seat && _coreMatchParticipantsPlayersAccess._players[victim].TeamId != team && _coreMatchParticipantsPlayersAccess._players[victim].General.Id is "shenwu:zhuyin-left" or "shenwu:zhuyin-right")
                        choice = "kill-minion";
                    else if (victim != owner.Seat && _coreMatchParticipantsPlayersAccess._players[victim].TeamId == team)
                        choice = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[victim]) == "god" ? "fallen-god" : "fallen-player";
                }

                BindShenwuParticipant(frame, effect, null, choice);
                break;
            }

            default:
                throw new InvalidOperationException("Unsupported Shenwu NPC operation.");
        }

        return SkillProgramStepOutcome.Continue;
    }

    internal GameEngine.IDamageAttempt ShenwuAppliedDamage(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)is not { } window)
            throw new InvalidOperationException("Shenwu requires its actual applied-damage parent.");
        return _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window);
    }

    internal void BindShenwuParticipant(ProgramSkillFrame frame, SkillProgramEffect effect, int? seat, string choice) => _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = seat is { } actual ? Array.AsReadOnly(new[] { actual }) : [], ChoiceBindings = Array.AsReadOnly(frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(effect.ResultBind!, choice, frame.OwnerSeat)).ToArray()) });

    internal bool ShenwuEnabled(CharacterState player, string skillId) => player.IsAlive && _contentRegistryOperations._contentRegistry.Skills.ContainsKey(skillId) && _coreSkillProgramsOperations.EnabledSkillPrograms(player).Any(p => p.Id == skillId);
    internal bool ShenwuOwnActualTurn(int seat) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == seat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase is not (TurnPhase.NotStarted or TurnPhase.Finished);
    internal IReadOnlyList<int> ShenwuMingwanVictims(int ownerSeat) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShenwuMingwanVictimRecordedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.OwnerSeat == ownerSeat).Select(e => e.VictimSeat).Distinct().ToArray();
    internal bool ShenwuMingwanForbidsTarget(int actorSeat, int targetSeat) => ShenwuOwnActualTurn(actorSeat) && actorSeat != targetSeat && ShenwuEnabled(_coreMatchParticipantsPlayersAccess._players[actorSeat], "shenwu:mingwan") && ShenwuMingwanVictims(actorSeat)is { Count: > 0 } victims && !victims.Contains(targetSeat);
    internal bool ShenwuFutaiForbidsPeach(int actorSeat, CardKind kind, CardActionType type) => kind == CardKind.Peach && type == CardActionType.Use && _coreMatchParticipantsPlayersAccess._players.Any(p => p.Seat != actorSeat && !ShenwuOwnActualTurn(p.Seat) && ShenwuEnabled(p, "shenwu:futai"));
    internal bool ShenwuCannotTurnOver(CharacterState target) => ShenwuEnabled(target, "shenwu:xiongshou") || _coreProgramShanheSourceWarElephantOperations.Value.ShanheWarElephantCannotTurnOver(target);
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 230)]
    internal int ShenwuNpcDamageBonus(GameEngine.IDamageAttempt attack)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var bonus = 0;
        if (!attack.IsSourceLess && !attack.IsChainPropagation && attack.CardUserSeat == attack.SourceSeat && attack.EffectiveCardKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash && ShenwuEnabled(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], "shenwu:xiongshou") && target.Hp < _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].Hp)
            bonus++;
        if (!attack.IsSourceLess && attack.SourceSeat != attack.TargetSeat && _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ShenwuArmedSourceDamageBonus) && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]).Any(c => EquipmentCatalog.Get(c.Kind).Slot == EquipmentSlot.Weapon))
            bonus++;
        if (!ShenwuOwnActualTurn(target.Seat) && _coreNativeCardClassificationsOperations.GetDamageNature(attack) == DamageNature.Fire && ShenwuEnabled(target, "shenwu:nitai"))
            bonus++;
        return bonus;
    }

    internal static bool IsShenwuJicaiTrigger(SkillProgramTrigger trigger) => trigger.Window == SkillProgramTriggerWindow.AfterHpRecovered && trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.BindShenwuRecoveryBeneficiary);
    internal IEnumerable<ProgramTriggerCandidate> CollectShenwuRecoveryCandidates(HpChangeContext change) => change.Kind != HpChangeKind.Recovery || change.Amount <= 0 ? [] : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.AfterHpRecovered)).Where(c => IsShenwuJicaiTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)));
    internal ProgramSkillWindowContext CreateShenwuRecoveryContext(HpChangeContext change, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.AfterHpRecovered, change.Id, candidate.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: change.TargetSeat, Amount: change.Amount, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])with { HpChangeAmount = change.Amount, HpBeforeChange = change.HpBefore, HpAfterChange = change.HpAfter }, HpChange: change);
    internal bool IsShenwuRecoveryCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsShenwuJicaiTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && context.HpChange is { Kind: HpChangeKind.Recovery, Amount: > 0 } hp && context.OwnerSeat == candidate.OwnerSeat && context.ParentFrameId == hp.Id && context.TargetSeat == hp.TargetSeat && _resolutionStackOperations._resolutionStack.OfType<HpChangedTriggerWindowFrame>().LastOrDefault()is { } parent && parent.Id == hp.Id && parent.Change == hp && parent.Candidates.ElementAtOrDefault(parent.CandidateIndex) == candidate;
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 50)]
    internal bool ShenwuFutaiCardUseForbidden(ProgramCardUseQuery query) => ShenwuFutaiForbidsPeach(query.ActorSeat, query.Kind, query.ActionType);
    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 20, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveShenwuRecovery(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsShenwuJicaiTrigger(trigger) ? IsShenwuRecoveryCandidate(candidate, context) : null;
}
