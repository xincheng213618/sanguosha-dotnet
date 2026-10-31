#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanLiangWenChouBoundaryMobileEntries
{
    internal SkillProgramStepOutcome BeginBoundaryYanLiangWenChouClaim(ProgramSkillFrame supplied) => _resolve().BeginBoundaryYanLiangWenChouClaim(supplied);
    internal bool BoundaryYanLiangWenChouOriginalDuel(int owner, string instance, ProgramSkillWindowContext context, scoped out CardUseFrame use) => _resolve().BoundaryYanLiangWenChouOriginalDuel(owner, instance, context, out use);
    internal IReadOnlyList<int> BoundaryYanLiangWenChouResponseEntities(CardUseFrame use, int owner) => _resolve().BoundaryYanLiangWenChouResponseEntities(use, owner);
    internal void ContinueBoundaryYanLiangWenChouClaim(long id) => _resolve().ContinueBoundaryYanLiangWenChouClaim(id);
    internal bool KeepPaidBoundaryYanLiangWenChouClaim(ProgramSkillFrame f) => _resolve().KeepPaidBoundaryYanLiangWenChouClaim(f);
    internal bool OfferBoundaryYanLiangWenChouClaim(ProgramTriggerLegalityContext c) => _resolve().OfferBoundaryYanLiangWenChouClaim(c);
    internal bool ResumeBoundaryYanLiangWenChouClaim(long id) => _resolve().ResumeBoundaryYanLiangWenChouClaim(id);
    internal bool ReturnBoundaryYanLiangWenChouClaim(ProgramSkillFrame f) => _resolve().ReturnBoundaryYanLiangWenChouClaim(f);
    internal bool ValidBoundaryYanLiangWenChouClaim(ProgramSkillFrame f) => _resolve().ValidBoundaryYanLiangWenChouClaim(f);
}
