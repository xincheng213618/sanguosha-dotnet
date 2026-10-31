#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaYiOlMagicEntries
{
    internal void AddMoGuifuNativeActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddMoGuifuNativeActions(actions, actor, playable, selectedPhysicalCardId);
    internal void BindMoSimaYiSlash(global::CardGame.Core.CardUseDeclaredEvent fact) => _resolve().BindMoSimaYiSlash(fact);
    internal bool CanAcquireMoGuifu(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanAcquireMoGuifu(gate);
    internal bool CanMoSimaYiRetaliationSlash(int actor, int target) => _resolve().CanMoSimaYiRetaliationSlash(actor, target);
    internal bool CanOfferMoSimaYi(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferMoSimaYi(c);
    internal void CaptureMoGuifuActualName(global::CardGame.Core.IGameEvent fact) => _resolve().CaptureMoGuifuActualName(fact);
    internal bool FinishMoSimaYiSlash(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMoSimaYiSlash(attack, out paused);
    internal bool HasMoGuifuNativeUse(int actor, int id, global::CardGame.Core.CardKind kind) => _resolve().HasMoGuifuNativeUse(actor, id, kind);
    internal bool IsMoGuifuHandLimitExempt(global::CardGame.Core.CharacterState owner, int id) => _resolve().IsMoGuifuHandLimitExempt(owner, id);
    internal bool IsMoSimaYiRetaliation(global::CardGame.Core.CardUseFrame use) => _resolve().IsMoSimaYiRetaliation(use);
    internal bool IsMoSimaYiRumoEntry(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsMoSimaYiRumoEntry(f);
    internal bool IsMoSimaYiRumoOrigin(long id, global::CardGame.Core.CardConversionSource source, string hash) => _resolve().IsMoSimaYiRumoOrigin(id, source, hash);
    internal global::CardGame.Core.CardKind MoGuifuAiSlashKind(global::CardGame.Core.ProgramAiNativeSlashQuery q) => _resolve().MoGuifuAiSlashKind(q);
    internal bool MoGuifuAlcoholPrivilege(global::CardGame.Core.ProgramNativeUseMaterialQuery q) => _resolve().MoGuifuAlcoholPrivilege(q);
    internal bool MoGuifuConversionUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery q) => _resolve().MoGuifuConversionUnavailable(q);
    internal string? MoGuifuEntityLabel(int id) => _resolve().MoGuifuEntityLabel(id);
    internal bool MoGuifuMaterial(global::CardGame.Core.CharacterState owner, int id, global::CardGame.Core.CardKind kind) => _resolve().MoGuifuMaterial(owner, id, kind);
    internal bool MoGuifuNativeOutputBan(global::CardGame.Core.ProgramNativeSlashConversionQuery q) => _resolve().MoGuifuNativeOutputBan(q);
    internal bool MoGuifuNativeSlashAlias(global::CardGame.Core.ProgramNativeSlashAliasQuery q) => _resolve().MoGuifuNativeSlashAlias(q);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> MoGuifuPhysicalSlashVariants(global::CardGame.Core.ProgramNativeSlashMaterialQuery q) => _resolve().MoGuifuPhysicalSlashVariants(q);
    internal global::CardGame.Core.CardKind MoGuifuSelectedSlashKind(global::CardGame.Core.ProgramSelectedNativeSlashQuery q) => _resolve().MoGuifuSelectedSlashKind(q);
    internal bool MoGuifuSlashDebitExempt(long useId, int actor) => _resolve().MoGuifuSlashDebitExempt(useId, actor);
    internal bool MoGuifuSlashQuota(global::CardGame.Core.ProgramSlashQuotaQuery q) => _resolve().MoGuifuSlashQuota(q);
    internal bool MoGuifuUnspecifiedForcedSlashBan(global::CardGame.Core.ProgramNativeSlashConversionQuery q) => _resolve().MoGuifuUnspecifiedForcedSlashBan(q);
    internal bool RecordMoSimaYiDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId) => _resolve().RecordMoSimaYiDamage(attack, damageFrameId);
    internal bool ReturnMoSimaYiAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt receipt) => _resolve().ReturnMoSimaYiAttack(receipt);
    internal void ReturnMoSimaYiUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMoSimaYiUse(use);
    internal void ValidateMoSimaYiUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateMoSimaYiUse(use);
    internal bool HasMoSimaYi { get => _resolve().HasMoSimaYi; }
    internal global::CardGame.Core.MoSimaYiRuntime MoSimaYi { get => _resolve().MoSimaYi; }
}
