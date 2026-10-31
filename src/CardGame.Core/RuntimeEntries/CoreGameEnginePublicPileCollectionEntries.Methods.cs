#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileCollectionEntries
{
    internal SkillProgramStepOutcome ExecutePublicPileCollection(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().ExecutePublicPileCollection(effect, frame);
    internal IReadOnlyList<PromptChoice> PublicPileCollectionChoices(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().PublicPileCollectionChoices(frame, effect);
    internal void ResolvePublicPileCollectionChoice(PromptChoice selected) => _resolve().ResolvePublicPileCollectionChoice(selected);
}
