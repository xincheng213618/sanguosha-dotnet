namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool IsProgramSlashRedirectTarget(CardAttackHandle attack, int redirectorSeat, int targetSeat)
    {
        if (attack.TargetSeat != redirectorSeat ||
            !_players[redirectorSeat].IsAlive || !_players[targetSeat].IsAlive ||
            targetSeat == redirectorSeat || targetSeat == attack.SourceSeat)
            return false;
        var kind = attack.EffectiveCardKind ?? CardKind.Slash;
        return (ActiveFangtianHalberd is null ||
                !_resolutionStack.OfType<CardUseFrame>()
                    .Single(frame => frame.Id == attack.ResolutionId).TargetSeats.Contains(targetSeat)) &&
               (HasCardDistanceExemption(_players[attack.SourceSeat], _players[targetSeat], kind, attack.ResolutionId) ||
                IsWithinAttackRange(redirectorSeat, targetSeat)) &&
               !IsDirectedCardTargetProhibited(attack.SourceSeat, targetSeat, kind, attack.ResolutionId) &&
               !IsSlashProhibited(_players[targetSeat]);
    }

    private void ProhibitCurrentProgramResponse(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized,
                CardUse: { } context, TargetSeat: { } target })
        {
            var use = _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == context.ParentCardUseFrameId);
            if (use is not { Action: { Type: CardActionType.Use } action } || !IsSlashCard(action.EffectiveKind) ||
                action.ActionId != context.CardActionId || action.ActorSeat != frame.OwnerSeat || context.ActorSeat != frame.OwnerSeat ||
                !action.EffectiveDesignatedTargetSeats.Contains(target) || !IsValidPlayerSeat(target))
                throw new InvalidOperationException("A final-target response prohibition lost its original Slash and exact designated target.");
            if (!_players[target].IsAlive) return;
            var name = _contentRegistry.GetSkill(frame.SkillId).Name;
            CardAttackState Prohibit(CardAttackState state) => state with
            {
                ProhibitsDodge = true,
                ResponseProhibitingSkillNames = state.ResponseProhibitingSkillNames.Contains(name, StringComparer.Ordinal)
                    ? state.ResponseProhibitingSkillNames : Array.AsReadOnly(state.ResponseProhibitingSkillNames.Append(name).ToArray())
            };
            // The target-specific attack objects were already frozen during
            // native target preparation. The ordinary Fangtian continuation
            // restores these same states for each subsequent target.
            var current = use.CardAttack;
            var prepared = use.PreparedTargetAttacks;
            if (current?.TargetSeat != target && prepared?.Any(state => state.TargetSeat == target) != true)
                throw new InvalidOperationException("A final-target response prohibition lost its prepared native attack.");
            ReplaceRuntimeFrame(use.Id, use with
            {
                CardAttack = current is { } active && active.TargetSeat == target ? Prohibit(active) : current,
                PreparedTargetAttacks = prepared is null ? null : Array.AsReadOnly(prepared.Select(state => state.TargetSeat == target ? Prohibit(state) : state).ToArray())
            });
            return;
        }
        var attack = ProgramCardAttack ?? throw new InvalidOperationException(
            "A response prohibition requires the active Slash.");
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.SlashBeforeResponse ||
            !SameAttackOwner(ActiveCardAttack, attack) || frame.OwnerSeat != attack.SourceSeat ||
            frame.WindowContext.TargetSeat != attack.TargetSeat)
            throw new InvalidOperationException("Response prohibition lost its frozen attack boundary.");
        attack.ProhibitDodgeBy(_contentRegistry.GetSkill(frame.SkillId).Name);
    }

    private void RedirectCurrentProgramAttack(ProgramSkillFrame frame, int targetSeat)
    {
        var attack = ProgramCardAttack ?? throw new InvalidOperationException(
            "Attack redirection requires the active Slash.");
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.SlashTargetRedirecting ||
            !SameAttackOwner(ActiveCardAttack, attack) || frame.OwnerSeat != attack.TargetSeat ||
            !IsProgramSlashRedirectTarget(attack, frame.OwnerSeat, targetSeat))
            throw new InvalidOperationException("Attack redirection lost its frozen legal target.");
        var previousSeat = attack.TargetSeat;
        attack.SetDamageParticipants(attack.SourceSeat, targetSeat);
        attack.SetIgnoresArmor(
            HasArmorBypass(_players[attack.SourceSeat]) ||
            HasCardArmorBypass(_players[attack.SourceSeat], _players[targetSeat],
                attack.EffectiveCardKind ?? CardKind.Slash));
        RedirectCardUseTarget(attack.ResolutionId, previousSeat, targetSeat);
        RecordCurrentSlashFireRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordShortRangeSlashRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordMaWanHunjiangRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordZhangYanLangdaoRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordWeifuRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordBingzhongRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordChenglieRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordZixiExtraEffectRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordPijingRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordKenshangRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordDesignatedExtraTargetRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordCurrentEnhancementSlashRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        RecordSoleTargetAfterProtectionRedirect(frame, attack.ResolutionId, previousSeat, targetSeat);
        AddLog("SkillTriggered",
            $"{_players[frame.OwnerSeat].Name} 发动【{_contentRegistry.GetSkill(frame.SkillId).Name}】，将【杀】转移给 {_players[targetSeat].Name}。",
            frame.OwnerSeat, targetSeat);
    }
}
