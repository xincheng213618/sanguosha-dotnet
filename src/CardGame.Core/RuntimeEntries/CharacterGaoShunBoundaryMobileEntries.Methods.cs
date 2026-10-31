#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoShunBoundaryMobileEntries
{
    internal BoundaryGaoShunXianzhenIssuedEvent? ActiveBoundaryGaoShunXianzhen(int actor) => _resolve().ActiveBoundaryGaoShunXianzhen(actor);
    internal bool BoundaryGaoShunDistance(ProgramCardDistanceQuery q) => _resolve().BoundaryGaoShunDistance(q);
    internal bool BoundaryGaoShunQuota(ProgramTargetQuotaQuery q) => _resolve().BoundaryGaoShunQuota(q);
    internal bool BoundaryGaoShunSlashBan(ProgramCardUseQuery q) => _resolve().BoundaryGaoShunSlashBan(q);
    internal bool BoundaryGaoShunSlashDistance(ProgramSlashDistanceQuery q) => _resolve().BoundaryGaoShunSlashDistance(q);
    internal bool BoundaryGaoShunSlashQuota(ProgramSlashQuotaQuery q) => _resolve().BoundaryGaoShunSlashQuota(q);
    internal bool BoundaryGaoShunWonTarget(int actor, int target) => _resolve().BoundaryGaoShunWonTarget(actor, target);
    internal bool CanRunBoundaryGaoShunJinjiu(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunBoundaryGaoShunJinjiu(c, context);
    internal void CaptureBoundaryGaoShunPindian(PindianResultDeterminedEvent fact) => _resolve().CaptureBoundaryGaoShunPindian(fact);
    internal void ExecuteBoundaryGaoShun(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ExecuteBoundaryGaoShun(supplied, op);
    internal bool HasBoundaryGaoShunArmorBypass(int actor, int target, CardKind kind) => _resolve().HasBoundaryGaoShunArmorBypass(actor, target, kind);
    internal bool TryBoundaryGaoShunAlcoholBonus(ProgramSkillWindowContext context, int owner, scoped out int bonus) => _resolve().TryBoundaryGaoShunAlcoholBonus(context, owner, out bonus);
}
