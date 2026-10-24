namespace CardGame.Core;

public enum ModeConfigurationValidationPhase { Adapter, BeforeModeResolution }

/// <summary>Validates the selected match's mode-specific immutable options before setup is applied.</summary>
public interface IMatchModeConfigurationValidator : IGameModeFeature
{
    void ValidateOptions();
}

/// <summary>Equipment additions and policy selection supplied by the installed mode implementation.</summary>
public interface IMatchModeEquipmentProfile : IGameModeFeature
{
    ModeEquipmentRecipe EquipmentRecipe { get; }
}

/// <summary>Frozen content selection only; no cards, acquisition state or runtime service is retained.</summary>
public sealed class ModeEquipmentRecipe
{
    public ModeEquipmentRecipe(IEnumerable<CardKind>? requiredKinds = null,
        IEnumerable<string>? disabledDestinationPolicyPackageIds = null)
    {
        RequiredKinds = Array.AsReadOnly((requiredKinds ?? []).Distinct().ToArray());
        DisabledDestinationPolicyPackageIds = Array.AsReadOnly((disabledDestinationPolicyPackageIds ?? [])
            .Distinct(StringComparer.Ordinal).ToArray());
    }
    public IReadOnlyList<CardKind> RequiredKinds { get; }
    public IReadOnlyList<string> DisabledDestinationPolicyPackageIds { get; }
}
