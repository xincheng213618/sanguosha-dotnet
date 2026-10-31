#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoShunBoundaryMobileEntries
{
    internal global::CardGame.Core.BoundaryGaoShunXianzhenIssuedEvent? ActiveBoundaryGaoShunXianzhen(int actor) => _resolve().ActiveBoundaryGaoShunXianzhen(actor);
    internal bool BoundaryGaoShunDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().BoundaryGaoShunDistance(q);
    internal bool BoundaryGaoShunQuota(global::CardGame.Core.ProgramTargetQuotaQuery q) => _resolve().BoundaryGaoShunQuota(q);
    internal bool BoundaryGaoShunSlashBan(global::CardGame.Core.ProgramCardUseQuery q) => _resolve().BoundaryGaoShunSlashBan(q);
    internal bool BoundaryGaoShunSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().BoundaryGaoShunSlashDistance(q);
    internal bool BoundaryGaoShunSlashQuota(global::CardGame.Core.ProgramSlashQuotaQuery q) => _resolve().BoundaryGaoShunSlashQuota(q);
    internal bool BoundaryGaoShunWonTarget(int actor, int target) => _resolve().BoundaryGaoShunWonTarget(actor, target);
    internal bool CanRunBoundaryGaoShunJinjiu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunBoundaryGaoShunJinjiu(c, context);
    internal void CaptureBoundaryGaoShunPindian(global::CardGame.Core.PindianResultDeterminedEvent fact) => _resolve().CaptureBoundaryGaoShunPindian(fact);
    internal void ExecuteBoundaryGaoShun(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteBoundaryGaoShun(supplied, op);
    internal bool HasBoundaryGaoShunArmorBypass(int actor, int target, global::CardGame.Core.CardKind kind) => _resolve().HasBoundaryGaoShunArmorBypass(actor, target, kind);
    internal bool TryBoundaryGaoShunAlcoholBonus(global::CardGame.Core.ProgramSkillWindowContext context, int owner, scoped out int bonus) => _resolve().TryBoundaryGaoShunAlcoholBonus(context, owner, out bonus);
}
