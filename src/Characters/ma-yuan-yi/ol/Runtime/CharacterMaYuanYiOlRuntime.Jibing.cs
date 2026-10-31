#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaYuanYiOlRuntime
{
    internal IReadOnlyList<Card> JibingPileCards(CharacterState owner) => Array.AsReadOnly(_corePublicPileSourcesOperations.PublicPileSources(owner.Seat).Where(s => s.SkillId == "ol:jibing" && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, s.SkillId, s.SkillInstanceId)).SelectMany(_corePublicPileSourcesOperations.PublicPileCards).ToArray());
}
