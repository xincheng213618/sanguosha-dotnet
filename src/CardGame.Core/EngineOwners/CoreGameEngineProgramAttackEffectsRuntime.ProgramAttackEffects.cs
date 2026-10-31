#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramAttackEffectsRuntime
{
    internal bool IsProgramSlashRedirectBaseTarget(NativeOwners.CardAttackHandle attack, int redirectorSeat, int targetSeat)
    {
        if (attack.TargetSeat != redirectorSeat || !_coreMatchParticipantsPlayersAccess._players[redirectorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || targetSeat == redirectorSeat || targetSeat == attack.SourceSeat)
            return false;
        var kind = attack.EffectiveCardKind ?? CardKind.Slash;
        return (_coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd is null || !_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId).TargetSeats.Contains(targetSeat)) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(attack.SourceSeat, targetSeat, kind, attack.ResolutionId) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat]);
    }

    internal bool IsProgramSlashRedirectTarget(NativeOwners.CardAttackHandle attack, int redirectorSeat, int targetSeat, bool paymentCompleted = false)
    {
        if (_characterDaQiaoMobileOperations.PostCostSlashRedirectFrame(attack, redirectorSeat)is { } frame)
            return paymentCompleted ? _characterDaQiaoMobileOperations.IsPostCostSlashRedirectTarget(attack, redirectorSeat, targetSeat, null) : _characterDaQiaoMobileOperations.HasPostCostSlashRedirectPayment(frame, attack, targetSeat);
        if (attack.TargetSeat != redirectorSeat || !_coreMatchParticipantsPlayersAccess._players[redirectorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || targetSeat == redirectorSeat || targetSeat == attack.SourceSeat)
            return false;
        var kind = attack.EffectiveCardKind ?? CardKind.Slash;
        return (_coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd is null || !_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId).TargetSeats.Contains(targetSeat)) && (_coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], kind, attack.ResolutionId) || _coreProgramDodgeResponsesOperations.IsWithinAttackRange(redirectorSeat, targetSeat)) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(attack.SourceSeat, targetSeat, kind, attack.ResolutionId) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat]);
    }

    internal void ProhibitCurrentProgramResponse(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context, TargetSeat: { } target })
        {
            var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == context.ParentCardUseFrameId);
            if (use is not { Action: { Type: CardActionType.Use } action } || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.ActionId != context.CardActionId || action.ActorSeat != frame.OwnerSeat || context.ActorSeat != frame.OwnerSeat || !action.EffectiveDesignatedTargetSeats.Contains(target) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target))
                throw new InvalidOperationException("A final-target response prohibition lost its original Slash and exact designated target.");
            if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                return;
            var name = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Name;
            CardAttackState Prohibit(CardAttackState state) => state with
            {
                ProhibitsDodge = true,
                ResponseProhibitingSkillNames = state.ResponseProhibitingSkillNames.Contains(name, StringComparer.Ordinal) ? state.ResponseProhibitingSkillNames : Array.AsReadOnly(state.ResponseProhibitingSkillNames.Append(name).ToArray())
            };
            // The target-specific attack objects were already frozen during
            // native target preparation. The ordinary Fangtian continuation
            // restores these same states for each subsequent target.
            var current = use.CardAttack;
            var prepared = use.PreparedTargetAttacks;
            if (current?.TargetSeat != target && prepared?.Any(state => state.TargetSeat == target) != true)
                throw new InvalidOperationException("A final-target response prohibition lost its prepared native attack.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { CardAttack = current is { } active && active.TargetSeat == target ? Prohibit(active) : current, PreparedTargetAttacks = prepared is null ? null : Array.AsReadOnly(prepared.Select(state => state.TargetSeat == target ? Prohibit(state) : state).ToArray()) });
            return;
        }

        var attack = _coreCardActionsOperations.ProgramCardAttack ?? throw new InvalidOperationException("A response prohibition requires the active Slash.");
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.SlashBeforeResponse || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || frame.OwnerSeat != attack.SourceSeat || frame.WindowContext.TargetSeat != attack.TargetSeat)
            throw new InvalidOperationException("Response prohibition lost its frozen attack boundary.");
        attack.ProhibitDodgeBy(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Name);
    }

    internal void RedirectCurrentProgramAttack(ProgramSkillFrame frame, int targetSeat)
    {
        var attack = _coreCardActionsOperations.ProgramCardAttack ?? throw new InvalidOperationException("Attack redirection requires the active Slash.");
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.SlashTargetRedirecting || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || frame.OwnerSeat != attack.TargetSeat)
            throw new InvalidOperationException("Attack redirection lost its frozen legal target.");
        if (!IsProgramSlashRedirectTarget(attack, frame.OwnerSeat, targetSeat, paymentCompleted: true))
        {
            if (_characterDaQiaoMobileOperations.PostCostSlashRedirectFrame(attack, frame.OwnerSeat)?.Id != frame.Id)
                throw new InvalidOperationException("Attack redirection lost its frozen legal target.");
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "已付弃牌的原流离目标不再合法，取消转移并返回原杀。");
            return;
        }

        var previousSeat = attack.TargetSeat;
        attack.SetDamageParticipants(attack.SourceSeat, targetSeat);
        attack.SetIgnoresArmor(_coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]) || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], attack.EffectiveCardKind ?? CardKind.Slash));
        _coreNativeCardUseFramesOperations.RedirectCardUseTarget(attack.ResolutionId, previousSeat, targetSeat);
        _coreCurrentSlashFireOffersOperations.RecordCurrentSlashFireRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _coreShortRangeSlashTargetsOperations.RecordShortRangeSlashRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _characterMaWanOlOperations.RecordMaWanHunjiangRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _characterZhangYanOlOperations.RecordZhangYanLangdaoRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _characterQianZhaoOlOperations.RecordWeifuRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _characterYangFuOlOperations.RecordBingzhongRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _characterMaChengOlOperations.RecordChenglieRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _characterDaQiaoLeOperations.RecordZixiExtraEffectRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _characterLiuPanOlOperations.RecordPijingRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _characterMaXiuMaTieOlOperations.RecordKenshangRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _coreDesignatedExtraTargetsOperations.RecordDesignatedExtraTargetRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _coreCurrentEnhancementSlashRedirectsOperations.RecordCurrentEnhancementSlashRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _coreSoleTargetRedirectOperations.RecordSoleTargetAfterProtectionRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Name)}】，将【杀】转移给 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)}。", frame.OwnerSeat, targetSeat);
    }
}
