#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class ModeSetupEquipmentHost(global::System.Func<global::CardGame.Core.EquipmentContentRegistry> getEquipmentContentRegistry) : IModeSetupEquipmentHost
    {
        public EquipmentDefinition Definition(CardKind kind) => getEquipmentContentRegistry().Get(kind);
        public EquipmentSkillBinding? SkillBinding(CardKind kind) => getEquipmentContentRegistry().GetSkillBinding(kind);
    }
