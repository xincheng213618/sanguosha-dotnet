namespace CardGame.Core;

internal static class ModeDamageRuleComposition
{
    internal static IReadOnlyList<ModeDamageRuleRegistration> Freeze(ProgramDamageHookCatalog native,
        IEnumerable<ModeDamageRuleRegistration> registrations)
    {
        ArgumentNullException.ThrowIfNull(registrations);
        var rules = registrations.ToArray();
        if (rules.Any(rule => rule is null) || rules.GroupBy(rule => rule.Order).Any(group => group.Count() > 1) ||
            rules.Any(rule => native.Phases[ProgramDamagePhase.AdditiveAmount].Any(row => row.Order == rule.Order)))
            throw new InvalidOperationException("Native and mode damage hooks require unique orders within AdditiveAmount.");
        return Array.AsReadOnly(rules.OrderBy(rule => rule.Order).ToArray());
    }
}

internal sealed partial class MatchModeRuntime
{
    private ModeDamagePorts? _modeDamage;
    private ModeDamagePorts DamagePorts => _modeDamage ??= new(_features.Values);
    internal IReadOnlyList<ModeDamageRuleRegistration> DamageRules => DamagePorts.Rules;
    private void ValidateDamagePorts() => _ = DamagePorts;
    internal DamageNature TransformDamageNature(DamageNature nature)
    {
        foreach (var policy in DamagePorts.Nature) nature = policy.TransformDamageNature(nature);
        return nature;
    }

    private sealed class ModeDamagePorts
    {
        internal ModeDamagePorts(IEnumerable<object> features)
        {
            var installed = features.ToArray();
            Rules = ModeDamageRuleComposition.Freeze(EngineOwners.CoreProgramDamageHooksRuntime.NativeDamageHookCatalog,
                installed.OfType<IMatchModeDamageRuleProvider>().SelectMany(provider => provider.DamageRules ??
                    throw new InvalidOperationException("A mode damage rule provider returned no registrations.")));
            var nature = installed.OfType<IMatchModeDamageNaturePolicy>().OrderBy(policy => policy.Order).ToArray();
            if (nature.Any(policy => policy.Order < 0) || nature.GroupBy(policy => policy.Order).Any(group => group.Count() > 1))
                throw new InvalidOperationException("Mode damage nature policies require unique nonnegative orders.");
            Nature = Array.AsReadOnly(nature);
        }
        internal IReadOnlyList<ModeDamageRuleRegistration> Rules { get; }
        internal IReadOnlyList<IMatchModeDamageNaturePolicy> Nature { get; }
    }
}
