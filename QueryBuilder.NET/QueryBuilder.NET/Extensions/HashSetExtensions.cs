namespace QueryBuilderDotNet.Extensions;

public static class HashSetExtensions
{
    /// <summary>
    /// Adds all elements from provided <paramref name="collection"/> to a source hash set
    /// </summary>
    /// <param name="hashSet">Instance of <see cref="HashSet{T}"/> to add the element to</param>
    /// <param name="collection">Source collection to add the elements from</param>
    /// <typeparam name="T">Type of the elemets</typeparam>
    /// <returns>True if all elements were added successfully, false if not</returns>
    public static bool AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> collection)
    {
        bool success = true;
        foreach (var item in collection)
        {
            if (!hashSet.Add(item)) success = false;
        }
        
        return success;
    }
}