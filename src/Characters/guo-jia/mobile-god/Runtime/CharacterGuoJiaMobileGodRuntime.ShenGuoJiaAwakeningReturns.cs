#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaMobileGodRuntime
{
    internal bool HasShenGuoJiaAwakeningReceipt(ProgramSkillFrame frame) => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(frame.SkillId, out var definition) && (definition.Tags & SkillTag.Awakening) != 0 && definition.Program?.GameplayHash == frame.GameplayHash && ShenGuoJiaAwakeningBindings(frame.SkillId).Contains(CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), StringComparer.Ordinal) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaAwakeningOverrideEvent>().Any(fact => fact.FrameId < frame.Id && fact.Recipient == frame.OwnerSeat && fact.SkillId == frame.SkillId && fact.InstanceId == frame.SkillInstanceId && fact.GameplayHash == frame.GameplayHash);
}
