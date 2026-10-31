#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDamageHandlesRuntime
{
    internal static bool SameAttackOwner(GameEngine.IDamageAttempt? left, GameEngine.IDamageAttempt? right) => left is not null && right is not null && left.ResolutionId == right.ResolutionId;
    internal static bool SameContinuationOwner(GameEngine.ICardContinuationHandle? left, GameEngine.ICardContinuationHandle? right) => left is not null && right is not null && left.GetType() == right.GetType() && left.OwnerFrameId == right.OwnerFrameId;
    internal NativeOwners.CardAttackHandle? GetJudgmentAttack(JudgmentFrame frame)
    {
        if (frame.Continuation is JudgmentContinuationKind.Indulgence or JudgmentContinuationKind.SupplyShortage or JudgmentContinuationKind.Lightning)
            return null;
        if (frame.Continuation == JudgmentContinuationKind.ProgramSkill && frame.ParentAttackId is null && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(parent => parent.Id == frame.ParentFrameId)is { ZhouQun: not null } ningwu && _characterZhouQunOlOperations.Value.IsNingwuJudgment(frame, ningwu))
            return null;
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId != frame.ParentAttackId || (frame.Continuation is JudgmentContinuationKind.Bagua or JudgmentContinuationKind.FactionDefenseBagua && frame.ParentAttackId != frame.ParentFrameId))
            throw new InvalidOperationException("A judgment lost its parent attack.");
        return _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
    }

    internal Card? GetJudgmentCard(JudgmentFrame frame) => frame.CardId is { } cardId ? _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(card => card.Id == cardId) : null;
    internal Card? GetDelayedJudgmentCard(JudgmentFrame frame) => frame.DelayedCardId is { } cardId ? _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(card => card.Id == cardId) : null;
    internal GameEngine.IDamageAttempt? GetDyingAttack(DyingFrame frame)
    {
        return GetCompletedDyingAttack(new GameEngine.DyingCompletionReceipt(frame.Id, frame.ParentFrameId, frame.VictimSeat, frame.Continuation));
    }

    internal GameEngine.IDamageAttempt? GetCompletedDyingAttack(GameEngine.DyingCompletionReceipt receipt)
    {
        if (receipt.Continuation is DyingContinuationKind.ProgramSkill or DyingContinuationKind.WenhePoison or DyingContinuationKind.SkillInvocation)
            return null;
        if (_coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || (receipt.Continuation == DyingContinuationKind.AttackHpLoss ? receipt.ParentFrameId != attack.ResolutionId : !_resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Any(damage => damage.Id == receipt.ParentFrameId && damage.ParentFrameId == attack.ResolutionId)))
            throw new InvalidOperationException("The dying frame lost its active attack.");
        return attack;
    }

    internal GameEngine.IDamageAttempt GetDamageTriggerAttack(DamageTriggerWindowFrame frame) => _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger is { } active && active.Id == frame.Id && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack ? attack : throw new InvalidOperationException("The damage trigger lost its active attack.");
    internal bool SupportsGodFactionSelection => _modeRulesOperations._modeRules.SupportsGodFactionSelection;
    internal bool IsClassicIdentityMode => _modeRulesOperations._modeRules.IsClassicIdentityContext;
    internal bool UsesClassicBattleRules => _modeRulesOperations._modeRules.UsesClassicBattleRules;
    internal bool UsesGeneralBaseHp => _modeRulesOperations._modeRules.UsesGeneralBaseHp;
    internal bool UsesFormalKongchengTargeting => UsesClassicBattleRules;
    internal bool UsesFormalJianxiongDamageCard => UsesClassicBattleRules;
    internal bool UsesFormalWushuang => UsesClassicBattleRules;
    internal bool UsesFormalDaQiao => UsesClassicBattleRules;
    internal bool UsesFormalDiaoChan => UsesClassicBattleRules;
    internal bool UsesFormalSunShangxiang => UsesClassicBattleRules;
    internal bool UsesFormalLuXun => UsesClassicBattleRules;
    internal bool UsesFormalXunYu => UsesClassicBattleRules;
    internal bool UsesFormalTaishiCi => UsesClassicBattleRules;
    internal bool UsesFormalYuanShao => UsesClassicBattleRules;
    internal bool UsesFormalXiahouYuan => UsesClassicBattleRules;
    internal bool UsesFormalHuaXiong => UsesClassicBattleRules;
    internal bool UsesFormalGongsunZan => UsesClassicBattleRules;
    internal bool UsesFormalZhangJiao => UsesClassicBattleRules;
    internal bool UsesFormalMengHuo => UsesClassicBattleRules;
    internal bool UsesFormalZhuRong => UsesClassicBattleRules;
    internal bool UsesFormalYuJin => UsesClassicBattleRules;
    internal bool UsesFormalBorrowedSword => UsesClassicBattleRules;
    internal bool UsesFormalStoneAxe => UsesClassicBattleRules;
    internal bool UsesFormalZhangbaSerpentSpear => UsesClassicBattleRules;
    internal bool UsesFormalCixiongDoubleSwords => UsesClassicBattleRules;
    internal bool UsesFormalQinglongCrescentBlade => UsesClassicBattleRules;
    internal bool UsesFormalIceSword => UsesClassicBattleRules;
    internal bool UsesFormalQilinBow => UsesClassicBattleRules;
    internal bool UsesFormalFangtianHalberd => UsesClassicBattleRules;
    internal bool UsesFormalGudingBlade => UsesClassicBattleRules;
    internal bool UsesFormalZhuqueFan => UsesClassicBattleRules;
    internal bool UsesFormalTengjia => UsesClassicBattleRules;
    internal bool UsesFormalSilverLion => UsesClassicBattleRules;
    internal bool UsesFormalWoodenOx => UsesClassicBattleRules;
    internal bool IsTeamMode => _modeRulesOperations._modeRules.Visibility.ShowsPublicTeams;
    internal bool IsSurvivalMode => _modeRulesOperations._modeRules.Visibility.UsesSurvivalRanking;
    internal bool IsNationalWarMode => _modeRulesOperations._modeRules.Visibility.UsesHiddenFaction;
    internal bool IsDualGeneralMode => _modeRulesOperations._modeRules.Visibility.UsesSecondaryGeneral;
}
