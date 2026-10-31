#nullable enable
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace CardGame.Core.NativeOwners;

internal sealed class SelectionQueries(global::System.Func<global::CardGame.Core.ProgramCapabilityRegistry> getProgramCapabilityRegistry) : IProgramSelectionQueries
    { public T Get<T>() where T : class, IProgramSelectionQuery => getProgramCapabilityRegistry().Get<T>(); }
