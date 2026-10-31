#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanLiangWenChouBoundaryMobileEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryYanLiangWenChouClaim(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginBoundaryYanLiangWenChouClaim(supplied);
    internal bool BoundaryYanLiangWenChouOriginalDuel(int owner, string instance, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().BoundaryYanLiangWenChouOriginalDuel(owner, instance, context, out use);
    internal global::System.Collections.Generic.IReadOnlyList<int> BoundaryYanLiangWenChouResponseEntities(global::CardGame.Core.CardUseFrame use, int owner) => _resolve().BoundaryYanLiangWenChouResponseEntities(use, owner);
    internal void ContinueBoundaryYanLiangWenChouClaim(long id) => _resolve().ContinueBoundaryYanLiangWenChouClaim(id);
    internal bool KeepPaidBoundaryYanLiangWenChouClaim(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidBoundaryYanLiangWenChouClaim(f);
    internal bool OfferBoundaryYanLiangWenChouClaim(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().OfferBoundaryYanLiangWenChouClaim(c);
    internal bool ProveBoundaryYanLiangWenChouClaimMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().ProveBoundaryYanLiangWenChouClaimMovement(f, effect, pending);
    internal bool ResumeBoundaryYanLiangWenChouClaim(long id) => _resolve().ResumeBoundaryYanLiangWenChouClaim(id);
    internal bool ReturnBoundaryYanLiangWenChouClaim(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryYanLiangWenChouClaim(f);
    internal bool ValidBoundaryYanLiangWenChouClaim(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBoundaryYanLiangWenChouClaim(f);
}
