#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeQilinBowRuntime
{
    internal IReadOnlyList<Card> GetQilinBowTargetMounts(CharacterState target) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Where(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card)is EquipmentSlot.OffensiveHorse or EquipmentSlot.DefensiveHorse).ToArray();
    internal bool TryBeginQilinBowChoice(NativeOwners.CardAttackHandle attack)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (!_coreNativeDamageHandlesOperations.Value.UsesFormalQilinBow || attack.QilinBowAttempted || attack.IsChainPropagation || attack.EffectiveCardKind is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash) || !source.IsAlive || !target.IsAlive || !_coreAdvancedWeaponsOperations.HasWeaponAbility(source, CardKind.QilinBow) || GetQilinBowTargetMounts(target).Count == 0)
        {
            return false;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveQilinBow is not null)
        {
            throw new InvalidOperationException("The engine cannot open two Qilin Bow choices at once.");
        }

        attack.MarkQilinBowAttempted();
        _coreNativeCardContinuationCursorsOperations.ActiveQilinBow = _kernelCoreMainOperations.CreateQilinBowHandle(attack);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.AwaitingResponse);
        PublishQilinBowChoice(_coreNativeCardContinuationCursorsOperations.ActiveQilinBow);
        return true;
    }

    internal void PublishQilinBowChoice(NativeOwners.QilinBowHandle pending)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveQilinBow, pending) || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack))
        {
            throw new InvalidOperationException("The Qilin Bow choice has no current Slash continuation.");
        }

        var attack = pending.Attack;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var mounts = GetQilinBowTargetMounts(target);
        if (mounts.Count == 0)
        {
            throw new InvalidOperationException("A Qilin Bow prompt requires at least one target mount.");
        }

        var choices = mounts.Select(card => new PromptChoice(new ChoiceId($"qilin-bow.mount-{(card.Id)}.resolution-{(attack.ResolutionId)}"), $"发动【麒麟弓】，弃置 {(target.Name)} 的【{(card.DisplayName)}】。", [card.Id], [target.Seat], new Dictionary<string, string> { ["action"] = "qilin-bow-discard", ["target-zone"] = "equipment" })).ToList();
        choices.Add(new PromptChoice(new ChoiceId($"qilin-bow.skip.resolution-{(attack.ResolutionId)}"), "不发动【麒麟弓】，继续结算此伤害。", [], [], new Dictionary<string, string> { ["action"] = "qilin-bow-skip" }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.QilinBow, source.Seat, $"你的【{(CardCatalog.Get(CoreNativeDamagePoliciesRuntime.RequireAttackCardKind(attack)).DisplayName)}】将对 {(target.Name)} 造成伤害，是否发动【麒麟弓】弃置其一张坐骑？", mounts.Select(card => card.Id).ToArray(), [target.Seat], SourceSeat: source.Seat, IncomingCard: attack.EffectiveCardKind)
        {
            PromptId = source.IsHuman ? _coreNativePromptBoundaryOperations.CreatePromptId() : default,
            IsPrivate = true,
            TargetSeat = target.Seat,
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = source.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveQilinBowChoice(PromptChoice selected)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveQilinBow ?? throw new InvalidOperationException("There is no Qilin Bow choice to resolve.");
        var attack = pending.Attack;
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.QilinBow } decision || decision.PlayerSeat != attack.SourceSeat)
        {
            throw new InvalidOperationException("The Qilin Bow choice is not the current Slash continuation.");
        }

        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var action = selected.Parameters.GetValueOrDefault("action");
        Card? discardedMount = null;
        if (action == "qilin-bow-discard" && selected.Targets.SequenceEqual([target.Seat]) && selected.Cards.Count == 1)
        {
            discardedMount = GetQilinBowTargetMounts(target).SingleOrDefault(card => card.Id == selected.Cards[0]) ?? throw new InvalidOperationException("The selected Qilin Bow mount is no longer available.");
        }
        else if (action != "qilin-bow-skip" || selected.Cards.Count != 0 || selected.Targets.Count != 0)
        {
            throw new InvalidOperationException("The Qilin Bow choice is malformed.");
        }

        _coreNativeCardContinuationCursorsOperations.ActiveQilinBow = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        if (discardedMount is not null && _coreNativeEquipmentInvocationsOperations.TryBeginQilinBowEquipmentInvocation(attack, discardedMount))
            return;
        _coreNativeEquipmentInvocationsOperations.ContinueInvokedQilinBow(attack, discardedMount);
    }
}
