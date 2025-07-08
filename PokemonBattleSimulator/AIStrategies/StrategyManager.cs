using System;
using System.Reflection;

namespace PokemonBattleSimulator;

/// <summary>
/// Manages the strategies for AI in battles, allowing for dynamic selection and execution of strategy methods.
/// </summary>
internal static class StrategyManager
{
    /// <summary>
    /// Retrieves all static public methods from the specified strategy class that can be assigned to the given delegate type.
    /// </summary>
    /// <param name="strategyClass">Strategy class type containing the methods.</param>
    /// <param name="delegateType">Delegate type to which the methods must be assignable.</param>
    /// <returns>List of MethodInfo objects representing the methods that match the criteria.</returns>
    public static List<MethodInfo> GetStrategyMethods(Type strategyClass, Type delegateType)
    {
        return strategyClass
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(m =>
            {
                try
                {
                    Delegate.CreateDelegate(delegateType, m);
                    return true;    // Assignable to the delegate type
                }
                catch (ArgumentException)
                {
                    return false; // Not assignable to the delegate type
                }
            })
            .ToList();
    }

    /// <summary>
    /// Selects a strategy method from a list of methods based on user input.
    /// </summary>
    /// <param name="methods">List of MethodInfo objects representing the available strategy methods.</param>
    /// <param name="title">Title for the selection prompt, displayed to the user.</param>
    /// <returns>MethodInfo of the selected strategy method, or null if no valid selection was made.</returns>
    public static MethodInfo? SelectStrategyMethod(List<MethodInfo> methods, string title)
    {
        var names = methods.Select(m => m.Name).ToList();
        int? index = PaginatedLists.PaginatedListWithSelection(names, title);
        return index.HasValue ? methods[index.Value] : null;
    }

    /// <summary>
    /// Creates a delegate for the specified method that matches the AIStrategy signature.
    /// </summary>
    /// <param name="method">MethodInfo of the method to create a delegate for.</param>
    /// <returns>AIStrategy delegate that can be invoked with the method's parameters.</returns>
    public static AIStrategy CreateAIStrategyDelegate(MethodInfo method)
    {
        return (AIStrategy)Delegate.CreateDelegate(typeof(AIStrategy), method);
    }

    /// <summary>
    /// Creates a delegate for the specified method that matches the AITeamStrategy signature.
    /// </summary>
    /// <param name="method">MethodInfo of the method to create a delegate for.</param>
    /// <returns>AITeamStrategy delegate that can be invoked with the method's parameters.</returns>
    public static AITeamStrategy CreateAITeamStrategyDelegate(MethodInfo method)
    {
        return (AITeamStrategy)Delegate.CreateDelegate(typeof(AITeamStrategy), method);
    }
}
