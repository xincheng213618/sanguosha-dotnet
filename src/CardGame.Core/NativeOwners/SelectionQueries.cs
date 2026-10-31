#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class SelectionQueries(Func<ProgramCapabilityRegistry> getProgramCapabilityRegistry) : IProgramSelectionQueries
    { public T Get<T>() where T : class, IProgramSelectionQuery => getProgramCapabilityRegistry().Get<T>(); }
