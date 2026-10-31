#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceNamanEntries
{
    internal bool CanOfferShanheSourceNaman(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheSourceNaman(candidate, trigger, context);
    internal SkillProgramStepOutcome ClaimShanheSourceNamanResponseCards(ProgramSkillFrame supplied) => _resolve().ClaimShanheSourceNamanResponseCards(supplied);
    internal int[] ShanheSourceNamanMaterials(ProgramSkillWindowContext? context, int ownerSeat) => _resolve().ShanheSourceNamanMaterials(context, ownerSeat);
    internal ProgramCardTriggerWindowFrame? ShanheSourceNamanResponse(ProgramSkillWindowContext? context, int ownerSeat) => _resolve().ShanheSourceNamanResponse(context, ownerSeat);
}
