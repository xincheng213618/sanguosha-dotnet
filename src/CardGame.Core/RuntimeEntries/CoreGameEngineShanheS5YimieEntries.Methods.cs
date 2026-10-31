#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5YimieEntries
{
    internal bool CanOfferShanheS5Yimie(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferShanheS5Yimie(c);
    internal bool CompleteShanheS5YimieCardDamage(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().CompleteShanheS5YimieCardDamage(attack, out paused);
    internal bool ContinuePaidShanheS5Yimie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidShanheS5Yimie(f);
    internal void ContinueShanheS5YimieRecovery(long id) => _resolve().ContinueShanheS5YimieRecovery(id);
    internal bool HasShanheS5YimieCostPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasShanheS5YimieCostPaid(f);
    internal void RecordShanheS5YimieActualDamage(global::CardGame.Core.DamageAppliedEvent damage) => _resolve().RecordShanheS5YimieActualDamage(damage);
    internal void RequireShanheS5YimieOrigin(global::CardGame.Core.ShanheS5YimieOrigin r) => _resolve().RequireShanheS5YimieOrigin(r);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShanheS5Yimie(global::CardGame.Core.ProgramSkillFrame supplied, string stage) => _resolve().ResolveShanheS5Yimie(supplied, stage);
    internal bool TryBeginShanheS5YimieRecovery(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().TryBeginShanheS5YimieRecovery(attack);
}
