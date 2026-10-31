#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceNamanEntries
{
    internal bool CanOfferShanheSourceNaman(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheSourceNaman(candidate, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome ClaimShanheSourceNamanResponseCards(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ClaimShanheSourceNamanResponseCards(supplied);
    internal int[] ShanheSourceNamanMaterials(global::CardGame.Core.ProgramSkillWindowContext? context, int ownerSeat) => _resolve().ShanheSourceNamanMaterials(context, ownerSeat);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame? ShanheSourceNamanResponse(global::CardGame.Core.ProgramSkillWindowContext? context, int ownerSeat) => _resolve().ShanheSourceNamanResponse(context, ownerSeat);
}
