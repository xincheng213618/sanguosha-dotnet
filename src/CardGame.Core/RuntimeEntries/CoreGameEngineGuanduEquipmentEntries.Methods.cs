#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduEquipmentEntries
{
    internal void ContinueGuanduPolu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduPolu(f);
    internal void ContinueGuanduThunderChariot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduThunderChariot(f);
    internal global::CardGame.Core.Card[] GuanduChariotDiscardable(int actor, int target) => _resolve().GuanduChariotDiscardable(actor, target);
    internal string? GuanduEquipmentSkillGrantId(global::CardGame.Core.Card card) => _resolve().GuanduEquipmentSkillGrantId(card);
    internal bool IsGuanduDestroyedChariotMaterial(long useId, int cardId) => _resolve().IsGuanduDestroyedChariotMaterial(useId, cardId);
    internal void RecordGuanduChariotDestruction(global::CardGame.Core.IGameEvent payload) => _resolve().RecordGuanduChariotDestruction(payload);
}
