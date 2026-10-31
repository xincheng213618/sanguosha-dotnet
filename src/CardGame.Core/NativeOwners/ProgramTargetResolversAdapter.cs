#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class ProgramTargetResolversAdapter(Func<ProgramTargetResolverRegistry> getProgramTargetResolverRegistry) : IProgramTargetResolvers
    {
        public bool TryResolve(ProgramTargetResolverContext context, out IReadOnlyList<int> targets) => getProgramTargetResolverRegistry().TryResolve(context, out targets);
        public int MinimumTargets(SkillProgramTargetKind kind) => getProgramTargetResolverRegistry().MinimumTargets(kind);
        public string EmptySelectionLabel(SkillProgramTargetKind kind) => getProgramTargetResolverRegistry().EmptySelectionLabel(kind);
    }
