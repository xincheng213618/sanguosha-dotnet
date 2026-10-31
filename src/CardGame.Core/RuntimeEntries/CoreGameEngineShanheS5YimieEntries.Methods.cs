#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5YimieEntries
{
    internal bool CanOfferShanheS5Yimie(ProgramTriggerLegalityContext c) => _resolve().CanOfferShanheS5Yimie(c);
    internal bool CompleteShanheS5YimieCardDamage(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().CompleteShanheS5YimieCardDamage(attack, out paused);
    internal bool ContinuePaidShanheS5Yimie(ProgramSkillFrame f) => _resolve().ContinuePaidShanheS5Yimie(f);
    internal void ContinueShanheS5YimieRecovery(long id) => _resolve().ContinueShanheS5YimieRecovery(id);
    internal bool HasShanheS5YimieCostPaid(ProgramSkillFrame f) => _resolve().HasShanheS5YimieCostPaid(f);
    internal void RecordShanheS5YimieActualDamage(DamageAppliedEvent damage) => _resolve().RecordShanheS5YimieActualDamage(damage);
    internal void RequireShanheS5YimieOrigin(ShanheS5YimieOrigin r) => _resolve().RequireShanheS5YimieOrigin(r);
    internal SkillProgramStepOutcome ResolveShanheS5Yimie(ProgramSkillFrame supplied, string stage) => _resolve().ResolveShanheS5Yimie(supplied, stage);
    internal bool TryBeginShanheS5YimieRecovery(GameEngine.IDamageAttempt attack) => _resolve().TryBeginShanheS5YimieRecovery(attack);
}
