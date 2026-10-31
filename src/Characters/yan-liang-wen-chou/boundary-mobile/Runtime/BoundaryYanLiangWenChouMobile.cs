using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record BoundaryYanLiangWenChouClaimReceipt(CardConversionSource Source, string GameplayHash,
    long WindowId, long DamageId, long UseId, long ActionId, int InstructionIndex, int OccurrenceIndex,
    IReadOnlyList<int> CardIds, int Before, int After)
{
    private readonly IReadOnlyList<int> _cardIds = Array.AsReadOnly(CardIds.ToArray());
    public IReadOnlyList<int> CardIds { get => _cardIds; init => _cardIds = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundaryYanLiangWenChouClaimReceipt? BoundaryYanLiangWenChouClaim { get; init; }
}
// The response entities are public already; one scalar fact per actual gain
// also avoids publishing a mutable or private collection.
public sealed record BoundaryYanLiangWenChouResponseClaimedEvent(long ProgramFrameId, long UseId, long ActionId,
    int OwnerSeat, int CardId, int ClaimIndex, int MovementSequence) : IGameEvent;

internal interface IBoundaryYanLiangWenChouHost
{ SkillProgramStepOutcome Claim(ProgramSkillFrame frame); }
internal sealed class ClaimBoundaryYanLiangWenChouDuelResponsesDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses;
    public override ISkillProgramEffectHandler Handler { get; } = new ClaimBoundaryYanLiangWenChouDuelResponsesHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(8d));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}
internal sealed class ClaimBoundaryYanLiangWenChouDuelResponsesHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int t, ISkillProgramEffectHost h) => h.GetCapability<IBoundaryYanLiangWenChouHost>().Claim(f);
}
internal static class BoundaryYanLiangWenChouComposition
{
    [ProgramCatalogValidation(65710)]
    private static void Validate(string path, SkillProgram p)
    {
        foreach (var t in p.Triggers.Where(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses)))
            if (p.Id != "boundary:shuangxiong-mobile" || t.Window != SkillProgramTriggerWindow.AfterDamageApplied ||
                t.Subject != SkillProgramTriggerSubject.Owner || !t.Optional || t.DamageOccurrence != SkillProgramDamageOccurrence.PerDamage ||
                t.Effects is not [{ Op: SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses }])
                throw new InvalidOperationException(path + ": mobile Shuangxiong requires one optional original Duel damage-recipient response claim.");
    }
}

public sealed partial class GameEngine
{
}
