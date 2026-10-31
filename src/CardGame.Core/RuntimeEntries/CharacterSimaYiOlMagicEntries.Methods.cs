#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaYiOlMagicEntries
{
    internal void AddMoGuifuNativeActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = default) => _resolve().AddMoGuifuNativeActions(actions, actor, playable, selectedPhysicalCardId);
    internal void BindMoSimaYiSlash(CardUseDeclaredEvent fact) => _resolve().BindMoSimaYiSlash(fact);
    internal bool CanAcquireMoGuifu(ProgramTriggerLegalityContext gate) => _resolve().CanAcquireMoGuifu(gate);
    internal bool CanMoSimaYiRetaliationSlash(int actor, int target) => _resolve().CanMoSimaYiRetaliationSlash(actor, target);
    internal bool CanOfferMoSimaYi(ProgramTriggerLegalityContext c) => _resolve().CanOfferMoSimaYi(c);
    internal void CaptureMoGuifuActualName(IGameEvent fact) => _resolve().CaptureMoGuifuActualName(fact);
    internal bool FinishMoSimaYiSlash(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMoSimaYiSlash(attack, out paused);
    internal bool HasMoGuifuNativeUse(int actor, int id, CardKind kind) => _resolve().HasMoGuifuNativeUse(actor, id, kind);
    internal bool IsMoGuifuHandLimitExempt(CharacterState owner, int id) => _resolve().IsMoGuifuHandLimitExempt(owner, id);
    internal bool IsMoSimaYiRetaliation(CardUseFrame use) => _resolve().IsMoSimaYiRetaliation(use);
    internal bool IsMoSimaYiRumoEntry(ProgramSkillFrame f) => _resolve().IsMoSimaYiRumoEntry(f);
    internal bool IsMoSimaYiRumoOrigin(long id, CardConversionSource source, string hash) => _resolve().IsMoSimaYiRumoOrigin(id, source, hash);
    internal CardKind MoGuifuAiSlashKind(ProgramAiNativeSlashQuery q) => _resolve().MoGuifuAiSlashKind(q);
    internal bool MoGuifuAlcoholPrivilege(ProgramNativeUseMaterialQuery q) => _resolve().MoGuifuAlcoholPrivilege(q);
    internal bool MoGuifuConversionUnavailable(ProgramConfiguredViewAsQuery q) => _resolve().MoGuifuConversionUnavailable(q);
    internal string? MoGuifuEntityLabel(int id) => _resolve().MoGuifuEntityLabel(id);
    internal bool MoGuifuMaterial(CharacterState owner, int id, CardKind kind) => _resolve().MoGuifuMaterial(owner, id, kind);
    internal bool MoGuifuNativeOutputBan(ProgramNativeSlashConversionQuery q) => _resolve().MoGuifuNativeOutputBan(q);
    internal bool MoGuifuNativeSlashAlias(ProgramNativeSlashAliasQuery q) => _resolve().MoGuifuNativeSlashAlias(q);
    internal IReadOnlyList<SlashUseVariant> MoGuifuPhysicalSlashVariants(ProgramNativeSlashMaterialQuery q) => _resolve().MoGuifuPhysicalSlashVariants(q);
    internal CardKind MoGuifuSelectedSlashKind(ProgramSelectedNativeSlashQuery q) => _resolve().MoGuifuSelectedSlashKind(q);
    internal bool MoGuifuSlashDebitExempt(long useId, int actor) => _resolve().MoGuifuSlashDebitExempt(useId, actor);
    internal bool MoGuifuSlashQuota(ProgramSlashQuotaQuery q) => _resolve().MoGuifuSlashQuota(q);
    internal bool MoGuifuUnspecifiedForcedSlashBan(ProgramNativeSlashConversionQuery q) => _resolve().MoGuifuUnspecifiedForcedSlashBan(q);
    internal bool RecordMoSimaYiDamage(GameEngine.IDamageAttempt attack, long damageFrameId) => _resolve().RecordMoSimaYiDamage(attack, damageFrameId);
    internal bool ReturnMoSimaYiAttack(GameEngine.AttackCompletionReceipt receipt) => _resolve().ReturnMoSimaYiAttack(receipt);
    internal void ReturnMoSimaYiUse(CardUseFrame use) => _resolve().ReturnMoSimaYiUse(use);
    internal void ValidateMoSimaYiUse(CardUseFrame use) => _resolve().ValidateMoSimaYiUse(use);
    internal bool HasMoSimaYi { get => _resolve().HasMoSimaYi; }
    internal MoSimaYiRuntime MoSimaYi { get => _resolve().MoSimaYi; }
}
