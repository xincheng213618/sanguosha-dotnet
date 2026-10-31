#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageHandlesEntries
{
    internal global::CardGame.Core.GameEngine.IDamageAttempt? GetCompletedDyingAttack(global::CardGame.Core.GameEngine.DyingCompletionReceipt receipt) => _resolve().GetCompletedDyingAttack(receipt);
    internal global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame) => _resolve().GetDamageTriggerAttack(frame);
    internal global::CardGame.Core.Card? GetDelayedJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => _resolve().GetDelayedJudgmentCard(frame);
    internal global::CardGame.Core.GameEngine.IDamageAttempt? GetDyingAttack(global::CardGame.Core.DyingFrame frame) => _resolve().GetDyingAttack(frame);
    internal global::CardGame.Core.NativeOwners.CardAttackHandle? GetJudgmentAttack(global::CardGame.Core.JudgmentFrame frame) => _resolve().GetJudgmentAttack(frame);
    internal global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => _resolve().GetJudgmentCard(frame);
    internal bool IsClassicIdentityMode { get => _resolve().IsClassicIdentityMode; }
    internal bool IsDualGeneralMode { get => _resolve().IsDualGeneralMode; }
    internal bool IsNationalWarMode { get => _resolve().IsNationalWarMode; }
    internal bool IsSurvivalMode { get => _resolve().IsSurvivalMode; }
    internal bool IsTeamMode { get => _resolve().IsTeamMode; }
    internal bool SupportsGodFactionSelection { get => _resolve().SupportsGodFactionSelection; }
    internal bool UsesClassicBattleRules { get => _resolve().UsesClassicBattleRules; }
    internal bool UsesFormalBorrowedSword { get => _resolve().UsesFormalBorrowedSword; }
    internal bool UsesFormalCixiongDoubleSwords { get => _resolve().UsesFormalCixiongDoubleSwords; }
    internal bool UsesFormalDaQiao { get => _resolve().UsesFormalDaQiao; }
    internal bool UsesFormalDiaoChan { get => _resolve().UsesFormalDiaoChan; }
    internal bool UsesFormalFangtianHalberd { get => _resolve().UsesFormalFangtianHalberd; }
    internal bool UsesFormalGongsunZan { get => _resolve().UsesFormalGongsunZan; }
    internal bool UsesFormalGudingBlade { get => _resolve().UsesFormalGudingBlade; }
    internal bool UsesFormalHuaXiong { get => _resolve().UsesFormalHuaXiong; }
    internal bool UsesFormalIceSword { get => _resolve().UsesFormalIceSword; }
    internal bool UsesFormalJianxiongDamageCard { get => _resolve().UsesFormalJianxiongDamageCard; }
    internal bool UsesFormalKongchengTargeting { get => _resolve().UsesFormalKongchengTargeting; }
    internal bool UsesFormalLuXun { get => _resolve().UsesFormalLuXun; }
    internal bool UsesFormalMengHuo { get => _resolve().UsesFormalMengHuo; }
    internal bool UsesFormalQilinBow { get => _resolve().UsesFormalQilinBow; }
    internal bool UsesFormalQinglongCrescentBlade { get => _resolve().UsesFormalQinglongCrescentBlade; }
    internal bool UsesFormalSilverLion { get => _resolve().UsesFormalSilverLion; }
    internal bool UsesFormalStoneAxe { get => _resolve().UsesFormalStoneAxe; }
    internal bool UsesFormalSunShangxiang { get => _resolve().UsesFormalSunShangxiang; }
    internal bool UsesFormalTaishiCi { get => _resolve().UsesFormalTaishiCi; }
    internal bool UsesFormalTengjia { get => _resolve().UsesFormalTengjia; }
    internal bool UsesFormalWoodenOx { get => _resolve().UsesFormalWoodenOx; }
    internal bool UsesFormalWushuang { get => _resolve().UsesFormalWushuang; }
    internal bool UsesFormalXiahouYuan { get => _resolve().UsesFormalXiahouYuan; }
    internal bool UsesFormalXunYu { get => _resolve().UsesFormalXunYu; }
    internal bool UsesFormalYuJin { get => _resolve().UsesFormalYuJin; }
    internal bool UsesFormalYuanShao { get => _resolve().UsesFormalYuanShao; }
    internal bool UsesFormalZhangJiao { get => _resolve().UsesFormalZhangJiao; }
    internal bool UsesFormalZhangbaSerpentSpear { get => _resolve().UsesFormalZhangbaSerpentSpear; }
    internal bool UsesFormalZhuRong { get => _resolve().UsesFormalZhuRong; }
    internal bool UsesFormalZhuqueFan { get => _resolve().UsesFormalZhuqueFan; }
    internal bool UsesGeneralBaseHp { get => _resolve().UsesGeneralBaseHp; }
}
