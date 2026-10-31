#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduEquipmentEntries
{
    internal void ContinueGuanduPolu(ProgramSkillFrame f) => _resolve().ContinueGuanduPolu(f);
    internal void ContinueGuanduThunderChariot(ProgramSkillFrame f) => _resolve().ContinueGuanduThunderChariot(f);
    internal Card[] GuanduChariotDiscardable(int actor, int target) => _resolve().GuanduChariotDiscardable(actor, target);
    internal string? GuanduEquipmentSkillGrantId(Card card) => _resolve().GuanduEquipmentSkillGrantId(card);
    internal bool IsGuanduDestroyedChariotMaterial(long useId, int cardId) => _resolve().IsGuanduDestroyedChariotMaterial(useId, cardId);
    internal void RecordGuanduChariotDestruction(IGameEvent payload) => _resolve().RecordGuanduChariotDestruction(payload);
}
