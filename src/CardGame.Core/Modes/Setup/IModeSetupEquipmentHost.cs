namespace CardGame.Core;

/// <summary>Immutable equipment content from this match's installed packages.</summary>
public interface IModeSetupEquipmentHost
{
    EquipmentDefinition Definition(CardKind kind);
    EquipmentSkillBinding? SkillBinding(CardKind kind);
}
