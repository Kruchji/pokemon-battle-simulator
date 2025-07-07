using System;
using System.Reflection;

namespace PokemonBattleSimulator;

internal static class StrategyManager
{
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

    public static MethodInfo? SelectStrategyMethod(List<MethodInfo> methods, string title)
    {
        var names = methods.Select(m => m.Name).ToList();
        int? index = PaginatedLists.PaginatedListWithSelection(names, title);
        return index.HasValue ? methods[index.Value] : null;
    }

    public static AIStrategy CreateAIStrategyDelegate(MethodInfo method)
    {
        return (AIStrategy)Delegate.CreateDelegate(typeof(AIStrategy), method);
    }

    public static AITeamStrategy CreateAITeamStrategyDelegate(MethodInfo method)
    {
        return (AITeamStrategy)Delegate.CreateDelegate(typeof(AITeamStrategy), method);
    }
}
